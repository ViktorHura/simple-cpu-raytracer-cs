using System;
using System.Collections.Generic;
using Medallion;

namespace raytrace
{
    class Raytracer
    {
        Camera cam;


        public int width;
        public int height;

        List<Sobject> scene = new List<Sobject>();

        List<Light> lights = new List<Light>();


        public Raytracer(int w, int h)
        {
            width = w;
            height = h;
        }

        public void SetCamera(Vector pos, double camf, double camps)
        {
            cam = new Camera(pos.x, pos.y, pos.z, camf, camps);

        }

        public void AddSphere(Vector position, Vector color, String material, double radius)
        {
            Sobject sphere = new Sobject("sphere", position, color, material, radius);
            scene.Add(sphere);
        }

        public void AddPlaneU(Vector position, Vector color, String material, double width, double depth)
        {
            Sobject plane = new Sobject("planeu", position, color, material, width, depth);
            scene.Add(plane);
        }
        public void AddPlaneD(Vector position, Vector color, String material, double width, double depth)
        {
            Sobject plane = new Sobject("planed", position, color, material, width, depth);
            scene.Add(plane);
        }
        public void AddWallF(Vector position, Vector color, String material, double width, double height)
        {
            Sobject wall = new Sobject("wallf", position, color, material, width, height);
            scene.Add(wall);
        }
        public void AddWallB(Vector position, Vector color, String material, double width, double height)
        {
            Sobject wall = new Sobject("wallb", position, color, material, width, height);
            scene.Add(wall);
        }

        public void AddSideWallL(Vector position, Vector color, String material, double width, double height)
        {
            Sobject sidewall = new Sobject("sidewalll", position, color, material, width, height);
            scene.Add(sidewall);
        }
        public void AddSideWallR(Vector position, Vector color, String material, double width, double height)
        {
            Sobject sidewall = new Sobject("sidewallr", position, color, material, width, height);
            scene.Add(sidewall);
        }

        public void AddLight(Vector position, Vector color)
        {
            Light light = new Light(position, color);
            lights.Add(light);
        }


        public void Render(int x, int y, ref int r, ref int g, ref int b, int n, int sampling)
        {

            Vector cm = cam.position.Copy();

            double cx = cm.x - ((double)width * cam.pixelsize / 2.0) + ((double)x * cam.pixelsize);
            double cy = cm.y + ((double)height * cam.pixelsize / 2.0) - ((double)y * cam.pixelsize);
            double cz = cm.z + cam.focaldistance;

            double xs = Rand.NextDouble() * cam.pixelsize;
            double ys = Rand.NextDouble() * cam.pixelsize;


            Vector canvaspoint = new Vector(cx + xs, cy + ys, cz);

            Vector direction = new Vector(canvaspoint.x - cm.x, canvaspoint.y - cm.y, canvaspoint.z - cm.z);





            Ray ray = new Ray(cm, direction, 0);

            Vector color = ray.Trace(ref lights, ref scene);

            r = (int)color.x;
            g = (int)color.y;
            b = (int)color.z;

        }

    }
}
