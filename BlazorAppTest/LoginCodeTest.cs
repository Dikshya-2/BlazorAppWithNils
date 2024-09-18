﻿using BlazorAppWithNils.Components.Pages;
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
    public class LoginCodeTest
    {
        [Fact]

        public void TestLoginAsAdmin()
        {
            // Arrange
            using var ctx = new TestContext();
            var authContext = ctx.AddTestAuthorization();
            authContext.SetAuthorized("TestUser"); 
            authContext.SetRoles("Admin"); 

            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null
            );

            // Set up the mock UserManager behavior
            mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                           .ReturnsAsync(new ApplicationUser { UserName = "testuser" });

            // Register the mock UserManager in the service provider
            ctx.Services.AddSingleton(mockUserManager.Object);

            // Act
            var cut = ctx.RenderComponent<Home>();
            var ins = cut.Instance;

            // Assert
            Assert.True(ins._isAuthenticated); 
            Assert.True(ins._isAdmin); 
        }
    }
}

