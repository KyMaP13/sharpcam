using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Storages;
using System.Windows;

namespace Model
{
    public class GCodeGenerator
    {
        public static List<String> gcode = new List<String>();
//       public static List<List<double[]>> point = new List<List<double[]>>();
        public static TrajectoryStorage trajectoryStor = new TrajectoryStorage();

        private static void init(double safe_dist)
        {
            gcode.Add("G21");                           // Метрическая система
            gcode.Add("G90");                           // Абсолютное позиционирование
            gcode.Add("F180.000");                        // Скорость подачи 3мм в сек, не уверен
            gcode.Add("");
            gcode.Add("G00 Z" + Convert.ToString(safe_dist));                  // Холостой ход, поднятие фрезы на 5см над поверхностью
            gcode.Add("G00 X0,00000 Y0,00000");         // Холостой ход в начало системы координат
        }

        private static void end(double safe_dist)
        {
            GCodeGenerator.separator();
            GCodeGenerator.G00_Z(safe_dist);
            GCodeGenerator.G00_X(0);
            GCodeGenerator.G00_Y(0);
            GCodeGenerator.end();
        }

        private static void end()
        {
            gcode.Add("M30");                           // Конец
        }

        private static void separator()
        {
            gcode.Add("");
        }

        private static void G01_X(double x)
        {
            GCodeGenerator.gcode.Add("G01 X" + Convert.ToString(x));
        }

        private static void G01_Y(double y)
        {
            GCodeGenerator.gcode.Add("G01 Y" + Convert.ToString(y));
        }

        private static void G01_Z(double z)
        {
            GCodeGenerator.gcode.Add("G01 Z" + Convert.ToString(z));
        }

        private static void G01_XY(double x, double y)
        {
            GCodeGenerator.gcode.Add("G01 X" + Convert.ToString(x) + " Y" + Convert.ToString(y));
        }

        private static void G02(double r, Point center)
        {
            GCodeGenerator.gcode.Add("G02 X" + Convert.ToString(center.X - r) + " Y" + Convert.ToString(center.Y) + " I" + Convert.ToString(center.X) + " J" + Convert.ToString(center.Y));
        }

        private static void G00_X(double x)
        {
            GCodeGenerator.gcode.Add("G00 X" + Convert.ToString(x));
        }

        private static void G00_Y(double y)
        {
            GCodeGenerator.gcode.Add("G00 Y" + Convert.ToString(y));
        }

        private static void G00_Z(double z)
        {
            GCodeGenerator.gcode.Add("G00 Z" + Convert.ToString(z));
        }

        private static void G_Set_FeedRate(double feed_rate)
        {
            GCodeGenerator.gcode.Add("F" + Convert.ToString(feed_rate));
        }
//////////////////////////////////////////////////////////////////////////////
        private static void AddModel(List<ExtendedOpenGLPoint> list)
        {
            trajectoryStor.AddModel(list);      // добавление нового листа который содержит список точек (траекторию)
        }

/*        private static void Add_Point(List<double[]> list, double x, double y, double z)
        {
            double[] point = {x, y, z};
            list.Add(point);                // тут в каждый лист (те каждую модель) вставляются точки для отрисовки траекторий
        }*/

        public static void generate(Model.Project project)
        {
            GCodeGenerator.flush();
            GCodeGenerator.init(project.Settings.SafeDistance + project.Settings.Height);

            foreach (Model.Operation iter in project.Operations)
            {
                if (iter.Shape.Name.Substring(0,6) == "BoltHo")
                    GCodeGenerator.G_BoltHole(project, iter);
                if (iter.Shape.Name.Substring(0,6) == "Pocket")
                    GCodeGenerator.G_Pocket(project, iter);
            }

            GCodeGenerator.end(project.Settings.SafeDistance + project.Settings.Height);
        }

