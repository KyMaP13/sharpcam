using System;
using System.Windows;
using SharpGL;
using System.Collections.ObjectModel;
using System.IO;
using System.Collections.Generic;

namespace Viewer
{

    class BoltHole
    {
        public static void Draw(OpenGL gl, Point Center, List<double[]> Bolt)
        {
            gl.Begin(OpenGL.GL_LINE_STRIP);
            gl.Color(0, 1, 0);
            foreach (var point in Bolt)
            {
                gl.Vertex(point);
            }
            gl.End();
        }

        public static List<double[]> ReCalc(Model.BoltHole Bolt, double stepY, Point Center)
        {
            List<double[]> bolt = new List<double[]>();

            double stepR = (Bolt.Radius - Bolt.radius) / ((Bolt.lenAll - Bolt.Length) / stepY); // рассчитываем шаг для радиуса
            double stR = Bolt.Radius;

            for (double i = Bolt.Y; i > Bolt.Y - (Bolt.lenAll - Bolt.Length); i -= stepY)
            {
                Circle( stR, i, Center, bolt);
                stR -= stepR;
            }

            for (double i = Bolt.Y - (Bolt.lenAll - Bolt.Length); i > Bolt.Y - Bolt.lenAll; i -= stepY)
            {
                Circle( Bolt.radius, i, Center, bolt);
            }
            Circle( Bolt.radius, Bolt.Y - Bolt.lenAll, Center, bolt);

            return bolt;
        }



        private static void Circle( double r, double y, Point Center, List<double[]> bolt)
        {
            int n = (int)Math.Round(r) * 20;
            Point start = new Point();
            for (int i = 0; i < n; i++)
            {
                double angle = 2 * Math.PI * i / n;
                if (i == 0)
                {
                    start.X = Math.Round(r * Math.Sin(angle), 5);
                    start.Y = Math.Round(r * Math.Cos(angle), 5);
                }
                bolt.Add(Features.Adder((r * Math.Sin(angle) + Center.X), y, (r * Math.Cos(angle) + Center.Y)));
            }
            bolt.Add(Features.Adder((start.X + Center.X), y, (start.Y + Center.Y)));
        }

        //public static void DrawTracking(OpenGL gl, double R, double r, double lenAll, double len, double tooldiam, double stepY, double Y, Point Center)
        //{
        //    gl.Begin(OpenGL.GL_LINE_STRIP);
        //    double stepR = (R - r) / ((lenAll - len) / stepY); // рассчитываем шаг для радиуса
        //    double stR = R;
        //    double stY = Y;

        //    for (double i = Y; i > Y - (lenAll - len); i -= stepY)
        //    {
        //        DrawTrack(gl, stR, tooldiam, Center, i);
        //        stR -= stepR;
        //    }

        //    for (double i = Y - (lenAll - len); i > Y - lenAll; i -= stepY)
        //    {
        //        DrawTrack(gl, r, tooldiam, Center, i);
        //    }
        //    DrawTrack(gl, r, tooldiam, Center, Y - lenAll);
        //    gl.End();
        //}

        //private static void DrawTrack(OpenGL gl, double stR, double tooldiam, Point Center, double stY)
        //{
        //    for (double j = 0; j < stR - tooldiam; j += tooldiam)
        //    {
        //        DrawCircle(gl, j, stY, Center, "green");
        //    }
        //    DrawCircle(gl, stR - tooldiam, stY, Center, "green");
        //    DrawCircle(gl, 0, stY, Center, "green");
        //}
    }

    class Pocket
    {

        public static void Draw(OpenGL gl, Point Center, List<double[]> Poc)
        {
            int i = 0;
            while (i<Poc.Count)
            {
                gl.Begin(OpenGL.GL_LINE_LOOP);
                gl.Color(0, 0, 0);
                for (int j = i; j < i + 4; j++)
                {
                    gl.Vertex(Poc[j]);
                }
                gl.End();
                i += 4;
            }
        }

        public static List<double[]> ReCalc(Model.Pocket Poc, double stepY, Point Center)
        {
            List<double[]> pocket = new List<double[]>();

            double stY = Poc.Y;
            for (double i = 0; i < Poc.height ; i += stepY)
            {
                Rectangle(pocket, Poc.length, Poc.width, stY, Center);
                stY -= stepY;
            }
            Rectangle(pocket, Poc.length, Poc.width, Poc.height, Center);
            
            return pocket;
        }

        private static void Rectangle(List<double[]> pocket,double len, double wid, double y, Point Center)
        {
            pocket.Add(Features.Adder((-wid / 2) + Center.X, y, (-len / 2) + Center.Y));
            pocket.Add(Features.Adder((-wid / 2) + Center.X, y, (len / 2) + Center.Y));
            pocket.Add(Features.Adder((wid / 2) + Center.X, y, (len / 2) + Center.Y));
            pocket.Add(Features.Adder((wid / 2) + Center.X, y, (-len / 2) + Center.Y));
        }
    }

    class Billet
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
    class Features
    {
        public static double[] Adder(double x, double y, double z)
        {
            double[] point = { x, y, z };
            return point;
        }
    }
}

