namespace Course.ViewModel
{
    public class StudentViewModel
    {
        public StudentViewModel()
        {
            Courses = new HashSet<CourseViewModel>();
        }
        public string Name { get; set; }
        public int TotalCredits { get; set; }
        public ICollection<CourseViewModel> Courses { get; set; }
    }
}
