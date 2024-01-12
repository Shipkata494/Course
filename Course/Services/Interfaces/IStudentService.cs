using Course.Models;
using Course.ViewModel;

namespace Course.Services.Interfaces
{
    public interface IStudentService
    {
        Task<ICollection<StudentViewModel>> GetStudentsAsync(ServiceModel model);
        void SaveToCsv(string filePath, ICollection<StudentViewModel> studentsModel);
    }
}
