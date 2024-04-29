using DataModels;
using IronXL;
using Microsoft.JSInterop;

namespace StaffManagement.Data
{
    public class GenerateExcel
    {
        internal void ExcelGenerate(IJSRuntime jSRuntime, List<StaffModel> model)
        {
            byte[] fileContents;
            WorkBook workBook = WorkBook.Create(IronXL.ExcelFileFormat.XLSX);
            workBook.Metadata.Author = "IronXL";

            WorkSheet worksheet = workBook.CreateWorkSheet("new_sheet");
            int row = 2;

            foreach (var dynamicTxtBx in model)
            {
                worksheet["A1"].Value = "ID";
                worksheet["B1"].Value = "FULLNAME";
                worksheet["C1"].Value = "GENDER";
                worksheet["D1"].Value = "DOB";
                worksheet["E1"].Value = "CREATE_DATE";


                worksheet["A"+ row.ToString()].Value = dynamicTxtBx.ID.ToString();
                worksheet["B" + row.ToString()].Value = dynamicTxtBx.FULLNAME;
                worksheet["C" + row.ToString()].Value = dynamicTxtBx.GENDER == 1 ? "Male" : "Female";
                worksheet["D" + row.ToString()].Value = dynamicTxtBx.DOB.ToString("dd-MMM-yyyy");
                worksheet["E" + row.ToString()].Value = dynamicTxtBx.CREATE_DATE.ToString();
                row++;
            }











            fileContents = workBook.ToByteArray();
            jSRuntime.InvokeAsync<GenerateExcel>("saveasfile", "GeneratedExcel.xlsx", Convert.ToBase64String(fileContents));

        }
    }
}
