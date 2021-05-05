using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models3DLib
{
    class RenderPipeline : IRenderPipeline
    {
        IEnumerable<Triangle> IRenderPipeline.RenderPipeline(Model model, IRenderPipelineParameters irenderPipelineParameters)
        {
            IEnumerable<Triangle> triangles = null;

            if (!irenderPipelineParameters.PerspectiveEnable)
            {
                // TODO
            }
            else
            {
                // TODO
            }

            return triangles;
        }
    }
}
