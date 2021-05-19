using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models3DLib
{
    /// <summary>
    /// Декларация методов для создания конвейера отображения.
    /// </summary>
    public interface IRenderPipeline
    {
        /// <summary>
        /// Конвейер отображения.
        /// </summary>
        /// <param name="model">Исходная модель.</param>
        /// <param name="irenderPipelineParameters">Праметры конвейера.</param>
        /// <returns>Коллекция треугольников для отображения.</returns>
        IEnumerable<Triangle> RenderPipeline(Model model, IRenderPipelineParameters irenderPipelineParameters);
    }
}
