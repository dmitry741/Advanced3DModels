﻿using System;
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
        public static Models3DLib.Model GetModel(int index, float modelSize,  ModelQuality modelQuality)
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
                model = Models3DLib.Model.CubeColored(modelSize, sizePrimitive);
            }
            else if (index == 2)
            {
                model = Models3DLib.Model.ChessBoard(288, sizePrimitive, 8, 24, Color.DarkRed, Color.Ivory);
            }
            else
            {
                model = Models3DLib.Model.Cube(modelSize, sizePrimitive);
            }

            return model;
        }
    }
}