using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models3DLib
{
    public class ParallelepipedPixelTextureModel : AbstractConvexPixelModel
    {
        byte[][] _rgbValues = new byte[6][];

        public ParallelepipedPixelTextureModel(float width, float height, float depth, bool[] panels)
        {
            // TODO:
        }

        public void SetTexture(int plane, byte[] rgbValues)
        {
            _rgbValues[plane] = rgbValues;
        }

        public void SetColor(int plane, Color color)
        {

        }
    }
}
