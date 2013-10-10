using System;
using System.Collections.Generic;
using Model.Primitives;
using Model.Storages;
using System.Windows;
using Model.Tree;

namespace Model
{
    public static class GCodeGenerator
    {
        public static readonly List<String> Gcode = new List<String>();
        public static readonly TrajectoryStorage TrajectoryStor = new TrajectoryStorage();

        private static void Init(double safeDist)
        {
            Gcode.Add("G21");                           // Метрическая система
            Gcode.Add("G90");                           // Абсолютное позиционирование
            Gcode.Add("F180.000");                        // Скорость подачи 3мм в сек, не уверен
            Gcode.Add("");
            Gcode.Add("G00 Z" + Convert.ToString(safeDist));                  // Холостой ход, поднятие фрезы на 5см над поверхностью
            Gcode.Add("G00 X0,00000 Y0,00000");         // Холостой ход в начало системы координат
        }

        private static void EndMills(double safeDist)
        {
            Separator();
            G00Z(safeDist);
            G00X(0);
            G00Y(0);
            EndMills();
        }

        private static void EndMills()
        {
            Gcode.Add("M30");                           // Конец
        }

        private static void Separator()
        {
            Gcode.Add("");
        }

        private static void G01X(double x)
        {
            Gcode.Add(string.Format("G01 X{0}", Convert.ToString(x)));
        }

        private static void G01Y(double y)
        {
            Gcode.Add(string.Format("G01 Y{0}", Convert.ToString(y)));
        }

        private static void G01Z(double z)
        {
            Gcode.Add("G01 Z" + Convert.ToString(z));
        }

        private static void G01Xy(double x, double y)
        {
            Gcode.Add("G01 X" + Convert.ToString(x) + " Y" + Convert.ToString(y));
        }

        private static void G02(double r, Point center)
        {
            Gcode.Add("G02 X" + Convert.ToString(center.X - r) + " Y" + Convert.ToString(center.Y) + " I" + Convert.ToString(center.X) + " J" + Convert.ToString(center.Y));
        }

        private static void G00X(double x)
        {
            Gcode.Add("G00 X" + Convert.ToString(x));
        }

        private static void G00Y(double y)
        {
            Gcode.Add("G00 Y" + Convert.ToString(y));
        }

        private static void G00Z(double z)
        {
            Gcode.Add("G00 Z" + Convert.ToString(z));
        }

        private static void GSetFeedRate(double feedRate)
        {
            Gcode.Add("F" + Convert.ToString(feedRate));
        }

        private static void AddModel(List<ExtendedOpenGlPoint> list)
        {
            TrajectoryStor.AddModel(list);      // добавление нового листа который содержит список точек (траекторию)
        }

        public static void Generate(Project project)
        {
            Flush();
            Init(project.Settings.SafeDistance + project.Settings.Height);

            foreach (var iter in project.Operations)
            {
                if (iter.Shape.Name.Substring(0, 6) == "BoltHo") GBoltHole(project, iter);
                if (iter.Shape.Name.Substring(0, 6) == "Pocket") GPocket(project, iter);
            }

            EndMills(project.Settings.SafeDistance + project.Settings.Height);
        }

