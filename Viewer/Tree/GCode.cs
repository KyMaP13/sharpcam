using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Viewer.Tree
{
    class GCode
    {
        public static List<String> gcode = new List<String>();
        public static List<List<double[]>> point = new List<List<double[]>>();

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
            GCode.separator();
            GCode.G00_Z(safe_dist);
            GCode.G00_X(0);
            GCode.G00_Y(0);
            GCode.end();
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
            GCode.gcode.Add("G01 X" + Convert.ToString(x));
        }

        private static void G01_Y(double y)
        {
            GCode.gcode.Add("G01 Y" + Convert.ToString(y));
        }

        private static void G01_Z(double z)
        {
            GCode.gcode.Add("G01 Z" + Convert.ToString(z));
        }

        private static void G01_XY(double x, double y)
        {
            GCode.gcode.Add("G01 X" + Convert.ToString(x) + " Y" + Convert.ToString(y));
        }

        private static void G02(double r, Point center)
        {
            GCode.gcode.Add("G02 X" + Convert.ToString(center.X - r) + " Y" + Convert.ToString(center.Y) + " I" + Convert.ToString(center.X) + " J" + Convert.ToString(center.Y));
        }

        private static void G00_X(double x)
        {
            GCode.gcode.Add("G00 X" + Convert.ToString(x));
        }

        private static void G00_Y(double y)
        {
            GCode.gcode.Add("G00 Y" + Convert.ToString(y));
        }

        private static void G00_Z(double z)
        {
            GCode.gcode.Add("G00 Z" + Convert.ToString(z));
        }

        private static void G_Set_FeedRate(double feed_rate)
        {
            GCode.gcode.Add("F" + Convert.ToString(feed_rate));
        }

        private static void Add_Model(List<double[]> list)
        {
            GCode.point.Add(list);      // добавление нового листа который содержит список точек (траекторию)
        }

        private static void Add_Point(List<double[]> list, double x, double y, double z)
        {
            double[] point = {x, y, z};
            list.Add(point);                // тут в каждый лист (те каждую модель) вставляются точки для отрисовки траекторий
        }

        public static void generate(Model.Project project)
        {
            GCode.flush();
            GCode.init(project.Settings.SafeDistance + project.Settings.Height);

            foreach (Model.Operation iter in project.Operations)
            {
                if (iter.Shape.Name.Substring(0,6) == "BoltHo")
                    GCode.G_BoltHole(project, iter);
                if (iter.Shape.Name.Substring(0,6) == "Pocket")
                    GCode.G_Pocket(project, iter);
            }

            GCode.end(project.Settings.SafeDistance + project.Settings.Height);
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
                List<double[]> list = new List<double[]>();
                GCode.Add_Model(list);

                Point Center = new Point(enumerator.Current.X, enumerator.Current.Y);
                GCode.Add_Point(list, Center.X, project.Settings.Height + project.Settings.SafeDistance, Center.Y);

                GCode.G00_Z(project.Settings.SafeDistance);
                GCode.G00_X(Center.X);
                GCode.G00_Y(Center.Y);
                GCode.G01_Z(project.Settings.Height);

                double stR = R;
                double stY = Y;
                if (R >= tooldiam)
                {
                    for (int i = 0; i <= (lenAll - len) / stepY; i++, stR -= stepR)
                    {
                        for (int j = 0; j < stR / tooldiam; j++)
                        {
                            GCode.Circle(list, j * tooldiam, project.Settings.Height - i * stepY, Center);
                            GCode.G02(j * tooldiam, Center);
                            if (j + 1 < R / (2 * tooldiam))
                            {
                                GCode.Add_Point(list, Center.X, project.Settings.Height - i * stepY, Center.Y + j * tooldiam);
                                GCode.G01_XY(Center.X, Center.Y + j * tooldiam);
                            }
                        }
                        /* Сюда надо вставить заключительный проход по окружности */
                        GCode.Add_Point(list, Center.X, project.Settings.Height - i * stepY, Center.Y);
                        GCode.G01_XY(Center.X, Center.Y);
                        GCode.separator();
                        if (i + 1 < len)
                        {
                            GCode.Add_Point(list, Center.X, project.Settings.Height - (i + 1) * stepY, Center.Y);
                            GCode.G01_Z(project.Settings.Height - (i + 1) * stepY);
                        }
                    }

                    for (double i = len; i <= lenAll / stepY; i++)
                    {
                        for (int j = 0; j < r / tooldiam; j++)
                        {
                            GCode.Circle(list, j * tooldiam, project.Settings.Height - i * stepY, Center);
                            GCode.G02(j * tooldiam, Center);
                            if (j + 1 < r / (2 * tooldiam))
                            {
                                GCode.Add_Point(list, Center.X, project.Settings.Height - i * stepY, Center.Y + j * tooldiam);
                                GCode.G01_XY(Center.X, Center.Y + j * tooldiam);
                            }
                        }
                        /* Сюда надо вставить заключительный проход по окружности */
                        GCode.Add_Point(list, Center.X, project.Settings.Height - i * stepY, Center.Y);
                        GCode.G01_XY(Center.X, Center.Y);
                        GCode.separator();
                        if (i + 1 < len)
                        {
                            GCode.Add_Point(list, Center.X, project.Settings.Height - (i + 1) * stepY, Center.Y);
                            GCode.G01_Z(project.Settings.Height - (i + 1) * stepY);
                        }
                    }
                    GCode.Add_Point(list, Center.X, project.Settings.Height + project.Settings.SafeDistance, Center.Y);
                    GCode.G01_Z(project.Settings.Height + 2);
                    GCode.G00_Z(project.Settings.Height + project.Settings.SafeDistance);
                    GCode.separator();
                }
            }
        }

        private static void Circle(List<double[]> list, double r, double y, Point Center)
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
                GCode.Add_Point(list, r * Math.Sin(angle) + Center.X, y, r * Math.Cos(angle) + Center.Y);
            }
            GCode.Add_Point(list, start.X + Center.X, y, start.Y + Center.Y);
        }

        private static void G_Pocket(Model.Project project, Model.Operation iter) // X ~ width ; Y ~ length
        {
            List<double[]> list = new List<double[]>();
            GCode.Add_Model(list);
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

            GCode.G_Set_FeedRate(180);      // Стандартная скорость подачи пока нет такого поля в проекте
            GCode.G00_X(origin.X + tooldiam / 2);
            GCode.G00_Y(origin.Y + tooldiam / 2);
            GCode.G01_Z(project.Settings.Height);

            double temp = 0;

            GCode.Add_Point(list, origin.X + (tooldiam / 2), project.Settings.Height + project.Settings.SafeDistance, origin.Y + (tooldiam / 2));
            Add_Point(
                        list,
                        origin.X + (tooldiam / 2),
                        project.Settings.Height,
                        origin.Y + (tooldiam / 2)
                    );
            for (double i = project.Settings.Height; i >= project.Settings.Height - height; i -= stepY)         // дописать именно генерацию кодов...
            {
                GCode.G01_Z(i);
                for (double j = 1; j <= width / tooldiam; j++)
                {
                    Add_Point(
                        list, 
                        origin.X + (tooldiam / 2) + temp,
                        i, //i
                        origin.Y + (tooldiam / 2) + ((j * length) % (length * 2)) - tooldiam * (j % 2)
                    );
                    GCode.G01_XY(origin.X + (tooldiam / 2) + temp, origin.Y + (tooldiam / 2) + ((j * length) % (length * 2)) - tooldiam * (j % 2));
                    if (tooldiam / 2 + temp + tooldiam < width)
                    {
                        temp += tooldiam;
                        Add_Point(
                            list,
                            origin.X + (tooldiam / 2) + temp, 
                            i, //i
                            origin.Y + (tooldiam / 2) + ((j * length) % (length * 2)) - tooldiam * (j % 2)
                        );
                        GCode.G01_XY(origin.X + (tooldiam / 2) + temp, origin.Y + (tooldiam / 2) + ((j * length) % (length * 2)) - tooldiam * (j % 2));
                    }
                }
                temp = 0;
                Add_Point(list, origin.X + (tooldiam / 2), i, origin.Y + (tooldiam / 2));
                GCode.G01_XY(origin.X + (tooldiam / 2), origin.Y + (tooldiam / 2));
                Add_Point(list,origin.X + width - tooldiam / 2,i,origin.Y + (tooldiam / 2));                // Периметр
                GCode.G01_XY(origin.X + width - tooldiam / 2, origin.Y + (tooldiam / 2));
                Add_Point(list, origin.X + width - tooldiam / 2,i,origin.Y + length - tooldiam / 2);
                GCode.G01_XY(origin.X + width - tooldiam / 2, origin.Y + length - tooldiam / 2);
                Add_Point(list, origin.X + (tooldiam / 2), i,origin.Y + length - tooldiam / 2);
                GCode.G01_XY(origin.X + (tooldiam / 2), origin.Y + length - tooldiam / 2);
                Add_Point(list, origin.X + (tooldiam / 2),i,origin.Y + (tooldiam / 2) );
                GCode.G01_XY(origin.X + (tooldiam / 2), origin.Y + (tooldiam / 2));

                GCode.separator();
            }
            Add_Point(list, origin.X + (tooldiam / 2) ,project.Settings.Height + project.Settings.SafeDistance, origin.Y + (tooldiam / 2));
            GCode.G01_Z(project.Settings.Height + 2);
            GCode.G00_Z(project.Settings.Height + project.Settings.SafeDistance);
            GCode.separator();
        }

        public static void flush()
        {
            gcode.Clear();
            point.Clear();
        }
        
    }
}
