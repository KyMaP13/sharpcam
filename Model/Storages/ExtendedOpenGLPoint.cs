using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Storages
{
    public class ExtendedOpenGLPoint
    {
        private double x;
        private double y;
        private double z;
        private int type;   // dotted, line, ...
        private float[] color;

        public ExtendedOpenGLPoint(double x, double y, double z, int type, float[] color)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.type = type;
            this.color = color;
        }

        public double[] getCoordinates(){
            return new double[3] {x, y, z};
        }
    }
}
