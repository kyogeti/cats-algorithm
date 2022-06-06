using CatAlg.Domain.Interfaces;
using CatAlg.Domain.Models;

namespace CatAlg.Domain.Actions
{
    public class PawsActions : IPawsActions
    {
        public virtual void DoScratchFurniture(Furniture furniture)
        {
            switch (furniture.QualityStatus)
            {
                case Quality.New:
                    furniture.QualityStatus = Quality.Scratched;
                    break;
                case Quality.Scratched:
                    furniture.QualityStatus = Quality.Ruined;
                    break;
            }
        }
    }
}