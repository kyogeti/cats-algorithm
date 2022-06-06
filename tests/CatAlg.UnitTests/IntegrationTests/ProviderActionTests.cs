using AutoFixture;
using CatAlg.Domain.Actions;
using CatAlg.Domain.Models;
using FluentAssertions;
using Moq;
using RestSharp;
using Xunit;

namespace CatAlg.UnitTests.IntegrationTests
{
    public class ProviderActionTests : BaseIntegrationSetups
    {
        private readonly Fixture _fixture;

        public ProviderActionTests()
        {
            _fixture = new Fixture();
        }
        
        [Fact]
        public void ProvideFood_GivenAvailableProvider_ShouldReturnFood()
        {
            StartServer();
            SetupGetRequest("/api/food", _fixture.Create<Food>(), 200);
            var restClient = new RestClient(GetServerUrl());
            var provider = new ProviderActions(restClient);

            var result = provider.ProvideFood();
            result.Should().NotBeNull();
            StopServer();
        }
        
        [Fact]
        public void ProvideFood_GivenUnavailableProvider_ShouldThrow()
        {
            StartServer();
            SetupGetRequest("/api/food", "Service Unavailable", 503);
            var restClient = new RestClient(GetServerUrl());
            var provider = new ProviderActions(restClient);

            Assert.Throws<ProviderActions.ProviderNotRespondingException>(() =>
            {
                var result = provider.ProvideFood();
            });
            StopServer();
        } 
    }
}