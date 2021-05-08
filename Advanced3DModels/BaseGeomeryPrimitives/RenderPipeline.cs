using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Models3DLib
{
    class RenderPipeline : IRenderPipeline
    {
        IEnumerable<Triangle> IRenderPipeline.RenderPipeline(Model model, IRenderPipelineParameters irpp)
        {
            Matrix4x4 translate = Matrix4x4.CreateTranslation(irpp.TranslateX, irpp.TranslateY, irpp.TranslateZ);
            model.Transform(translate);

            IEnumerable<Triangle> triangles = null;
            IEnumerable<Triangle> all = model.Planes.SelectMany(pl => pl.Triangles);

            if (irpp.renderModelType == RenderModelType.FillFull)
            {
                if (!irpp.PerspectiveEnable)
                {                    
                    IEnumerable<Triangle> trs = all.Where(x => x.VisibleBackSide || x.Normal.Z < 0);
                    triangles = model.NeedToSort ? trs.OrderByDescending(t => t.MinZ).AsEnumerable() : trs;

                    LightModelParameters lightModelParameters = new LightModelParameters
                    {
                        LightSources = irpp.lightSources,
                        PointObserver = irpp.pointObserver,
                        Fog = irpp.Fog
                    };

                    foreach (Triangle triangle in triangles)
                    {
                        lightModelParameters.Normal = triangle.Normal;
                        lightModelParameters.ReflectionEnable = triangle.ReflectionEnable;
                        lightModelParameters.ReflectionBrightness = triangle.ReflectionBrightness;
                        lightModelParameters.ReflcetionCone = triangle.ReflectionCone;
                        lightModelParameters.BaseColor = triangle.BaseColor;

                        if (irpp.renderFillTriangle == RenderFillTriangle.Flat0 || !triangle.AllowToGouraudMethod)
                        {
                            lightModelParameters.Point = triangle.Point0;
                            triangle.RenderColors[0] = LightModel.GetColor(lightModelParameters);
                        }
                        else if (irpp.renderFillTriangle == RenderFillTriangle.Flat3 || irpp.renderFillTriangle == RenderFillTriangle.Gouraud)
                        {
                            Color[] colors = new Color[3];

                            for (int i = 0; i < 3; i++)
                            {
                                lightModelParameters.Point = triangle.Point3Ds[i];
                                colors[i] = LightModel.GetColor(lightModelParameters);
                            }

                            triangle.RenderColors = colors;
                        }
                    }
                }
                else
                {
                    model.PushState();
                    model.Transform(irpp.PerspectiveTransform, irpp.CenterPerspective);

                    IEnumerable<Triangle> trs = all.Where(x => x.VisibleBackSide || x.Normal.Z < 0);
                    triangles = model.NeedToSort ? trs.OrderByDescending(t => t.MinZ).AsEnumerable() : trs;

                    model.PopState();

                    LightModelParameters lightModelParameters = new LightModelParameters
                    {
                        LightSources = irpp.lightSources,
                        PointObserver = irpp.pointObserver,
                        Fog = irpp.Fog
                    };

                    foreach (Triangle triangle in triangles)
                    {
                        lightModelParameters.Normal = triangle.Normal;
                        lightModelParameters.ReflectionEnable = triangle.ReflectionEnable;
                        lightModelParameters.ReflectionBrightness = triangle.ReflectionBrightness;
                        lightModelParameters.ReflcetionCone = triangle.ReflectionCone;
                        lightModelParameters.BaseColor = triangle.BaseColor;

                        if (irpp.renderFillTriangle == RenderFillTriangle.Flat0 || !triangle.AllowToGouraudMethod)
                        {
                            lightModelParameters.Point = triangle.Point0;
                            triangle.RenderColors[0] = LightModel.GetColor(lightModelParameters);
                        }
                        else if (irpp.renderFillTriangle == RenderFillTriangle.Flat3 || irpp.renderFillTriangle == RenderFillTriangle.Gouraud)
                        {
                            Color[] colors = new Color[3];

                            for (int i = 0; i < 3; i++)
                            {
                                lightModelParameters.Point = triangle.Point3Ds[i];
                                colors[i] = LightModel.GetColor(lightModelParameters);
                            }

                            triangle.RenderColors = colors;
                        }
                    }

                    model.Transform(irpp.PerspectiveTransform, irpp.CenterPerspective);
                }
            }
            else if (irpp.renderModelType == RenderModelType.FillSolidColor)
            {
                if (!irpp.PerspectiveEnable)
                {
                    // TODO
                }
                else
                {
                    // TODO
                }
            }
            else if (irpp.renderModelType == RenderModelType.Triangulations)
            {
                if (irpp.PerspectiveEnable)
                {
                    model.Transform(irpp.PerspectiveTransform, irpp.CenterPerspective);
                }
                    
                triangles = all;
            }

            return triangles;
        }
    }
}
