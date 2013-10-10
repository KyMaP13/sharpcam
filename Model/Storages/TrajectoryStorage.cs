using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Model.Storages
{
    public class TrajectoryStorage
    {
        protected List<List<ExtendedOpenGLPoint>> list = new List<List<ExtendedOpenGLPoint>>();
        
        public TrajectoryStorage()
        {
            var list = new List<ExtendedOpenGLPoint>();
        }

        public List<List<ExtendedOpenGLPoint>> GetTrajectorys()
        {
              return list;
        }
/*
        public void AddPoint(List<ExtendedOpenGLPoint> list)    
        {
            list.Add(list);
        }
*/
        public void AddModel(List<ExtendedOpenGLPoint> list)    // модель это List<ExtendedOpenGLPoint> лучше добавлть моделями а не точками
        {
            (this.GetTrajectorys()).Add(list);
        }

        public void Clear(){
            list.Clear();
        }
    }
}
