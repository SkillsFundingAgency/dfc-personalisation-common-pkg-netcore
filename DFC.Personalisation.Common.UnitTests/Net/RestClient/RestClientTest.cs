using System;

using System.Collections.Generic;

using System.Net;

using System.Net.Http;

using System.Threading;

using System.Threading.Tasks;

using DFC.Personalisation.Common.Net.RestClient;

using FluentAssertions;

using Moq;

using Moq.Protected;

using Newtonsoft.Json;

using NUnit.Framework;





namespace DFC.Personalisation.Common.UnitTests.Net

{



    public class RestClientTests

    {

    

        public class CallServiceAsyncTest

        {

            private Mock<HttpMessageHandler> _handlerMock;

            private RestClient _subjectUnderTest;

            [SetUp] 

            public void Init()

            {

                _handlerMock= GetMockMessageHandler();

                _subjectUnderTest = new RestClient(_handlerMock.Object);
                var apiResponse = new RestClient.APIResponse();
                apiResponse.StatusCode = HttpStatusCode.OK;
                apiResponse.Content = null;
                apiResponse.IsSuccess = true;
            }

            

            #region ***** Test Get *****

            [TestCase("https://jsonplaceholder.typicode.com/todos/1")]

            public async Task When_MockServiceGet_Then_ShouldReturnObject(string url)

