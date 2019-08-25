using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;



namespace raytrace
{
    class Program
    {




        public static void Render(int xb, int yb, int xe, int ye, int canvas_w, int canvas_h, ref Dictionary<int, Bitmap> maps, ref Raytracer rt, int sampling, int slicelog)
        {
            Bitmap i = new Bitmap(xe - xb, ye - yb);
            Graphics gr = Graphics.FromImage(i);

            double gamma = 2;

            for (int x = 0; x < xe - xb; x++)
            {
                for (int y = 0; y < ye - yb; y++)
                {
                    int tr = 0;
                    int tg = 0;
                    int tb = 0;

                    for (int n = 0; n < sampling; n++)
                    {
                        int r = 0;
                        int g = 0;
                        int b = 0;
                        rt.Render(xb + x, yb + y, ref r, ref g, ref b, n, sampling);

                        tr += r;
                        tg += g;
                        tb += b;

                    }

                    tr = tr / sampling;
                    tg = tg / sampling;
                    tb = tb / sampling;

                    tr = (int)(Math.Pow(((double)tr / 255), (1 / gamma)) * 255);
                    tg = (int)(Math.Pow(((double)tg / 255), (1 / gamma)) * 255);
                    tb = (int)(Math.Pow(((double)tb / 255), (1 / gamma)) * 255);

                    Drawer.DrawPixel(x, y, tr, tg, tb, 255, gr);
                }

            }

            if (slicelog == 1)
            {
                Drawer.Save(ref i, "" + xb);
            }

            lock (maps)
            {
                maps.Add(xb, i);
            }



        }

        static void Main(string[] args)
        {

            int screenheight = 2160;
            int screenwidth;

            Console.WriteLine("Screen height in pixels?");

            String inputh = Console.ReadLine();

            if (inputh == "")
            {
                screenheight = 2160;

            }
            else
            {
                screenheight = Convert.ToInt32(inputh);
            }

            screenwidth = (int)((double)screenheight / 3 * 4);


            Console.WriteLine("" + screenwidth + " x " + screenheight);






            Raytracer rt = new Raytracer(screenwidth, screenheight);

            rt.SetCamera(new Vector(0, 45, -50), 2.5, 2160 / (double)screenheight * 0.00208);

            rt.AddLight(new Vector(40, 50, -40), new Vector(100, 100, 255));
            rt.AddLight(new Vector(-40, 50, -40), new Vector(255, 100, 100));
            rt.AddLight(new Vector(0, 90, 40), new Vector(100, 255, 100));


            rt.AddSphere(new Vector(0, 15, 16), new Vector(255, 109, 165), "matte", 15);

            rt.AddSphere(new Vector(-32, 32, 22), new Vector(204, 153, 51), "metal", 15);

            rt.AddSphere(new Vector(22, 70, 22), new Vector(255, 109, 165), "mirror", 25);

            rt.AddSphere(new Vector(-16, 28, -8), new Vector(255, 255, 255), "glass", 12.5);




            rt.AddPlaneU(new Vector(0, 0, 0), new Vector(50, 50, 50), "metal", 100, 100);
            rt.AddPlaneD(new Vector(0, 100, 0), new Vector(50, 50, 50), "metal", 100, 100);

            rt.AddWallF(new Vector(0, 50, 50), new Vector(50, 50, 50), "metal", 100, 100);
            rt.AddWallB(new Vector(0, 50, -50), new Vector(50, 50, 50), "metal", 100, 100);

            rt.AddSideWallL(new Vector(-50, 50, 0), new Vector(175, 80, 90), "metal", 100, 100);
            rt.AddSideWallR(new Vector(50, 50, 0), new Vector(90, 80, 175), "metal", 100, 100);





            ///Distribute rendeing in multiple cores and render
            ///
            int threadstospawn = 1;

            Console.WriteLine("How many cores to use?");

            String inputt = Console.ReadLine();

            if (inputt == "")
            {
                threadstospawn = 8;

            }
            else
            {
                threadstospawn = Convert.ToInt32(inputt);
            }
            Console.WriteLine(threadstospawn);

            int sampling = 256;

            Console.WriteLine("Sampling rate?");

            String inputs = Console.ReadLine();

            if (inputs == "")
            {
                sampling = 2;

            }
            else
            {
                sampling = Convert.ToInt32(inputs);
            }
            Console.WriteLine(sampling);


            int slicelog = 0;

            Console.WriteLine("Save individual slices?(0/1)");

            String inputl = Console.ReadLine();

            if (inputl == "")
            {
                slicelog = 0;

            }
            else
            {
                slicelog = Convert.ToInt32(inputl);
            }
            Console.WriteLine(slicelog);

            Console.WriteLine("Started rendering, this might take a while depending on your configuration.");


            Dictionary<int, Bitmap> maps = new Dictionary<int, Bitmap>();



            Task[] tasks = new Task[threadstospawn];

            Bitmap image = new Bitmap(rt.width, rt.height);
            Graphics graph = Graphics.FromImage(image);

            int slicew = image.Width / threadstospawn;


            for (int i = 0; i < threadstospawn; i++)
            {
                int xb = i * slicew;
                int xe = (i + 1) * slicew;

                if (i == threadstospawn - 1)
                {
                    xe = image.Width;
                }

                tasks[i] = Task.Run(() => Render(xb, 0, xe, rt.height, rt.width, rt.height, ref maps, ref rt, sampling, slicelog));

            }

            Task.WaitAll(tasks);



            while (maps.Count != threadstospawn)
            {
                //extra wait just in case
            }


            foreach (KeyValuePair<int, Bitmap> entry in maps)
            {

                graph.DrawImageUnscaled(entry.Value, entry.Key, 0); // recombine slices
            }

            for (int i = 0; i < threadstospawn; i++)
            {

                tasks[i].Dispose();

            }


            Drawer.Save(ref image, "out");

            //Console.Read();
        }
    }
}
