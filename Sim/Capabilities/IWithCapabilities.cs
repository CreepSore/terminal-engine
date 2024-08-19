using Sim.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim.Capabilities
{
    public interface IWithCapabilities
    {
        IList<ICapability> Capabilities { get; }

        void AddCapability(ICapability capability);
        bool HasCapability<T>() where T : ICapability;
        T GetCapability<T>() where T : ICapability;
    }
}
