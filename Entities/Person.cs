using System;
using CraftingEngine;

namespace CraftingEngine.Entities
{
    /// <summary>
    /// Base class for any character that owns an inventory.
    /// </summary>
    public abstract class Person
    {
        public string Name { get; }
        public Inventory Inventory { get; }

        protected Person(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Inventory = new Inventory();

            // ---- Temporary starter items (will be removed later) ----
            Inventory.Add(GameItems.Milk.Id.ToString(),          10m);
            Inventory.Add(GameItems.ChocolateChip.Id.ToString(),  2m);
            Inventory.Add(GameItems.Sugar.Id.ToString(),         1m);
        }
    }
}