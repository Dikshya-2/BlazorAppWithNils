using BlazorAppWithNils.Components.Pages;
using BlazorAppWithNils.Data;
using Bunit;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Bunit.TestDoubles;

namespace BlazorAppTest
{
    public class LoginTestViewWithNils
    {
        [Fact]
        public void TestLoginView()
        {
            // Arrange
            using var ctx = new TestContext();
            var authContext = ctx.AddTestAuthorization();
           
            // Act
            var cut = ctx.RenderComponent<BlazorAppWithNils.Components.Pages.Home>();

            // Assert
            cut.MarkupMatches("<p>You are not logged in</p>");

            
        }
        [Fact]
        public void TestLoginCode()
        {
            // Arrange
            using var ctx = new TestContext();
            var authContext = ctx.AddTestAuthorization();

            // Act
            var cut = ctx.RenderComponent<BlazorAppWithNils.Components.Pages.Home>();
            var ins = cut.Instance;
            Assert.False(ins._isAuthenticated);
        }
    }
}
