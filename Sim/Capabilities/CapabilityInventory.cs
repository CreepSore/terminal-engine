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
            if (stack == null)
            {
                return null;
            }

            var startedStacks = Stacks.Where(s => s != null && s.Item.Id == stack.Item.Id && s.Amount != s.MaxAmount).ToArray();
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

        public IList<ItemStack> RemoveStacks(IItem item, int amount, bool partialTake = false)
        {
            var result = new List<ItemStack>();
            var validStacks = Stacks.Where(s => s != null && s.Item.Id == item.Id).ToArray();
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

            if (startedStack.Amount > 0)
            {
                result.Add(startedStack);
            }

            if (!partialTake && toRemove > 0)
            {
                return null;
            }

            return result;
        }

        public IList<ItemStack> TransferFullInventory(CapabilityInventory targetInventory)
        {
            return new List<ItemStack>(Stacks).Select(stack => TransferStack(targetInventory, stack)).ToList();
        }

        /// <param name="stack">The stack to split</param>
        /// <param name="amount">The amount that the splitted stack should have</param>
        /// <returns>The splitted Item Stack or null if it wasn't possible</returns>
        public ItemStack<T> SplitStack<T>(ItemStack<T> stack, int amount) where T : IItem
        {
            if (stack.Amount <= amount)
            {
                return null;
            }

            stack.Amount -= amount;
            var splitted = new ItemStack(stack.Item, amount);
            Stacks.Add(splitted);

            return splitted as ItemStack<T>;
        }

        /// <summary>
        /// Transfers an item stack to a specified target inventory
        /// </summary>
        /// <param name="targetInventory">The target inventory that should receive the item stack</param>
        /// <param name="stack">The stack to transfer</param>
        /// <returns>Returns a new item stack containing items that could not be transferred</returns>
        public ItemStack TransferStack(CapabilityInventory targetInventory, ItemStack stack)
        {
            if (!Stacks.Contains(stack))
            {
                return stack;
            }

            Stacks.Remove(stack);
            var nonTransferrable = targetInventory.AddStack(stack);
            Stacks.Add(nonTransferrable);

            return nonTransferrable;
        }

        public ItemStack GetItemStack<T>(T item, int minAmount = 1) where T : IItem
        {
            return Stacks.FirstOrDefault(s => s != null && s.Item.Id == item.Id && s.Amount >= minAmount);
        }

        public bool HasItem(IItem item)
        {
            return HasItem(item, 1);
        }

        public bool HasItem(IItem item, int amount)
        {
            return Stacks
                .Where(s => s != null && s.Item.Id == item.Id)
                .Select(s => s.Amount)
                .Sum() >= amount;
        }
    }
}
