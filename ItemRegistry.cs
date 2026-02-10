using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using CraftingEngine;

namespace CraftingEngine
{
    public static class ItemRegistry
    {
        private static readonly Dictionary<Guid, Item> _lookup = new();

        public static void Register(Item item) => _lookup[item.Id] = item;

        public static bool TryGet(
            Guid id,
            [NotNullWhen(true)] out Item? item)
        {
            return _lookup.TryGetValue(id, out item);
        }
    }
}