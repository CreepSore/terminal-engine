using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim.Items
{
    public static class CraftingRecipes
    {
        public static readonly Dictionary<IItem, Tuple<ItemStack, int>> Recipes = new Dictionary<IItem, Tuple<ItemStack, int>>()
        {
            {Items.ItemChest, new Tuple<ItemStack, int>(new ItemStack(Items.ItemWood, 4), 1)}
        };
    }
}
