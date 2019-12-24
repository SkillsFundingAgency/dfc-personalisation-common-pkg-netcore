using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace DFC.Personalisation.Common.IntegrationTests.Net
{
    public class RestClientTests
    {

          #region 
            private const string _ocpApimSubscriptionKeyHeader = "Ocp-Apim-Subscription-Key";
            private string _ApiKey;
            [OneTimeSetUp]
            public void Init()
            {
                var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();
                _ApiKey = config.GetSection("ServiceTaxonomySettings").GetSection("ApiKey").Value;  
           

            }


            [TestCase("https://jsonplaceholder.typicode.com/todos/1")]
            public async Task When_ServiceGet_Then_ShouldReturnRow(string url)
            {
                // ARRANGE
                var httpClient = new HttpClient();
                var subjectUnderTest = new Common.Net.RestClient.RestClient(httpClient);

                // ACT
                var result = await subjectUnderTest.Get<ToDo>(url);

                // ASSERT
                result.Should().NotBeNull();
                result.Id.Should().NotBe(0);
                result.UserId.Should().NotBe(0);
                result.Title.Should().NotBe("");
                result.Completed.Should().BeFalse();
            }
            
            [TestCase("https://jsonplaceholder.typicode.com/todos")]
            public async Task When_ServiceGetMany_Then_ShouldReturnMultiplRows(string url)
            {
                // ARRANGE
                var httpClient = new HttpClient();
                var subjectUnderTest = new Common.Net.RestClient.RestClient(httpClient);

                // ACT
                var result = await subjectUnderTest.Get<IList<ToDo>>(url);

                // ASSERT
                result.Should().NotBeNull()
                    .And
                    .HaveCountGreaterOrEqualTo(2);
            }
            
            [TestCase("https://dev.api.nationalcareersservice.org.uk/GetAllSkills/Execute/")]
            public async Task When_ServiceGetWithHeader_Then_ShouldReturnRows(string url)
            {
                // ARRANGE
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add(_ocpApimSubscriptionKeyHeader, _ApiKey);
                var subjectUnderTest = new Common.Net.RestClient.RestClient(httpClient);

                // ACT
                var result = await subjectUnderTest.Get<SkillsList>(url);
                
                // ASSERT
                result.Should().NotBeNull();
                result.Skills.Length.Should().BeGreaterThan(0);
                result.Skills[1].Uri.Should().NotBeEmpty();
                result.Skills[1].SkillType.Should().NotBeEmpty();
                result.Skills[1].Skill.Should().NotBeEmpty();
            }

            [TestCase("https://dev.api.nationalcareersservice.org.uk/GetAllSkills/Execute/")]
            public async Task When_ServiceGetWithocpApimSubscriptionKey_Then_ShouldReturnObject(string url)
            {
                // ARRANGE
                var httpClient = new HttpClient();
                var subjectUnderTest = new Common.Net.RestClient.RestClient(httpClient);

                // ACT
                var result = await subjectUnderTest.Get<SkillsList>(url,_ApiKey);
                
                // ASSERT
                result.Should().NotBeNull();
                
            }

          


            #endregion
    }



    #region ***** Classes used by tests *****
    class SkillsList
    {
        public STSkill[] Skills { get; set; }
    }

    class STSkill
    {
        public string Uri { get; set; }
        public string Skill { get; set; }
        public string SkillType { get; set; }
        public string[] AlternativeLabels { get; set; }
    }

    class ToDo
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }
    }
    class MockResult
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }


    #endregion
}
