using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sim.Items;

namespace Sim.Capabilities
{
    public class CapabilityCrafting : BaseCapability
    {
        public IList<IItem> CraftableItems { get; set; }

        public CapabilityCrafting(IList<IItem> craftableItems)
        {
            CraftableItems = craftableItems ?? new List<IItem>();
        }

        public override void Tick()
        {
            
        }

        public ItemStack CraftItems(IItem item)
        {
            if (!CanCraft(item, out var recipe))
            {
                return null;
            }

            var inventory = CapabilityObject.GetCapability<CapabilityInventory>();
            if (inventory.RemoveStacks(recipe.Item1.Item, recipe.Item1.Amount, false).Count == 0)
            {
                return null;
            }

            var craftingResult = new ItemStack(item, recipe.Item2);
            inventory.AddStack(craftingResult);
            return craftingResult;
        }

        public bool CanCraft(IItem item, out Tuple<ItemStack, int> recipe)
        {
            recipe = null;
            var inventory = CapabilityObject.GetCapability<CapabilityInventory>();
            if (inventory == null)
            {
                return false;
            }

            if (!CraftingRecipes.Recipes.TryGetValue(item, out recipe))
            {
                return false;
            }

            if (!inventory.HasItem(recipe.Item1.Item, recipe.Item2))
            {
                recipe = null;
                return false;
            }

            return true;
        }
    }
}
