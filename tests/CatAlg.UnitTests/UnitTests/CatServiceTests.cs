using CatAlg.Domain.Interfaces;
using CatAlg.Domain.Models;
using CatAlg.Domain.Services;
using FluentAssertions;
using Moq;
using RestSharp;
using Xunit;

namespace CatAlg.UnitTests.UnitTests
{
    public class CatServiceTests
    {

        private readonly Mock<IHeadActions> _headActionsMock;
        private readonly Mock<IPawsActions> _pawsActionsMock;
        private readonly Mock<IProviderActions> _providerActionsMock;
        private readonly Mock<ICatRepository> _catRepository;
        private CatService _catService;

        public CatServiceTests()
        {
            _headActionsMock = new Mock<IHeadActions>();
            _pawsActionsMock = new Mock<IPawsActions>();
            _providerActionsMock = new Mock<IProviderActions>();
            _catRepository = new Mock<ICatRepository>();
            _catService = new CatService("xpto", _headActionsMock.Object, _pawsActionsMock.Object, _providerActionsMock.Object, _catRepository.Object);
        }
        
        [Fact]
        public void IsHungry_GivenDefaultCat_ShouldReturnAsExpected()
        {
            _catService.IsHungry().Should().BeTrue();
        }

        [Fact]
        public void IsSatisfied_GivenDefaultCat_ShouldReturnAsExpected()
        {
            _catService.IsSatisfied().Should().BeFalse();
        }
        
        [Fact]
        public void ShouldIRun_GivenDefaultCat_ShouldReturnAsExpected()
        {
            _catService.ShouldIRun().Should().BeFalse();
        }

        [Fact]
        public void BeCarried_GivenDefaultCatAndSingleCarry_ShouldBeUnsatisfied()
        {
            _catService.BeCarried();
            _catService.IsSatisfied().Should().BeFalse();
        }
        
        [Fact]
        public void BeCarried_GivenDefaultCatAndMultipleCarries_ShouldBeUnsatisfied()
        {
            DisturbTheCat(5, true);
            _catService.IsSatisfied().Should().BeFalse();
        }
        
        [Fact]
        public void ScratchBelly_GivenDefaultCatAndSingleScratch_ShouldBeUnsatisfied()
        {
            _catService.BeBellyScratched();
            _catService.IsSatisfied().Should().BeFalse();
        }
        
        [Fact]
        public void ScratchBelly_GivenDefaultCatAndMultipleScratches_ShouldBeUnsatisfied()
        {
            DisturbTheCat(5, false, true);
            _catService.IsSatisfied().Should().BeFalse();
        }

        [Fact]
        public void ShouldIRun_GivenExtremelyCarriedAndBellyScratchedCat_IDefinitelyShouldRun()
        {
            DisturbTheCat(5000, true, true);
            _catService.ShouldIRun().Should().BeTrue();
        }

        [Theory]
        [InlineData("Aurora", true)]
        [InlineData("Hanna", false)]
        [InlineData("Ravena", false)]
        [InlineData("xpto", false)]
        public void BeCarried_GivenSpecificCat_ShouldDoAsExpected(string catName, bool isSatisfied)
        {
            _catService = new CatService(catName, _headActionsMock.Object, _pawsActionsMock.Object,
                _providerActionsMock.Object, _catRepository.Object);
            
            DisturbTheCat(100, true);
            
            _catService.IsSatisfied().Should().Be(isSatisfied);
        }
        
        [Theory]
        [InlineData("Aurora", false)]
        [InlineData("Hanna", false)]
        [InlineData("Ravena", true)]
        [InlineData("xpto", false)]
        public void ScratchBelly_GivenSpecificCat_ShouldDoAsExpected(string catName, bool isSatisfied)
        {
            _catService = new CatService(catName, _headActionsMock.Object, _pawsActionsMock.Object,
                _providerActionsMock.Object, _catRepository.Object);
            
            DisturbTheCat(100, false, true);
            
            _catService.IsSatisfied().Should().Be(isSatisfied);
        }

        private void DisturbTheCat(int times, bool carry = false, bool scratchBelly = false)
        {
            for (var i = 0; i <= times; i++)
            {
                if(carry)
                    _catService.BeCarried();
                if(scratchBelly)
                    _catService.BeBellyScratched();
            }
        }
    }
}