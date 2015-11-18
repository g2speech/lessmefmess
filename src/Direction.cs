namespace LessMefMess
{
    using System;

    public static class DirectionExtension
    {
        public static Direction Create(string value)
        {
            switch (value.ToLowerInvariant())
            {
                case "on":
                    return Direction.Ex;
                case "from":
                    return Direction.Im;
                default:
                    throw new ArgumentException();
            }
        }
    }

    public enum Direction
    {
        Ex,
        Im,
    }
}