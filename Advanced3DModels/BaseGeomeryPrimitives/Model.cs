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
        protected List<Point3D> _points = new List<Point3D>();

        #endregion

        #region === public ===

        public bool NeedToSort { get; set; } = false;
        public IEnumerable<Plane> Planes => _planes;
        public void Transform(Matrix4x4 matrix)
        {
            foreach(Plane plane in _planes)
            {
                foreach (Point3D point in plane.Points)
                {
                    Vector3 vector = Vector3.Transform(point.ToVector3(), matrix);

                    point.X = vector.X;
                    point.Y = vector.Y;
                    point.Z = vector.Z;
                }
            }
        }

        /// <summary>
        /// Объеденить текущую модель с моделью заданную параметром model.
        /// </summary>
        /// <param name="model">модель для добавления в текущую модель.</param>
        void UnionWith(Model model)
        {
            _planes.AddRange(model.Planes);
        }

        #endregion

        #region === private ===

        private static Model Parallelepiped(float width, float height, float depth, float sizePrimitive, bool[] panels, Color[] colors)
        {
            List<Point3D> points = new List<Point3D>
            {
                new Point3D(-width / 2, height / 2, depth / 2),
                new Point3D(width / 2, height / 2, depth / 2),
                new Point3D(width / 2, -height / 2, depth / 2),
                new Point3D(-width / 2, -height / 2, depth / 2),
                new Point3D(-width / 2, height / 2, -depth / 2),
                new Point3D(width / 2, height / 2, -depth / 2),
                new Point3D(width / 2, -height / 2, -depth / 2),
                new Point3D(-width / 2, -height / 2, -depth / 2)
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

            // добавляем борта
            Vector3 trans;
            Matrix4x4 matrixTrans;
            Model border;
            bool[] bordersVisible;
            Color[] bordersColors = new Color[] { color2, color2, color2, color2, color2, color2 };

            bordersVisible = new bool[] { true, true, false, true, true, true };
            border = Parallelepiped(sizeSide / tileRowCount, sizeSide + 2 * sizeSide / tileRowCount, depth, sizePrimitive, bordersVisible, bordersColors);
            trans = new Vector3(-sizeSide / 2 - sizeSide / tileRowCount / 2, 0, 0);
            matrixTrans = Matrix4x4.CreateTranslation(trans);
            border.Transform(matrixTrans);
            model.UnionWith(border);

            bordersVisible = new bool[] { true, true, true, true, false, true };
            border = Parallelepiped(sizeSide / tileRowCount, sizeSide + 2 * sizeSide / tileRowCount, depth, sizePrimitive, bordersVisible, bordersColors);
            trans = new Vector3(sizeSide / 2 + sizeSide / tileRowCount / 2, 0, 0);
            matrixTrans = Matrix4x4.CreateTranslation(trans);
            border.Transform(matrixTrans);
            model.UnionWith(border);

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
