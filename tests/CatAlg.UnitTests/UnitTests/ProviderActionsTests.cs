using System.Linq;
using AutoFixture;
using CatAlg.Domain.Actions;
using CatAlg.Domain.Interfaces;
using CatAlg.Domain.Models;
using CatAlg.Domain.Models.ValueObjects;
using FluentAssertions;
using Moq;
using RestSharp;
using Xunit;

namespace CatAlg.UnitTests.UnitTests
{
    public class ProviderActionsTests : BaseActionsTests<ProviderActions>
    {
        private readonly Mock<IProviderActions> _providerMock;
        private readonly Fixture _fixture;

        public ProviderActionsTests()
        {
            _fixture = new Fixture();
            _providerMock = new Mock<IProviderActions>();

            _providerMock.Setup(x => x.ProvideFood())
                .Returns(_fixture.Create<Food>());
            _providerMock.Setup(x => x.GetSleepStatus())
                .Returns(_fixture.Build<SleepStatus>().With(x=> x.Sleeping, true).Create());
            _providerMock.Setup(x => x.ProvideFurniture())
                .Returns(_fixture.CreateMany<Furniture>(2));
        }

        [Fact]
        public void ProvideFood_GivenExemplarProvider_ShouldReturnFood()
        {
            _providerMock.Setup(x => x.ProvideFood())
                .Returns(_fixture.Build<Food>().With(x => x.IsYummy, true).Create());
            var food = _providerMock.Object.ProvideFood();

            food.Should().NotBeNull();
            food.IsYummy.Should().BeTrue();
        }
        
        [Fact]
        public void ProvideFood_GivenBadProvider_ShouldReturnFood()
        {
            _providerMock.Setup(x => x.ProvideFood())
                .Returns(_fixture.Build<Food>().With(x => x.IsYummy, false).Create());
            var food = _providerMock.Object.ProvideFood();

            food.Should().NotBeNull();
            food.IsYummy.Should().BeFalse();
        }

        [Fact]
        public void GetSleepStatus_GivenSleepyProvider_ShouldBeSleeping()
        {
            var sleepStatus = _providerMock.Object.GetSleepStatus();
            sleepStatus.Sleeping.Should().BeTrue();
        }

        [Fact]
        public void GetSleepStatus_GivenCoffeeLoverProvider_ShouldBeAwaken()
        {
            _providerMock.Setup(x => x.GetSleepStatus())
                .Returns(_fixture.Build<SleepStatus>().With(x => x.Sleeping, false).Create());

            var sleepStatus = _providerMock.Object.GetSleepStatus();
            sleepStatus.Sleeping.Should().BeFalse();
        }

        [Fact]
        public void ProvideFurniture_GivenNotMinimalistProvider_ShouldProvideFurniture()
        {
            var furniture = _providerMock.Object.ProvideFurniture();

            furniture.Count().Should().BeGreaterThan(0);
        }
    }
}