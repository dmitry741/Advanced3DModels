using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Advanced3DModels
{
    enum ModelQuality
    {
        Low,
        Middle,
        High
    }

    class ModelFactory
    {
        public static Models3DLib.Model GetModel(int index, ModelQuality modelQuality)
        {
            float sizePrimitive;

            switch (modelQuality)
            {
                case ModelQuality.Middle: 
                    sizePrimitive = 12; 
                    break;
                case ModelQuality.High:
                    sizePrimitive = 8;
                    break;
                default:
                    sizePrimitive = 16;
                    break;
            }

            Models3DLib.Model model;

            if (index == 1)
            {
                model = Models3DLib.PresetsModel.CubeColored(200.0f, sizePrimitive);
            }
            else if (index == 2)
            {
                model = Models3DLib.PresetsModel.ChessBoard(288, sizePrimitive, 8, 24, Color.DarkRed, Color.Ivory);
            }
            else if (index == 3)
            {
                model = Models3DLib.PresetsModel.CubeSet(36, sizePrimitive, 288);
            }
            else if (index == 4)
            {
                model = Models3DLib.PresetsModel.TransparentTable(288, sizePrimitive, 24);
            }
            else if (index == 5)
            {
                model = Models3DLib.PresetsModel.Octahedron(272, sizePrimitive);
            }
            else
            {
                model = Models3DLib.PresetsModel.Cube(200.0f, sizePrimitive);
            }

            return model;
        }
    }
}
