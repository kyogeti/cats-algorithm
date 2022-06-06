using CatAlg.Domain.Actions;
using CatAlg.Domain.Models;
using FluentAssertions;
using Xunit;

namespace CatAlg.UnitTests.UnitTests
{
    public class PawsActionsTests : BaseActionsTests<PawsActions>
    {
        [Fact]
        public void DoScratchFurniture_GivenNewFurniture_ShouldBeScratched()
        {
            var furniture = new Furniture()
            {
                QualityStatus = Quality.New
            };
            var pawsActions = new PawsActions();
            
            pawsActions.DoScratchFurniture(furniture);

            furniture.QualityStatus.Should().Be(Quality.Scratched);
        }
        
        [Fact]
        public void DoScratchFurniture_GivenScratchedFurniture_ShouldBeRuined()
        {
            var furniture = new Furniture()
            {
                QualityStatus = Quality.Scratched
            };
            var pawsActions = new PawsActions();
            
            pawsActions.DoScratchFurniture(furniture);

            furniture.QualityStatus.Should().Be(Quality.Ruined);
        }
        
        [Fact]
        public void DoScratchFurniture_GivenRuinedFurniture_ShouldBeRuined()
        {
            var furniture = new Furniture()
            {
                QualityStatus = Quality.Ruined
            };
            var pawsActions = new PawsActions();
            
            pawsActions.DoScratchFurniture(furniture);

            furniture.QualityStatus.Should().Be(Quality.Ruined);
        }
        
    }
}