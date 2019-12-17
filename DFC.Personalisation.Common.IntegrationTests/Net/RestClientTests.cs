using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using DFC.Personalisation.Common.Net.RestClient;
using System.Threading;
using System.Threading.Tasks;
using DFC.Personalisation.Common;
using FluentAssertions;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace DFC.Personalisation.Common.IntegrationTests.Net
{
    public class RestClientTests
    {

          #region 
            private const string _ocpApimSubscriptionKeyHeader = "Ocp-Apim-Subscription-Key";

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
                httpClient.DefaultRequestHeaders.Add("_ocpApimSubscriptionKeyHeader", "8ed8640b25004e26992beb9164d95139");
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
                var result = await subjectUnderTest.Get<SkillsList>(url,"8ed8640b25004e26992beb9164d95139");
                
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
