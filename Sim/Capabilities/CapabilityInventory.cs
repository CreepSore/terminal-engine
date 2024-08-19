using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sim.Items;

namespace Sim.Capabilities
{
    public class CapabilityInventory : BaseCapability
    {
        public int MaximumStacks;
        public HashSet<ItemStack> Stacks { get; } = new HashSet<ItemStack>();

        public CapabilityInventory(int maximumStacks = 32)
        {
            MaximumStacks = maximumStacks;
        }

        /// <summary>
        /// Tries to add a stack to the inventory
        /// </summary>
        /// <returns>Returns null or the item stack containing the amount of items that couldn't be added</returns>
        public ItemStack AddStack(ItemStack stack)
        {
            var startedStacks = Stacks.Where(s => s.Amount != s.MaxAmount).ToArray();
            var toAdd = stack.Amount;

            for (int i = 0; i < startedStacks.Count() && toAdd > 0; i++)
            {
                var currentStack = startedStacks.ElementAt(i);

                toAdd = currentStack.Add(toAdd);
            }

            stack.Amount = toAdd;
            if (toAdd <= 0)
            {
                return null;
            }

            if (Stacks.Count >= MaximumStacks)
            {
                return stack;
            }

            Stacks.Add(stack);
            return null;
        }

        public IList<ItemStack> RemoveStacks(Item item, int amount, bool partialTake = false)
        {
            var result = new List<ItemStack>();
            var validStacks = Stacks.Where(s => s.Item.Id == item.Id).ToArray();
            var toRemove = amount;
            var startedStack = new ItemStack(item, 0);

            for (int i = 0; i < validStacks.Count() && toRemove > 0; i++)
            {
                var toAdd = Math.Min(toRemove, startedStack.MaxAmount - startedStack.Amount);
                startedStack.Amount = toAdd;
                toRemove -= toAdd;

                if (startedStack.Amount == startedStack.MaxAmount)
                {
                    result.Add(startedStack);
                    startedStack = new ItemStack(item, 0);
                }
            }

            if (!partialTake && toRemove > 0)
            {
                return null;
            }

            return result;
        }

        public override void Tick()
        {
            
        }
    }
}
