using System;
using System.Linq;
using CatAlg.Domain.Interfaces;
using CatAlg.Domain.Models;

namespace CatAlg.Domain.Services
{
    public class CatService : ICatService
    {
        private readonly IHeadActions _headActions;
        private readonly IPawsActions _pawsActions;
        private readonly IProviderActions _providerActions;
        private Cat _cat;

        public CatService(string catName, IHeadActions headActions, IPawsActions pawsActions, IProviderActions providerActions)
        {
            _cat = new Cat(catName);
            _headActions = headActions;
            _pawsActions = pawsActions;
            _providerActions = providerActions;
        }

        public bool IsHungry() => _cat.IsHungry;
        public bool IsSatisfied() => _cat.CurrentHumor == Humor.Satisfied;
        public bool ShouldIRun() => _cat.CurrentHumor == Humor.YouBetterRun;

        public void SeekSatisfaction()
        {
            var furniture = _providerActions.ProvideFurniture().ToList();
            
            while (!IsSatisfied())
            {
                if (_providerActions.GetSleepStatus().Sleeping)
                {
                    if(IsHungry())
                        Eat(DateTime.Now);

                    if (furniture.Any())
                    {
                        furniture.ForEach(f =>
                        {
                            _pawsActions.DoScratchFurniture(f);
                            if (f.QualityStatus != Quality.New)
                                _cat.MakeSatisfied();
                        });
                    }
                }
            }
        }

        public void BeCarried()
        {
            if (_cat.Name == "Aurora") // because Aurora is the only cat that doesn't mind being carried
                return;

            switch (_cat.CurrentHumor)
            {
                case Humor.Satisfied:
                    _cat.MakeUnsatisfied();
                    break;
                case Humor.Unsatisfied:
                    _cat.MakeYouBetterRun();
                    break;
            }
        }

        public void BeBellyScratched()
        {
            if (_cat.Name == "Ravena") // because Ravena is the only cat that doesn't mind being scratched in the belly 
                return;

            switch (_cat.CurrentHumor)
            {
                case Humor.Satisfied:
                    _cat.MakeUnsatisfied();
                    break;
                case Humor.Unsatisfied:
                    _cat.MakeYouBetterRun();
                    break;
            }
        }

        public void Eat(DateTime hour)
        {
            if(_providerActions.GetSleepStatus().Sleeping)
            {
                _headActions.DoMeow(hour);
            }

            var food = _providerActions.ProvideFood();

            if (food.IsYummy)
            {
                _headActions.DoEat(food);
                _cat.MakeSatisfied();
                _cat.UnmakeHungry();
                return;
            }

            _cat.MakeYouBetterRun();
        }
    }
}