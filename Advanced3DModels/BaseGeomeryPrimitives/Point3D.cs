using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;

namespace Models3DLib
{
    public class Point3D
    {
        readonly float[] _X;

        public Point3D(float X, float Y, float Z)
        {
            _X = new float[3];
            _X[0] = X;
            _X[1] = Y;
            _X[2] = Z;
        }

        public float X
        {
            get { return _X[0]; }
            set { _X[0] = value; }
        }

        public float Y
        {
            get { return _X[1]; }
            set { _X[1] = value; }
        }

        public float Z
        {
            get { return _X[2]; }
            set { _X[2] = value; }
        }

        public Vector3 ToVector3()
        {
            return new Vector3(X, Y, Z);
        }

        public Vector4 ToVector4()
        {
            return new Vector4(X, Y, Z, 1.0f);
        }

        public PointF ToPointF()
        {
            return new PointF(X, Y);
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z}";
        }
    }
}
