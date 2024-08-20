using System.Collections.Generic;
using System.ComponentModel;
using Sim.Objects;

namespace Sim.Items
{
    public static class Items
    {
        public static Item ItemWood => new Item(1000, "Wood", ItemType.Resource, ItemCategory.Resources, null, new[]
        {
            ItemCategory.Resources,
            ItemCategory.BuildingMaterials,
            ItemCategory.CraftingMaterials
        });

        public static BuildableItem ItemChest => new BuildableItem(
            10001,
            "Chest",
            ItemType.Placeable,
            ItemCategory.Container,
            () => new ObjectChest()
        );

        public static IList<IItem> GetAll()
        {
            var result = new List<IItem>();
            var type = typeof(Items);
            
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                result.Add(property.GetValue(null) as IItem);
            }

            return result;
        }
    }
}