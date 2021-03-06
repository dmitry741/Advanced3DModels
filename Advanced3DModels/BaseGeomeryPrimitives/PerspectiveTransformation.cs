﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models3DLib
{
    /// <summary>
    /// Класс реализующий перспективные преобразования.
    /// </summary>
    public class PerspectiveTransformation : IPerspectiveTransform
    {
        public IPoint3D Transform(IPoint3D point, IPoint3D center)
        {
            float X = center.Z / (center.Z - point.Z) * (point.X - center.X) + center.X;
            float Y = center.Z / (center.Z - point.Z) * (point.Y - center.Y) + center.Y;

            return ResolverInterface.ResolveIPoint3D(X, Y, point.Z);
        }
    }
}
