using Course.Models;
using Course.Services.Interfaces;
using Course.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Course.Services
{
    public class StudentService : IStudentService
    {
        private readonly CourseraContext dbContext;

        public StudentService(CourseraContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public Task<CourseViewModel> GetCoursesByIdAsync()
        {
            throw new NotImplementedException();
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
