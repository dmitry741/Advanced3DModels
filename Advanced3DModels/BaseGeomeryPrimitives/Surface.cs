using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models3DLib
{
    public class Surface : AbstractModel
    {
        public Surface()
        {
            NeedToSort = true;
        }

        public override IEnumerable<Triangle> GetTrianglesForRender(IEnumerable<ILightSource> lightSources, IPoint3D pointObserver, IFog ifog, RenderModelType renderType)
        {
            IEnumerable<Triangle> triangles = null;

            if (renderType == RenderModelType.FillFull)
            {
                List<Triangle> trs = new List<Triangle>();

                LightModelParameters lightModelParameters = new LightModelParameters
                {
                    LightSources = lightSources,
                    PointObserver = pointObserver,
                    Fog = ifog
                };

                Plane plane0 = Planes[0];

                foreach (Triangle triangle in plane0.Triangles)
                {
                    lightModelParameters.Normal = triangle.Normal;
                    lightModelParameters.Reflection = triangle.Reflection;
                    lightModelParameters.ReflectionBrightness = triangle.ReflectionBrightness;
                    lightModelParameters.ReflcetionCone = triangle.ReflectionCone;

                    Point3DColor[] point3DColor = triangle.Point3Ds.Select(p => Point3DColor.DeepCopy(p)).ToArray();
                    Triangle triangleForRender = new Triangle(point3DColor);

                    for (int i = 0; i < 3; i++)
                    {
                        lightModelParameters.Point = triangleForRender.Point3Ds[i];
                        lightModelParameters.BaseColor = triangleForRender.Point3Ds[i].BaseColor;

                        triangleForRender.Point3Ds[i].ColorForRender = LightModel.GetColor(lightModelParameters);
                    }

                    trs.Add(triangleForRender);
                }

                triangles = trs.OrderByDescending(t => t.MinZ).AsEnumerable();
            }
            else if (renderType == RenderModelType.FillSolidColor)
            {
                IEnumerable<Triangle> trs = Planes.SelectMany(x => x.Triangles);
                triangles = trs.OrderByDescending(t => t.MinZ).AsEnumerable();
            }
            else if (renderType == RenderModelType.Triangulations)
            {
                triangles = Planes.SelectMany(x => x.Triangles);
            }

            return triangles;
        }
    }
}
