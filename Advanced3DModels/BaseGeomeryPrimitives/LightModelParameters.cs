﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;

namespace Models3DLib
{
    public class LightModelParameters
    {
        public Point3D Point { get; set; }
        public Vector3 Normal { get; set; }
        public bool Reflection { get; set; }
        public float ReflectionBrightness { get; set; }
        public float ReflcetionCone { get; set; }
        public Color BaseColor { get; set; }
        public IEnumerable<AbstractLightSource> LightSources { get; set; }
        public Point3D PointObserver { get; set; }
        public IFog Fog { get; set; }
    }
}
