using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim.Items
{
    public class ItemStack : ItemStack<IItem>
    {
        public ItemStack(IItem item, int amount) : base(item, amount)
        {
        }
    }

    public class ItemStack<T> where T : IItem
    {
        public const int DefaultMaximumAmount = 64;

        public int MaxAmount { get; }

        public T Item { get; }
        public int Amount { get; set; }

        public ItemStack(T item, int amount)
        {
            Item = item;
            Amount = amount;
            MaxAmount = (int)Math.Floor(DefaultMaximumAmount * item.MaxStackMultiplier);
        }

        public int Add(int amount)
        {
            var addedAmount = Amount + amount;
            var delta = MaxAmount - addedAmount;
            var newAmount = Math.Min(MaxAmount, addedAmount);

            Amount = newAmount;

            return delta < 0 ? -delta : 0;
        }

        public ItemStack<T> Remove(int amount)
        {
            var removable = Math.Min(amount, Amount);
            var remaining = Amount - amount;

            if (remaining < 0)
            {
                remaining = 0;
            }

            Amount = remaining;

            return new ItemStack<T>(Item, removable);
        }
    }
}
