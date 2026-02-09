/*
 * Craft System
 * Connor Lonergan
 * Application created in PROG 305-01 Programming II
 * Fall 2026
 */

using System;
using System.Linq;
using CraftingEngine.Entities;   // new entity types
using CraftingEngine.Test;      // test‑data loader

namespace CraftingEngine
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // Load the combined recipe list (original starters + temporary test recipe)
            var recipes = TestData.LoadTestRecipes();

            // Create a player (holds his own inventory)
            var player = new Player("Connor");

            Console.WriteLine("=== All Recipes ===");
            foreach (var r in recipes) Console.WriteLine(r);
            Console.WriteLine();

            foreach (var recipe in recipes)
            {
                Console.WriteLine($"Attempting to craft: {recipe.Name}");
                if (recipe.Craft(player.Inventory))
                {
                    var res = recipe.Result;
                    Console.WriteLine(
                        $"  SUCCESS – you now have {player.Inventory.GetAmount(res.Item.Id.ToString())} {res.Item.Unit} {res.Item.Name}");
                }
                else
                {
                    Console.WriteLine("  FAILED – insufficient ingredients");
                }
                Console.WriteLine();
            }

            Console.WriteLine("=== Final Inventory ===");
            foreach (var kvp in player.Inventory.Contents.Where(k => k.Value > 0))
            {
                if (ItemRegistry.TryGet(Guid.Parse(kvp.Key), out var item))
                {
                    Console.WriteLine($"{kvp.Value} {item.Unit} {item.Name}");
                }
                else
                {
                    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                }
            }
        }
    }
}