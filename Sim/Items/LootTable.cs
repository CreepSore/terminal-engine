using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim.Items
{
    public class LootTable
    {
        private readonly Random random;
        public Dictionary<IItem, IList<LootTableEntry>> Table { get; } = new Dictionary<IItem, IList<LootTableEntry>>();

        public LootTable(int? seed = null)
        {
            if (seed != null)
            {
                random = new Random((int)seed);
                return;
            }

            random = new Random();
        }

        /// <param name="item">The item to add</param>
        /// <param name="priority">Priority is from highest to lowest, meaning lower values will be checked last</param>
        /// <param name="chance">Chance from 0 to 100</param>
        /// <param name="amount">The amount dropped</param>
        /// <param name="group">The drop group. Grouped entries can only drop once per group.</param>
        public LootTable AddEntry(IItem item, int priority, int chance, int amount, int? group = null)
        {
            if (!Table.ContainsKey(item))
            {
                Table[item] = new List<LootTableEntry>();
            }

            if (Table[item].Any(e => e.Priority == priority))
            {
                throw new Exception($"LootTableEntry with priority {priority} has already been added for item {item.Name}");
            }

            Table[item].Add(new LootTableEntry(priority, chance, amount, group));

            return this;
        }

        public IList<ItemStack> GetDrops(IItem item)
        {
            var drops = new List<ItemStack>();
            var potentialDrops = Table[item].OrderBy(e => e.Priority);
            var droppedGroups = new List<int>();

            foreach (var potentialDrop in potentialDrops)
            {
                if (potentialDrop.Group != null && droppedGroups.Contains((int)potentialDrop.Group))
                {
                    continue;
                }

                var throwResult = random.Next(0, 99);
                if (throwResult >= 100 - potentialDrop.Chance)
                {
                    if (potentialDrop.Group != null)
                    {
                        droppedGroups.Add((int)potentialDrop.Group);
                    }

                    drops.Add(new ItemStack(item, potentialDrop.Amount));
                }
            }

            return drops;
        }

        public IList<ItemStack> GetDrops()
        {
            var drops = new List<ItemStack>();

            foreach (var key in Table.Keys)
            {
                drops.AddRange(GetDrops(key));
            }

            return drops;
        }

        public class LootTableEntry
        {
            /// <summary>
            /// Priority is from highest to lowest, meaning lower values will be checked last
            /// </summary>
            public int Priority { get; }
            /// <summary>
            /// Chance from 0 to 100
            /// </summary>
            public int Chance { get; }
            /// <summary>
            /// The amount dropped
            /// </summary>
            public int Amount { get; }
            /// <summary>
            /// The drop group. Grouped entries can only drop once per group.
            /// </summary>
            public int? Group { get; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="priority">Priority is from highest to lowest, meaning lower values will be checked last</param>
            /// <param name="chance">Chance from 0 to 100</param>
            /// <param name="amount">The amount dropped</param>
            /// <param name="group">The drop group. Grouped entries can only drop once per group.</param>
            public LootTableEntry(int priority, int chance, int amount, int? group = null)
            {
                Priority = priority;
                Chance = chance;
                Amount = amount;
                Group = group;
            }
        }
    }
}
