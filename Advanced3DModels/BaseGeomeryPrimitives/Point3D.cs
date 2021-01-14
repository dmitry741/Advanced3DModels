using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseGeomeryPrimitives
{
    public class Point3D
    {
        readonly double[] _X;

        public Point3D(double X, double Y, double Z)
        {
            _X = new double[3];
            _X[0] = X;
            _X[1] = Y;
            _X[2] = Z;
        }

        public double X
        {
            get { return _X[0]; }
            set { _X[0] = value; }
        }

        public double Y
        {
            get { return _X[1]; }
            set { _X[1] = value; }
        }

        public double Z
        {
            get { return _X[2]; }
            set { _X[2] = value; }
        }
    }
}
