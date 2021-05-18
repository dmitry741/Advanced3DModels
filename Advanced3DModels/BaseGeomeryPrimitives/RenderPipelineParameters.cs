using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models3DLib
{
    public class RenderPipelineParameters : IRenderPipelineParameters
    {
        public float TranslateX { get; set; }
        public float TranslateY { get; set; }
        public float TranslateZ { get; set; }
        public RenderModelType renderModelType { get; set; }
        public bool PerspectiveEnable { get; set; }
        public IPerspectiveTransform PerspectiveTransform { get; set; }
        public IPoint3D CenterPerspective { get; set; }
        public bool FogEnable { get; set; }
        public IFog Fog { get; set; }
        public IEnumerable<ILightSource> lightSources { get; set; }
        public IPoint3D pointObserver { get; set; }
        public RenderFillTriangle renderFillTriangle { get; set; }
    }
}
