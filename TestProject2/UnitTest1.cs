using Bunit;
using DataModels;
using Microsoft.AspNetCore.Components;
using StaffManagement.Pages;
using StaffManagement.Services;
using System.Diagnostics.Metrics;
using System.Net.Http;

namespace TestProject2
{
    public class UnitTest1 : TestContext
    {
        [Inject] IStaffService _IStaffService { get; set; } = default!;

        [Fact]
        public void CounterShouldIncrementWhenClicked()
        {
            var cut = RenderComponent<Counter>();

            cut.Find("button").Click();
            cut.Find("P").MarkupMatches("<p role=\"status\">Current count: 1</p>");
        }

        [Fact]
        public async void CreateUser_Success()
        {
            // Arrange
            var user = new StaffModel { ID = 1 };

            var userRepository = await _IStaffService.StaffGetList( 1 , 0, null, null);
            var dbuser = userRepository.FirstOrDefault();

            // Assert
            //Assert.True(userRepository.);
            //Assert.NotNull(retrievedUser);
            Assert.Equal(user.ID, dbuser.ID);
        }
    }
}