using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Model.Primitives;
using Model.Tree;
using SharpGL;
using SharpGL.SceneGraph;
using System.Collections.ObjectModel;
using Model;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Viewer.Tree;

namespace Viewer
{

    // <summary>
    // Логика взаимодействия для MainWindow.xaml
    // </summary>

    public partial class MainWindow
    {
        private readonly Project testProject = Samples.DummyProject;
        private uint counter;
        private readonly List<List<double[]>> detailCache = new List<List<double[]>>();

        public MainWindow()
        {
            InitializeComponent();
            var x = new ObservableCollection<Object>();
            
            var y = new ContainerNode {Items = testProject.Operations, Name = "Operations"};

            x.Add(testProject.Settings);
            x.Add(y);
            OperationsTree.ItemsSource = x;
        }

        #region OpenGL stuff

        float zoom; // а эта нужна для зума колёсиком мышки

        private void OpenGlControlOpenGlDraw(object sender, OpenGLEventArgs args)
        {
            //  Get the OpenGL instance that's been passed to us.
            OpenGL gl = args.OpenGL;
            gl.ClearColor(255, 255, 0.9f, 1.0f);
        
            //  Clear the color and depth buffers.
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            //  Reset the modelview matrix.

            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.LoadIdentity();

            //gl.LookAt(0, 50, 300, 50, 50, 0, 0, 100, 0);// тестовая фича для просмотра

            //label1.Content = zoom;
            var dr = gl.RenderContextProvider.Height*15 + zoom * 15;
            dr /= 1000;
            //label2.Content = dr;
            gl.Translate((float)drag.X / dr, -(float)drag.Y / dr, zoom);//перемещение с зажатой левой кнопкой мыши

            var bill = testProject.Settings;

            gl.Translate(bill.Length / 2, bill.Height / 2, bill.Width / 2);//высчитывается из размера заготовки
            gl.Rotate((float)rotation.Y, (float)rotation.X, 0); // вращение с зажатой средней кнопкой мыши
            gl.Translate(-bill.Length / 2, -bill.Height / 2, -bill.Width / 2);
            
            //этот блок с отрисовкой осей придётся всегда пересчитывать
            gl.Begin(OpenGL.GL_LINES);//рисуем все оси
            gl.Color(0.0f, 0.0f, 0.0f);
            gl.Vertex(0, 0, 0);
            gl.Vertex(Math.Abs(zoom) / 15, 0, 0);

            gl.Vertex(0, 0, 0);
            gl.Vertex(0, Math.Abs(zoom) / 15, 0);

            gl.Vertex(0, 0, 0);
            gl.Vertex(0, 0, Math.Abs(zoom) / 15);
            gl.End();

            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            gl.Begin(OpenGL.GL_TRIANGLES);
            gl.Color(0.0f, 1.0f, 0.0f);      // рисуем ось Y
            gl.Vertex(-Math.Abs(zoom) / 250, Math.Abs(zoom) / 15, 0);
            gl.Vertex(0, Math.Abs(zoom) / 10, 0);
            gl.Vertex(Math.Abs(zoom) / 250, Math.Abs(zoom) / 15, 0);
            gl.End();

            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            gl.Begin(OpenGL.GL_TRIANGLES);
            gl.Color(1.0f, 0.0f, 0.0f);      // рисуем ось X
            gl.Vertex(Math.Abs(zoom) / 15, -Math.Abs(zoom) / 250, 0);
            gl.Vertex(Math.Abs(zoom) / 10, 0, 0);
            gl.Vertex(Math.Abs(zoom) / 15, Math.Abs(zoom) / 250, 0);
            gl.End();

            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            gl.Begin(OpenGL.GL_TRIANGLES);
            gl.Color(0.0f, 0.0f, 1.0f);      // рисуем ось Z
            gl.Vertex(0, -Math.Abs(zoom) / 250, Math.Abs(zoom) / 15);
            gl.Vertex(0, 0, Math.Abs(zoom) / 10);
            gl.Vertex(0, Math.Abs(zoom) / 250, Math.Abs(zoom) / 15);
            gl.End();

            ///////////////////////////////////

            //var Bill = this.testProject.Settings;
            Billet.Draw(gl, bill.Height, bill.Length, bill.Width); // заготовка

            var operations = testProject.Operations;//колличество операций

            var boltReg = new Regex("BoltHole");
            var pocketReg = new Regex("Pocket");
            if (detailCache.Count != operations.Count) detailCache.Clear();

            for (int i = 0; i < operations.Count; i++)//главный цикл отрисовки
            {
                //label1.Content = operations.Count;
                label2.Content = i;
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
                        label1.Content = p.Count;
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

            gl.End();            

            gl.Flush();
        }

        private void OpenGlControlOpenGlInitialized(object sender, OpenGLEventArgs args)
        {
            //  Enable the OpenGL depth testing functionality.
            args.OpenGL.Enable(OpenGL.GL_DEPTH_TEST);
        }

        private void OpenGlControlResized(object sender, OpenGLEventArgs args)
        {
            var gl = args.OpenGL;
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();
            zoom = -gl.RenderContextProvider.Width / gl.RenderContextProvider.Height * 150; //изменение размера самой картинки при изменении размера окна
            gl.Perspective(45.0f, gl.RenderContextProvider.Width / (float)gl.RenderContextProvider.Height, 0.1f, 2000.0f);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }

        //блок навигации
        private void OpenGlControlMouseWheel(object sender, MouseWheelEventArgs e) // действие по колёсику мыши
        {
            if (e.Delta > 0)
            {
                zoom += 10;
            }
            else
            {
                zoom -= 10;
            }
        }
        
        Point start=new Point(0,0);
        Vector rotation = new Vector(0, 0);
        Vector twist = new Vector(0, 0);
        Vector drag = new Vector(0, 0);
        Vector offset = new Vector(0, 0);
        bool dragging;
        bool rotated;
        private IInputElement OpenGLControl = null;
        private void OpenGlControlMouseMove(object sender, MouseEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                var stop = e.GetPosition(OpenGLControl);
                rotation = stop - start + twist;
                rotated = false;
            }
            else if (e.LeftButton == MouseButtonState.Pressed)
            {
                var stop = e.GetPosition(OpenGLControl);
                drag = stop - start + offset;
                dragging = false;
            } else {
                start = e.GetPosition(OpenGLControl);
                label3.Content = "X: "+(start.X-200);
                label4.Content = "Y: "+(start.Y-20);
                if (!dragging)
                {
                    offset = drag;
                    dragging = true;
                }
                if (!rotated)
                {
                    twist = rotation;
                    rotated = true;
                }
            }  
        }
        #endregion        //конец блока навигации

