using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Numerics;

namespace Models3DLib
{
    /// <summary>
    /// Набор готовых моделей.
    /// </summary>
    public class PresetsModel
    {
        // Параллелепипед
        public static Model Parallelepiped(float width, float height, float depth, float sizePrimitive, bool[] panels, Color[] colors)
        {
            List<IPoint3D> points = new List<IPoint3D>
            {
                ResolverInterface.ResolveIPoint3D(-width / 2, height / 2, depth / 2),
                ResolverInterface.ResolveIPoint3D(width / 2, height / 2, depth / 2),
                ResolverInterface.ResolveIPoint3D(width / 2, -height / 2, depth / 2),
                ResolverInterface.ResolveIPoint3D(-width / 2, -height / 2, depth / 2),
                ResolverInterface.ResolveIPoint3D(-width / 2, height / 2, -depth / 2),
                ResolverInterface.ResolveIPoint3D(width / 2, height / 2, -depth / 2),
                ResolverInterface.ResolveIPoint3D(width / 2, -height / 2, -depth / 2),
                ResolverInterface.ResolveIPoint3D(-width / 2, -height / 2, -depth / 2)
            };

            List<Plane> planes = new List<Plane>();

            if (panels[0])
            {
                planes.Add(new Polygon4Plane(points[0], points[1], points[2], points[3], sizePrimitive, colors[0], colors[0].ToString()));
            }

            if (panels[1])
            {
                planes.Add(new Polygon4Plane(points[0], points[4], points[5], points[1], sizePrimitive, colors[1], colors[1].ToString()));
            }

            if (panels[2])
            {
                planes.Add(new Polygon4Plane(points[1], points[5], points[6], points[2], sizePrimitive, colors[2], colors[2].ToString()));
            }

            if (panels[3])
            {
                planes.Add(new Polygon4Plane(points[2], points[6], points[7], points[3], sizePrimitive, colors[3], colors[3].ToString()));
            }

            if (panels[4])
            {
                planes.Add(new Polygon4Plane(points[3], points[7], points[4], points[0], sizePrimitive, colors[4], colors[4].ToString()));
            }

            if (panels[5])
            {
                planes.Add(new Polygon4Plane(points[7], points[6], points[5], points[4], sizePrimitive, colors[5], colors[5].ToString()));
            }

            return new Model
            {
                Planes = planes
            };
        }

        // Октаэдр
        public static Model Octahedron(float sideSize, float sizePrimitive)
        {
            float s = Convert.ToSingle(Math.Sqrt(2) / 2);

            IPoint3D[] corePoints = new IPoint3D[6];

            corePoints[0] = ResolverInterface.ResolveIPoint3D(sideSize * s, 0, 0);
            corePoints[1] = ResolverInterface.ResolveIPoint3D(0, sideSize * s, 0);
            corePoints[2] = ResolverInterface.ResolveIPoint3D(-sideSize * s, 0, 0);
            corePoints[3] = ResolverInterface.ResolveIPoint3D(0, -sideSize * s, 0);
            corePoints[4] = ResolverInterface.ResolveIPoint3D(0, 0, sideSize * s);
            corePoints[5] = ResolverInterface.ResolveIPoint3D(0, 0, -sideSize * s);

            List<Plane> planes = new List<Plane>
            {
                new Polygon3Plane(corePoints[4], corePoints[0], corePoints[1], sizePrimitive, Color.LightGreen, string.Empty),
                new Polygon3Plane(corePoints[4], corePoints[1], corePoints[2], sizePrimitive, Color.Green, string.Empty),
                new Polygon3Plane(corePoints[4], corePoints[2], corePoints[3], sizePrimitive, Color.GreenYellow, string.Empty),
                new Polygon3Plane(corePoints[4], corePoints[3], corePoints[0], sizePrimitive, Color.DarkOliveGreen, string.Empty),
                new Polygon3Plane(corePoints[5], corePoints[1], corePoints[0], sizePrimitive, Color.DeepSkyBlue, string.Empty),
                new Polygon3Plane(corePoints[5], corePoints[2], corePoints[1], sizePrimitive, Color.Blue, string.Empty),
                new Polygon3Plane(corePoints[5], corePoints[3], corePoints[2], sizePrimitive, Color.BlueViolet, string.Empty),
                new Polygon3Plane(corePoints[5], corePoints[0], corePoints[3], sizePrimitive, Color.DarkBlue, string.Empty)
            };

            return new Model
            {
                Planes = planes
            };
        }

