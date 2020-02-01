using System;

namespace GGJ
{
    public class Pair<T1, T2>
    {
        public override bool Equals(object obj)
        {
            if (obj is Pair<T1, T2> otherPair)
            {
                return Item1.Equals(otherPair.Item1) && Item2.Equals(otherPair.Item2);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return $"({Item1}, {Item2})";
        }

        public Pair()
        {
            Item1 = default;
            Item2 = default;
        }

        public Pair(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        public T1 Item1 { get; set; }
        public T2 Item2 { get; set; }
    }
}