using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using Model;
using Model.Tree;
using SharpGL;

namespace Viewer
{
    public static class OperationDrawer
    {
        public static void DrawOperation(OpenGL gl, Project testProject, Vector rotation, List<List<double[]>> detailCache)
        {
            var bill = testProject.Settings;

            gl.Translate(bill.Length / 2, bill.Height / 2, bill.Width / 2);//высчитывается из размера заготовки
            gl.Rotate((float)rotation.Y, (float)rotation.X, 0); // вращение с зажатой средней кнопкой мыши
            gl.Translate(-bill.Length / 2, -bill.Height / 2, -bill.Width / 2);

            //var Bill = this.testProject.Settings;
            Billet.Draw(gl, bill.Height, bill.Length, bill.Width); // заготовка

            var operations = testProject.Operations;//колличество операций

            var boltReg = new Regex("BoltHole");
            var pocketReg = new Regex("Pocket");
            if (detailCache.Count != operations.Count) detailCache.Clear();

            for (var i = 0; i < operations.Count; i++)//главный цикл отрисовки
            {
                var shapeName = operations[i].Shape.Name;

                
                if (boltReg.IsMatch(shapeName))
                {
                    var bolt = (Model.Primitives.BoltHole)operations[i].Shape;
                    var boltlocation = operations[i].Location.LocationsList.GetEnumerator();
                    while (boltlocation.MoveNext())
                    {
                        if (bolt.Modified || boltlocation.Current.Modified || detailCache.Count<=i)
                        {

                            try
                            {
                                detailCache.RemoveAt(i);
                            }
                            catch{}
                            var location = new Point(boltlocation.Current.X, boltlocation.Current.Y);
                            detailCache.Insert(i, BoltHole.ReCalc(bolt, 0.5, location)); //здесь уже всё ок, кроме величины шага
                            boltlocation.Current.IsDrawn();
                            bolt.IsDrawn();//значит в кэше лежит актуальная информация
                        }
                        else
                        {
                            var location = new Point(boltlocation.Current.X, boltlocation.Current.Y);
                            BoltHole.Draw(gl, detailCache[i]); //здесь уже всё ок, кроме величины шага
                        }
                    }
                }

                if (!pocketReg.IsMatch(shapeName)) continue;
                var poc = (Model.Primitives.Pocket)operations[i].Shape;
                var poclocation = operations[i].Location.LocationsList.GetEnumerator();
                while (poclocation.MoveNext())
                {
                    if (poc.Modified || poclocation.Current.Modified || detailCache.Count <= i)
                    {
                        try
                        {
                            detailCache.RemoveAt(i);
                        }
                        catch { }
                        var location = new Point(poclocation.Current.X, poclocation.Current.Y);
                        var p = Pocket.ReCalc(poc, 0.5, location);
                        detailCache.Insert(i, p); //здесь уже всё ок, кроме величины шага
                        poclocation.Current.IsDrawn();
                        poc.IsDrawn();//значит в кэше лежит актуальная информация
                    }
                    else
                    {
                        var location = new Point(poclocation.Current.X, poclocation.Current.Y);
                        Pocket.Draw(gl, detailCache[i]); //здесь уже всё ок, кроме величины шага
                    }
                }
            }

            

            //отрисовщик траекторий
            gl.Begin(OpenGL.GL_LINE_STRIP);
            gl.Color(1f, 0, 0);

            var trajectorys = GCodeGenerator.TrajectoryStor.GetTrajectorys();
            foreach (var point in trajectorys.SelectMany(operation => operation))
            {
                gl.Vertex(point.GetCoordinates());
            }
        }
    }
}