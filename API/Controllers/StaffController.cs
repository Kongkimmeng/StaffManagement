using Dapper;
using DataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        public SqlConnection conn = new SqlConnection("Server=DESKTOP-2HE773K;Initial Catalog=STAFF;User ID=sa;Password=123");

        [Route("staff_getlist/{parameter}"), HttpGet]
        public async Task<IActionResult> StaffGet(string parameter)
        {
            string[] substrings = parameter.Split('&');
            int id = Convert.ToInt32(substrings[0]);
            int gender = Convert.ToInt32(substrings[1]);
            string startdate = substrings[2];
            string enddate = substrings[3];

            conn.Open();
            var parameters = new DynamicParameters();
            parameters.Add("ID", id , DbType.Int32);
            parameters.Add("GENDER", gender , DbType.Int32);
            parameters.Add("DOB_S", startdate == "" ? null : startdate, DbType.DateTime);
            parameters.Add("DOB_E", enddate == "" ? null : enddate, DbType.DateTime);
            var t = await conn.QueryAsync<StaffModel>("SP_STAFF_GET", parameters, commandType: CommandType.StoredProcedure);
            List<StaffModel> Data = t.ToList();
            conn.Close();

            return Ok(Data);
        }       

        [Route("staff_post"), HttpPost]
        public async Task<IActionResult> StaffPost(StaffModel StaffModel)
        {
            conn.Open();
            var parameters = new DynamicParameters();
            parameters.Add("ID", StaffModel.ID, DbType.Int32);
            parameters.Add("FULLNAME", StaffModel.FULLNAME, DbType.String);
            parameters.Add("GENDER", StaffModel.GENDER, DbType.Int32);
            parameters.Add("DOB", StaffModel.DOB, DbType.DateTime);
            await conn.QueryAsync<StaffModel>("SP_STAFF_POST", parameters, commandType: CommandType.StoredProcedure);
            conn.Close();
            return Ok();
        }

        [Route("staff_delete/{ID}"), HttpPost]
        public async Task<IActionResult> StaffDelte(int ID)
        {
            conn.Open();
            var parameters = new DynamicParameters();
            parameters.Add("ID", ID, DbType.Int32);          
            await conn.QueryAsync<StaffModel>("SP_STAFF_DELETE", parameters, commandType: CommandType.StoredProcedure);
            conn.Close();
            return Ok();
        }

    }
}
