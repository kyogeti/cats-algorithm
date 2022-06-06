using System.Collections.Generic;
using CatAlg.Domain.Models;
using CatAlg.Domain.Models.ValueObjects;

namespace CatAlg.Domain.Interfaces
{
    public interface IProviderActions
    {
        Food ProvideFood();
        SleepStatus GetSleepStatus();
        IEnumerable<Furniture> ProvideFurniture();
    }
}