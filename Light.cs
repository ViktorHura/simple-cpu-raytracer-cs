namespace raytrace
{
    class Light
    {
        public Vector position;
        public Vector color;
        public Light(Vector position, Vector color)
        {
            this.position = position;
            this.color = color;
        }
    }
}
