using System;
using System.Linq;

namespace CraftingEngine
{
    // --------------------------------------------------------------
    // Public entry‑point class – required for the .NET runtime.
    // --------------------------------------------------------------
    public static class Program
    {
        // Public static Main – the canonical entry point.
        // You may keep the parameterless signature or accept args.
        public static void Main(string[] args)
        {
            // Load recipes (they reference the shared GameItems)
            var recipes = RecipeCatalog.LoadStarterRecipes();

            Console.WriteLine("=== All Recipes ===");
            foreach (var r in recipes) Console.WriteLine(r);
            Console.WriteLine();

            // Populate inventory using the same Item instances
            var inventory = new Inventory();
            inventory.Add(GameItems.Milk.Id.ToString(),          10m);
            inventory.Add(GameItems.ChocolateChip.Id.ToString(),  2m);
            inventory.Add(GameItems.Flour.Id.ToString(),         5m);
            inventory.Add(GameItems.Water.Id.ToString(),         5m);
            inventory.Add(GameItems.Yeast.Id.ToString(),         0.1m);
            inventory.Add(GameItems.Herb.Id.ToString(),          5m);
            inventory.Add(GameItems.Sugar.Id.ToString(),         1m); // needed for Sweet Hot Chocolate

            // Attempt to craft each recipe
            foreach (var recipe in recipes)
            {
                Console.WriteLine($"Attempting to craft: {recipe.Name}");
                if (recipe.Craft(inventory))
                {
                    var res = recipe.Result;
                    Console.WriteLine(
                        $"  SUCCESS – you now have {inventory.GetAmount(res.Item.Id.ToString())} {res.Item.Unit} {res.Item.Name}");
                }
                else
                {
                    Console.WriteLine("  FAILED – insufficient ingredients");
                }
                Console.WriteLine();
            }

            // Final inventory snapshot (human‑readable)
            Console.WriteLine("=== Final Inventory ===");
            foreach (var kvp in inventory.Contents.Where(k => k.Value > 0))
            {
                if (ItemRegistry.TryGet(Guid.Parse(kvp.Key), out var item))
                {
                    Console.WriteLine($"{kvp.Value} {item.Unit} {item.Name}");
                }
                else
                {
                    // Should never happen if the registry is complete
                    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                }
            }
        }
    }
}