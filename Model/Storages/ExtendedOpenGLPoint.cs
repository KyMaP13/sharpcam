namespace Model.Storages
{
    public class ExtendedOpenGlPoint
    {
        private double x;
        private double y;
        private double z;
        private int type;   // dotted, line, ...
        private float[] color;

        public ExtendedOpenGlPoint(double x, double y, double z, int type, float[] color)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.type = type;
            this.color = color;
        }

        public double[] GetCoordinates(){
            return new[] {x, y, z};
        }
    }
}