        private static void G_BoltHole(Model.Project project, Model.Operation iter)
        {
            double R = ((Model.BoltHole)iter.Shape).Radius;
            double r = ((Model.BoltHole)iter.Shape).radius;
            double lenAll = ((Model.BoltHole)iter.Shape).lenAll;
            double len = ((Model.BoltHole)iter.Shape).Length;
            double tooldiam = project.Settings.ToolDiam;
            double stepY = 1;

            double Y = ((Model.BoltHole)iter.Shape).Y;
            double stepR = (R - r) / ((lenAll - len) / stepY); // рассчитываем шаг для радиуса

            var enumerator = iter.Location.LocationsList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                List<ExtendedOpenGLPoint> list = new List<ExtendedOpenGLPoint>();
                float[] color = new float[3] {0,0,0};
//                GCodeGenerator.Add_Model(list);
                
                Point Center = new Point(enumerator.Current.X, enumerator.Current.Y);
                list.Add(
                    new ExtendedOpenGLPoint
                    (
                        Center.X, 
                        project.Settings.Height + project.Settings.SafeDistance, 
                        Center.Y,
                        0,
                        new float[3] {0,0,0}
                    )
                );

                GCodeGenerator.G00_Z(project.Settings.SafeDistance);
                GCodeGenerator.G00_X(Center.X);
                GCodeGenerator.G00_Y(Center.Y);
                GCodeGenerator.G01_Z(project.Settings.Height);

                double stR = R;
                double stY = Y;
                if (R >= tooldiam)
                {
                    for (int i = 0; i <= (lenAll - len) / stepY; i++, stR -= stepR)
                    {
                        for (int j = 0; j < stR / tooldiam; j++)
                        {
                            GCodeGenerator.Circle(list, j * tooldiam, project.Settings.Height - i * stepY, Center);
                            GCodeGenerator.G02(j * tooldiam, Center);
                            if (j + 1 < R / (2 * tooldiam))
                            {
                                list.Add(
                                    new ExtendedOpenGLPoint
                                        (
                                        Center.X,
                                        project.Settings.Height - i * stepY,
                                        Center.Y + j * tooldiam,
                                        0,
                                        new float[3] {0,0,0}
                                        )
                                );
                                GCodeGenerator.G01_XY(Center.X, Center.Y + j * tooldiam);
                            }
                        }
                        /* Сюда надо вставить заключительный проход по окружности */
                        list.Add(
                            new ExtendedOpenGLPoint
                            (
                                Center.X, 
                                project.Settings.Height - i * stepY, 
                                Center.Y,
                                0,
                                new float[3] {0,0,0}
                            )
                        );
                        GCodeGenerator.G01_XY(Center.X, Center.Y);
                        GCodeGenerator.separator();
                        if (i + 1 < len)
                        {
                            list.Add(
                                new ExtendedOpenGLPoint
                                (
                                    Center.X, 
                                    project.Settings.Height - (i + 1) * stepY, 
                                    Center.Y,
                                    0,
                                    new float[3] {0,0,0}
                                    )
                                );
                            GCodeGenerator.G01_Z(project.Settings.Height - (i + 1) * stepY);
                        }
                    }

                    for (double i = len; i <= lenAll / stepY; i++)
                    {
                        for (int j = 0; j < r / tooldiam; j++)
                        {
                            GCodeGenerator.Circle(list, j * tooldiam, project.Settings.Height - i * stepY, Center);
                            GCodeGenerator.G02(j * tooldiam, Center);
                            if (j + 1 < r / (2 * tooldiam))
                            {
                                list.Add(
                                    new ExtendedOpenGLPoint
                                    (
                                        Center.X, 
                                        project.Settings.Height - i * stepY, 
                                        Center.Y + j * tooldiam,
                                        0,
                                        new float[3] {0,0,0}
                                    )
                                );
                                GCodeGenerator.G01_XY(Center.X, Center.Y + j * tooldiam);
                            }
                        }
                        /* Сюда надо вставить заключительный проход по окружности */
                        list.Add(
                                new ExtendedOpenGLPoint
                                (
                                    Center.X, 
                                    project.Settings.Height - i * stepY, 
                                    Center.Y,
                                    0,
                                    new float[3] {0,0,0}
                                )
                        );
                        GCodeGenerator.G01_XY(Center.X, Center.Y);
                        GCodeGenerator.separator();
                        if (i + 1 < len)
                        {
                            list.Add(
                                new ExtendedOpenGLPoint
                                (
                                    Center.X, 
                                    project.Settings.Height - (i + 1) * stepY, 
                                    Center.Y,
                                    0,
                                    new float[3] {0,0,0}
                                )
                            );
                            GCodeGenerator.G01_Z(project.Settings.Height - (i + 1) * stepY);
                        }
                    }
                    list.Add(
                            new ExtendedOpenGLPoint
                            (
                                Center.X, 
                                project.Settings.Height + project.Settings.SafeDistance, 
                                Center.Y,
                                0,
                                new float[3] {0,0,0}
                            )
                    );
                    GCodeGenerator.G01_Z(project.Settings.Height + 2);
                    GCodeGenerator.G00_Z(project.Settings.Height + project.Settings.SafeDistance);
                    GCodeGenerator.separator();
                }
                trajectoryStor.AddModel(list);
            }
        }

