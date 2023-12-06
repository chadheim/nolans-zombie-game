using System;
using System.Collections.Generic;

namespace Assets.Core
{

    public class Vec2
    {
        public int x;
        public int y;

        public Vec2()
        {
            x = 0;
            y = 0;
        }

        public Vec2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Vec2(Vec2 v)
        {
            x = v.x;
            y = v.y;
        }

        public int Length => Math.Abs(x) + Math.Abs(y);

        public override bool Equals(object obj)
        {
            return obj is Vec2 vec && x == vec.x && y == vec.y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }

        public static bool operator ==(Vec2 left, Vec2 right)
        {
            return EqualityComparer<Vec2>.Default.Equals(left, right);
        }

        public static bool operator !=(Vec2 left, Vec2 right)
        {
            return !(left == right);
        }

        public static Vec2 operator -(Vec2 left, Vec2 right)
        {
            return new(left.x - right.x, left.y - right.y);
        }
    }
}