        // Набор кубиков
        public static Model CubeSet(float sizeSide, float sizePrimitive, float totalSize)
        {
            const int cRowCount = 4;

            Model resultModel = new Model();

            Color[] colors;
            bool[] visible;
            Color color;
            float v;
            Matrix4x4 translateMatrix;
            Vector3 translateVector;
            Model pld;

            color = Color.LightGreen;
            colors = new Color[] { color, color, color, color, color, color };
            visible = new bool[] { true, true, true, true, true, true };

            for (int i = 0; i < cRowCount; i++)
            {
                v = totalSize * i / (cRowCount - 1) - totalSize / 2;
                pld = Parallelepiped(sizeSide, sizeSide, sizeSide, sizePrimitive, visible, colors);
                translateVector = new Vector3(v, totalSize / 2, totalSize / 2);
                translateMatrix = Matrix4x4.CreateTranslation(translateVector);
                pld.Transform(translateMatrix);

                resultModel.UnionWith(pld);
            }

            for (int i = 0; i < cRowCount; i++)
            {
                v = totalSize * i / (cRowCount - 1) - totalSize / 2;
                pld = Parallelepiped(sizeSide, sizeSide, sizeSide, sizePrimitive, visible, colors);
                translateVector = new Vector3(v, -totalSize / 2, totalSize / 2);
                translateMatrix = Matrix4x4.CreateTranslation(translateVector);
                pld.Transform(translateMatrix);

                resultModel.UnionWith(pld);
            }

            for (int i = 0; i < cRowCount; i++)
            {
                v = totalSize * i / (cRowCount - 1) - totalSize / 2;
                pld = Parallelepiped(sizeSide, sizeSide, sizeSide, sizePrimitive, visible, colors);
                translateVector = new Vector3(v, totalSize / 2, -totalSize / 2);
                translateMatrix = Matrix4x4.CreateTranslation(translateVector);
                pld.Transform(translateMatrix);

                resultModel.UnionWith(pld);
            }

            for (int i = 0; i < cRowCount; i++)
            {
                v = totalSize * i / (cRowCount - 1) - totalSize / 2;
                pld = Parallelepiped(sizeSide, sizeSide, sizeSide, sizePrimitive, visible, colors);
                translateVector = new Vector3(v, -totalSize / 2, -totalSize / 2);
                translateMatrix = Matrix4x4.CreateTranslation(translateVector);
                pld.Transform(translateMatrix);

                resultModel.UnionWith(pld);
            }

            color = Color.Brown;
            colors = new Color[] { color, color, color, color, color, color };

            for (int i = 1; i < cRowCount - 1; i++)
            {
                v = totalSize * i / (cRowCount - 1) - totalSize / 2;
                pld = Parallelepiped(sizeSide, sizeSide, sizeSide, sizePrimitive, visible, colors);
                translateVector = new Vector3(-totalSize / 2, v, totalSize / 2);
                translateMatrix = Matrix4x4.CreateTranslation(translateVector);
                pld.Transform(translateMatrix);

                resultModel.UnionWith(pld);
            }

            for (int i = 1; i < cRowCount - 1; i++)
            {
                v = totalSize * i / (cRowCount - 1) - totalSize / 2;
                pld = Parallelepiped(sizeSide, sizeSide, sizeSide, sizePrimitive, visible, colors);
                translateVector = new Vector3(totalSize / 2, v, totalSize / 2);
                translateMatrix = Matrix4x4.CreateTranslation(translateVector);
                pld.Transform(translateMatrix);

                resultModel.UnionWith(pld);
            }

            for (int i = 1; i < cRowCount - 1; i++)
            {
                v = totalSize * i / (cRowCount - 1) - totalSize / 2;
                pld = Parallelepiped(sizeSide, sizeSide, sizeSide, sizePrimitive, visible, colors);
                translateVector = new Vector3(-totalSize / 2, v, -totalSize / 2);
                translateMatrix = Matrix4x4.CreateTranslation(translateVector);
                pld.Transform(translateMatrix);

                resultModel.UnionWith(pld);
            }

            for (int i = 1; i < cRowCount - 1; i++)
            {
                v = totalSize * i / (cRowCount - 1) - totalSize / 2;
                pld = Parallelepiped(sizeSide, sizeSide, sizeSide, sizePrimitive, visible, colors);
                translateVector = new Vector3(totalSize / 2, v, -totalSize / 2);
                translateMatrix = Matrix4x4.CreateTranslation(translateVector);
                pld.Transform(translateMatrix);

                resultModel.UnionWith(pld);
            }

            color = Color.Indigo;
            colors = new Color[] { color, color, color, color, color, color };

            for (int i = 1; i < cRowCount - 1; i++)
            {
                v = totalSize * i / (cRowCount - 1) - totalSize / 2;
                pld = Parallelepiped(sizeSide, sizeSide, sizeSide, sizePrimitive, visible, colors);
                translateVector = new Vector3(-totalSize / 2, totalSize / 2, v);
                translateMatrix = Matrix4x4.CreateTranslation(translateVector);
                pld.Transform(translateMatrix);

                resultModel.UnionWith(pld);
            }

            color = Color.DarkOrange;
            colors = new Color[] { color, color, color, color, color, color };

            for (int i = 1; i < cRowCount - 1; i++)
            {
                v = totalSize * i / (cRowCount - 1) - totalSize / 2;
                pld = Parallelepiped(sizeSide, sizeSide, sizeSide, sizePrimitive, visible, colors);
                translateVector = new Vector3(totalSize / 2, totalSize / 2, v);
                translateMatrix = Matrix4x4.CreateTranslation(translateVector);
                pld.Transform(translateMatrix);

                resultModel.UnionWith(pld);
            }

            for (int i = 1; i < cRowCount - 1; i++)
            {
                v = totalSize * i / (cRowCount - 1) - totalSize / 2;
                pld = Parallelepiped(sizeSide, sizeSide, sizeSide, sizePrimitive, visible, colors);
                translateVector = new Vector3(totalSize / 2, -totalSize / 2, v);
                translateMatrix = Matrix4x4.CreateTranslation(translateVector);
                pld.Transform(translateMatrix);

                resultModel.UnionWith(pld);
            }

            color = Color.Indigo;
            colors = new Color[] { color, color, color, color, color, color };

            for (int i = 1; i < cRowCount - 1; i++)
            {
                v = totalSize * i / (cRowCount - 1) - totalSize / 2;
                pld = Parallelepiped(sizeSide, sizeSide, sizeSide, sizePrimitive, visible, colors);
                translateVector = new Vector3(-totalSize / 2, totalSize / 2, v);
                translateMatrix = Matrix4x4.CreateTranslation(translateVector);
                pld.Transform(translateMatrix);

                resultModel.UnionWith(pld);
            }

            for (int i = 1; i < cRowCount - 1; i++)
            {
                v = totalSize * i / (cRowCount - 1) - totalSize / 2;
                pld = Parallelepiped(sizeSide, sizeSide, sizeSide, sizePrimitive, visible, colors);
                translateVector = new Vector3(-totalSize / 2, -totalSize / 2, v);
                translateMatrix = Matrix4x4.CreateTranslation(translateVector);
                pld.Transform(translateMatrix);

                resultModel.UnionWith(pld);
            }

            return resultModel;
        }

