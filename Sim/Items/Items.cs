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
    }
}