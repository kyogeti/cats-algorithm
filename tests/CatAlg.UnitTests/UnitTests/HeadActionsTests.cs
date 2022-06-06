using System;
using System.Configuration;
using System.Net;
using AutoFixture;
using CatAlg.Domain.Actions;
using CatAlg.Domain.Interfaces;
using CatAlg.Domain.Models;
using Moq;
using Moq.Protected;
using RestSharp;
using Xunit;

namespace CatAlg.UnitTests.UnitTests
{
    public class HeadActionsTests : BaseActionsTests<HeadActions>
    {

        private IHeadActions _headActions;
        private readonly Fixture _fixture;

        public HeadActionsTests()
        {
            _headActions = new HeadActions(LoggerMock.Object, RestClientMock.Object);
        }
        
        [Theory]
        [InlineData(true, 0, 1)]
        [InlineData(false, 1, 0)]
        public void DoEat_GivenFood_ShouldLogAsExpected(bool isYummy, int criticalCalls, int informationCalls)
        {
            var food = new Food()
            {
                IsYummy = isYummy
            };
            _headActions.DoEat(food);
            
            AssertInformationLogCalls(informationCalls);
            AssertCriticalLogCalls(criticalCalls);
        }

        [Fact]
        public void DoMeow_GivenConvenientHour_ShouldDoCuteMeow()
        {
            var hour = new DateTime(2022, 06, 3, 10, 10, 10);
            
            _headActions.DoMeow(hour);
            
            AssertInformationLogCalls(1);
        }
        
        [Theory]
        [InlineData(2, 0)]
        [InlineData(3, 1)]
        [InlineData(4, 1)]
        [InlineData(5, 1)]
        [InlineData(6, 1)]
        [InlineData(7, 0)]
        public void DoMeow_GivenInconvenientHour_ShouldDoKillerMeowAsExpected(int hour, int logCalls)
        {
            var date = new DateTime(2022, 06, 3, hour, 10, 10);
            
            _headActions.DoMeow(date);
            
            AssertCriticalLogCalls(logCalls);
        }

        [Fact]
        public void DoKillerLookWhileYouSleep_GivenProviderId_ShouldCallAsExpected()
        {
            var providerId = Guid.NewGuid();
            SetupOkRequest();
            
            _headActions.DoKillerLookWhileYouSleep(providerId);

            RestClientMock.Verify(x=> x.Execute(It.IsAny<IRestRequest>()), Times.Once);
            AssertWarningLogCalls(1);
        }
    }
}