using Sim.Const;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;

namespace Sim.Structs
{
    public struct Vec3d
    {
        public static readonly Vec3d Zero = new Vec3d(0, 0, 0);
        public static readonly Vec3d Up = new Vec3d(0, -1, 0);
        public static readonly Vec3d Right = new Vec3d(1, 0, 0);
        public static readonly Vec3d Down = new Vec3d(0, 1, 0);
        public static readonly Vec3d Left = new Vec3d(-1, 0, 0);

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        #region Constructors
        public Vec3d(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vec3d(Vec3d vec3d)
        {
            X = vec3d.X;
            Y = vec3d.Y;
            Z = vec3d.Z;
        }
        #endregion

        #region Functions
        public Vec3d Add(Vec3d vec) {
            X += vec.X;
            Y += vec.Y;
            Z += vec.Z;

            return this;
        }

        public Vec3d Subtract(Vec3d vec)
        {
            X -= vec.X;
            Y -= vec.Y;
            Z -= vec.Z;

            return this;
        }

        public Vec3d Multiply(Vec3d vec)
        {
            X *= vec.X;
            Y *= vec.Y;
            Z *= vec.Z;

            return this;
        }

        public Vec3d Divide(Vec3d vec)
        {
            X /= vec.X;
            Y /= vec.Y;
            Z /= vec.Z;

            return this;
        }

        public double Distance2d(Vec3d vec)
        {
            Vec3d delta = this - vec;
            return Math.Sqrt((delta.X * delta.X) + (delta.Y * delta.Y));
        }

        public Direction GetDirectionTo(Vec3d vec)
        {
            if (vec.Y < Y) return Direction.Up;
            if (vec.X > X) return Direction.Right;
            if (vec.Y > Y) return Direction.Down;
            if (vec.X < X) return Direction.Left;

            return Direction.None;
        }

        public IList<Vec3d> GetNeighbors()
        {
            return new List<Vec3d>() {
                this + Vec3d.Up,
                this + Vec3d.Right,
                this + Vec3d.Down,
                this + Vec3d.Left,
            };
        }
        #endregion

        #region Operators
        public static bool operator ==(Vec3d a, Vec3d b)
        {
            return a.X.GetHashCode() == b.X.GetHashCode() && a.Y.GetHashCode() == b.Y.GetHashCode() && a.Z.GetHashCode() == b.Z.GetHashCode();
        }

        public static bool operator !=(Vec3d a, Vec3d b)
        {
            return !(a == b);
        }

        public static Vec3d operator +(Vec3d vec1, Vec3d vec2)
        {
            return new Vec3d(
                vec1.X + vec2.X,
                vec1.Y + vec2.Y,
                vec1.Z + vec2.Z
            );
        }

        public static Vec3d operator -(Vec3d vec1, Vec3d vec2)
        {
            return new Vec3d(
                vec1.X - vec2.X,
                vec1.Y - vec2.Y,
                vec1.Z - vec2.Z
            );
        }

        public static Vec3d operator /(Vec3d vec1, Vec3d vec2)
        {
            return new Vec3d(
                vec1.X / vec2.X,
                vec1.Y / vec2.Y,
                vec1.Z / vec2.Z
            );
        }

        public static Vec3d operator *(Vec3d vec1, Vec3d vec2)
        {
            return new Vec3d(
                vec1.X * vec2.X,
                vec1.Y * vec2.Y,
                vec1.Z * vec2.Z
            );
        }

        public static Vec3d operator %(Vec3d vec1, Vec3d vec2)
        {
            return new Vec3d(
                vec1.X % vec2.X,
                vec1.Y % vec2.Y,
                vec1.Z % vec2.Z
            );
        }
        #endregion
    
        public static Vec3d FromDirection(Direction direction)
        {
            switch(direction)
            {
                case Direction.None: return new Vec3d(Zero);
                case Direction.Up: return new Vec3d(Up);
                case Direction.Right: return new Vec3d(Right);
                case Direction.Down: return new Vec3d(Down);
                case Direction.Left: return new Vec3d(Left);
                default: return new Vec3d(Zero);
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vec3d vec))
            {
                return false;
            }

            return this == vec;
        }

        public override string ToString()
        {
            return $"{X}x{Y}y{Z}z";
        }
    }
}
