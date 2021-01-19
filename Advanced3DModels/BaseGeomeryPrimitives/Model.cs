using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models3DLib
{
    public class Model
    {
        #region === members ===

        List<Plane> _planes = new List<Plane>();

        #endregion

        #region === public ===

        public IEnumerable<Plane> Planes => _planes;

        #endregion

        #region === presets ===

        public static Model Cube
        {
            get
            {
                return default;
            }
        }

        #endregion
    }
}