        // Столик с прозрачной столешницей
        public static Model TransparentTable(float sizeSide, float sizePrimitive, float depth)
        {
            bool[] panels;
            Color[] colors;
            const int transparency = 128;

            Model model = new Model();

            float boxSize = sizeSide / 6;

            // прозрачная столешница
            Color tableColor = Color.FromArgb(transparency, Color.DarkGray);
            panels = new bool[] { true, false, false, false, false, true };
            colors = new Color[] { tableColor, Color.Black, Color.Black, Color.Black, Color.Black, tableColor };
            Model table = Parallelepiped(sizeSide - 2 * boxSize, sizeSide - 2 * boxSize, depth, sizePrimitive, panels, colors);

            foreach (Plane plane in table.Planes)
            {
                foreach (Triangle triangle in plane.Triangles)
                {
                    triangle.AllowToGouraudMethod = false;
                    triangle.VisibleBackSide = true;
                }
            }

            model.UnionWith(table);

            // непрозрачные боковины
            Color boxColor = Color.Brown;
            Model pld;
            Vector3 trans;
            Matrix4x4 matrix4;

            colors = new Color[] { boxColor, boxColor, boxColor, boxColor, boxColor, boxColor };
            panels = new bool[] { true, false, true, false, true, true };

            // левая вертикальная
            pld = Parallelepiped(boxSize, sizeSide - 2 * boxSize, depth, sizePrimitive, panels, colors);
            trans = new Vector3(-sizeSide / 2 + boxSize / 2, 0, 0);
            matrix4 = Matrix4x4.CreateTranslation(trans);
            pld.Transform(matrix4);
            model.UnionWith(pld);

            // правая вертикальная
            pld = Parallelepiped(boxSize, sizeSide - 2 * boxSize, depth, sizePrimitive, panels, colors);
            trans = new Vector3(sizeSide / 2 - boxSize / 2, 0, 0);
            matrix4 = Matrix4x4.CreateTranslation(trans);
            pld.Transform(matrix4);
            model.UnionWith(pld);

            panels = new bool[] { true, true, false, true, false, true };

            // нижняя горизонтальная
            pld = Parallelepiped(sizeSide - 2 * boxSize, boxSize, depth, sizePrimitive, panels, colors);
            trans = new Vector3(0, sizeSide / 2 - boxSize / 2, 0);
            matrix4 = Matrix4x4.CreateTranslation(trans);
            pld.Transform(matrix4);
            model.UnionWith(pld);

            // верхняя горизонтальная
            pld = Parallelepiped(sizeSide - 2 * boxSize, boxSize, depth, sizePrimitive, panels, colors);
            trans = new Vector3(0, -sizeSide / 2 + boxSize / 2, 0);
            matrix4 = Matrix4x4.CreateTranslation(trans);
            pld.Transform(matrix4);
            model.UnionWith(pld);

            // вставки-квадратики по углам

            // левый верхний
            panels = new bool[] { true, false, false, true, true, true };
            pld = Parallelepiped(boxSize, boxSize, depth, sizePrimitive, panels, colors);
            trans = new Vector3(-sizeSide / 2 + boxSize / 2, -sizeSide / 2 + boxSize / 2, 0);
            matrix4 = Matrix4x4.CreateTranslation(trans);
            pld.Transform(matrix4);
            model.UnionWith(pld);

            // правый верхний
            panels = new bool[] { true, false, true, true, false, true };
            pld = Parallelepiped(boxSize, boxSize, depth, sizePrimitive, panels, colors);
            trans = new Vector3(sizeSide / 2 - boxSize / 2, -sizeSide / 2 + boxSize / 2, 0);
            matrix4 = Matrix4x4.CreateTranslation(trans);
            pld.Transform(matrix4);
            model.UnionWith(pld);

            // правый нижний
            panels = new bool[] { true, true, true, false, false, true };
            pld = Parallelepiped(boxSize, boxSize, depth, sizePrimitive, panels, colors);
            trans = new Vector3(sizeSide / 2 - boxSize / 2, sizeSide / 2 - boxSize / 2, 0);
            matrix4 = Matrix4x4.CreateTranslation(trans);
            pld.Transform(matrix4);
            model.UnionWith(pld);

            // левый нижний
            panels = new bool[] { true, true, false, false, true, true };
            pld = Parallelepiped(boxSize, boxSize, depth, sizePrimitive, panels, colors);
            trans = new Vector3(-sizeSide / 2 + boxSize / 2, sizeSide / 2 - boxSize / 2, 0);
            matrix4 = Matrix4x4.CreateTranslation(trans);
            pld.Transform(matrix4);
            model.UnionWith(pld);

            // ножки
            boxColor = Color.SandyBrown;
            colors = new Color[] { boxColor, boxColor, boxColor, boxColor, boxColor, boxColor };
            panels = new bool[] { true, true, true, true, true, false };
            const float cFootSize = 24;
            const float cFootLen = 56;
            const float ck = 1.2f;

            // левая верхняя
            pld = Parallelepiped(cFootSize, cFootSize, cFootLen, sizePrimitive, panels, colors);
            trans = new Vector3(-sizeSide / 2 - cFootLen / 2 + ck * boxSize, -sizeSide / 2 - cFootLen / 2 + ck * boxSize, cFootLen / 2 + depth / 2);
            matrix4 = Matrix4x4.CreateTranslation(trans);
            pld.Transform(matrix4);
            model.UnionWith(pld);

            // правая верхняя
            pld = Parallelepiped(cFootSize, cFootSize, cFootLen, sizePrimitive, panels, colors);
            trans = new Vector3(sizeSide / 2 + cFootLen / 2 - ck * boxSize, -sizeSide / 2 - cFootLen / 2 + ck * boxSize, cFootLen / 2 + depth / 2);
            matrix4 = Matrix4x4.CreateTranslation(trans);
            pld.Transform(matrix4);
            model.UnionWith(pld);

            // правая нижняя
            pld = Parallelepiped(cFootSize, cFootSize, cFootLen, sizePrimitive, panels, colors);
            trans = new Vector3(sizeSide / 2 + cFootLen / 2 - ck * boxSize, sizeSide / 2 + cFootLen / 2 - ck * boxSize, cFootLen / 2 + depth / 2);
            matrix4 = Matrix4x4.CreateTranslation(trans);
            pld.Transform(matrix4);
            model.UnionWith(pld);

            // левая нижняя
            pld = Parallelepiped(cFootSize, cFootSize, cFootLen, sizePrimitive, panels, colors);
            trans = new Vector3(-sizeSide / 2 - cFootLen / 2 + ck * boxSize, sizeSide / 2 + cFootLen / 2 - ck * boxSize, cFootLen / 2 + depth / 2);
            matrix4 = Matrix4x4.CreateTranslation(trans);
            pld.Transform(matrix4);
            model.UnionWith(pld);

            return model;
        }

