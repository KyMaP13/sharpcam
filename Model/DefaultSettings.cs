using Model.Tree;

namespace Model
{
    /// <summary>
    /// Used as parameter to generate operations
    /// </summary>
    public static class Samples
    {
        /// <summary>
        /// Sample with two BoltHole operations
        /// </summary>
        public static Project DummyProject
        {
            get
            {
                return new Project
                {
                    Operations = new OperationsList(),
                    Settings = new ProjectSettings
                    {
                        Height = 10,
                        Width = 40,
                        Length = 40,
                        ToolDiam = 2,
                        SafeDistance = 15,
                        FeedRate = 500
                    }
                };
            }
        }
    }
}
