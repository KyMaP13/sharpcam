using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public interface IGCamPrimitive
    {
    }

    public class GCamLine : IGCamPrimitive
    {
        public double X0, Y0, X1, Y1;
    }

    public class GCamArc : IGCamPrimitive
    {
        
    }

}
