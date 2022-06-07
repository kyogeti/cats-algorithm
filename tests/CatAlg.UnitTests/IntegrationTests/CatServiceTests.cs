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
        private readonly Fixture _fixture;

        public CatServiceTests()
        {
            _headActionsMock = new Mock<IHeadActions>();
            _pawsActionsMock = new Mock<IPawsActions>();
            _catRepository = new Mock<ICatRepository>();
            _fixture = new Fixture();
        }

        #region CatService only

        [Fact]
        public void SeekSatisfaction_GivenUnsatisfiedCatAndGoodProvider_ShouldHaveScratchedAtLeastOnce()
        {
            StartServer();
            var catService = BuildCatService();
            SetupUnsatisfiedCatAndGoodProvider();

            catService.SeekSatisfaction();

            _pawsActionsMock.Verify(x=> x.DoScratchFurniture(It.IsAny<Furniture>()), Times.AtLeastOnce);
            StopServer();
        }
        
        [Fact]
        public void SeekSatisfaction_GivenUnsatisfiedCatAndGoodProvider_ShouldNotBeHungry()
        {
            StartServer();
            var catService = BuildCatService();
            SetupUnsatisfiedCatAndGoodProvider();

            catService.SeekSatisfaction();
            
            catService.IsHungry().Should().BeFalse();
            StopServer();
        }
        
        [Fact]
        public void SeekSatisfaction_GivenUnsatisfiedCatAndGoodProvider_IShouldNotRun()
        {
            StartServer();
            var catService = BuildCatService();
            SetupUnsatisfiedCatAndGoodProvider();

            catService.SeekSatisfaction();
            
            catService.ShouldIRun().Should().BeFalse();
            StopServer();
        }
        
        [Fact]
        public void SeekSatisfaction_GivenUnsatisfiedCatAndGoodProvider_ShouldBeSatisfied()
        {
            StartServer();
            var catService = BuildCatService();
            SetupUnsatisfiedCatAndGoodProvider();

            catService.SeekSatisfaction();
            
            catService.IsSatisfied().Should().BeTrue();
            StopServer();
        }

        [Fact]
        public void Eat_GivenCatAndGoodProvider_ShouldNotBeHungry()
        {
            StartServer();
            var catService = BuildCatService();
            SetupGoodProvider();
            
            catService.Eat(DateTime.Now);

            catService.IsHungry().Should().BeFalse();
            StopServer();
        }
        
        [Fact]
        public void Eat_GivenCatAndGoodProvider_IShouldNotRun()
        {
            StartServer();
            var catService = BuildCatService();
            SetupGoodProvider();
            
            catService.Eat(DateTime.Now);

            catService.ShouldIRun().Should().BeFalse();
            StopServer();
        }
        
        [Fact]
        public void Eat_GivenCatAndGoodProvider_ShouldBeSatisfied()
        {
            StartServer();
            var catService = BuildCatService();
            SetupGoodProvider();
            
            catService.Eat(DateTime.Now);

            catService.IsSatisfied().Should().BeTrue();
            StopServer();
        }
        
        [Fact]
        public void Eat_GivenCatAndSleepyProvider_ShouldBeSatisfied()
        {
            StartServer();
            var catService = BuildCatService();
            SetupSleepyProvider();

            catService.Eat(DateTime.Now);

            catService.IsSatisfied().Should().BeTrue();
            StopServer();
        }
        
        [Fact]
        public void Eat_GivenCatAndSleepyProvider_ShouldMeowAtLeastOneTime()
        {
            StartServer();
            var catService = BuildCatService();
            SetupSleepyProvider();

            catService.Eat(DateTime.Now);

            _headActionsMock.Verify(x=> x.DoMeow(It.IsAny<DateTime>()),
                Times.AtLeast(1));
            StopServer();
        }
        
        [Fact]
        public void Eat_GivenCatAndSleepyProvider_ShouldNotBeHungry()
        {
            StartServer();
            var catService = BuildCatService();
            SetupSleepyProvider();

            catService.Eat(DateTime.Now);

            catService.IsHungry().Should().BeFalse();
            StopServer();
        }
        
        [Fact]
        public void Eat_GivenCatAndSleepyProvider_IShouldNotRun()
        {
            StartServer();
            var catService = BuildCatService();
            SetupSleepyProvider();

            catService.Eat(DateTime.Now);
            
            catService.ShouldIRun().Should().BeFalse();
            StopServer();
        }

        #endregion

        #region CatService + Database

        [Fact]
        public void SeekSatisfaction_GivenUnsatisfiedCatAndGoodProvider_WithDatabase_ShouldHaveScratchedAtLeastOnce()
        {
            StartServer();
            var catService = BuildCatServiceWithDatabase();
            SetupUnsatisfiedCatAndGoodProvider();

            catService.SeekSatisfaction();

            _pawsActionsMock.Verify(x=> x.DoScratchFurniture(It.IsAny<Furniture>()), Times.AtLeastOnce);
            StopServer();
        }
        
        [Fact]
        public void SeekSatisfaction_GivenUnsatisfiedCatAndGoodProvider_WithDatabase_ShouldNotBeHungry()
        {
            StartServer();
            var catService = BuildCatServiceWithDatabase();
            SetupUnsatisfiedCatAndGoodProvider();

            catService.SeekSatisfaction();
            
            catService.IsHungry().Should().BeFalse();
            StopServer();
        }
        
        [Fact]
        public void SeekSatisfaction_GivenUnsatisfiedCatAndGoodProvider_WithDatabase_IShouldNotRun()
        {
            StartServer();
            var catService = BuildCatServiceWithDatabase();
            SetupUnsatisfiedCatAndGoodProvider();

            catService.SeekSatisfaction();
            
            catService.ShouldIRun().Should().BeFalse();
            StopServer();
        }
        
        [Fact]
        public void SeekSatisfaction_GivenUnsatisfiedCatAndGoodProvider_WithDatabase_ShouldBeSatisfied()
        {
            StartServer();
            var catService = BuildCatServiceWithDatabase();
            SetupUnsatisfiedCatAndGoodProvider();

            catService.SeekSatisfaction();
            
            catService.IsSatisfied().Should().BeTrue();
            StopServer();
        }

        [Fact]
        public void Eat_GivenCatAndGoodProvider_WithDatabase_ShouldNotBeHungry()
        {
            StartServer();
            var catService = BuildCatServiceWithDatabase();
            SetupGoodProvider();
            
            catService.Eat(DateTime.Now);

            catService.IsHungry().Should().BeFalse();
            StopServer();
        }
        
        [Fact]
        public void Eat_GivenCatAndGoodProvider_WithDatabase_IShouldNotRun()
        {
            StartServer();
            var catService = BuildCatServiceWithDatabase();
            SetupGoodProvider();
            
            catService.Eat(DateTime.Now);

            catService.ShouldIRun().Should().BeFalse();
            StopServer();
        }
        
        [Fact]
        public void Eat_GivenCatAndGoodProvider_WithDatabase_ShouldBeSatisfied()
        {
            StartServer();
            var catService = BuildCatServiceWithDatabase();
            SetupGoodProvider();
            
            catService.Eat(DateTime.Now);

            catService.IsSatisfied().Should().BeTrue();
            StopServer();
        }
        
        [Fact]
        public void Eat_GivenCatAndSleepyProvider_WithDatabase_ShouldBeSatisfied()
        {
            StartServer();
            var catService = BuildCatServiceWithDatabase();
            SetupSleepyProvider();

            catService.Eat(DateTime.Now);

            catService.IsSatisfied().Should().BeTrue();
            StopServer();
        }
        
        [Fact]
        public void Eat_GivenCatAndSleepyProvider_WithDatabase_ShouldMeowAtLeastOneTime()
        {
            StartServer();
            var catService = BuildCatServiceWithDatabase();
            SetupSleepyProvider();

            catService.Eat(DateTime.Now);

            _headActionsMock.Verify(x=> x.DoMeow(It.IsAny<DateTime>()),
                Times.AtLeast(1));
            StopServer();
        }
        
        [Fact]
        public void Eat_GivenCatAndSleepyProvider_WithDatabase_ShouldNotBeHungry()
        {
            StartServer();
            var catService = BuildCatServiceWithDatabase();
            SetupSleepyProvider();

            catService.Eat(DateTime.Now);

            catService.IsHungry().Should().BeFalse();
            StopServer();
        }
        
        [Fact]
        public void Eat_GivenCatAndSleepyProvider_WithDatabase_IShouldNotRun()
        {
            StartServer();
            var catService = BuildCatServiceWithDatabase();
            SetupSleepyProvider();

            catService.Eat(DateTime.Now);
            
            catService.ShouldIRun().Should().BeFalse();
            StopServer();
        }

        #endregion
        
    }
}