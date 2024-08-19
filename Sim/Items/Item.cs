using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim.Items
{
    public class Item : IItem
    {
        public int Id { get; }
        public string Name { get; }
        public ItemType MainType { get; }
        public IEnumerable<ItemType> Types { get; }
        public ItemCategory MainCategory { get; }
        public IEnumerable<ItemCategory> Categories { get; }
        public double MaxStackMultiplier { get; }

        public Item(
            int id,
            string name,
            ItemType mainType,
            ItemCategory mainCategory,
            IEnumerable<ItemType> types = null,
            IEnumerable<ItemCategory> categories = null,
            double maxStackMultiplier = 1
        )
        {
            Id = id;
            Name = name;
            MainType = mainType;
            MainCategory = mainCategory;
            Types = types ?? new List<ItemType>() {MainType};
            Categories = categories ?? new List<ItemCategory>() {MainCategory};
            MaxStackMultiplier = maxStackMultiplier;

            if (!Types.Contains(MainType))
            {
                Types.Append(MainType);
            }

            if (!Categories.Contains(MainCategory))
            {
                Categories.Append(MainCategory);
            }
        }

        public Item Clone()
        {
            return new Item(
                Id,
                Name,
                MainType,
                MainCategory,
                Types,
                Categories
            );
        }

        public override bool Equals(object obj)
        {
            if(!(obj is Item item))
            {
                return false;
            }

            return Id == item.Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
