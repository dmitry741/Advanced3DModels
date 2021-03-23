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
        protected IPoint3D[] _state = null;

        /// <summary>
        /// Сохранить состояние грани в пространстве.
        /// </summary>
        public void SaveState()
        {
            if (_state == null || _state.Length != _point3Ds.Count)
            {
                _state = ResolverInterface.ResolveArrayIPoint3D(_point3Ds.Count);
                _state = _state.Select(p => ResolverInterface.ResolveIPoint3D(0, 0, 0)).ToArray();
            }

            for (int i = 0; i < _state.Length; i++)
            {
                _state[i].X = _point3Ds[i].X;
                _state[i].Y = _point3Ds[i].Y;
                _state[i].Z = _point3Ds[i].Z;
            }
        }

        /// <summary>
        /// Восстановить состояние грани.
        /// </summary>
        public void RestoreState()
        {
            for (int i = 0; i < _state.Length; i++)
            {
                _point3Ds[i].X = _state[i].X;
                _point3Ds[i].Y = _state[i].Y;
                _point3Ds[i].Z = _state[i].Z;
            }
        }

        /// <summary>
        /// Установить цвет грани.
        /// </summary>
        /// <param name="color"></param>
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
