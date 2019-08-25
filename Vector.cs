using Medallion;
using System;


namespace raytrace
{
    class Vector
    {
        public double x;
        public double y;
        public double z;

        public Vector(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector Copy()
        {
            Vector vec = new Vector(this.x, this.y, this.z);
            return vec;
        }

        public void ScalarMultiply(double t)
        {
            x *= t;
            y *= t;
            z *= t;

        }

        public void ScalarDivide(double t)
        {
            x /= t;
            y /= t;
            z /= t;

        }

        public void Add(Vector v)
        {
            x += v.x;
            y += v.y;
            z += v.z;
        }

        public void Subtract(Vector v)
        {
            x -= v.x;
            y -= v.y;
            z -= v.z;
        }

        public static Vector InUnitSphere()
        {
            Vector p;
            p = new Vector(2 * Rand.NextDouble() - 1, 2 * Rand.NextDouble() - 1, 2 * Rand.NextDouble() - 1);

            while (Vector.Dist(new Vector(0, 0, 0), p) > 1.0)
            {
                p = new Vector(2 * Rand.NextDouble() - 1, 2 * Rand.NextDouble() - 1, 2 * Rand.NextDouble() - 1);
            }


            return p;
        }

        public static double Dot(Vector v1, Vector v2)
        {
            return (v1.x * v2.x + v1.y * v2.y + v1.z * v2.z);
        }

        public void Cross(Vector v)
        {
            x *= v.x;
            y *= v.y;
            z *= v.z;
        }

        public void Normalize()
        {
            double mag = Math.Sqrt(x * x + y * y + z * z);
            if (mag > 0)
            {
                x /= mag;
                y /= mag;
                z /= mag;
            }

        }

        public static double Dist(Vector v1, Vector v2)
        {
            double xdif = v2.x - v1.x;
            double ydif = v2.y - v1.y;
            double zdif = v2.z - v1.z;

            return Math.Sqrt(xdif * xdif + ydif * ydif + zdif * zdif);
        }




    }
}
