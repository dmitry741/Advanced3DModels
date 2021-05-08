using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models3DLib
{
    /// <summary>
    /// Класс грани модели.
    /// </summary>
    public class Plane
    {
        readonly protected List<IPoint3D> _point3Ds = new List<IPoint3D>();
        readonly protected List<Triangle> _triangles = new List<Triangle>();
        protected Stack<IPoint3D[]> _stackState = new Stack<IPoint3D[]>();

        /// <summary>
        /// Сохранить состояние грани в стеке.
        /// </summary>
        public void PushState()
        {
            IPoint3D[] point3Ds = _point3Ds.Select(p => ResolverInterface.ResolveIPoint3D(p)).ToArray();
            _stackState.Push(point3Ds);
        }

        /// <summary>
        /// Восстановить состояние грани.
        /// </summary>
        public void PopState()
        {
            if (_stackState.Count > 0)
            {
                IPoint3D[] point3Ds = _stackState.Pop();

                for (int i = 0; i < point3Ds.Length; i++)
                {
                    _point3Ds[i].X = point3Ds[i].X;
                    _point3Ds[i].Y = point3Ds[i].Y;
                    _point3Ds[i].Z = point3Ds[i].Z;
                }
            }
        }

        /// <summary>
        /// Установить цвет грани.
        /// </summary>
        /// <param name="color">Объект Color.</param>
        public void SetColor(Color color)
        {
            // цвет для треугольников
            foreach (Triangle triangle in _triangles)
            {
                triangle.BaseColor = color;
            }
        }

        /// <summary>
        /// Коллекция точек грани.
        /// </summary>
        public IEnumerable<IPoint3D> Points => _point3Ds;

        /// <summary>
        /// Коллекция треугольников из которых состоит грань.
        /// </summary>
        public IEnumerable<Triangle> Triangles => _triangles;

        /// <summary>
        /// Имя грани.
        /// </summary>
        public string Name { get; set; }
    }
}
