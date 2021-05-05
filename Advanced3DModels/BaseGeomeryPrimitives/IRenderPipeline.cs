using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models3DLib
{
    interface IRenderPipeline
    {
        IEnumerable<Triangle> RenderPipeline(Model model, IRenderPipelineParameters irenderPipelineParameters);
    }
}
