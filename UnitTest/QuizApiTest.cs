using BackendLab01;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Identity.Client;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using WebAPI.DTO;
using Xunit;



namespace UnitTest
{
    public class QuizApiTest
    {
        [Fact]
        public async void GetShouldReturnTwoQuizzes()
        {
            //Arrange
            await using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            //Act
            var result = await client.GetFromJsonAsync<List<QuizDto>>("/api/v1/user/quizzes");

            //Assert
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async void GetShouldReturnOkStatus()
        {
            //Arrange
            await using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            //Act
            var result = await client.GetAsync("/api/v1/user/quizzes");

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Contains("application/json", result.Content.Headers.GetValues("Content-Type").First());
        }

        [Fact]
        public async void CreateQuiz()
        {
            //Arrange
            await using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            NewQuizDto testQuizDto = new NewQuizDto()
            {
                Title = "TestTitle"
            };

            var json = JsonSerializer.Serialize(testQuizDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri("https://localhost:7123/api/v1/admin/quizzes"),
                Method = HttpMethod.Post,
                Content = content
            };

            // Act
            var response = await client.SendAsync(request);

            // Assert 
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}