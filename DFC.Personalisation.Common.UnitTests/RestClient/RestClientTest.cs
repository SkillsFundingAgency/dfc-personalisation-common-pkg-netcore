using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;

using System.Threading;
using System.Threading.Tasks;
using DFC.Personalisation.Common.RestClient;
using FluentAssertions;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace DFC.Personalisation.Common.UnitTests.RestClient
{

    [TestFixture]
    public class RestClientTests
    {
        [TestFixture]
        public class CallServiceAsyncTest
        {
            #region ***** Test Get *****
            [TestCase("https://jsonplaceholder.typicode.com/todos/1")]
            public async Task When_MockServiceGet_Then_ShouldReturnObject(string url)
            {
                // ARRANGE
                var handlerMock = GetMockMessageHandler();

                // use real http client with mocked handler here
                var httpClient = new HttpClient(handlerMock.Object);
                var subjectUnderTest = new Common.RestClient.RestClient(httpClient);

                // ACT
                MockResult result = await subjectUnderTest.Get<MockResult>(url);

                // ASSERT
                result.Should().NotBeNull(); // this is fluent assertions here...
                result.Id.Should().Be(1);

                // also check the 'http' call was like we expected it
                var expectedUri = new Uri(url);

                handlerMock.Protected().Verify(
                    "SendAsync",
                    Times.Exactly(1), // we expected a single external request
                    ItExpr.Is<HttpRequestMessage>(req =>
                            req.Method == HttpMethod.Get // we expected a GET request
                            && req.RequestUri == expectedUri // to this uri
                    ),
                    ItExpr.IsAny<CancellationToken>()
                );
            }

            [TestCase("https://jsonplaceholder.typicode.com/todos/1")]
            public async Task When_MockServiceGet_Then_ShouldReturnString(string url)
            {
                // ARRANGE
                var handlerMock = GetMockMessageHandler();

                // use real http client with mocked handler here
                var httpClient = new HttpClient(handlerMock.Object);
                var subjectUnderTest = new Common.RestClient.RestClient(httpClient);

                // ACT
                var result = await subjectUnderTest.Get(url);

                // ASSERT
                result.Should().NotBeNull(); 
                
            }

            [TestCase("https://jsonplaceholder.typicode.com/todos/error")]
            public async Task When_IncorrectUrl_Then_ShouldReturn404(string url)
            {
                // ARRANGE
                var httpClient = new HttpClient();
                var subjectUnderTest = new Common.RestClient.RestClient(httpClient);

                // ACT
                Exception ex = Assert.ThrowsAsync<HttpRequestException>(() =>  subjectUnderTest.Get<ToDo>(url));

                // ASSERT
                StringAssert.Contains("404", ex.Message);
            }
            #endregion

            #region ***** Test Post *****
            [TestCase("https://jsonplaceholder.typicode.com/todos/1")]
            public async Task When_MockServicePostHttpContent_Then_ShouldReturnObject(string url)
            {
                // ARRANGE
                var handlerMock = GetMockMessageHandler();

                var values = new Dictionary<string, string>
                {
                    { "prop1", "Test prop1" },
                    { "prop2", "Test prop2" }
                };

                var content = new FormUrlEncodedContent(values);

                // use real http client with mocked handler here
                var httpClient = new HttpClient(handlerMock.Object);
                var subjectUnderTest = new Common.RestClient.RestClient(httpClient);

                // ACT
                MockResult result = await subjectUnderTest.Post<MockResult>(url,content);

                // ASSERT
                result.Should().NotBeNull(); // this is fluent assertions here...
                result.Id.Should().Be(1);

                // also check the 'http' call was like we expected it
                var expectedUri = new Uri(url);

                handlerMock.Protected().Verify(
                    "SendAsync",
                    Times.Exactly(1), // we expected a single external request
                    ItExpr.Is<HttpRequestMessage>(req =>
                            req.Method == HttpMethod.Post // we expected a GET request
                            && req.RequestUri == expectedUri // to this uri
                    ),
                    ItExpr.IsAny<CancellationToken>()
                );
            }

            [TestCase("https://jsonplaceholder.typicode.com/todos/1")]
            public async Task When_MockServicePostObject_Then_ShouldReturnObject(string url)
            {
                // ARRANGE
                var handlerMock = GetMockMessageHandler();

                var values = new Dictionary<string, string>
                {
                    { "prop1", "Test prop1" },
                    { "prop2", "Test prop2" }
                };

                // use real http client with mocked handler here
                var httpClient = new HttpClient(handlerMock.Object);
                var subjectUnderTest = new Common.RestClient.RestClient(httpClient);

                // ACT
                var result = await subjectUnderTest.Post(url,values);

                // ASSERT
                result.Should().NotBeNull(); // this is fluent assertions here...

                // also check the 'http' call was like we expected it
                var expectedUri = new Uri(url);

                handlerMock.Protected().Verify(
                    "SendAsync",
                    Times.Exactly(1), // we expected a single external request
                    ItExpr.Is<HttpRequestMessage>(req =>
                            req.Method == HttpMethod.Post // we expected a GET request
                            && req.RequestUri == expectedUri // to this uri
                    ),
                    ItExpr.IsAny<CancellationToken>()
                );
            }
           
            [TestCase("https://jsonplaceholder.typicode.com/todos/1")]
            public async Task When_MockServicePostList_Then_ShouldReturnObject(string url)
            {
                // ARRANGE
                var handlerMock = GetMockMessageHandler();

                var testList = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("Prop1", "1"),
                    new KeyValuePair<string, string>("Prop2", "2"),
                    new KeyValuePair<string, string>("Prop3", "3")
                };

                // use real http client with mocked handler here
                var httpClient = new HttpClient(handlerMock.Object);
                var subjectUnderTest = new Common.RestClient.RestClient(httpClient);

                // ACT
                var result = await subjectUnderTest.PostFormUrlEncodedContent<MockResult>(url,testList);

                // ASSERT
                result.Should().NotBeNull(); // this is fluent assertions here...

                // also check the 'http' call was like we expected it
                var expectedUri = new Uri(url);

                handlerMock.Protected().Verify(
                    "SendAsync",
                    Times.Exactly(1), // we expected a single external request
                    ItExpr.Is<HttpRequestMessage>(req =>
                            req.Method == HttpMethod.Post // we expected a GET request
                            && req.RequestUri == expectedUri // to this uri
                    ),
                    ItExpr.IsAny<CancellationToken>()
                );
            }
            #endregion

            #region ***** Test Delete *****
            [TestCase("https://jsonplaceholder.typicode.com/todos/1")]
            public async Task When_MockServiceDelete_Then_ResponseOK(string url)
            {
                // ARRANGE
                var handlerMock = GetMockMessageHandler();

                // use real http client with mocked handler here
                var httpClient = new HttpClient(handlerMock.Object);
                var subjectUnderTest = new Common.RestClient.RestClient(httpClient);

                // ACT
                var result = await subjectUnderTest.Delete<MockResult>(url);

                // ASSERT
                result.Should().NotBeNull(); 
               

                // also check the 'http' call was like we expected it
                var expectedUri = new Uri(url);

                handlerMock.Protected().Verify(
                    "SendAsync",
                    Times.Exactly(1), // we expected a single external request
                    ItExpr.Is<HttpRequestMessage>(req =>
                            req.Method == HttpMethod.Delete // we expected a GET request
                            && req.RequestUri == expectedUri // to this uri
                    ),
                    ItExpr.IsAny<CancellationToken>()
                );
            }
            #endregion

            #region ***** Test Patch *****
            [TestCase("https://jsonplaceholder.typicode.com/todos/1")]
            public async Task When_MockServicePatch_Then_ResponseOK(string url)
            {
                // ARRANGE
                var handlerMock = GetMockMessageHandler();

                // use real http client with mocked handler here
                var httpClient = new HttpClient(handlerMock.Object);
                var subjectUnderTest = new Common.RestClient.RestClient(httpClient);

                var testList = new List<KeyValuePair<string, string>>{
                    new KeyValuePair<string, string>("Prop1", "1"),
                    new KeyValuePair<string, string>("Prop2", "2"),
                    new KeyValuePair<string, string>("Prop3", "3")
                };

                // ACT
                var result = await  subjectUnderTest.Patch<MockResult>(url,testList);

                // ASSERT
                result.Should().NotBe(null);
                
                // also check the 'http' call was like we expected it
                var expectedUri = new Uri(url);

                handlerMock.Protected().Verify(
                    "SendAsync",
                    Times.Exactly(1), // we expected a single external request
                    ItExpr.Is<HttpRequestMessage>(req =>
                            req.Method == HttpMethod.Patch // we expected a GET request
                            && req.RequestUri == expectedUri // to this uri
                    ),
                    ItExpr.IsAny<CancellationToken>()
                );
            }
            #endregion 
          
            #region ***** Test for Debugging only as they connect to real Api   *****
              
            [TestCase("https://jsonplaceholder.typicode.com/todos/1")]
            [Ignore("Connects to external API for debug use only")]
            public async Task When_ServiceGet_Then_ShouldReturnRow(string url)
            {
                // ARRANGE
                var httpClient = new HttpClient();
                var subjectUnderTest = new Common.RestClient.RestClient(httpClient);

                // ACT
                var result = await subjectUnderTest.Get<ToDo>(url);

                // ASSERT
                result.Should().NotBeNull();
            }

            [TestCase("https://jsonplaceholder.typicode.com/todos")]
            [Ignore("Connects to external API for debug use only")]
            public async Task When_ServiceGetMany_Then_ShouldReturnMultiplRows(string url)
            {
                // ARRANGE
                var httpClient = new HttpClient();
                var subjectUnderTest = new Common.RestClient.RestClient(httpClient);

                // ACT
                var result = await subjectUnderTest.Get<IList<ToDo>>(url);

                // ASSERT
                result.Should().NotBeNull()
                    .And
                    .HaveCountGreaterOrEqualTo(2);
            }

            [TestCase("https://dev.api.nationalcareersservice.org.uk/GetAllSkills/Execute/")]
            [Ignore("Connects to external API for debug use only")]
            public async Task When_ServiceGetWithHeader_Then_ShouldReturnRows(string url)
            {
                // ARRANGE
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "8ed8640b25004e26992beb9164d95139");
                var subjectUnderTest = new Common.RestClient.RestClient(httpClient);

                // ACT
                var result = await subjectUnderTest.Get<Skills>(url);
                
                // ASSERT
                 result.Should().NotBeNull();
            }
            
            #endregion
            
        }

        #region ***** Classes used by tests *****

        public class SkillsList
        {
            public STSkill[] Skills { get; set; }
        }

        public class STSkill
        {
            public string Uri { get; set; }
            public string Skill { get; set; }
            public string SkillType { get; set; }
            public string[] AlternativeLabels { get; set; }
        }


        public class ToDo
        {
            public int UserId { get; set; }
            public int Id { get; set; }
            public string Title { get; set; }
            public bool Completed { get; set; }
        }


        public class MockResult
        {
            public int Id { get; set; }
            public string Value { get; set; }
        }

        #endregion


        private static Mock<HttpMessageHandler> GetMockMessageHandler()
        {
            var handlerMock =  new Mock<HttpMessageHandler>(MockBehavior.Loose);
            handlerMock
                .Protected()
                // Setup the PROTECTED method to mock
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )

                // prepare the expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{'Id':1,'Value':'1'}"),
                })
                .Verifiable();
            return handlerMock;
        }
    

    }
}