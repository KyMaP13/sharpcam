using System;
using System.Windows;
//using System.Windows.Controls;
using System.Windows.Input;
using SharpGL;
using SharpGL.SceneGraph;
using System.Collections.ObjectModel;
using Model;
using Model.Samples;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Viewer
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Project testProject = Samples.DummyProject;
        private ObservableCollection<Project> source = new ObservableCollection<Project>();
        private uint counter = 0;
        List<List<double[]>> detailCache = new List<List<double[]>>();

        public MainWindow()
        {
            InitializeComponent();
            var x = new ObservableCollection<Object>();
            
            var y = new ContainerNode();
            y.Items = this.testProject.Operations;
            y.Name = "Operations";// (see MainWindow constructor)";

            x.Add(this.testProject.Settings);
            x.Add(y);
            OperationsTree.ItemsSource = x;
        }

        #region OpenGL stuff

        float zoom; // а эта нужна для зума колёсиком мышки

        private void OpenGLControl_OpenGLDraw(object sender, OpenGLEventArgs args)
        {
            //  Get the OpenGL instance that's been passed to us.
            OpenGL gl = args.OpenGL;
            List<List<double[]>> carcass = new List<List<double[]>>();
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

            var Bill = this.testProject.Settings;

            gl.Translate(Bill.Length / 2, Bill.Height / 2, Bill.Width / 2);//высчитывается из размера заготовки
            gl.Rotate((float)rotation.Y, (float)rotation.X, 0); // вращение с зажатой средней кнопкой мыши
            gl.Translate(-Bill.Length / 2, -Bill.Height / 2, -Bill.Width / 2);
            
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
            Billet.Draw(gl, Bill.Height, Bill.Length, Bill.Width); // заготовка

            var operations = this.testProject.Operations;//колличество операций

            var oper = operations.GetEnumerator();

            Regex BoltReg = new Regex("BoltHole");
            Regex PocketReg = new Regex("Pocket");
            for (int i = 0; i < operations.Count; i++)//главный цикл отрисовки
            {
                //label1.Content = operations.Count;
                label2.Content = i;
                string ShapeName = operations[i].Shape.Name.ToString();

                
                if (BoltReg.IsMatch(ShapeName))
                {
                    var Bolt = (Model.BoltHole)operations[i].Shape;
                    var boltlocation = operations[i].Location.LocationsList.GetEnumerator();
                    while (boltlocation.MoveNext())
                    {
                        if (Bolt.modified || boltlocation.Current.modified)
                        {

                            try
                            {
                                detailCache.RemoveAt(i);
                            }
                            catch { }
                            Point location = new Point(boltlocation.Current.X, boltlocation.Current.Y);
                            detailCache.Insert(i, BoltHole.ReCalc(Bolt, 0.5, location)); //здесь уже всё ок, кроме величины шага
                            boltlocation.Current.isDrawn();
                            Bolt.isDrawn();//значит в кэше лежит актуальная информация
                        }
                        else
                        {
                            Point location = new Point(boltlocation.Current.X, boltlocation.Current.Y);
                            BoltHole.Draw(gl, location, detailCache[i]); //здесь уже всё ок, кроме величины шага
                        }
                    }
                }

                if (PocketReg.IsMatch(ShapeName))
                {
                    var Poc = (Model.Pocket)operations[i].Shape;
                    var poclocation = operations[i].Location.LocationsList.GetEnumerator();
                    while (poclocation.MoveNext())
                    {
                        if (Poc.modified || poclocation.Current.modified)
                        {
                            try
                            {
                                detailCache.RemoveAt(i);
                            }
                            catch { }
                            Point location = new Point(poclocation.Current.X, poclocation.Current.Y);
                            List<double[]> p = Pocket.ReCalc(Poc, 0.5, location);
                            label1.Content = p.Count;
                            detailCache.Insert(i, p); //здесь уже всё ок, кроме величины шага
                            poclocation.Current.isDrawn();
                            Poc.isDrawn();//значит в кэше лежит актуальная информация
                        }
                        else
                        {
                            Point location = new Point(poclocation.Current.X, poclocation.Current.Y);
                            Pocket.Draw(gl, location, detailCache[i]); //здесь уже всё ок, кроме величины шага
                        }
                    }
                }
            }

            

            //отрисовщик траекторий
            gl.Begin(OpenGL.GL_LINE_STRIP);
            gl.Color(1f, 0, 0);

            foreach (var operation in Tree.GCode.point)
            {
                foreach (var point in operation)
                {
                    gl.Vertex(point);
                }
            }

            gl.End();            

            gl.Flush();
        }

        private void OpenGLControl_OpenGLInitialized(object sender, OpenGLEventArgs args)
        {
            //  Enable the OpenGL depth testing functionality.
            args.OpenGL.Enable(OpenGL.GL_DEPTH_TEST);
        }

        private void OpenGLControl_Resized(object sender, OpenGLEventArgs args)
        {
            // Get the OpenGL instance.
            OpenGL gl = args.OpenGL;

            // Load and clear the projection matrix.
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();
            zoom = -gl.RenderContextProvider.Width / gl.RenderContextProvider.Height * 150; //изменение размера самой картинки при изменении размера окна
            // Perform a perspective transformation
            gl.Perspective(45.0f, (float)gl.RenderContextProvider.Width / (float)gl.RenderContextProvider.Height, 0.1f, 2000.0f);
            // Load the modelview.
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }

        //блок навигации
        private void OpenGLControl_MouseWheel(object sender, MouseWheelEventArgs e) // действие по колёсику мыши
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
        bool dragging = false;
        bool rotated = false;
        private IInputElement OpenGLControl = null;
        private void OpenGLControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                Point stop = e.GetPosition(OpenGLControl);
                rotation = stop - start + twist;
                rotated = false;
            }
            else if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point stop = e.GetPosition(OpenGLControl);
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

        private void button1_Click(object sender, RoutedEventArgs e)
        {   
            var op = new Operation {
                        Shape = new Model.BoltHole {
                            Name = "BoltHole" + this.counter.ToString(),
                            Radius = 3,
                            radius = 2,
                            Length = 4,
                            lenAll = 7,
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
            this.counter++;
            this.testProject.Operations.Add(op);

            Tree.GCode.flush();
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            SmartItem item = ((System.Windows.Controls.Button)sender).DataContext as SmartItem;
            if (item != null)
            {
                item.AddEvent();
            }
            for (int i = 0; i < testProject.Operations.Count; i++)
            {
                int j = 0;
                foreach (var x in testProject.Operations[i].Location.LocationsList)
                {
                    j++;
                }
                label1.Content += " | " + j.ToString();
            }
            Tree.GCode.flush();
        }
        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            SmartItem item = ((System.Windows.Controls.Button)sender).DataContext as SmartItem;
            if (item != null)
            {
                item.DeleteEvent();
            }
            for (int i = 0; i < testProject.Operations.Count; i++)
            {
                int j = 0;
                foreach (var x in testProject.Operations[i].Location.LocationsList)
                {
                    j++;
                }
                label1.Content += " ; " + j.ToString();
            }

            Tree.GCode.flush();
        }
        private void UpClick(object sender, RoutedEventArgs e)
        {
            SmartItem item = ((System.Windows.Controls.Button)sender).DataContext as SmartItem;
            if (item != null)
            {
                item.MoveUpEvent();
            }
            Tree.GCode.flush();
        }
        private void DownClick(object sender, RoutedEventArgs e)
        {
            SmartItem item = ((System.Windows.Controls.Button)sender).DataContext as SmartItem;
            if (item != null)
            {
                item.MoveDownEvent();
            }
            Tree.GCode.flush();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            var op = new Operation
            {
                Shape = new Model.Pocket
                {
                    Name = "Pocket" + this.counter.ToString(),
                    height = 5,
                    width = 6,
                    length = 8,
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
            this.counter++;
            this.testProject.Operations.Add(op);
            Tree.GCode.flush();
        }

        private void _gcodeExport(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Progect";
            dlg.DefaultExt = ".nc";
            dlg.Filter = "G-code file (.nc)|*.nc";

            Nullable<bool> result = dlg.ShowDialog();
            Tree.GCode.generate(this.testProject);

            if (result == true)
            {
                try
                {
                    StreamWriter file = new StreamWriter(dlg.FileName);
                    foreach (String line in Tree.GCode.gcode)
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
        }

        private void _gcodeTrajectory(object sender, RoutedEventArgs e)
        {
            Tree.GCode.generate(this.testProject);
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            //this.testProject.Operations[0].Location.FrameList;
            
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var b = (Model.Operation)OperationsTree.SelectedItem;
                int remIndex = this.testProject.Operations.IndexOf(b);
                this.testProject.Operations.RemoveAt(remIndex);
                detailCache.RemoveAt(remIndex);
            }
            catch { }
        }
    }
}