        private static void Circle(List<ExtendedOpenGLPoint> list, double r, double y, Point Center)
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
                list.Add(
                    new ExtendedOpenGLPoint(
                        r * Math.Sin(angle) + Center.X,
                        y,
                        r * Math.Cos(angle) + Center.Y,
                        0,
                        new float[3] {0,0,0}
                    )
                );
            }
            list.Add(
                new ExtendedOpenGLPoint(
                    start.X + Center.X,
                    y,
                    start.Y + Center.Y,
                    0,
                    new float[3] {0,0,0}
                )
            );
        }

        private static void G_Pocket(Model.Project project, Model.Operation iter) // X ~ width ; Y ~ length
        {
            List<ExtendedOpenGLPoint> list = new List<ExtendedOpenGLPoint>();
//            GCodeGenerator.Add_Model(list);
            var enumerator = iter.Location.LocationsList.GetEnumerator();
            double length = ((Model.Pocket)iter.Shape).length;
            double height = ((Model.Pocket)iter.Shape).height;
            double width = ((Model.Pocket)iter.Shape).width;
            enumerator.MoveNext();
            Point origin = new Point(
                enumerator.Current.X - width / 2, 
                enumerator.Current.Y - length / 2
                );       // для обычного покета, не матрично/радиально расположенных
            double stepY = 1;
            double tooldiam = project.Settings.ToolDiam;

            GCodeGenerator.G_Set_FeedRate(180);      // Стандартная скорость подачи пока нет такого поля в проекте
            GCodeGenerator.G00_X(origin.X + tooldiam / 2);
            GCodeGenerator.G00_Y(origin.Y + tooldiam / 2);
            GCodeGenerator.G01_Z(project.Settings.Height);

            double temp = 0;

            list.Add(
                new ExtendedOpenGLPoint(
                    origin.X + (tooldiam / 2), 
                    project.Settings.Height + project.Settings.SafeDistance, 
                    origin.Y + (tooldiam / 2),
                    0,
                    new float[3] {0,0,0}
                    )
            );
            list.Add(
                new ExtendedOpenGLPoint(
                        origin.X + (tooldiam / 2),
                        project.Settings.Height,
                        origin.Y + (tooldiam / 2),
                        0,
                        new float[3] {0,0,0}
                        )
            );
            for (double i = project.Settings.Height; i >= project.Settings.Height - height; i -= stepY)         // дописать именно генерацию кодов...
            {
                GCodeGenerator.G01_Z(i);
                for (double j = 1; j <= width / tooldiam; j++)
                {
                    list.Add(
                        new ExtendedOpenGLPoint(
                            origin.X + (tooldiam / 2) + temp,
                            i, //i
                            origin.Y + (tooldiam / 2) + ((j * length) % (length * 2)) - tooldiam * (j % 2),
                            0,
                            new float[3] {0,0,0}
                        )
                    );
                    GCodeGenerator.G01_XY(origin.X + (tooldiam / 2) + temp, origin.Y + (tooldiam / 2) + ((j * length) % (length * 2)) - tooldiam * (j % 2));
                    if (tooldiam / 2 + temp + tooldiam < width)
                    {
                        temp += tooldiam;
                        list.Add(
                            new ExtendedOpenGLPoint(
                                origin.X + (tooldiam / 2) + temp, 
                                i, //i
                                origin.Y + (tooldiam / 2) + ((j * length) % (length * 2)) - tooldiam * (j % 2),
                                0,
                                new float[3] {0,0,0}
                            )
                        );
                        GCodeGenerator.G01_XY(origin.X + (tooldiam / 2) + temp, origin.Y + (tooldiam / 2) + ((j * length) % (length * 2)) - tooldiam * (j % 2));
                    }
                }
                temp = 0;
                list.Add(new ExtendedOpenGLPoint(origin.X + (tooldiam / 2), i, origin.Y + (tooldiam / 2),0, new float[3] {0,0,0}));
                GCodeGenerator.G01_XY(origin.X + (tooldiam / 2), origin.Y + (tooldiam / 2));
                list.Add(new ExtendedOpenGLPoint(origin.X + width - tooldiam / 2,i,origin.Y + (tooldiam / 2),0, new float[3] {0,0,0}));                // Периметр
                GCodeGenerator.G01_XY(origin.X + width - tooldiam / 2, origin.Y + (tooldiam / 2));
                list.Add(new ExtendedOpenGLPoint(origin.X + width - tooldiam / 2,i,origin.Y + length - tooldiam / 2,0, new float[3] {0,0,0}));
                GCodeGenerator.G01_XY(origin.X + width - tooldiam / 2, origin.Y + length - tooldiam / 2);
                list.Add(new ExtendedOpenGLPoint(origin.X + (tooldiam / 2), i,origin.Y + length - tooldiam / 2,0, new float[3] {0,0,0}));
                GCodeGenerator.G01_XY(origin.X + (tooldiam / 2), origin.Y + length - tooldiam / 2);
                list.Add(new ExtendedOpenGLPoint(origin.X + (tooldiam / 2), i, origin.Y + (tooldiam / 2), 0, new float[3] { 0, 0, 0 }));
                GCodeGenerator.G01_XY(origin.X + (tooldiam / 2), origin.Y + (tooldiam / 2));

                GCodeGenerator.separator();
            }
            list.Add(new ExtendedOpenGLPoint(origin.X + (tooldiam / 2), project.Settings.Height + project.Settings.SafeDistance, origin.Y + (tooldiam / 2), 0, new float[3] { 0, 0, 0 }));
            trajectoryStor.AddModel(list);
            GCodeGenerator.G01_Z(project.Settings.Height + 2);
            GCodeGenerator.G00_Z(project.Settings.Height + project.Settings.SafeDistance);
            GCodeGenerator.separator();
        }

        public static void flush()
        {
            gcode.Clear();
            trajectoryStor.Clear();

        }
        
    }
}
