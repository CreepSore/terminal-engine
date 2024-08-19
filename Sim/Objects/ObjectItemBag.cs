using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sim.Items;
using Sim.Structs;

namespace Sim.Objects
{
    public class ObjectItemBag : BaseObject
    {
        public IList<ItemStack> Stacks { get; }

        public ObjectItemBag(IList<ItemStack> stacks, Vec3d position)
        {
            Position = position;
            Stacks = stacks;
        }
    }
}
