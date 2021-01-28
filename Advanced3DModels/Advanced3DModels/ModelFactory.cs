using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static Models3DLib.Model GetModel(int index, float edgeLen,  ModelQuality modelQuality)
        {
            float sizePrimitive;

            switch (modelQuality)
            {
                case ModelQuality.Middle: 
                    sizePrimitive = 20; 
                    break;
                case ModelQuality.High:
                    sizePrimitive = 10;
                    break;
                default:
                    sizePrimitive = 32;
                    break;
            }

            Models3DLib.Model model;

            if (index == 1)
            {
                model = Models3DLib.Model.CubeColored(edgeLen, sizePrimitive);
            }
            else
            {
                model = Models3DLib.Model.Cube(edgeLen, sizePrimitive);
            }

            return model;
        }
    }
}
