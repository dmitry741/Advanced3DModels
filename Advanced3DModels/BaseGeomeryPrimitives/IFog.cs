using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Models3DLib
{
    public interface IFog
    {
        Color Correct(float d, Color color);
    }
}
