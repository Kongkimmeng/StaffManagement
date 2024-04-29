using DataModels;

namespace StaffManagement.Services
{
    public interface IStaffService
    {
        Task<IEnumerable<StaffModel>> StaffGetList(int searchID, int searchGender, DateTime? startDate, DateTime? endDate);
        Task<bool> StaffPost(StaffModel staffModel);
        Task<bool> StaffDelete(int ID);
    }
}
