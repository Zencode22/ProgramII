// Test/TestData.cs
using System.Collections.Generic;
using CraftingEngine;

namespace CraftingEngine.Test
{
    /// <summary>
    /// Temporary test data used while the real data‑import pipeline is under construction.
    /// All objects defined here are deliberately simple and self‑contained.
    /// </summary>
    public static class TestData
    {
        /// <summary>
        /// Constructs a recipe whose ingredients exactly match the three items
        /// automatically added to every <see cref="Person"/> (Milk, Chocolate Chip, Sugar).
        /// The result is a “Sweet Hot Chocolate” – the same output item used in the
        /// official starter catalog, but with a custom ingredient list.
        /// </summary>
        public static Recipe CreateStarterKitRecipe()
        {
            // Ingredients correspond to the three items added in Person's constructor.
            var ingredients = new[]
            {
                new Ingredient(GameItems.Milk,          4m),   // 4 cups Milk
                new Ingredient(GameItems.ChocolateChip, 0.5m), // ½ cup Chocolate Chips
                new Ingredient(GameItems.Sugar,         0.25m) // ¼ cup Sugar (new material)
            };

            // The result mirrors the existing Sweet Hot Chocolate recipe:
            // 12 ounces of Hot Chocolate.
            var result = new Result(GameItems.HotChocolate, 12m);

            return new Recipe(
                name: "Starter Sweet Hot Chocolate",
                result: result,
                ingredients: ingredients,
                isStarter: true);
        }

        /// <summary>
        /// Returns a combined list of recipes:
        ///   • All starter recipes defined in <see cref="RecipeCatalog"/>.
        ///   • The temporary “Starter Sweet Hot Chocolate” recipe above.
        /// This method is the single point of entry the main program should call.
        /// </summary>
        public static List<Recipe> LoadTestRecipes()
        {
            // Grab the existing hard‑coded starter recipes.
            var list = RecipeCatalog.LoadStarterRecipes();

            // Append the temporary recipe for testing purposes.
            list.Add(CreateStarterKitRecipe());

            return list;
        }
    }
}