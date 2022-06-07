using AutoFixture;
using CatAlg.Domain.Models;
using CatAlg.Domain.Repository;
using Moq;
using Xunit;

namespace CatAlg.UnitTests.UnitTests
{
    public class CatRepositoryTests
    {
        private readonly Mock<IWillSmithDatabase> _database;
        private readonly Fixture _fixture;

        public CatRepositoryTests()
        {
            _database = new Mock<IWillSmithDatabase>();
            _database.Setup(x => x.Save(It.IsAny<object>()))
                .Returns(true);
            _fixture = new Fixture();
        }
        
        [Fact]
        public void SaveCatStatus_GivenCat_ShouldSave()
        {
            var repository = new CatRepository(_database.Object);
            var cat = _fixture.Create<Cat>();
            repository.SaveCatStatus(cat);
            
            _database.Verify(x=> x.Save(cat), Times.Exactly(1));
        }
    }
}