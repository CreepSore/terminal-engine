using System.Collections.Generic;

namespace Sim.Items
{
    public interface IItem
    {
        int Id { get; }
        string Name { get; }
        ItemType MainType { get; }
        IEnumerable<ItemType> Types { get; }
        ItemCategory MainCategory { get; }
        IEnumerable<ItemCategory> Categories { get; }

        double MaxStackMultiplier { get; }
    }
}