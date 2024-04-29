
using DataModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace StaffManagement.Services
{
    public class StaffService : IStaffService
    {
        private readonly HttpClient httpClient;

        public async Task<IEnumerable<StaffModel>> StaffGetList(int searchID, int searchGender, DateTime? startDate, DateTime? endDate)
        {
            string url = "api/Staff/staff_getlist/" + searchID + "&" + searchGender + "&" + (startDate == null ? "" : Convert.ToDateTime(startDate).ToString("yyyy-MM-dd")) + "&" + (endDate == null ? "" : Convert.ToDateTime(endDate).ToString("yyyy-MM-dd"));
            var claim = await httpClient.GetFromJsonAsync<StaffModel[]>(url);
            return claim;
        }

        public async Task<bool> StaffPost(StaffModel _staffModel)
        {
            var claim = await httpClient.PostAsJsonAsync("api/Staff/staff_post", _staffModel);
            return claim.IsSuccessStatusCode;
        }

        public async Task<bool> StaffDelete(int ID)
        {
            var claim = await httpClient.PostAsync("api/Staff/staff_delete/" + ID, null);
            return claim.IsSuccessStatusCode;
        }

        public StaffService(HttpClient _httpClient)
        {
            this.httpClient = _httpClient;
        }
    }

}