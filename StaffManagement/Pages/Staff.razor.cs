using BlazorBootstrap;
using Dapper;
using DataModels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StaffManagement.Services;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;
using IronXL;
using StaffManagement.Data;
using SixLabors.ImageSharp.Drawing;
using Microsoft.Extensions.Options;

namespace StaffManagement.Pages
{
    public partial class Staff
    {
        [Inject] ToastService ToastService { get; set; } = default!;
        [Inject] IStaffService _IStaffService { get; set; } = default!;
        private ConfirmDialog dialog = default!;
        private Modal modalEdit = default!;
        private List<StaffModel>? StaffModelList = new();
        private StaffModel? StaffModel = new();

        public string? SearchID;
        public int SearchGender = 0;
        public DateTime? StartDate;
        public DateTime? EndDate;
        private Button saveButton4;

        protected override void OnInitialized()
        {
            LoadData();
        }
        private async void LoadData()
        {
            var data = await _IStaffService.StaffGetList(SearchID == "" ? 0 : Convert.ToInt32(SearchID), SearchGender, StartDate, EndDate);
            StaffModelList = data.ToList();
            StateHasChanged();
        }
        private async Task OnEdit(int ID)
        {
            StaffModel = StaffModelList.SingleOrDefault(o => o.ID == ID);
            await modalEdit.ShowAsync();
        }
        private async Task OnDelete(int ID)
        {
            var options = new ConfirmDialogOptions { Size = DialogSize.Small };
            var confirmation = await dialog.ShowAsync(
           title: "Confirm Dialog",
           message1: "Do you want to delete ?",
           confirmDialogOptions: options);

            if (confirmation)
            {
                var data = await _IStaffService.StaffDelete(ID);
                LoadData();
                ToastService.Notify(new ToastMessage(ToastType.Success, $"Staff deleted successfully."));
            }
        }
        private async Task OnHideModalClick()
        {
            LoadData();
            await modalEdit.HideAsync();
        }
        private async Task OnNew()
        {
            StaffModel = new();
            StaffModel.DOB = DateTime.Now;
            await modalEdit.ShowAsync();
        }
        private async void OnSave()
        {
            var options = new ConfirmDialogOptions { Size = DialogSize.Small };
            var confirmation = await dialog.ShowAsync(
           title: "Confirm Dialog",
           message1: "Do you want to save ?",
           confirmDialogOptions: options);

            if (confirmation)
            {
                if (StaffModel.FULLNAME == "" || StaffModel.GENDER == 0)
                {
                    ToastService.Notify(new ToastMessage(ToastType.Warning, $"Please Fill in the blank"));
                    return;
                }

                var data = await _IStaffService.StaffPost(StaffModel);

                LoadData();
                await modalEdit.HideAsync();
                ToastService.Notify(new ToastMessage(ToastType.Success, $"Staff Information Saved"));
            }       

        }


        private async void OnDownload()
        {
             saveButton4?.ShowLoading("download...");
            await Task.Run(() =>
            {
                GenerateExcel excel = new GenerateExcel();
                excel.ExcelGenerate(JS, StaffModelList);


            });
            ToastService.Notify(new ToastMessage(ToastType.Success, $"Excel Downloaded"));

            saveButton4?.HideLoading();
        }
    }
}
