using CatAlg.Domain.Models;

namespace CatAlg.Domain.Interfaces
{
    public interface ICatRepository
    {
        void SaveCatStatus(Cat cat);
    }
}