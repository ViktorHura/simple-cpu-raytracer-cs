using System.Collections.Generic;

namespace raytrace
{
    class Ray
    {
        public Vector origin;
        public Vector direction;
        public int depth;

        public Ray(Vector o, Vector d, int depth)
        {
            this.origin = o;
            this.direction = d;
            this.depth = depth;

        }

        public Vector Point(double t)
        {
            return new Vector(origin.x + t * direction.x, origin.y + t * direction.y, origin.z + t * direction.z);
        }

        public Vector Trace(ref List<Light> lights, ref List<Sobject> scene)
        {
            double minDist = double.PositiveInfinity;
            int minInt = -1;

            double minT = -1;

            Vector color = new Vector(125, 200, 250);
            this.depth++;



            for (int i = 0; i < scene.Count; i++)
            {
                bool hit = false;
                double t = 0;





                scene[i].CheckHit(this, ref hit, ref t);

                if (hit && t > 0)
                {
                    Vector hitp = Point(t);
                    double d = Vector.Dist(origin, hitp);




                    if (d < minDist)
                    {

                        minDist = d;
                        minInt = i;
                        minT = t;

                    }




                }


            }

            if (minInt > -1)
            {
                Vector hitpoint = Point(minT);
                color = scene[minInt].GetColor(ref lights, hitpoint, ref scene, this);
            }


            return color;
        }

    }
}
