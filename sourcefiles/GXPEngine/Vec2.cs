using System;
using System.Drawing;
namespace GXPEngine
{
    public class Vec2
    {
        public static Vec2 zero { get { return new Vec2(0, 0); } }

        public float x = 0;
        public float y = 0;

        public Vec2(float pX = 0, float pY = 0) {
            x = pX;
            y = pY;
        }

        public override string ToString() {
            return String.Format("({0}, {1})", x, y);
        }

        public Vec2 Add(Vec2 other) {
            x += other.x;
            y += other.y;
            return this;
        }

        public Vec2 Substract(Vec2 other) {
            x -= other.x;
            y -= other.y;
            return this;
        }

        public Vec2 Scale(Vec2 other) {
            x *= other.x;
            y *= other.y;
            return this;
        }

        public double length() {
            return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        }

        public Vec2 Normalize() {
            float length = (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            x = (1 / length) * x;
            y = (1 / length) * y;
            return this;
        }

        public Vec2 Normal()
        {
            float dX = -1 * y;
            float dY = x;
            return new Vec2(dX, dY).Normalize();
        }

        public Vec2 Rotate(float deg)
        {
            deg = deg * Mathf.PI / 180;
            float sin = Mathf.Sin(deg);
            float cos = Mathf.Cos(deg);
            return new Vec2((x * cos) - (y * sin), (x * sin) + (y * cos));
        }

        public PointF ReturnPoint(Vec2 other)
        {
            return new PointF(other.x, other.y);
        }


        public Vec2 Clone()
        {
            Vec2 clone = new Vec2(x, y);
            return clone;
        }

        public float Dot(Vec2 other)
        {
            float dot = x * other.x + y * other.y;
            return dot;
        }

        //public override Vec2 SetXY(Vec2 other) {
        //    return new Vec2(other.x, other.y);
        //}
        
    }
}

