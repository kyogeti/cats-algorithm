using System;
using CatAlg.Domain.Models;

namespace CatAlg.Domain.Interfaces
{
    public interface IHeadActions
    {
        void DoEat(Food food);
        void DoMeow(DateTime inconvenientHour);
        void DoKillerLookWhileYouSleep(Guid providerId);
    }
}