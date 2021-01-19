using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Models3DLib;

namespace Advanced3DModels
{
    enum RenderType
    {
        Edges,
        Triangulations,
        Fill
    }

    class RenderingModel
    {
        public static void Render(Graphics g, Model model, RenderType renderType)
        {
            if (RenderType.Edges == renderType)
            {
                foreach(Edge edge in model.Edges)
                {
                    g.DrawLine(Pens.Black, edge.Point1.ToPointF(), edge.Point2.ToPointF());
                }
            }
        }
    }
}
