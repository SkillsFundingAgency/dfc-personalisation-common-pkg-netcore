using System.Collections.Generic;
using System.Threading.Tasks;
using DFC.Personalisation.Common.Net.RestClient;
using FluentAssertions;
using NUnit.Framework;

namespace DFC.Personalisation.Common.IntegrationTests.Net
{
    public class RestClientTests
    {

          #region 
            private RestClient _subjectUnderTest;

            [OneTimeSetUp]
            public void Init()
            {
                _subjectUnderTest = new RestClient();
            }


            [TestCase("https://jsonplaceholder.typicode.com/todos/1")]
            public async Task When_ServiceGet_Then_ShouldReturnRow(string url)
            {
                // ACT
                var result = await _subjectUnderTest.GetAsync<ToDo>(url);

                // ASSERT
                result.Should().NotBeNull();
                result.Id.Should().NotBe(0);
                result.UserId.Should().NotBe(0);
                result.Title.Should().NotBe("");
                result.Completed.Should().BeFalse();
            }
            
            [TestCase("https://jsonplaceholder.typicode.com/todos")]
            public async Task When_ServiceGetMany_Then_ShouldReturnMultipleRows(string url)
            {
                // ACT
                var result = await _subjectUnderTest.GetAsync<IList<ToDo>>(url);

                // ASSERT
                result.Should().NotBeNull()
                    .And
                    .HaveCountGreaterOrEqualTo(2);
            }
            
            #endregion
    }



    #region ***** Classes used by tests *****
    class ToDo
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }
    }

    #endregion
}
