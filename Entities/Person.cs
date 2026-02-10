using System;
using CraftingEngine;

namespace CraftingEngine.Entities
{
    public abstract class Person
    {
        public string Name { get; }
        public Inventory Inventory { get; }

        protected Person(string name, bool isTrader = false)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Inventory = new Inventory();

            // Traders get their inventory from Trader constructor
            // Players start with empty inventory (will be populated in Program.cs)
        }
    }
}