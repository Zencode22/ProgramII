using System.Collections.Generic;

namespace CraftingEngine
{
    public static class RecipeCatalog
    {
        public static List<Recipe> LoadStarterRecipes()
        {
            // ----- Aliases for readability -----
            var m  = GameItems.Milk;
            var cc = GameItems.ChocolateChip;
            var hc = GameItems.HotChocolate;
            var f  = GameItems.Flour;
            var w  = GameItems.Water;
            var y  = GameItems.Yeast;
            var b  = GameItems.Bread;
            var h  = GameItems.Herb;
            var hp = GameItems.HealingPotion;
            var s  = GameItems.Sugar;   // new material

            // ----- 1️⃣ Hot Chocolate -----
            var hotChocolateRecipe = new Recipe(
                name: "Hot Chocolate",
                result: new Result(hc, 12m),   // 12 ounces
                ingredients: new[]
                {
                    new Ingredient(m, 4m),       // 4 cups Milk
                    new Ingredient(cc, 0.5m)     // ½ cup Chocolate Chips
                },
                isStarter: true);

            // ----- 2️⃣ Bread -----
            var breadRecipe = new Recipe(
                name: "Bread",
                result: new Result(b, 1m),      // 1 loaf
                ingredients: new[]
                {
                    new Ingredient(f, 3m),       // 3 cups Flour
                    new Ingredient(w, 1.5m),     // 1.5 cups Water
                    new Ingredient(y, 0.02m)     // 0.02 cup Yeast
                },
                isStarter: true);

            // ----- 3️⃣ Healing Potion -----
            var potionRecipe = new Recipe(
                name: "Healing Potion",
                result: new Result(hp, 1m),     // 1 bottle
                ingredients: new[]
                {
                    new Ingredient(h, 2m),       // 2 pieces Herb
                    new Ingredient(w, 0.5m)      // ½ cup Water
                },
                isStarter: true);

            // ----- 4️⃣ Sweet Hot Chocolate (uses Sugar) -----
            var sweetHotChocolateRecipe = new Recipe(
                name: "Sweet Hot Chocolate",
                result: new Result(hc, 12m),    // same result item, same amount
                ingredients: new[]
                {
                    new Ingredient(m, 4m),       // 4 cups Milk
                    new Ingredient(cc, 0.5m),    // ½ cup Chocolate Chips
                    new Ingredient(s, 0.25m)     // ¼ cup Sugar (new material)
                },
                isStarter: false); // not part of the initial three

            // Return the full list (three starters + the new Sweet recipe)
            return new List<Recipe>
            {
                hotChocolateRecipe,
                breadRecipe,
                potionRecipe,
                sweetHotChocolateRecipe
            };
        }
    }
}