        // Шахматный столик
        public static Model ChessBoard(float sizeSide, float sizePrimitive, int tileRowCount, float depth, Color color1, Color color2)
        {
            float xStart = -sizeSide / 2.0f;
            float yStart = -sizeSide / 2.0f;
            float xEnd = sizeSide / 2.0f;
            float yEnd = sizeSide / 2.0f;

            bool[] panels = new bool[] { true, false, false, false, false, true };

            Color[] colors1 = new Color[] { color2, Color.Black, Color.Black, Color.Black, Color.Black, color1 };
            Color[] colors2 = new Color[] { color2, Color.Black, Color.Black, Color.Black, Color.Black, color2 };

            Model model = new Model();

            for (int i = 0; i < tileRowCount; i++)
            {
                float x = (xEnd - xStart) * i / tileRowCount + xStart;

                for (int j = 0; j < tileRowCount; j++)
                {
                    float y = (yEnd - yStart) * j / tileRowCount + yStart;

                    Color[] colors = (i + j) % 2 == 0 ? colors1 : colors2;
                    Model pld = Parallelepiped(sizeSide / tileRowCount, sizeSide / tileRowCount, depth, sizePrimitive, panels, colors);
                    Vector3 translation = new Vector3(x + sizeSide / tileRowCount / 2, y + sizeSide / tileRowCount / 2, 0);
                    Matrix4x4 matrix = Matrix4x4.CreateTranslation(translation);
                    pld.Transform(matrix);
                    model.UnionWith(pld);
                }
            }

            Vector3 trans;
            Matrix4x4 matrixTrans;
            Model furnitureItem;
            bool[] visible;

            // добавляем борта
            Color[] itemsColors = new Color[] { color2, color2, color2, color2, color2, color2 };

            visible = new bool[] { true, true, false, true, true, true };
            furnitureItem = Parallelepiped(sizeSide / tileRowCount, sizeSide + 2 * sizeSide / tileRowCount, depth, sizePrimitive, visible, itemsColors);
            trans = new Vector3(-sizeSide / 2 - sizeSide / tileRowCount / 2, 0, 0);
            matrixTrans = Matrix4x4.CreateTranslation(trans);
            furnitureItem.Transform(matrixTrans);
            model.UnionWith(furnitureItem);

            visible = new bool[] { true, true, true, true, false, true };
            furnitureItem = Parallelepiped(sizeSide / tileRowCount, sizeSide + 2 * sizeSide / tileRowCount, depth, sizePrimitive, visible, itemsColors);
            trans = new Vector3(sizeSide / 2 + sizeSide / tileRowCount / 2, 0, 0);
            matrixTrans = Matrix4x4.CreateTranslation(trans);
            furnitureItem.Transform(matrixTrans);
            model.UnionWith(furnitureItem);

            visible = new bool[] { true, true, false, false, false, true };
            furnitureItem = Parallelepiped(sizeSide, sizeSide / tileRowCount, depth, sizePrimitive, visible, itemsColors);
            trans = new Vector3(0, sizeSide / 2 + sizeSide / tileRowCount / 2, 0);
            matrixTrans = Matrix4x4.CreateTranslation(trans);
            furnitureItem.Transform(matrixTrans);
            model.UnionWith(furnitureItem);

            visible = new bool[] { true, false, false, true, false, true };
            furnitureItem = Parallelepiped(sizeSide, sizeSide / tileRowCount, depth, sizePrimitive, visible, itemsColors);
            trans = new Vector3(0, -sizeSide / 2 - sizeSide / tileRowCount / 2, 0);
            matrixTrans = Matrix4x4.CreateTranslation(trans);
            furnitureItem.Transform(matrixTrans);
            model.UnionWith(furnitureItem);

            // добавяем ножки
            const float cfh = 48.0f;
            visible = new bool[] { true, true, true, true, true, false };
            itemsColors = new Color[] { Color.SandyBrown, Color.SandyBrown, Color.SandyBrown, Color.SandyBrown, Color.SandyBrown, Color.SandyBrown, };

            furnitureItem = Parallelepiped(sizeSide / tileRowCount, sizeSide / tileRowCount, cfh, sizePrimitive, visible, itemsColors);
            trans = new Vector3(-sizeSide / 2 + sizeSide / tileRowCount / 2, -sizeSide / 2 + sizeSide / tileRowCount / 2, cfh / 2 + depth / 2);
            matrixTrans = Matrix4x4.CreateTranslation(trans);
            furnitureItem.Transform(matrixTrans);
            model.UnionWith(furnitureItem);

            furnitureItem = Parallelepiped(sizeSide / tileRowCount, sizeSide / tileRowCount, cfh, sizePrimitive, visible, itemsColors);
            trans = new Vector3(-sizeSide / 2 + sizeSide / tileRowCount / 2, sizeSide / 2 - sizeSide / tileRowCount / 2, cfh / 2 + depth / 2);
            matrixTrans = Matrix4x4.CreateTranslation(trans);
            furnitureItem.Transform(matrixTrans);
            model.UnionWith(furnitureItem);

            furnitureItem = Parallelepiped(sizeSide / tileRowCount, sizeSide / tileRowCount, cfh, sizePrimitive, visible, itemsColors);
            trans = new Vector3(sizeSide / 2 - sizeSide / tileRowCount / 2, sizeSide / 2 - sizeSide / tileRowCount / 2, cfh / 2 + depth / 2);
            matrixTrans = Matrix4x4.CreateTranslation(trans);
            furnitureItem.Transform(matrixTrans);
            model.UnionWith(furnitureItem);

            furnitureItem = Parallelepiped(sizeSide / tileRowCount, sizeSide / tileRowCount, cfh, sizePrimitive, visible, itemsColors);
            trans = new Vector3(sizeSide / 2 - sizeSide / tileRowCount / 2, -sizeSide / 2 + sizeSide / tileRowCount / 2, cfh / 2 + depth / 2);
            matrixTrans = Matrix4x4.CreateTranslation(trans);
            furnitureItem.Transform(matrixTrans);
            model.UnionWith(furnitureItem);

            return model;
        }

