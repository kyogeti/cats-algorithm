using CatAlg.Domain.Models;
using CatAlg.Domain.Models.ValueObjects;

namespace CatAlg.Domain.Interfaces
{
    public interface IPawsActions
    {
        void DoScratchFurniture(Furniture furniture);
    }
}