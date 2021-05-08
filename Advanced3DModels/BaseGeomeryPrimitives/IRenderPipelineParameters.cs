using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models3DLib
{
    interface IRenderPipelineParameters
    {
        float TranslateX { get; set; }
        float TranslateY { get; set; }
        float TranslateZ { get; set; }
        RenderModelType renderModelType { get; set; }
        bool PerspectiveEnable { get; set; }
        IPerspectiveTransform PerspectiveTransform { get; set; }
        IPoint3D CenterPerspective { get; set; }
        bool FogEnable { get; set; }
        IFog Fog { get; set; }
        IEnumerable<ILightSource> lightSources { get; set; }
        IPoint3D pointObserver { get; set; }
        RenderFillTriangle renderFillTriangle { get; set; }
    }
}
