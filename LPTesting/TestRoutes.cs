using NUnit.Framework;
using Moq;
using System;
using System.Reflection;
using System.Web;
using System.Web.Routing;
using LandingPad;

namespace LPTesting
{
    [TestFixture]
    public class TestRoutes
    {
        /// <summary>
        /// Setup the Mock routing for the test
        /// Note: HttpContextBase does not come from System.Web.MVC, it now
        /// comes from System.Web.dll ver: 4.0. This also adds the dependencies
        /// for the rest of the test.
        /// anymore.
        /// </summary>
        /// <param name="targetURL"></param>
        /// <param name="httpMethod"></param>
        /// <returns> mockContext </returns>
        private HttpContextBase CreateHttpContext(string targetURL = null, string httpMethod = "GET")
        {
            //Create Mock Request (Directly from MVC Book sans MSTest)
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(m => m.AppRelativeCurrentExecutionFilePath).Returns(targetURL);
            mockRequest.Setup(m => m.HttpMethod).Returns(httpMethod);

            //Create Mock Response
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Setup(m => m.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);

            //Create the Mock Context
            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            mockContext.Setup(m => m.Request).Returns(mockRequest.Object);
            mockContext.Setup(m => m.Response).Returns(mockResponse.Object);

            //Return the Object
            return mockContext.Object;

        }

        /// <summary>
        /// Test route helper method
        /// </summary>
        /// <param name="url"></param>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <param name="routeProperties"></param>
        /// <param name="httpMethod"></param>
        private void TestRouteMatch(string url, string controller, string action, 
            object routeProperties = null, string httpMethod = "GET")
        {
            //Arrange
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            //Act - Process the route
            RouteData result = routes.GetRouteData(CreateHttpContext(url, httpMethod));

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(TestIncomingRouteResult(result, controller, action, routeProperties));

        }

        /// <summary>
        /// Helper method for asserting incoming route values
        /// </summary>
        /// <param name="routeResult"></param>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <param name="propertySet"></param>
        /// <returns> t/f </returns>
        private bool TestIncomingRouteResult(RouteData routeResult, string controller,
            string action, object propertySet = null)
        {
            //Create a string comparer function in method
            Func<object, object, bool> valCompare = (v1, v2) =>{
                return StringComparer.InvariantCultureIgnoreCase.Compare(v1, v2) == 0;
            };

            //Compare the action and controller values
            bool result = valCompare(routeResult.Values["controller"], controller) &&
                valCompare(routeResult.Values["action"], action);

            //Compare property values
            if(propertySet != null)
            {
                PropertyInfo[] propInfo = propertySet.GetType().GetProperties();
                foreach(PropertyInfo pi in propInfo)
                {
                    if(!(routeResult.Values.ContainsKey(pi.Name) && 
                        valCompare(routeResult.Values[pi.Name], 
                        pi.GetValue(propertySet, null))))
                    {
                        result = false;
                        break;
                    }
                }
            }
            return result;
        }

        private void TestRouteFail(string url)
        {
            //Arrange
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            //Act - process the route
            RouteData result = routes.GetRouteData(CreateHttpContext(url));

            //Assert
            Assert.IsTrue(result == null || result.Route == null);

        }

        [Test]
        public void DefaultURL_ShouldMapTo_Writing_Index()
        {
            TestRouteMatch("~/Writing", "Writing", "Index");
        }

        [Test]
        public void EditErrorURL_ShouldMapTo_EditError()
        {
            TestRouteMatch("~/Error/EditError", "Error", "EditError");
        }

        [Test]
        public void DeleteErrorURL_ShouldMapTo_DeleteErorr()
        {
            TestRouteMatch("~/Error/DeleteError", "Error", "DeleteError");
        }

        [Test]
        public void StoreURL_ShouldMapTo_StoreView()
        {
            TestRouteMatch("~/Upload/Store", "Upload", "Store");
        }

        [Test]
        public void ProfileEditErrorURL_ShouldMapTo_ProfileEditError()
        {
            TestRouteMatch("~/Error/ProfileEditError", "Error", "ProfileEditError");
        }

        [Test]
        public void HomeIndexURL_ShouldMapTo_Index()
        {
            TestRouteMatch("~/Home/Index", "Home", "Index");
        }
    }
}