using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;   // for NotNullWhen

// NOTE: The namespace must match every other file (CraftingEngine)
// Adding an explicit `using CraftingEngine;` makes the dependency on Item obvious
// and prevents the CS0246 “type or namespace name 'Item' could not be found” error.
using CraftingEngine;

namespace CraftingEngine
{
    public static class ItemRegistry
    {
        // The lookup dictionary stores Item instances by their Guid.
        private static readonly Dictionary<Guid, Item> _lookup = new();

        // Register a new Item (called from GameItems.cs)
        public static void Register(Item item) => _lookup[item.Id] = item;

        // When this returns true, the out parameter is guaranteed non‑null.
        public static bool TryGet(
            Guid id,
            [NotNullWhen(true)] out Item? item)   // <-- attribute tells the compiler
        {
            return _lookup.TryGetValue(id, out item);
        }
    }
}