        private static void GBoltHole(Project project, Operation iter)
        {
            var externalRadius = ((BoltHole)iter.Shape).Radius;
            var internalRadius = ((BoltHole)iter.Shape).InternalRadius;
            var lenAll = ((BoltHole)iter.Shape).LenAll;
            var len = ((BoltHole)iter.Shape).Length;
            var tooldiam = project.Settings.ToolDiam;
            const double stepY = 1;

            var stepR = (externalRadius - internalRadius) / ((lenAll - len) / stepY); // рассчитываем шаг для радиуса

            var enumerator = iter.Location.LocationsList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var list = new List<ExtendedOpenGlPoint>();
                var color = new float[] { 0, 0, 0 };
                //                GCodeGenerator.Add_Model(list);

                var center = new Point(enumerator.Current.X, enumerator.Current.Y);
                list.Add(
                    new ExtendedOpenGlPoint
                    (
                        center.X,
                        project.Settings.Height + project.Settings.SafeDistance,
                        center.Y,
                        0,
                        new float[] { 0, 0, 0 }
                    )
                );

                G00Z(project.Settings.SafeDistance);
                G00X(center.X);
                G00Y(center.Y);
                G01Z(project.Settings.Height);

                var stR = externalRadius;

                if (externalRadius >= tooldiam)
                {
                    for (var i = 0; i <= (lenAll - len) / stepY; i++, stR -= stepR)
                    {
                        for (var j = 0; j < stR / tooldiam; j++)
                        {
                            Circle(list, j * tooldiam, project.Settings.Height - i * stepY, center);
                            G02(j * tooldiam, center);
                            if (!(j + 1 < externalRadius / (2 * tooldiam))) continue;
                            list.Add(new ExtendedOpenGlPoint(
                                    center.X,
                                    project.Settings.Height - i * stepY,
                                    center.Y + j * tooldiam,
                                    0,
                                    new float[] { 0, 0, 0 }));
                            G01Xy(center.X, center.Y + j * tooldiam);
                        }
                        /* Сюда надо вставить заключительный проход по окружности */
                        list.Add(new ExtendedOpenGlPoint(
                                center.X,
                                project.Settings.Height - i * stepY,
                                center.Y,
                                0,
                                new float[] { 0, 0, 0 }));
                        G01Xy(center.X, center.Y);
                        Separator();
                        if (!(i + 1 < len)) continue;
                        list.Add(
                            new ExtendedOpenGlPoint(
                                center.X,
                                project.Settings.Height - (i + 1) * stepY,
                                center.Y,
                                0,
                                new float[] { 0, 0, 0 }));
                        G01Z(project.Settings.Height - (i + 1) * stepY);
                    }

                    for (var i = len; i <= lenAll / stepY; i++)
                    {
                        for (var j = 0; j < internalRadius / tooldiam; j++)
                        {
                            Circle(list, j * tooldiam, project.Settings.Height - i * stepY, center);
                            G02(j * tooldiam, center);
                            if (!(j + 1 < internalRadius / (2 * tooldiam))) continue;
                            list.Add(new ExtendedOpenGlPoint(
                                    center.X,
                                    project.Settings.Height - i * stepY,
                                    center.Y + j * tooldiam,
                                    0,
                                    new float[] { 0, 0, 0 }));
                            G01Xy(center.X, center.Y + j * tooldiam);
                        }
                        /* Сюда надо вставить заключительный проход по окружности */
                        list.Add(new ExtendedOpenGlPoint(
                                    center.X,
                                    project.Settings.Height - i * stepY,
                                    center.Y,
                                    0,
                                    new float[] { 0, 0, 0 }));
                        G01Xy(center.X, center.Y);
                        Separator();
                        if (!(i + 1 < len)) continue;
                        list.Add(new ExtendedOpenGlPoint(
                                center.X,
                                project.Settings.Height - (i + 1) * stepY,
                                center.Y,
                                0,
                                new float[] { 0, 0, 0 }));
                        G01Z(project.Settings.Height - (i + 1) * stepY);
                    }
                    list.Add(new ExtendedOpenGlPoint(
                                center.X,
                                project.Settings.Height + project.Settings.SafeDistance,
                                center.Y,
                                0,
                                new float[] { 0, 0, 0 }));
                    G01Z(project.Settings.Height + 2);
                    G00Z(project.Settings.Height + project.Settings.SafeDistance);
                    Separator();
                }
                TrajectoryStor.AddModel(list);
            }
        }

