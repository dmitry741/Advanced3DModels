using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Models3DLib
{
    /// <summary>
    /// Класс представлющий источник света как прожектор (лучи параллельны друг другу).
    /// </summary>
    public class ProjectorLightSource : ILightSource
    {
        public Vector3 VectorLightSource { get; set; }
        public float Weight { get; set; } = 1.0f;

        public Vector3 GetRay(IPoint3D point)
        {
            return VectorLightSource;
        }
    }
}
