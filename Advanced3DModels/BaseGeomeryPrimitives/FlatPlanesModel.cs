using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models3DLib
{
    public class FlatPlanesModel : AbstractModel
    {
        public override IEnumerable<Triangle> GetTrianglesForRender(IEnumerable<ILightSource> lightSources,
            IPoint3D pointObserver,
            IFog ifog,
            RenderModelType renderType)
        {
            IEnumerable<Triangle> triangles = null;

            if (renderType == RenderModelType.FillFull)
            {
                IEnumerable<Plane> planesForRender = Planes.Where(x => x.VisibleBackSide || x.Normal.Z < 0);

                LightModelParameters lightModelParameters = new LightModelParameters
                {
                    LightSources = lightSources,
                    PointObserver = pointObserver,
                    Fog = ifog
                };

                foreach (Plane plane in planesForRender)
                {
                    lightModelParameters.Normal = plane.Normal;
                    lightModelParameters.Reflection = plane.Reflection;
                    lightModelParameters.ReflectionBrightness = plane.ReflectionBrightness;
                    lightModelParameters.ReflcetionCone = plane.ReflectionCone;

                    foreach (Point3DColor pc in plane.Points)
                    {
                        lightModelParameters.Point = pc;
                        lightModelParameters.BaseColor = pc.BaseColor;

                        pc.ColorForRender = LightModel.GetColor(lightModelParameters);
                    }
                }

                IEnumerable<Triangle> trs = planesForRender.SelectMany(x => x.Triangles);
                triangles = NeedToSort ? trs.OrderByDescending(t => t.MinZ).AsEnumerable() : trs;
            }
            else if (renderType == RenderModelType.FillSolidColor)
            {
                IEnumerable<Plane> planesForRender = Planes.Where(x => x.Normal.Z < 0 || x.VisibleBackSide);
                IEnumerable<Triangle> trs = planesForRender.SelectMany(x => x.Triangles);

                triangles = NeedToSort ? trs.OrderByDescending(t => t.MinZ).AsEnumerable() : trs;
            }
            else if (renderType == RenderModelType.Triangulations)
            {
                triangles = Planes.SelectMany(x => x.Triangles);
            }

            return triangles;
        }
    }
}
