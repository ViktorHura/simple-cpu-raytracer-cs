using System;
using System.Drawing;
using System.IO;


namespace raytrace
{
    class Drawer
    {

        public static void DrawPixel(int x, int y, int cr, int cg, int cb, int ca, Graphics g)
        {

            Pen p = new Pen(Color.FromArgb(ca, cr, cg, cb));
            g.DrawRectangle(p, x, y, 0.5f, 0.5f);

        }

        public static void Save(ref Bitmap bitmap, String name)
        {

            String fulp = Directory.GetCurrentDirectory() + "/" + name + ".png";

            bitmap.Save(@"" + fulp);
        }


    }
}