        // Куб с гранями одного цвета
        public static Model Cube(float sizeSide, float sizePrimitive)
        {
            Color[] colors = new Color[] { Color.LightGreen, Color.LightGreen, Color.LightGreen, Color.LightGreen, Color.LightGreen, Color.LightGreen };
            bool[] planes = new bool[] { true, true, true, true, true, true };

            return Parallelepiped(sizeSide, sizeSide, sizeSide, sizePrimitive, planes, colors);
        }

        // Куб с разноцветными гранями
        public static Model CubeColored(float sizeSide, float sizePrimitive)
        {
            Color[] colors = { Color.LightGreen, Color.Brown, Color.Gold, Color.Cornsilk, Color.DarkBlue, Color.BurlyWood };
            bool[] planes = new bool[] { true, true, true, true, true, true };

            return Parallelepiped(sizeSide, sizeSide, sizeSide, sizePrimitive, planes, colors);
        }

        // Стрелочка
        public static Model Arrow(float baseWidth, float baseHeight, float arrowRadius, float arrowHeight, float depth, float sizePrimitive, Color color)
        {
            Color[] colors = { color, color, color, color, color, color };
            bool[] planes = new bool[] { true, true, false, true, true, true };

            Model parallelepiped = Parallelepiped(baseWidth, baseHeight, depth, sizePrimitive, planes, colors);
            List<Plane> pl = new List<Plane>();
            IPoint3D point1, point2, point3, point4;

            point1 = ResolverInterface.ResolveIPoint3D(baseWidth / 2 - 0.5f, -arrowRadius, depth / 2);
            point2 = ResolverInterface.ResolveIPoint3D(baseWidth / 2 + arrowHeight - 0.5f, 0, depth / 2);
            point3 = ResolverInterface.ResolveIPoint3D(baseWidth / 2 - 0.5f, arrowRadius, depth / 2);

            pl.Add(new Polygon3Plane(point1, point2, point3, sizePrimitive, color, string.Empty));

            point1 = ResolverInterface.ResolveIPoint3D(baseWidth / 2 - 0.5f, arrowRadius, -depth / 2);
            point2 = ResolverInterface.ResolveIPoint3D(baseWidth / 2 + arrowHeight - 0.5f, 0, -depth / 2);
            point3 = ResolverInterface.ResolveIPoint3D(baseWidth / 2, -arrowRadius - 0.5f, -depth / 2);

            pl.Add(new Polygon3Plane(point1, point2, point3, sizePrimitive, color, string.Empty));

            point1 = ResolverInterface.ResolveIPoint3D(baseWidth / 2 - 0.5f, baseHeight / 2, depth / 2);
            point2 = ResolverInterface.ResolveIPoint3D(baseWidth / 2 - 0.5f, baseHeight / 2, -depth / 2);
            point3 = ResolverInterface.ResolveIPoint3D(baseWidth / 2 - 0.5f, arrowRadius,  -depth / 2);
            point4 = ResolverInterface.ResolveIPoint3D(baseWidth / 2 - 0.5f, arrowRadius, depth / 2);

            pl.Add(new Polygon4Plane(point1, point2, point3, point4, sizePrimitive, color, string.Empty));

            point1 = ResolverInterface.ResolveIPoint3D(baseWidth / 2 - 0.5f, -baseHeight / 2, depth / 2);
            point2 = ResolverInterface.ResolveIPoint3D(baseWidth / 2 - 0.5f, -arrowRadius, depth / 2);
            point3 = ResolverInterface.ResolveIPoint3D(baseWidth / 2 - 0.5f, -arrowRadius, -depth / 2);
            point4 = ResolverInterface.ResolveIPoint3D(baseWidth / 2 - 0.5f, -baseHeight / 2, -depth / 2);

            pl.Add(new Polygon4Plane(point1, point2, point3, point4, sizePrimitive, color, string.Empty));

            point1 = ResolverInterface.ResolveIPoint3D(baseWidth / 2 - 0.5f, arrowRadius, depth / 2);
            point2 = ResolverInterface.ResolveIPoint3D(baseWidth / 2 - 0.5f, arrowRadius, -depth / 2);
            point3 = ResolverInterface.ResolveIPoint3D(baseWidth / 2 - 0.5f + arrowHeight, 0, -depth / 2);
            point4 = ResolverInterface.ResolveIPoint3D(baseWidth / 2 - 0.5f + arrowHeight, 0, depth / 2);

            pl.Add(new Polygon4Plane(point1, point2, point3, point4, sizePrimitive, color, string.Empty));

            point1 = ResolverInterface.ResolveIPoint3D(baseWidth / 2 - 0.5f + arrowHeight, 0, depth / 2);
            point2 = ResolverInterface.ResolveIPoint3D(baseWidth / 2 - 0.5f + arrowHeight, 0, -depth / 2);
            point3 = ResolverInterface.ResolveIPoint3D(baseWidth / 2 - 0.5f, -arrowRadius, -depth / 2);
            point4 = ResolverInterface.ResolveIPoint3D(baseWidth / 2 - 0.5f, -arrowRadius, depth / 2);

            pl.Add(new Polygon4Plane(point1, point2, point3, point4, sizePrimitive, color, string.Empty));

            Model tr = new Model
            {
                Planes = pl
            };

            Model arrow = new Model();

            arrow.UnionWith(parallelepiped);
            arrow.UnionWith(tr);

            return arrow;
        }
    }
}
