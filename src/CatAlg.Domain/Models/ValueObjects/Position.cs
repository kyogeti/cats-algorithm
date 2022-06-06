namespace CatAlg.Domain.Models.ValueObjects
{
    public enum Paw
    {
        FrontLeft, FrontRight, BackLeft, BackRight
    }
    public class Position
    {
        public Paw Paw { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}