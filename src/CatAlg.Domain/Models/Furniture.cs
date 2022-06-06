using System;

namespace CatAlg.Domain.Models
{
    public enum Quality
    {
        New,
        Scratched,
        Ruined
    }
    public class Furniture
    {
        public Quality QualityStatus { get; set; }
    }
}