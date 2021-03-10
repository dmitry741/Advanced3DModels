using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Models3DLib;
using System.Drawing.Drawing2D;

namespace WinTextureModels
{
    class RenderingModel
    {
        public static void Render(Graphics g, 
            Model model, ILightSource lightSource, 
            IPoint3D pointObserver,
            RenderModelType renderType)
        {
            IEnumerable<Triangle> triangles = model.GetTrianglesForRender(renderType);

            LightModelParameters lightModelParameters = new LightModelParameters
            {
                LightSources = new List<ILightSource> { lightSource },
                PointObserver = pointObserver,
                ReflectionEnable = true
            };

            Color[] surroundColors = new Color[3];

            foreach (Triangle triangle in triangles)
            {
                lightModelParameters.Normal = triangle.Normal;                
                lightModelParameters.ReflectionBrightness = triangle.ReflectionBrightness;
                lightModelParameters.ReflcetionCone = triangle.ReflectionCone;                

                for (int i = 0; i < 3; i++)
                {
                    lightModelParameters.Point = triangle.Point3Ds[i];
                    lightModelParameters.BaseColor = (triangle.TextureColors != null) ? triangle.TextureColors[i] : triangle.BaseColor;
                    surroundColors[i] = LightModel.GetColor(lightModelParameters);
                }

                double R = surroundColors.Average(color => color.R);
                double G = surroundColors.Average(color => color.G);
                double B = surroundColors.Average(color => color.B);

                Color averageColor = Color.FromArgb(Convert.ToInt32(R), Convert.ToInt32(G), Convert.ToInt32(B));

                Brush brush = new SolidBrush(averageColor);
                g.FillPolygon(brush, triangle.Points);
            }
        }
    }
}
