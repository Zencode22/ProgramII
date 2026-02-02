// Result.cs
// No extra usings are required – the file shares the same namespace as the rest of the engine.
using CraftingEngine;

namespace CraftingEngine
{
    /// <summary>
    /// Represents the output of a recipe – an <see cref="Item"/> together with the amount produced.
    /// The class is immutable once constructed.
    /// </summary>
    public sealed class Result
    {
        /// <summary>The wrapped quantity (item + amount).</summary>
        public Quantity Value { get; }

        /// <summary>
        /// Creates a result from an <see cref="Item"/> and the quantity that will be added to the inventory.
        /// </summary>
        /// <param name="item">The item produced by the recipe.</param>
        /// <param name="amount">How many units of the item are produced.</param>
        public Result(Item item, decimal amount) => Value = new Quantity(item, amount);

        /// <summary>Shortcut to the produced <see cref="Item"/>.</summary>
        public Item Item => Value.Item;

        /// <summary>Shortcut to the produced amount.</summary>
        public decimal Amount => Value.Amount;

        /// <summary>Human‑readable representation, e.g. “5 unit Iron Ore”.</summary>
        public override string ToString() => Value.ToString();
    }
}