using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Samples
{
    /// <summary>
    /// Used as parameter to generate operations
    /// </summary>
    public enum OperationType
    {
        BoltHole
    }
    public class Samples
    {

        private const int MaxValue = 1000; //const is static too

        /// <summary>
        /// Sample with no operations and empty settings
        /// </summary>
        public static Project EmptyProject
        {
            get
            {
                return new Project
                {
                    Operations = new OperationsList(),
                    Settings = new ProjectSettings()
                };
            }
        }

        /// <summary>
        /// Sample with two BoltHole operations
        /// </summary>
        public static Project DummyProject
        {
            get
            {
                return new Project
                {
                    Operations = new OperationsList
                {
                },
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
        /// <summary>
        /// Project with uninitialized settings and operations
        /// </summary>
        public static Project NullProject
        {
            get
            {
                return new Project();
            }
        }
    }
}
