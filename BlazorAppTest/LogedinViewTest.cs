using BlazorAppWithNils.Components.Pages;
using BlazorAppWithNils.Data;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;



namespace BlazorAppTest
{
    public class LogedinViewTest
    {
        [Fact]
        public void TestLogin()
        {
            // Arrange
            using var ctx = new TestContext();
            var authContext = ctx.AddTestAuthorization();
            authContext.SetAuthorized("TestUser");
            authContext.SetRoles("Admin");

            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
               Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null
           );

            // // Optionally set up any UserManager behavior here
            mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                           .ReturnsAsync(new ApplicationUser { UserName = "testuser" });

            // // Register the mock UserManager in the service provider
             ctx.Services.AddSingleton(mockUserManager.Object);


            // Act
            var cut =ctx.RenderComponent<Home>();

            // Assert
            cut.MarkupMatches("<p>You are loggedin</p><p>You are Admin</p>");
          
        }

        //[Fact]
        //public void TestNotLogin()
        //{
        //    // Arrange
        //    using var ctx = new TestContext();
        //    var authContext = ctx.AddTestAuthorization();

        //    // Simulate the user is not logged in
        //    authContext.SetNotAuthorized();

        //    // Mock UserManager (even though user is not logged in, the component still depends on this service)
        //    var mockUserManager = new Mock<UserManager<ApplicationUser>>(
        //        Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null
        //    );

        //    // Register the mock UserManager in the service provider
        //    ctx.Services.AddSingleton(mockUserManager.Object);

        //    // Act
        //    var cut = ctx.RenderComponent<Home>();

        //    // Assert that the 'not logged in' message is rendered
        //    cut.MarkupMatches("<p>you are not logged in</p>");
        //}

        //[Fact]
        //public void TestLoginWithoutAdminRole()
        //{
        //    // Arrange
        //    using var ctx = new TestContext();
        //    var authContext = ctx.AddTestAuthorization();

        //    // Simulate user is logged in but does not have the Admin role
        //    authContext.SetAuthorized("TestUser");
        //    authContext.SetRoles(""); // No roles assigned

        //    // Mock UserManager
        //    var mockUserManager = new Mock<UserManager<ApplicationUser>>(
        //        Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null
        //    );

        //    // Set up any UserManager behavior if needed (this is optional depending on how your component uses UserManager)
        //    mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
        //                   .ReturnsAsync(new ApplicationUser { UserName = "testuser" });

        //    // Register the mock UserManager in the service provider
        //    ctx.Services.AddSingleton(mockUserManager.Object);

        //    // Act
        //    var cut = ctx.RenderComponent<Home>();

        //    // Assert that the login message is rendered, but no admin message
        //    cut.MarkupMatches("<p>You are login</p>");
        //}

    }
}