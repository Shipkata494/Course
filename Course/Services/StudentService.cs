using Course.Models;
using Course.Services.Interfaces;
using Course.ViewModel;
using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Course.Services
{
    public class StudentService : IStudentService
    {
        private readonly CourseraContext dbContext;

        public StudentService(CourseraContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public void SaveToCsv(string filePath, ICollection<StudentViewModel> studentsModel)
        {
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csv.WriteField("StudentName");
                csv.WriteField("TotalCredits");
                csv.WriteField("Courses");
                csv.NextRecord();
                foreach (var s in studentsModel)
                {
                    var courseResult = new List<CourseViewModel>();

                    foreach (var c in s.Courses)
                    {
                        var course = new CourseViewModel()
                        {
                            CourseName = c.CourseName,
                            Credit = c.Credit,
                            InstructorName = c.InstructorName,
                            Time = c.Time,
                        };
                        courseResult.Add(course);
                    }

                    csv.WriteRecord(new
                    {
                        StudentName = s.Name,
                        TotalCredits = s.TotalCredits,
                        Courses = string.Join(", ", courseResult.Select(course => $"{course.CourseName} ({course.Credit} credits) {course.InstructorName}"))
                    });

                    csv.NextRecord();
                }
            }
        }

        public async Task<ICollection<StudentViewModel>> GetStudentsAsync(ServiceModel model)
        {
            ICollection<StudentViewModel> result = new List<StudentViewModel>();
            ICollection<CourseViewModel> courseResult = new List<CourseViewModel>();
            var students = await dbContext.Students
                .Distinct()
                .Include(s => s.StudentsCoursesXrefs)
                .ThenInclude(c => c.Course)
                .ThenInclude(c => c.Instructor)
                .ToListAsync();

            foreach (var student in students)
            {
                foreach (var course in student.StudentsCoursesXrefs)
                {
                    var co = new CourseViewModel()
                    {

                        CourseName = course.Course.Name,
                        Credit = course.Course.Credit,
                        Time = course.Course.TotalTime,
                        InstructorName = course.Course.Instructor.FirstName + " " + course.Course.Instructor.LastName,
                    };

                    if (course.CompletionDate.HasValue &&
                         course.CompletionDate.Value > DateOnly.FromDateTime(model.StartDate) &&
                         course.CompletionDate.Value < DateOnly.FromDateTime(model.EndDate))
                    {
                    courseResult.Add(co);
                    }
                }
                var st = new StudentViewModel()
                {
                    Name = student.FirstName + " " + student.LastName,
                    Courses = courseResult,
                    TotalCredits = courseResult.Sum(c => c.Credit)
                };
                courseResult = new List<CourseViewModel>();
                if (st.TotalCredits > model.MinimumCredits)
                {

                result.Add(st);
                }
               
            }
            return result;
        }

    }
}
