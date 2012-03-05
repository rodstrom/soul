using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Soul
{
    public struct Circle
    {
        public Vector2 center;
        public float radius;

        public Circle(Vector2 position, Vector2 offset, float radius)
        {
            this.center = position + offset;
            this.radius = radius;
        }

        /*public bool Intersects(Rectangle rectangle)
        {
            this.v = new Vector2(MathHelper.Clamp(Center.X, rectangle.Left, rectangle.Right),
                                    MathHelper.Clamp(Center.Y, rectangle.Top, rectangle.Bottom));

            this.direction = Center - v;
            this.distanceSquared = direction.LengthSquared();

            return ((distanceSquared > 0) && (distanceSquared < Radius * Radius));
        }*/
    }
}
