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
        /// <summary>
        /// Объединить текущую модель с моделью заданную параметром model.
        /// </summary>
        /// <param name="model">Модель для добавления.</param>
        public void UnionWith(Model model)
        {
            Planes.AddRange(model.Planes);
        }

        /// <summary>
        /// Признак необхожимости сортировки по глубине (Z).
        /// </summary>
        public bool NeedToSort { get; set; } = false;

        public List<Plane> Planes { get; set; } = new List<Plane>();

        public void SaveState()
        {
            foreach(Plane plane in Planes)
            {
                plane.SaveState();
            }
        }

        public void RestoreState()
        {
            foreach (Plane plane in Planes)
            {
                plane.RestoreState();
            }
        }

        public void Transform(Matrix4x4 matrix)
        {
            foreach (Plane plane in Planes)
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
            foreach (Plane plane in Planes)
            {
                foreach (IPoint3D point in plane.Points)
                {
                    IPoint3D perspectivePoint = iperspectiveTransform.Transform(point, centerOfPerspective);

                    point.X = perspectivePoint.X;
                    point.Y = perspectivePoint.Y;
                }
            }
        }

        public IEnumerable<Triangle> GetTrianglesForRender(RenderModelType renderType)
        {
            IEnumerable<Triangle> triangles = null;

            if (renderType == RenderModelType.FillFull || renderType == RenderModelType.FillSolidColor)
            {
                IEnumerable<Triangle> all = Planes.SelectMany(pl => pl.Triangles);
                IEnumerable<Triangle> trs = all.Where(x => x.VisibleBackSide || x.Normal.Z < 0);
                triangles = NeedToSort ? trs.OrderByDescending(t => t.MinZ).AsEnumerable() : trs;
            }
            else if (renderType == RenderModelType.Triangulations)
            {
                triangles = Planes.SelectMany(x => x.Triangles);
            }

            return triangles;
        }
    }
}
