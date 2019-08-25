using System;
using System.Collections.Generic;

namespace raytrace
{
    class Sobject
    {
        public String type;
        public Vector position;
        public Vector color;
        public String material;
        public double width;
        public double height;
        public double depth;
        public double radius;


        public Sobject(String type, Vector position, Vector color, String material, double radius)
        {
            this.position = position;
            this.color = color;
            this.type = type;
            this.material = material;

            if (type == "sphere")
            {
                this.radius = radius;
            }
        }

        public Sobject(String type, Vector position, Vector color, String material, double width, double depth)
        {
            this.position = position;
            this.color = color;
            this.type = type;
            this.material = material;

            if (type == "planeu" || type == "planed")
            {
                this.width = width;
                this.depth = depth;
            }
            else if (type == "wallf" || type == "wallb")
            {
                this.width = width;
                this.height = depth;
            }
            else if (type == "sidewalll" || type == "sidewallr")
            {
                this.depth = depth;
                this.height = width;
            }
        }


        public Vector GetColor(ref List<Light> lights, Vector point, ref List<Sobject> scene, Ray incomingray)
        {

            if (incomingray.depth > 20)
            {
                return color;

            }


            Vector normalvector = point.Copy();

            if (type == "sphere")
            {
                normalvector.Subtract(position);
                normalvector.Normalize();
            }
            else if (type == "planeu")
            {
                normalvector = new Vector(0, 1, 0);

            }
            else if (type == "planed")
            {
                normalvector = new Vector(0, -1, 0);

            }
            else if (type == "wallf")
            {
                normalvector = new Vector(0, 0, -1);

            }
            else if (type == "wallb")
            {
                normalvector = new Vector(0, 0, 1);

            }
            else if (type == "sidewalll")
            {
                normalvector = new Vector(1, 0, 0);

            }
            else if (type == "sidewallr")
            {
                normalvector = new Vector(-1, 0, 0);

            }



            double furthestlight = Double.PositiveInfinity;

            for (int l = 0; l < lights.Count; l++)
            {
                double d = Vector.Dist(point, lights[l].position);

                if (d < furthestlight)
                {
                    furthestlight = d;
                }

            }

            Vector offset = normalvector.Copy();
            offset.ScalarDivide(100000);



            if (material == "matte")
            {

                Vector incomingvector = incomingray.direction.Copy();
                incomingvector.Normalize();

                Vector bouncedirection = normalvector.Copy();
                bouncedirection.ScalarMultiply(Vector.Dot(incomingvector, normalvector));
                bouncedirection.ScalarMultiply(-2);
                bouncedirection.Add(incomingvector);



                if (incomingray.depth > 100)
                {
                    return color;
                }

                Vector fuzzedvector = Vector.InUnitSphere();
                fuzzedvector.ScalarMultiply(1);

                bouncedirection.Add(fuzzedvector);


                Ray bounceray = new Ray(point, bouncedirection, incomingray.depth);



                bounceray.origin.Add(offset);



                Vector shadedcolor = new Vector(0, 0, 0);

                double lambert = 0;

                for (int l = 0; l < lights.Count; l++)
                {
                    Vector lightvector = lights[l].position.Copy();
                    lightvector.Subtract(point);
                    lightvector.Normalize();

                    Vector lightcolor = lights[l].color.Copy();
                    lightcolor.ScalarDivide(255);
                    lightcolor.ScalarMultiply(Vector.Dist(point, lights[l].position) / furthestlight);


                    shadedcolor.Add(lightcolor);

                    lambert += 1 * Vector.Dot(normalvector, lightvector);

                }

                shadedcolor.ScalarDivide(lights.Count);

                shadedcolor.Cross(color);

                lambert = lambert / lights.Count;


                if (lambert < 0)
                {
                    lambert = 0;
                }

                Vector reflectedcolor = bounceray.Trace(ref lights, ref scene);

                double reflectivity = 0.1;

                shadedcolor.x *= (1 - reflectivity);
                shadedcolor.y *= (1 - reflectivity);
                shadedcolor.z *= (1 - reflectivity);

                shadedcolor.x += (reflectedcolor.x * reflectivity);
                shadedcolor.y += (reflectedcolor.y * reflectivity);
                shadedcolor.z += (reflectedcolor.z * reflectivity);


                shadedcolor.ScalarMultiply(lambert);


                return shadedcolor;
            }
            else if (material == "mirror")
            {

                Vector incoming = incomingray.direction.Copy();
                incoming.Normalize();

                Vector reflecteddirection = normalvector.Copy();
                reflecteddirection.ScalarMultiply(Vector.Dot(incoming, normalvector));
                reflecteddirection.ScalarMultiply(-2);
                reflecteddirection.Add(incoming);



                Ray reflectedray = new Ray(point, reflecteddirection, incomingray.depth);



                reflectedray.origin.Add(offset);

                Vector shadedcolor = reflectedray.Trace(ref lights, ref scene);


                return shadedcolor;
            }
            else if (material == "metal")
            {
                Vector incoming = incomingray.direction.Copy();
                incoming.Normalize();

                Vector bouncedirection = normalvector.Copy();
                bouncedirection.ScalarMultiply(Vector.Dot(incoming, normalvector));
                bouncedirection.ScalarMultiply(-2);
                bouncedirection.Add(incoming);


                if (incomingray.depth > 100)
                {
                    return color;

                }

                Vector fuzzedvector = Vector.InUnitSphere();
                fuzzedvector.ScalarMultiply(0.15);

                bouncedirection.Add(fuzzedvector);


                Ray reflectedray = new Ray(point, bouncedirection, incomingray.depth);



                reflectedray.origin.Add(offset);

                Vector reflectioncolor = reflectedray.Trace(ref lights, ref scene);

                Vector shadedcolor = new Vector(0, 0, 0);


                double lambert = 0;



                for (int l = 0; l < lights.Count; l++)
                {
                    Vector lightvector = lights[l].position.Copy();
                    lightvector.Subtract(point);
                    lightvector.Normalize();

                    Vector lightcolor = lights[l].color.Copy();
                    lightcolor.ScalarDivide(255);
                    lightcolor.ScalarMultiply(Vector.Dist(point, lights[l].position) / furthestlight);

                    shadedcolor.Add(lightcolor);

                    lambert += 1 * Vector.Dot(normalvector, lightvector);

                }

                shadedcolor.ScalarDivide(lights.Count);

                shadedcolor.Cross(color);

                lambert = lambert / lights.Count;


                double reflectivity = 0.75;

                shadedcolor.x *= (1 - reflectivity);
                shadedcolor.y *= (1 - reflectivity);
                shadedcolor.z *= (1 - reflectivity);

                shadedcolor.x += (reflectioncolor.x * reflectivity);
                shadedcolor.y += (reflectioncolor.y * reflectivity);
                shadedcolor.z += (reflectioncolor.z * reflectivity);

                if (lambert < 0)
                {
                    lambert = 0;
                }

                shadedcolor.ScalarMultiply(lambert);


                return shadedcolor;

            }
            else if (material == "glass")
            {

                Vector shadedcolor = color.Copy();

                Vector normal = normalvector.Copy();
                double index1 = 1.7;
                double index2 = 1.0;


                double i_dot_n = Vector.Dot(incomingray.direction, normal);

                if (i_dot_n < 0)
                {
                    i_dot_n = -i_dot_n; //outside surface
                }
                else
                { //inside surface
                    normal.ScalarMultiply(-1);
                    index2 = index1;
                    index1 = 1.0;
                }

                double index = index1 / index2;

                double k = 1.0 - (index * index) * (1.0 - i_dot_n * i_dot_n);

                if (k < 0)
                {
                    Vector incoming = incomingray.direction.Copy();
                    incoming.Normalize();

                    Vector reflecteddirection = normalvector.Copy();
                    reflecteddirection.ScalarMultiply(Vector.Dot(incoming, normalvector));
                    reflecteddirection.ScalarMultiply(-2);
                    reflecteddirection.Add(incoming);



                    Ray reflectedray = new Ray(point, reflecteddirection, incomingray.depth);



                    reflectedray.origin.Add(offset);

                    shadedcolor = reflectedray.Trace(ref lights, ref scene);

                }
                else
                {

                    offset = normal.Copy();
                    offset.ScalarDivide(100000);

                    Vector refractionorigin = point.Copy();
                    refractionorigin.Subtract(offset);

                    Vector refractiondirection = incomingray.direction.Copy();

                    Vector a = normal.Copy();
                    a.ScalarMultiply(i_dot_n);

                    refractiondirection.Add(a);
                    refractiondirection.ScalarMultiply(index);

                    Vector b = normal.Copy();
                    b.ScalarMultiply(Math.Sqrt(k));

                    refractiondirection.Subtract(b);

                    Ray refractionray = new Ray(refractionorigin, refractiondirection, incomingray.depth);

                    shadedcolor = refractionray.Trace(ref lights, ref scene);


                }


                return shadedcolor;
            }
            else
            {
                return this.color;
            }
        }

