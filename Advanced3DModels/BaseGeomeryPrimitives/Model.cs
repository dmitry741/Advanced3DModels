using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;

namespace Models3DLib
{
    /// <summary>
    /// Класс полигональной модели.
    /// </summary>
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

        /// <summary>
        /// Список плоскостей (граней).
        /// </summary>
        public List<Plane> Planes { get; set; } = new List<Plane>();

        /// <summary>
        /// Сохранить состояние модели в стеке.
        /// </summary>
        public void PushState()
        {
            Planes.ForEach(plane => plane.PushState());
        }

        /// <summary>
        /// Восстановить состояние модели.
        /// </summary>
        public void PopState()
        {
            Planes.ForEach(plane => plane.PopState());
        }

        /// <summary>
        /// Преобразование модели (Параллельный перенос или вращение).
        /// </summary>
        /// <param name="matrix">Матрица преобразования.</param>
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

        /// <summary>
        /// Перспективное преобразование модели.
        /// </summary>
        /// <param name="iperspectiveTransform">Интерфейс перспективного преобразования/</param>
        /// <param name="centerOfPerspective">Центр перспективы.</param>
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

        /// <summary>
        /// Получение коллекции треугольников для рендеринга.
        /// </summary>
        /// <param name="renderType">Тип рендеринга.</param>
        /// <returns>Коллекция треугольников.</returns>
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

        /// <summary>
        /// Наложение текстуры на грани с именем tag.
        /// </summary>
        /// <param name="tag">Имя граней, на которые будет накладываться текстура.</param>
        /// <param name="bitmap">Объект Bitmap. Текстураю</param>
        /// <param name="stretch">Параметр накладывания текстуры. Если stretch равно True текстура растянется по грани, 
        /// в противном случае текстура будет дублироваться.
        /// </param>
        public void SetTexture(string tag, Bitmap bitmap, bool stretch)
        {
            Rectangle r = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            System.Drawing.Imaging.BitmapData bitmapData = bitmap.LockBits(r, System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            int stride = Math.Abs(bitmapData.Stride);
            int bytes = stride * bitmapData.Height;
            byte[] rgbValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(bitmapData.Scan0, rgbValues, 0, bytes);

            List<Polygon4Plane> planes = Planes.
                Where(plane => (plane.Name == tag) && plane is Polygon4Plane).
                Select(plane => plane as Polygon4Plane).
                ToList();

            planes.ForEach(plane => plane.SetTexture(rgbValues, stride, bitmap.Width, bitmap.Height, stretch));

            bitmap.UnlockBits(bitmapData);
        }
    }
}
