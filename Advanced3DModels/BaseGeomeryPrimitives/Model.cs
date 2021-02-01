﻿using System;
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

        public void AddPlane(Plane plane)
        {
            _planes.Add(plane);
        }

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
                _planes = planes,
            };
        }

        #endregion

        #region === models ===

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
