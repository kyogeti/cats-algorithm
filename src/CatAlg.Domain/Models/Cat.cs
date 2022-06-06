using System;
using System.Collections.Generic;
using System.Linq;
using RestSharp.Extensions;

namespace CatAlg.Domain.Models
{
    public enum Humor
    {
        Satisfied,
        Unsatisfied,
        YouBetterRun
    }
    public class Cat
    {
        public string Name { get; }
        public Humor CurrentHumor { get; private set; }
        public bool IsHungry { get; private set; }

        public Cat(string name)
        {
            Name = name;
            CurrentHumor = Humor.Unsatisfied;
            IsHungry = true;
            SetupWillsCatLogic();
        }

        private void SetupWillsCatLogic()
        {
            var willsCatsNames = new List<string>
            {
                "Aurora",
                "Hanna",
                "Ravena"
            };
            
            if (willsCatsNames.Any(x=> x == Name))
            {
                CurrentHumor = Humor.Satisfied;
                IsHungry = false;
            }
        }

        public void MakeSatisfied()
        {
            CurrentHumor = Humor.Satisfied;
        }

        public void MakeUnsatisfied()
        {
            CurrentHumor = Humor.Unsatisfied;
        }

        public void MakeYouBetterRun()
        {
            CurrentHumor = Humor.YouBetterRun;
        }

        public void MakeHungry()
        {
            IsHungry = true;
        }

        public void UnmakeHungry()
        {
            IsHungry = false;
        }
    }
}