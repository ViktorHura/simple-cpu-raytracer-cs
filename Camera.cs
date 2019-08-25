namespace raytrace
{
    class Camera
    {

        public Vector position;

        public double focaldistance;
        public double pixelsize;



        public Camera(double x, double y, double z, double focaldistance, double pixelsize)
        {
            this.position = new Vector(x, y, z);

            this.focaldistance = focaldistance;
            this.pixelsize = pixelsize;


        }


    }
}
