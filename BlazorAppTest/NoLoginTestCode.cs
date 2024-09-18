using BlazorAppWithNils.Components.Pages;
using BlazorAppWithNils.Data;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAppTest
{
    public class NoLoginTestCode
    {
        [Fact]
        public void TestNotLoggedIn()
        {
            // Arrange
            using var ctx = new TestContext();
            var authContext = ctx.AddTestAuthorization();
            authContext.SetNotAuthorized(); 

            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null
            );

            // Register the mock UserManager in the service provider
            ctx.Services.AddSingleton(mockUserManager.Object);

            // Act
            var cut = ctx.RenderComponent<Home>();
            var ins = cut.Instance;

            // Assert
            Assert.False(ins._isAuthenticated); 
            
        }
    }
}
