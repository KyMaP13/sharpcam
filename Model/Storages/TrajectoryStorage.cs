using System.Collections.Generic;

namespace Model.Storages
{
    public class TrajectoryStorage
    {
        private List<List<ExtendedOpenGlPoint>> list;
        
        public TrajectoryStorage()
        {
            list = new List<List<ExtendedOpenGlPoint>>();
        }

        public List<List<ExtendedOpenGlPoint>> GetTrajectorys()
        {
              return list;
        }

        public void AddModel(List<ExtendedOpenGlPoint> operationStorage)    // модель это List<ExtendedOpenGLPoint> лучше добавлть моделями а не точками
        {
            (GetTrajectorys()).Add(operationStorage);
        }

        public void Clear(){
            list.Clear();
        }
    }
}
