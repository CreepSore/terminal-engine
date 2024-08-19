using Sim.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim.Rendering
{
    public interface IRenderable : IWithPosition
    {
        IList<IRenderable> RenderableChildren { get; }
    }
}
