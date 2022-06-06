using System;
using AutoFixture;
using CatAlg.Domain.Actions;
using CatAlg.Domain.Interfaces;
using CatAlg.Domain.Models;
using CatAlg.Domain.Models.ValueObjects;
using CatAlg.Domain.Services;
using FluentAssertions;
using Moq;
using RestSharp;
using Xunit;

namespace CatAlg.UnitTests.IntegrationTests
{
    public class CatServiceTests : BaseIntegrationSetups
    {
        private readonly Mock<IHeadActions> _headActionsMock;
        private readonly Mock<IPawsActions> _pawsActionsMock;
        private readonly Fixture _fixture;
        private IProviderActions _providerActions;

        public CatServiceTests()
        {
            _headActionsMock = new Mock<IHeadActions>();
            _pawsActionsMock = new Mock<IPawsActions>();
            _fixture = new Fixture();
        }
        
        [Fact]
        public void SeekSatisfaction_GivenUnsatisfiedCatAndGoodProvider_ShouldDoAsExpected()
        {
            StartServer();
            _providerActions = new ProviderActions(new RestClient(GetServerUrl()));
            var catService = new CatService("xpto", _headActionsMock.Object, _pawsActionsMock.Object, _providerActions);
            SetupGetRequest("/api/sleep-status",
                _fixture.Build<SleepStatus>().With(x=> x.Sleeping, true).Create(), 
                200);
            SetupGetRequest("/api/food",
                _fixture.Build<Food>().With(x=> x.IsYummy, true).Create(), 
                200);
            SetupGetRequest("/api/furniture",
                _fixture.Build<Furniture>().With(x=> x.QualityStatus, Quality.New).CreateMany(5), 
                200);
            
            catService.SeekSatisfaction();

            _pawsActionsMock.Verify(x=> x.DoScratchFurniture(It.IsAny<Furniture>()), Times.AtLeastOnce);
            catService.IsHungry().Should().BeFalse();
            catService.ShouldIRun().Should().BeFalse();
            catService.IsSatisfied().Should().BeTrue();
            StopServer();
        }

        [Fact]
        public void Eat_GivenCatAndGoodProvider_ShouldEatAndBeSatisfied()
        {
            StartServer();
            _providerActions = new ProviderActions(new RestClient(GetServerUrl()));
            var catService = new CatService("xpto", _headActionsMock.Object, _pawsActionsMock.Object, _providerActions);
            SetupGetRequest("/api/sleep-status",
                _fixture.Build<SleepStatus>().With(x=> x.Sleeping, false).Create(), 
                200);
            SetupGetRequest("/api/food",
                _fixture.Build<Food>().With(x=> x.IsYummy, true).Create(), 
                200);
            
            catService.Eat(DateTime.Now);

            catService.IsHungry().Should().BeFalse();
            catService.ShouldIRun().Should().BeFalse();
            catService.IsSatisfied().Should().BeTrue();
            StopServer();
        }
        
        [Fact]
        public void Eat_GivenCatAndSleepyProvider_ShouldEatAndBeSatisfiedAfterALotOfMeows()
        {
            StartServer();
            _providerActions = new ProviderActions(new RestClient(GetServerUrl()));
            var catService = new CatService("xpto", _headActionsMock.Object, _pawsActionsMock.Object, _providerActions);
            SetupGetRequest("/api/sleep-status",
                _fixture.Build<SleepStatus>().With(x=> x.Sleeping, true).Create(), 
                200);
            SetupGetRequest("/api/food",
                _fixture.Build<Food>().With(x=> x.IsYummy, true).Create(), 
                200);
            
            catService.Eat(DateTime.Now);

            _headActionsMock.Verify(x=> x.DoMeow(It.IsAny<DateTime>()),
                Times.AtLeast(1));
            catService.IsHungry().Should().BeFalse();
            catService.ShouldIRun().Should().BeFalse();
            catService.IsSatisfied().Should().BeTrue();
            StopServer();
        }
    }
}