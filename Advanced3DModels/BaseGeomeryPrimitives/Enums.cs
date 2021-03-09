using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models3DLib
{
    public enum RenderModelType
    {
        Triangulations,
        FillFull,
        FillSolidColor
    }

    public enum RenderFillTriangle
    {
        Flat0,
        Flat3,
        Gouraud
    }
}