        private void Button1Click(object sender, RoutedEventArgs e)
        {   
            var op = new Operation {
                        Shape = new Model.Primitives.BoltHole {
                            Name = "BoltHole" + counter,
                            Radius = 3,
                            InternalRadius = 2,
                            Length = 4,
                            LenAll = 7,
                            Y = 10,
                        },
                        Location = new CustomLocations {
                            Locations = new FrameList {
                                new Frame {
                                    X = 5,
                                    Y = 5,
                                    Angle = 220.5
                                },
                            }
                        }
                    };
            counter++;
            testProject.Operations.Add(op);

            GCodeGenerator.Flush();
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            var item = ((System.Windows.Controls.Button)sender).DataContext as SmartItem;
            if (item != null)
            {
                item.AddEvent();
            }

            foreach (var t in testProject.Operations)
            {
                var j = t.Location.LocationsList.Count();
                label1.Content += " | " + j;
            }
            GCodeGenerator.Flush();
        }
        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            var item = ((System.Windows.Controls.Button)sender).DataContext as SmartItem;
            if (item != null)
            {
                item.DeleteEvent();
            }
            foreach (var operation in testProject.Operations)
            {
                var j = operation.Location.LocationsList.Count();
                label1.Content += " ; " + j;
            }

            GCodeGenerator.Flush();
        }
        private void UpClick(object sender, RoutedEventArgs e)
        {
            var item = ((System.Windows.Controls.Button)sender).DataContext as SmartItem;
            if (item != null)
            {
                detailCache.Clear();
                item.MoveUpEvent();
            }
            GCodeGenerator.Flush();
        }
        private void DownClick(object sender, RoutedEventArgs e)
        {
            var item = ((System.Windows.Controls.Button)sender).DataContext as SmartItem;
            if (item != null)
            {
                detailCache.Clear();
                item.MoveDownEvent();
            }
            GCodeGenerator.Flush();
        }

        private void Button3Click(object sender, RoutedEventArgs e)
        {
            var op = new Operation
            {
                Shape = new Model.Primitives.Pocket
                {
                    Name = "Pocket" + counter,
                    Height = 5,
                    Width = 6,
                    Length = 8,
                    Y = 10
                },
                Location = new CustomLocations
                {
                    Locations = new FrameList {
                                new Frame {
                                    X = 20,
                                    Y = 20,
                                    Angle = 220.5

                                },
                            }
                }
            };
            counter++;
            testProject.Operations.Add(op);
            GCodeGenerator.Flush();
        }

        private void GcodeExport(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.SaveFileDialog
                      {
                          FileName = "Progect",
                          DefaultExt = ".nc",
                          Filter = "G-code file (.nc)|*.nc"
                      };

            var result = dlg.ShowDialog();
            GCodeGenerator.Generate(testProject);

            if (result != true) return;
            try
            {
                var file = new StreamWriter(dlg.FileName);
                foreach (var line in GCodeGenerator.Gcode)
                {
                    file.WriteLine(line);
                }
                file.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Good Bye Cruel World...");
            }
        }

        private void GcodeTrajectory(object sender, RoutedEventArgs e)
        {
            GCodeGenerator.Generate(testProject);
        }

        private void Button2Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var b = (Operation)OperationsTree.SelectedItem;
                var remIndex = testProject.Operations.IndexOf(b);
                testProject.Operations.RemoveAt(remIndex);
                detailCache.RemoveAt(remIndex);
            }
            catch { }
        }
    }
}