            {

                // ACT

                MockResult result = await _subjectUnderTest.GetAsync<MockResult>(url);



                // ASSERT

                result.Should().NotBeNull(); // this is fluent assertions here...

                result.Id.Should().Be(1);



                // also check the 'http' call was like we expected it

                var expectedUri = new Uri(url);



                _handlerMock.Protected().Verify(

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

            public async Task When_MockServiceGetWithocpApimSubscriptionKey_Then_ShouldReturnObject(string url)

            {

                // ACT

                HttpRequestMessage request = new HttpRequestMessage();

                var result = await _subjectUnderTest.GetAsync<MockResult>(url,request);



                // ASSERT

                result.Should().NotBeNull(); // this is fluent assertions here...

                result.Id.Should().Be(1);



                // also check the 'http' call was like we expected it

                var expectedUri = new Uri(url);



                _handlerMock.Protected().Verify(

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

                

                // ACT

                var result = await _subjectUnderTest.GetAsync(url);



                // ASSERT

                result.Should().NotBeNull(); 

                

            }



            #endregion



            #region ***** Test Post *****

            [TestCase("https://jsonplaceholder.typicode.com/todos/1")]

            public async Task When_MockServicePostHttpContent_Then_ShouldReturnObject(string url)

            {

                //ARRANGE

                var values = new Dictionary<string, string>

                {

                    { "prop1", "Test prop1" },

                    { "prop2", "Test prop2" }

                };



                var content = new FormUrlEncodedContent(values);



                // ACT

                var result = await _subjectUnderTest.PostAsync<MockResult>(url,content);



                // ASSERT

                result.Should().NotBeNull(); // this is fluent assertions here...

                result.Id.Should().Be(1);

                result.Value.Should().Be("1");



                // also check the 'http' call was like we expected it

                var expectedUri = new Uri(url);



                _handlerMock.Protected().Verify(

                    "SendAsync",

                    Times.Exactly(1), // we expected a single external request

                    ItExpr.Is<HttpRequestMessage>(req =>

                            req.Method == HttpMethod.Post // we expected a POST request

                            && req.RequestUri == expectedUri // to this uri

                    ),

                    ItExpr.IsAny<CancellationToken>()

                );

            }



            [TestCase("https://jsonplaceholder.typicode.com/todos/1")]

            public async Task When_MockServicePostWithocpApimSubscriptionKey_Then_ShouldReturnObject(string url)

            {

                // ARRANGE

                var values = new Dictionary<string, string>

                {

                    { "prop1", "Test prop1" },

                    { "prop2", "Test prop2" }

                };



                var content = new FormUrlEncodedContent(values);

                // ACT

                var result = await _subjectUnderTest.PostAsync<MockResult>(url,content,"");



                // ASSERT

                result.Should().NotBeNull(); // this is fluent assertions here...

                result.Id.Should().Be(1);



                // also check the 'http' call was like we expected it

                var expectedUri = new Uri(url);



                _handlerMock.Protected().Verify(

                    "SendAsync",

                    Times.Exactly(1), // we expected a single external request

                    ItExpr.Is<HttpRequestMessage>(req =>

                            req.Method == HttpMethod.Post // we expected a POST request

                            && req.RequestUri == expectedUri // to this uri

                    ),

                    ItExpr.IsAny<CancellationToken>()

                );

            }



            [TestCase("https://jsonplaceholder.typicode.com/todos/1")]

            public async Task When_MockServicePostObject_Then_ShouldReturnObject(string url)

            {

                //ARRANGE

                var values = new Dictionary<string, string>

                {

                    { "prop1", "Test prop1" },

                    { "prop2", "Test prop2" }

                };



                // ACT

                var result = await _subjectUnderTest.PostAsync(url,values);



                // ASSERT

                result.Should().NotBeNull(); // this is fluent assertions here...



                // also check the 'http' call was like we expected it

                var expectedUri = new Uri(url);



                _handlerMock.Protected().Verify(

                    "SendAsync",

                    Times.Exactly(1), // we expected a single external request

                    ItExpr.Is<HttpRequestMessage>(req =>

                            req.Method == HttpMethod.Post // we expected a POST request

                            && req.RequestUri == expectedUri // to this uri

                    ),

                    ItExpr.IsAny<CancellationToken>()

                );

            }

           

            [TestCase("https://jsonplaceholder.typicode.com/todos/1")]

            public async Task When_MockServicePostList_Then_ShouldReturnObject(string url)

            {

                //ARRANGE
                var testList = new List<KeyValuePair<string, string>>

                {

                    new KeyValuePair<string, string>("Prop1", "1"),

                    new KeyValuePair<string, string>("Prop2", "2"),

                    new KeyValuePair<string, string>("Prop3", "3")

                };



                // ACT

                var result = await _subjectUnderTest.PostFormUrlEncodedContentAsync<MockResult>(url,testList);



                // ASSERT

                result.Should().NotBeNull(); 



                // also check the 'http' call was like we expected it

                var expectedUri = new Uri(url);



                _handlerMock.Protected().Verify(

                    "SendAsync",

                    Times.Exactly(1), // we expected a single external request

                    ItExpr.Is<HttpRequestMessage>(req =>

                            req.Method == HttpMethod.Post // we expected a POST request

                            && req.RequestUri == expectedUri // to this uri

                    ),

                    ItExpr.IsAny<CancellationToken>()

                );

            }

            

            [TestCase("https://jsonplaceholder.typicode.com/todos/1")]

            public async Task When_ServicePostWithHtpRequestMessage_Then_ShouldReturnObject(string url)

            {

                var request = new HttpRequestMessage();

                

                request.Headers.Add("Ocp-Apim-Subscription-Key", "");

                request.Headers.Add("version", "v1");

                // ACT

                var result = await _subjectUnderTest.PostAsync<MockResult>("https://dev.api.nationalcareersservice.org.uk/discover-skills-and-careers/assessment/short",request);

                

                // ASSERT

                result.Should().NotBeNull();

                

            }



            

            

            #endregion


            #region ***** Test Exception *****

            [TestCase("https://jsonplaceholder.typicode.com/todos/1")]

            public async Task When_MockServicePutHttpContentWithError_Then_ShouldThrowException(string url)

            {

                var _handlerMockError= GetMockMessageHandlerError();

                var subjectUnderTestError = new RestClient(_handlerMockError.Object);
                
                //ARRANGE

                using var content = new StringContent("{'Id':1'Value//{':'1'}", System.Text.Encoding.UTF8, "application/json");



                // ACT

                var result = await subjectUnderTestError.PutAsync<MockResult>(url,content);



                // ASSERT

                result.Should().NotBeNull(); // this is fluent assertions here...



                // also check the 'http' call was like we expected it

                var expectedUri = new Uri(url);



                _handlerMock.Protected().Verify(

                    "SendAsync",

                    Times.Exactly(1), // we expected a single external request

                    ItExpr.Is<HttpRequestMessage>(req =>

                            req.Method == HttpMethod.Put // we expected a GET request

                            && req.RequestUri == expectedUri // to this uri

                    ),

                    ItExpr.IsAny<CancellationToken>()

                );

            }

            #endregion


            #region ***** Test Put *****

            [TestCase("https://jsonplaceholder.typicode.com/todos/1")]

            public async Task When_MockServicePutHttpContent_Then_ShouldReturnObject(string url)

            {

                //ARRANGE

                using var content = new StringContent("{'Id':1,'Value':'1'}", System.Text.Encoding.UTF8, "application/json");



                // ACT

                var result = await _subjectUnderTest.PutAsync<MockResult>(url,content);



                // ASSERT

                result.Should().NotBeNull(); // this is fluent assertions here...



                // also check the 'http' call was like we expected it

                var expectedUri = new Uri(url);



                _handlerMock.Protected().Verify(

                    "SendAsync",

                    Times.Exactly(1), // we expected a single external request

                    ItExpr.Is<HttpRequestMessage>(req =>

                            req.Method == HttpMethod.Put // we expected a GET request

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

                // ACT

                var result = await _subjectUnderTest.DeleteAsync<MockResult>(url);



                // ASSERT

                result.Should().NotBeNull();

                

                // also check the 'http' call was like we expected it

                var expectedUri = new Uri(url);



                _handlerMock.Protected().Verify(

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

                //ARRANGE

                var testList = new List<KeyValuePair<string, string>>{

                    new KeyValuePair<string, string>("Prop1", "1"),

                    new KeyValuePair<string, string>("Prop2", "2"),

                    new KeyValuePair<string, string>("Prop3", "3")

                };



                // ACT

                var result = await  _subjectUnderTest.PatchAsync<MockResult>(url,testList);



                // ASSERT

                result.Should().NotBe(null);

                

                // also check the 'http' call was like we expected it

                var expectedUri = new Uri(url);



                _handlerMock.Protected().Verify(

                    "SendAsync",

                    Times.Exactly(1), // we expected a single external request

                    ItExpr.Is<HttpRequestMessage>(req =>

                            req.Method == HttpMethod.Patch // we expected a GET request

                            && req.RequestUri == expectedUri // to this uri

                    ),

                    ItExpr.IsAny<CancellationToken>()

                );

            }

            

            [TestCase("https://jsonplaceholder.typicode.com/posts/1")]

            public async Task When_ServicePatch_Then_ResponseOK(string url)

            {

                //ARRANGE

                var testList = new List<KeyValuePair<string, string>>{

                    new KeyValuePair<string, string>("title", "foo")

                };



                // ACT

                var result = await  _subjectUnderTest.PatchAsync<MockResult>(url,testList);



                // ASSERT

                result.Should().NotBe(null);

                

                // also check the 'http' call was like we expected it

                var expectedUri = new Uri(url);



                _handlerMock.Protected().Verify(

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

          

            #region ***** Test Response Class *****



            [TestCase("https://jsonplaceholder.typicode.com/todos/error")]

            public async Task When_MockApiCall_LastResponseNotNull(string url)

            {

                

                // ACT

                await _subjectUnderTest.GetAsync<MockResult>(url);



                // ASSERT

                _subjectUnderTest.LastResponse.Should().NotBe(null);

            }

            #endregion

            

        }



        #region ***** Classes used by tests *****



        class MockResult

        {

            public int Id { get; set; }

            public string Value { get; set; }

        }



        #endregion

        public static Mock<HttpMessageHandler> GetMockMessageHandler()

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

                .ReturnsAsync(new HttpResponseMessage

                {

                    StatusCode = HttpStatusCode.OK,

                    Content = new StringContent("{'Id':1,'Value':'1'}")

                })

                .Verifiable();

            return handlerMock;

        }

        public static Mock<HttpMessageHandler> GetMockMessageHandlerError()

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

                .ReturnsAsync(new HttpResponseMessage

                {

                    StatusCode = HttpStatusCode.OK,

                    Content = new StringContent("{'Id:':1,'Value':'1'}")

                })

                .Verifiable();

            return handlerMock;

        }

    }

    public class AssessmentShortResponse

    {

        public string PartitionKey { get; set; }

        public string SessionId { get; set; }

        public string Salt { get; set; }

        public System.DateTime CreatedDate { get; set; }

    }

}