        public void CheckHit(Ray r, ref bool hit, ref double t)
        {
            if (type == "sphere")
            {

                double a = (r.direction.x * r.direction.x) + (r.direction.y * r.direction.y) + (r.direction.z * r.direction.z);

                double b = 2.0 * (r.direction.x * (r.origin.x - position.x) + r.direction.y * (r.origin.y - position.y) + r.direction.z * (r.origin.z - position.z));

                double c = (position.x * position.x) + (position.y * position.y) + (position.z * position.z) + (r.origin.x * r.origin.x) + (r.origin.y * r.origin.y) + (r.origin.z * r.origin.z) - (2.0 * (position.x * r.origin.x + position.y * r.origin.y + position.z * r.origin.z)) - (radius * radius);

                double discriminant = (b * b) - (4 * a * c);

                if (discriminant < 0)
                {
                    t = -1;
                    hit = false;
                }
                else if (discriminant == 0)
                {
                    t = (-b) / (2 * a);
                    hit = true;
                }
                else
                {
                    double t1 = (-b + Math.Sqrt(discriminant)) / (2.0 * a);
                    double t2 = (-b - Math.Sqrt(discriminant)) / (2.0 * a);

                    if (t1 > 0 && t2 > 0)
                    {
                        t = Math.Min(t1, t2);

                    }
                    else if (t1 < 0 && t2 > 0)
                    {
                        t = t2;
                    }
                    else if (t1 > 0 && t2 < 0)
                    {
                        t = t1;
                    }
                    else
                    {
                        t = -1;
                    }


                    hit = true;
                }


            }
            else if (type == "planeu")
            {

                Vector normal = new Vector(0, 1, 0);


                double parallel = Vector.Dot(r.direction, normal);

                if (parallel == 0)
                {
                    t = -1;
                    hit = false;
                    return;
                }

                Vector ov0 = position.Copy();
                ov0.Subtract(r.origin);

                t = Vector.Dot(ov0, normal) / parallel;

                Vector point = r.Point(t);

                if (Math.Abs(position.x - point.x) <= (width / 2) && Math.Abs(position.z - point.z) <= (depth / 2))
                {
                    hit = true;
                }
                else
                {
                    t = -1;
                    hit = false;
                }

            }
            else if (type == "planed")
            {

                Vector normal = new Vector(0, -1, 0);


                double parallel = Vector.Dot(r.direction, normal);

                if (parallel == 0)
                {
                    t = -1;
                    hit = false;
                    return;
                }

                Vector ov0 = position.Copy();
                ov0.Subtract(r.origin);

                t = Vector.Dot(ov0, normal) / parallel;

                Vector point = r.Point(t);

                if (Math.Abs(position.x - point.x) <= (width / 2) && Math.Abs(position.z - point.z) <= (depth / 2))
                {
                    hit = true;
                }
                else
                {
                    t = -1;
                    hit = false;
                }

            }
            else if (type == "wallf")
            {

                Vector normal = new Vector(0, 0, -1);


                double parallel = Vector.Dot(r.direction, normal);

                if (parallel == 0)
                {
                    t = -1;
                    hit = false;
                    return;
                }

                Vector ov0 = position.Copy();
                ov0.Subtract(r.origin);

                t = Vector.Dot(ov0, normal) / parallel;

                Vector point = r.Point(t);

                if (Math.Abs(position.x - point.x) <= (width / 2) && Math.Abs(position.y - point.y) <= (height / 2))
                {
                    hit = true;
                }
                else
                {
                    t = -1;
                    hit = false;
                }

            }
            else if (type == "wallb")
            {

                Vector normal = new Vector(0, 0, 1);


                double parallel = Vector.Dot(r.direction, normal);

                if (parallel == 0)
                {
                    t = -1;
                    hit = false;
                    return;
                }

                Vector ov0 = position.Copy();
                ov0.Subtract(r.origin);

                t = Vector.Dot(ov0, normal) / parallel;

                Vector point = r.Point(t);

                if (Math.Abs(position.x - point.x) <= (width / 2) && Math.Abs(position.y - point.y) <= (height / 2))
                {
                    hit = true;
                }
                else
                {
                    t = -1;
                    hit = false;
                }

            }
            else if (type == "sidewalll")
            {

                Vector normal = new Vector(1, 0, 0);


                double parallel = Vector.Dot(r.direction, normal);

                if (parallel == 0)
                {
                    t = -1;
                    hit = false;
                    return;
                }

                Vector ov0 = position.Copy();
                ov0.Subtract(r.origin);

                t = Vector.Dot(ov0, normal) / parallel;

                Vector point = r.Point(t);

                if (Math.Abs(position.z - point.z) <= (depth / 2) && Math.Abs(position.y - point.y) <= (height / 2))
                {
                    hit = true;
                }
                else
                {
                    t = -1;
                    hit = false;
                }

            }
            else if (type == "sidewallr")
            {

                Vector normal = new Vector(-1, 0, 0);


                double parallel = Vector.Dot(r.direction, normal);

                if (parallel == 0)
                {
                    t = -1;
                    hit = false;
                    return;
                }

                Vector ov0 = position.Copy();
                ov0.Subtract(r.origin);

                t = Vector.Dot(ov0, normal) / parallel;

                Vector point = r.Point(t);

                if (Math.Abs(position.z - point.z) <= (depth / 2) && Math.Abs(position.y - point.y) <= (height / 2))
                {
                    hit = true;
                }
                else
                {
                    t = -1;
                    hit = false;
                }

            }

        }

    }
}
