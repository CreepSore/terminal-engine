using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sim.Objects;

namespace Sim.Items
{
    public class BuildableItem : Item
    {
        private Func<IObject> placeableObjectGenerator;

        public BuildableItem(
            int id,
            string name,
            ItemType mainType,
            ItemCategory mainCategory,
            Func<IObject> placeableObjectGenerator,
            IEnumerable<ItemType> types = null,
            IEnumerable<ItemCategory> categories = null,
            double maxStackMultiplier = 1
        ) : base(id, name, mainType, mainCategory, types, categories, maxStackMultiplier)
        {
            this.placeableObjectGenerator = placeableObjectGenerator;
        }

        public IObject GetPlaceableObject()
        {
            return placeableObjectGenerator();
        }
    }
}
