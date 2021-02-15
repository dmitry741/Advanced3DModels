using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;

namespace Models3DLib
{
    public class Model
    {
        #region === members ===

        protected List<Plane> _planes = new List<Plane>();

        #endregion

        #region === private ===

        /// <summary>
        /// Объединить текущую модель с моделью заданную параметром model.
        /// </summary>
        /// <param name="model">модель для добавления в текущую модель.</param>
        void UnionWith(Model model)
        {
            _planes.AddRange(model.Planes);
        }

        #endregion

        #region === public ===

        public bool NeedToSort { get; set; } = false;
        public IEnumerable<Plane> Planes => _planes;

        public void SaveState()
        {
            foreach(Plane plane in _planes)
            {
                plane.SaveState();
            }
        }

        public void RestoreState()
        {
            foreach (Plane plane in _planes)
            {
                plane.RestoreState();
            }
        }

        public void Transform(Matrix4x4 matrix)
        {
            foreach (Plane plane in _planes)
            {
                foreach (IPoint3D point in plane.Points)
                {
                    Vector3 vector = Vector3.Transform(point.ToVector3(), matrix);

                    point.X = vector.X;
                    point.Y = vector.Y;
                    point.Z = vector.Z;
                }
            }
        }

        public void Transform(IPerspectiveTransform iperspectiveTransform, IPoint3D centerOfPerspective)
        {
            foreach (Plane plane in _planes)
            {
                foreach (IPoint3D point in plane.Points)
                {
                    IPoint3D perspectivePoint = iperspectiveTransform.Transform(point, centerOfPerspective);

                    point.X = perspectivePoint.X;
                    point.Y = perspectivePoint.Y;
                    point.Z = perspectivePoint.Z;
                }
            }
        }

        #endregion

        #region === private ===

        private static Model Parallelepiped(float width, float height, float depth, float sizePrimitive, bool[] panels, Color[] colors)
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
                _planes = planes
            };
        }

        #endregion

        #region === models ===

        public static Model CubeSet(float sizeSide, float sizePrimitive, float totalSize)
        {
            const int cRowCount = 4;

            Model resultModel = new Model
            {
                NeedToSort = true
            };

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

        public static Model TransparentTable(float sizeSide, float sizePrimitive, float depth)
        {
            /*float xStart = -sizeSide / 2.0f;
            float yStart = -sizeSide / 2.0f;
            float xEnd = sizeSide / 2.0f;
            float yEnd = sizeSide / 2.0f;*/

            bool[] panels;
            Color[] colors;
            const int transparency = 128;

            Model model = new Model
            {
                NeedToSort = true,
            };

            float boxSize = sizeSide / 6;

            // прозрачная столешница
            Color tableColor = Color.FromArgb(transparency, Color.DarkGray);
            panels = new bool[] { true, false, false, false, false, true };
            colors = new Color[] { tableColor, Color.Black, Color.Black, Color.Black, Color.Black, tableColor };
            Model table = Parallelepiped(sizeSide - boxSize, sizeSide - boxSize, depth, sizePrimitive, panels, colors);

            foreach (Plane plane in table.Planes)
            {
                plane.VisibleBackSide = true;
            }

            model.UnionWith(table);

            // непрозрачные боковины
            Color boxColor = Color.Brown;
            Model box;
            Vector3 trans;
            Matrix4x4 matrix4;

            panels = new bool[] { true, true, true, true, true, true };
            colors = new Color[] { boxColor, boxColor, boxColor, boxColor, boxColor, boxColor };

            box = Parallelepiped(boxSize, sizeSide + boxSize, depth, sizePrimitive, panels, colors);
            trans = new Vector3(-sizeSide / 2, 0, 0);
            matrix4 = Matrix4x4.CreateTranslation(trans);
            box.Transform(matrix4);

            model.UnionWith(box);

            box = Parallelepiped(boxSize, sizeSide + boxSize, depth, sizePrimitive, panels, colors);

            trans = new Vector3(sizeSide / 2, 0, 0);
            matrix4 = Matrix4x4.CreateTranslation(trans);
            box.Transform(matrix4);

            model.UnionWith(box);

            box = Parallelepiped(sizeSide, boxSize, depth, sizePrimitive, panels, colors);

            trans = new Vector3(0, sizeSide / 2, 0);
            matrix4 = Matrix4x4.CreateTranslation(trans);
            box.Transform(matrix4);

            //model.UnionWith(box);


            box = Parallelepiped(sizeSide, boxSize, depth, sizePrimitive, panels, colors);

            trans = new Vector3(0, -sizeSide / 2, 0);
            matrix4 = Matrix4x4.CreateTranslation(trans);
            box.Transform(matrix4);

            //model.UnionWith(box);


            return model;
        }

        public static Model ChessBoard(float sizeSide, float sizePrimitive, int tileRowCount, float depth, Color color1, Color color2)
        {
            float xStart = -sizeSide / 2.0f;
            float yStart = -sizeSide / 2.0f;
            float xEnd = sizeSide / 2.0f;
            float yEnd = sizeSide / 2.0f;

            bool[] panels = new bool[] { true, false, false, false, false, true };

            Color[] colors1 = new Color[] { color2, Color.Black, Color.Black, Color.Black, Color.Black, color1 };
            Color[] colors2 = new Color[] { color2, Color.Black, Color.Black, Color.Black, Color.Black, color2 };

            Model model = new Model
            {
                NeedToSort = true
            };

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

        public static Model Cube(float sizeSide, float sizePrimitive)
        {
            Color[] colors = new Color[] { Color.LightGreen, Color.LightGreen, Color.LightGreen, Color.LightGreen, Color.LightGreen, Color.LightGreen };
            bool[] planes = new bool[] { true, true, true, true, true, true };

            return Parallelepiped(sizeSide, sizeSide, sizeSide, sizePrimitive, planes, colors);
        }

        public static Model CubeColored(float sizeSide, float sizePrimitive)
        {
            Color[] colors = { Color.LightGreen, Color.Brown, Color.Gold, Color.Cornsilk, Color.DarkBlue, Color.BurlyWood };
            bool[] planes = new bool[] { true, true, true, true, true, true };

            return Parallelepiped(sizeSide, sizeSide, sizeSide, sizePrimitive, planes, colors);
        }

        #endregion
    }
}
