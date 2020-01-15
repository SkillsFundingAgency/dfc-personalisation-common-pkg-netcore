using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DFC.Personalisation.Common.Net.RestClient;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace DFC.Personalisation.Common.IntegrationTests.Net
{
    public class RestClientTests
    {

          #region 
            private const string _ocpApimSubscriptionKeyHeader = "Ocp-Apim-Subscription-Key";
            private string _apiKey;
            private RestClient _subjectUnderTest;
            [OneTimeSetUp]
            public void Init()
            {
                var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();
                _apiKey = config.GetSection("ServiceTaxonomySettings").GetSection("ApiKey").Value;  
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
            
            [TestCase("https://dev.api.nationalcareersservice.org.uk/GetAllSkills/Execute/")]
            public async Task When_ServiceGetWithHeader_Then_ShouldReturnRows(string url)
            {
                _subjectUnderTest.DefaultRequestHeaders.Add(_ocpApimSubscriptionKeyHeader, _apiKey);
             
                // ACT
                var result = await _subjectUnderTest.GetAsync<SkillsList>(url);
                
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
                // ACT
                var result = await _subjectUnderTest.GetAsync<SkillsList>(url,_apiKey);
                
                // ASSERT
                result.Should().NotBeNull();
                
            }

            [TestCase("https://dev.api.nationalcareersservice.org.uk/GetOccupationsByLabel/Execute/?matchAltLabels=false")]
            public async Task When_ServicePostWithocpApimSubscriptionKey_Then_ShouldReturnObject(string url)
            {
                //ARANGE
                var postData = new StringContent("{ \"label\": \"writing\" }", Encoding.UTF8, "application/json");

                // ACT
                var result = await _subjectUnderTest.PostAsync<SkillsList>(url,postData,_apiKey);
                
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