        private static void Circle(ICollection<ExtendedOpenGlPoint> list, double r, double y, Point center)
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
                list.Add(new ExtendedOpenGlPoint(
                        r * Math.Sin(angle) + center.X,
                        y,
                        r * Math.Cos(angle) + center.Y,
                        0,
                        new float[] { 0, 0, 0 }));
            }
            list.Add(new ExtendedOpenGlPoint(
                    start.X + center.X,
                    y,
                    start.Y + center.Y,
                    0,
                    new float[] { 0, 0, 0 }));
        }

        private static void GPocket(Project project, Operation iter) // X ~ width ; Y ~ length
        {
            var list = new List<ExtendedOpenGlPoint>();
            var enumerator = iter.Location.LocationsList.GetEnumerator();
            var length = ((Pocket)iter.Shape).Length;
            var height = ((Pocket)iter.Shape).Height;
            var width = ((Pocket)iter.Shape).Width;
            enumerator.MoveNext();
            var origin = new Point(
                enumerator.Current.X - width / 2,
                enumerator.Current.Y - length / 2);       // для обычного покета, не матрично/радиально расположенных
            var x = origin.X;
            var y = origin.Y;
            
            const double stepY = 1;
            var toolDiam = project.Settings.ToolDiam;
            var toolRadius = toolDiam / 2;

            GSetFeedRate(180);      // Стандартная скорость подачи пока нет такого поля в проекте
            G00X(x + toolRadius);
            G00Y(y + toolRadius);
            G01Z(project.Settings.Height);

            double temp = 0;

            list.Add(new ExtendedOpenGlPoint(
                    x + toolRadius,
                    project.Settings.Height + project.Settings.SafeDistance,
                    y + toolRadius,
                    0,
                    new float[] { 0, 0, 0 }));
            list.Add(new ExtendedOpenGlPoint(
                        x + toolRadius,
                        project.Settings.Height,
                        y + toolRadius,
                        0,
                        new float[] { 0, 0, 0 }));
            for (var i = project.Settings.Height; i >= project.Settings.Height - height; i -= stepY)         // дописать именно генерацию кодов...
            {
                G01Z(i);
                for (double j = 1; j <= width / toolDiam; j++)
                {
                    list.Add(new ExtendedOpenGlPoint(
                            x + toolRadius + temp,
                            i,
                            y + toolRadius + ((j * length) % (length * 2)) - toolDiam * (j % 2),
                            0,
                            new float[] { 0, 0, 0 }));
                    G01Xy(x + toolRadius + temp, y + toolRadius + ((j * length) % (length * 2)) - toolDiam * (j % 2));
                    if (toolRadius + temp + toolDiam < width)
                    {
                        temp += toolDiam;
                        list.Add(new ExtendedOpenGlPoint(
                                x + toolRadius + temp,
                                i,
                                y + toolRadius + ((j * length) % (length * 2)) - toolDiam * (j % 2),
                                0,
                                new float[] { 0, 0, 0 }));
                        G01Xy(x + toolRadius + temp, y + toolRadius + ((j * length) % (length * 2)) - toolDiam * (j % 2));
                    }
                }
                temp = 0;
                list.Add(new ExtendedOpenGlPoint(x + toolRadius, i, y + toolRadius, 0, new float[] { 0, 0, 0 }));
                G01Xy(x + toolRadius, y + toolRadius);
                list.Add(new ExtendedOpenGlPoint(x + width - toolRadius, i, y + toolRadius, 0, new float[] { 0, 0, 0 }));                // Периметр
                G01Xy(x + width - toolRadius, y + toolRadius);
                list.Add(new ExtendedOpenGlPoint(x + width - toolRadius, i, y + length - toolRadius, 0, new float[] { 0, 0, 0 }));
                G01Xy(x + width - toolRadius, y + length - toolRadius);
                list.Add(new ExtendedOpenGlPoint(x + toolRadius, i, y + length - toolRadius, 0, new float[] { 0, 0, 0 }));
                G01Xy(x + toolRadius, y + length - toolRadius);
                list.Add(new ExtendedOpenGlPoint(x + toolRadius, i, y + toolRadius, 0, new float[] { 0, 0, 0 }));
                G01Xy(x + toolRadius, y + toolRadius);

                Separator();
            }
            list.Add(new ExtendedOpenGlPoint(x + toolRadius, project.Settings.Height + project.Settings.SafeDistance, y + toolRadius, 0, new float[] { 0, 0, 0 }));
            TrajectoryStor.AddModel(list);
            G01Z(project.Settings.Height + 2);
            G00Z(project.Settings.Height + project.Settings.SafeDistance);
            Separator();
        }

        public static void Flush()
        {
            Gcode.Clear();
            TrajectoryStor.Clear();
        }

    }
}
