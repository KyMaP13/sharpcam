using System;
using System.Windows;
using SharpGL;
using System.Collections.Generic;

namespace Viewer
{
    static class BoltHole
    {
        public static void Draw(OpenGL gl, IEnumerable<double[]> bolt)
        {
            gl.Begin(OpenGL.GL_LINE_STRIP);
            gl.Color(0, 1, 0);
            foreach (var point in bolt)
            {
                gl.Vertex(point);
            }
            gl.End();
        }

        public static List<double[]> ReCalc(Model.Primitives.BoltHole boltHole, double stepY, Point center)
        {
            if (boltHole == null) throw new ArgumentNullException("boltHole");
            var boltCarcass = new List<double[]>();

            var stepR = (boltHole.Radius - boltHole.InternalRadius) / ((boltHole.LenAll - boltHole.Length) / stepY); // рассчитываем шаг для радиуса
            var stR = boltHole.Radius;

            for (var i = boltHole.Y; i > boltHole.Y - (boltHole.LenAll - boltHole.Length); i -= stepY)
            {
                Circle( stR, i, center, boltCarcass);
                stR -= stepR;
            }

            for (var i = boltHole.Y - (boltHole.LenAll - boltHole.Length); i > boltHole.Y - boltHole.LenAll; i -= stepY)
            {
                Circle( boltHole.InternalRadius, i, center, boltCarcass);
            }
            Circle( boltHole.InternalRadius, boltHole.Y - boltHole.LenAll, center, boltCarcass);

            return boltCarcass;
        }



        private static void Circle( double r, double y, Point center, ICollection<double[]> bolt)
        {
            var n = (int)Math.Round(r) * 20;
            var start = new Point();
            for (var i = 0; i < n; i++)
            {
                var angle = 2 * Math.PI * i / n;
                if (i == 0)
                {
                    start.X = Math.Round(r * Math.Sin(angle), 5);
                    start.Y = Math.Round(r * Math.Cos(angle), 5);
                }
                bolt.Add(Features.Adder((r * Math.Sin(angle) + center.X), y, (r * Math.Cos(angle) + center.Y)));
            }
            bolt.Add(Features.Adder((start.X + center.X), y, (start.Y + center.Y)));
        }
    }

    static class Pocket
    {

        public static void Draw(OpenGL gl, List<double[]> poc)
        {
            if (poc == null) throw new ArgumentNullException("poc");
            var i = 0;
            while (i<poc.Count)
            {
                gl.Begin(OpenGL.GL_LINE_LOOP);
                gl.Color(0, 0, 0);
                for (var j = i; j < i + 4; j++)
                {
                    gl.Vertex(poc[j]);
                }
                gl.End();
                i += 4;
            }
        }

        public static List<double[]> ReCalc(Model.Primitives.Pocket poc, double stepY, Point center)
        {
            var pocket = new List<double[]>();

            var stY = poc.Y;
            for (double i = 0; i < poc.Height ; i += stepY)
            {
                Rectangle(pocket, poc.Length, poc.Width, stY, center);
                stY -= stepY;
            }
            Rectangle(pocket, poc.Length, poc.Width, poc.Height, center);
            
            return pocket;
        }

        private static void Rectangle(ICollection<double[]> pocket,double len, double wid, double y, Point center)
        {
            var halfRecWidth = wid / 2;
            var halfRecLength = len / 2;
            pocket.Add(Features.Adder(-halfRecWidth + center.X, y, -halfRecLength + center.Y));
            pocket.Add(Features.Adder(-halfRecWidth + center.X, y, halfRecLength + center.Y));
            pocket.Add(Features.Adder(halfRecWidth + center.X, y, halfRecLength + center.Y));
            pocket.Add(Features.Adder(halfRecWidth + center.X, y, -halfRecLength + center.Y));
        }
    }

    static class Billet
    {
        public static void Draw(OpenGL gl, double height, double length, double width)
        {
            gl.LineWidth(1);
            gl.Color(0.7, 0.7, 0.7);
            gl.Begin(OpenGL.GL_LINE_LOOP);
            gl.Vertex(0, height, 0 );
            gl.Vertex(0, height, width);
            gl.Vertex(length, height, width);
            gl.Vertex(length, height, 0 );
            gl.End();

            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex(0, height, 0);
            gl.Vertex(0, 0, 0);

            gl.Vertex(length, height, 0);
            gl.Vertex(length, 0, 0);

            gl.Vertex(0, height, width);
            gl.Vertex(0, 0, width);

            gl.Vertex(length, height, width);
            gl.Vertex(length, 0, width);
            gl.End();

            gl.Begin(OpenGL.GL_LINE_LOOP);
            gl.Vertex(0, 0, 0);
            gl.Vertex(0, 0, width);
            gl.Vertex(length, 0, width);
            gl.Vertex(length, 0, 0);
            gl.End();
        }
    }

    static class Features
    {
        public static double[] Adder(double x, double y, double z)
        {
            double[] point = { x, y, z };
            return point;
        }
    }
}

