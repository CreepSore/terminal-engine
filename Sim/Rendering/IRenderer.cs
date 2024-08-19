using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sim.Rendering
{
    public interface IRenderer
    {
        void Render(IList<IRenderable> renderables);
    }
}
