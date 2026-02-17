/*
 * Craft System
 * Connor Lonergan
 * Application created in PROG 305-01 Programming II
 * Fall 2026
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CraftingEngine.Entities;

namespace CraftingEngine
{
    public static class Program
    {
        private static readonly Random _random = new Random();
        private static readonly string RECIPE_OUTPUT_FILE = Path.Combine("ProgramII", "Text", "crafting_instructions.txt");
        
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Crafting System ===");
            Console.WriteLine("Welcome to the Crafting Engine!");
            Console.WriteLine("You'll need to ask the trader for ingredients first.\n");

            string? directoryPath = Path.GetDirectoryName(RECIPE_OUTPUT_FILE);
            if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                Console.WriteLine($"Created directory: {directoryPath}");
            }

            var allRecipes = RecipeCatalog.LoadStarterRecipes();

            Console.WriteLine("Available recipes in the game:");
            for (int i = 0; i < allRecipes.Count; i++)
            {
                Console.WriteLine($"  {i + 1}. {allRecipes[i].Name}");
            }
            Console.WriteLine();

            int randomIndex = _random.Next(0, allRecipes.Count);
            var recipeForTrader = allRecipes[randomIndex];

            var player = new Player("Connor");
            var trader = new Trader("Merchant", recipeForTrader);

            bool continuePlaying = true;
            bool hasCrafted = false;
            var availableRecipes = new List<Recipe>(allRecipes);

            while (continuePlaying && availableRecipes.Count > 0 && !hasCrafted)
            {
                Console.WriteLine("\n" + new string('=', 40));
                DisplayPlayerInventory(player.Inventory);
                DisplayRecipes(availableRecipes, player.Inventory);

                int recipeChoice = GetRecipeChoice(availableRecipes);
                
                if (recipeChoice == 0)
                {
                    Console.WriteLine("Thanks for playing!");
                    continuePlaying = false;
                    continue;
                }

                var selectedRecipe = availableRecipes[recipeChoice - 1];
                
                Console.WriteLine($"\nYou selected: {selectedRecipe.Name}");
                Console.WriteLine($"Required ingredients: {string.Join(", ", selectedRecipe.Ingredients.Select(i => i.ToString()))}");
                Console.WriteLine();
                
                Console.Write("Ask the trader for these ingredients? (Y/N): ");
                string? confirmInput = Console.ReadLine();
                string confirm = confirmInput?.Trim().ToUpper() ?? "N";
                
                if (confirm == "Y")
                {
                    Console.WriteLine($"\nAsking trader for ingredients to make {selectedRecipe.Name}...");

                    bool traderHasAllIngredients = true;
                    foreach (var ing in selectedRecipe.Ingredients)
                    {
                        decimal traderAmount = trader.Inventory.GetAmount(ing.Item.Id.ToString());
                        if (traderAmount < ing.Amount)
                        {
                            traderHasAllIngredients = false;
                            break;
                        }
                    }
                    
                    if (traderHasAllIngredients)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Trader says: \"Here are your ingredients!\"");
                        Console.ResetColor();

                        string? fullPath = GenerateCraftingInstructions(selectedRecipe);
                        
                        if (!string.IsNullOrEmpty(fullPath))
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine($"\n📝 Crafting instructions have been saved to: {fullPath}");

                            try
                            {
                                Process.Start(new ProcessStartInfo
                                {
                                    FileName = fullPath,
                                    UseShellExecute = true
                                });
                                Console.WriteLine("📖 Opening the instructions file...");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"⚠️ Could not automatically open the file: {ex.Message}");
                                Console.WriteLine($"Please open it manually at: {fullPath}");
                            }
                            Console.ResetColor();
                        }

                        foreach (var ing in selectedRecipe.Ingredients)
                        {
                            trader.Inventory.Remove(ing.Item.Id.ToString(), ing.Amount);
                            player.Inventory.Add(ing.Item.Id.ToString(), ing.Amount);
                        }
                        
                        Console.WriteLine($"You received the ingredients for {selectedRecipe.Name}!");

                        Console.WriteLine("\nNow attempting to craft...");
                        if (selectedRecipe.Craft(player.Inventory))
                        {
                            var res = selectedRecipe.Result;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"SUCCESS! You crafted {res.Amount} {res.Item.Unit} {res.Item.Name}");
                            Console.ResetColor();

                            hasCrafted = true;
                            Console.WriteLine("\nCongratulations! You successfully crafted an item!");
                        }

                        availableRecipes.Remove(selectedRecipe);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Trader says: \"I don't have all the ingredients you need for that recipe.\"");
                        Console.ResetColor();

                        availableRecipes.Remove(selectedRecipe);

                        if (availableRecipes.Count > 0)
                        {
                            Console.WriteLine("\nTry another recipe. Press Enter to continue...");
                            Console.ReadLine();
                            Console.WriteLine("\n" + new string('=', 40));
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Request cancelled.");

                    Console.WriteLine("\nPress Enter to continue...");
                    Console.ReadLine();
                    Console.WriteLine("\n" + new string('=', 40));
                }
            }

            if (!hasCrafted && availableRecipes.Count == 0)
            {
                Console.WriteLine("\nNo more recipes available to try!");
                Console.WriteLine("\n=== Final Player Inventory ===");
                DisplayPlayerInventory(player.Inventory);
            }
            
            if (hasCrafted)
            {
                Console.WriteLine("\n=== Final Player Inventory ===");
                DisplayPlayerInventory(player.Inventory);
            }
            
            Console.WriteLine("\nGoodbye!");
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }

        private static string? GenerateCraftingInstructions(Recipe recipe)
        {
            try
            {
                string fullPath = Path.GetFullPath(RECIPE_OUTPUT_FILE);

                string? directoryPath = Path.GetDirectoryName(fullPath);
                if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                
                using (StreamWriter writer = new StreamWriter(fullPath))
                {
                    writer.WriteLine("=".PadRight(60, '='));
                    writer.WriteLine("           CRAFTING INSTRUCTIONS");
                    writer.WriteLine("=".PadRight(60, '='));
                    writer.WriteLine();

                    writer.WriteLine($"RECIPE: {recipe.Name}");
                    writer.WriteLine(new string('-', 40));
                    writer.WriteLine();

                    writer.WriteLine("🎯 CRAFTING RESULT:");
                    writer.WriteLine($"   {recipe.Result}");
                    writer.WriteLine();

                    writer.WriteLine("📦 INGREDIENTS REQUIRED:");
                    foreach (var ing in recipe.Ingredients)
                    {
                        writer.WriteLine($"   • {ing}");
                    }
                    writer.WriteLine();

                    writer.WriteLine("📋 STEP-BY-STEP INSTRUCTIONS:");
                    writer.WriteLine();
                    
                    int stepNumber = 1;

                    writer.WriteLine($"   Step {stepNumber++}: Gather all required ingredients:");
                    foreach (var ing in recipe.Ingredients)
                    {
                        writer.WriteLine($"      - {ing.Amount} {ing.Item.Unit} of {ing.Item.Name}");
                    }
                    writer.WriteLine();

                    writer.WriteLine($"   Step {stepNumber++}: Verify you have all ingredients in your inventory");
                    writer.WriteLine($"      (The trader has provided these to you)");
                    writer.WriteLine();

                    writer.WriteLine($"   Step {stepNumber++}: Prepare your crafting workspace");
                    writer.WriteLine($"      Make sure all ingredients are within reach");
                    writer.WriteLine();

                    writer.WriteLine($"   Step {stepNumber++}: Combine the ingredients:");
                    
                    if (recipe.Name.Contains("Chocolate", StringComparison.OrdinalIgnoreCase))
                    {
                        var milkIng = recipe.Ingredients.FirstOrDefault(i => i.Item.Name.Contains("Milk"));
                        var chipIng = recipe.Ingredients.FirstOrDefault(i => i.Item.Name.Contains("Chip"));
                        
                        if (milkIng != null)
                            writer.WriteLine($"      - Heat the {milkIng.Amount} {milkIng.Item.Unit} of {milkIng.Item.Name} in a pot");
                        if (chipIng != null)
                            writer.WriteLine($"      - Slowly stir in the {chipIng.Amount} {chipIng.Item.Unit} of {chipIng.Item.Name}");
                        writer.WriteLine($"      - Whisk continuously until smooth");
                        writer.WriteLine($"      - Pour into a mug and enjoy!");
                    }
                    else if (recipe.Name.Contains("Bread", StringComparison.OrdinalIgnoreCase))
                    {
                        var flourIng = recipe.Ingredients.FirstOrDefault(i => i.Item.Name == "Flour");
                        var yeastIng = recipe.Ingredients.FirstOrDefault(i => i.Item.Name == "Yeast");
                        var waterIng = recipe.Ingredients.FirstOrDefault(i => i.Item.Name == "Water");
                        
                        if (flourIng != null && yeastIng != null)
                            writer.WriteLine($"      - Mix {flourIng.Amount} {flourIng.Item.Unit} of {flourIng.Item.Name} and {yeastIng.Amount} {yeastIng.Item.Unit} of {yeastIng.Item.Name} in a large bowl");
                        if (waterIng != null)
                            writer.WriteLine($"      - Gradually add {waterIng.Amount} {waterIng.Item.Unit} of {waterIng.Item.Name}");
                        writer.WriteLine($"      - Knead the dough for 10 minutes");
                        writer.WriteLine($"      - Let rise for 1 hour");
                        writer.WriteLine($"      - Bake at 375°F for 30 minutes");
                    }
                    else if (recipe.Name.Contains("Potion", StringComparison.OrdinalIgnoreCase))
                    {
                        var herbIng = recipe.Ingredients.FirstOrDefault(i => i.Item.Name == "Herb");
                        var waterIng = recipe.Ingredients.FirstOrDefault(i => i.Item.Name == "Water");
                        
                        if (herbIng != null)
                            writer.WriteLine($"      - Crush the {herbIng.Amount} {herbIng.Item.Unit} of {herbIng.Item.Name} in a mortar");
                        if (waterIng != null)
                            writer.WriteLine($"      - Add to the {waterIng.Amount} {waterIng.Item.Unit} of {waterIng.Item.Name}");
                        writer.WriteLine($"      - Stir gently while chanting healing incantations");
                        writer.WriteLine($"      - Let the mixture sit for 5 minutes");
                        writer.WriteLine($"      - Strain into a clean bottle");
                    }
                    else
                    {
                        writer.WriteLine($"      - Combine all ingredients in a proper crafting vessel");
                        writer.WriteLine($"      - Mix thoroughly until well combined");
                        writer.WriteLine($"      - Apply heat or magic as needed");
                        writer.WriteLine($"      - Wait for the crafting process to complete");
                    }
                    writer.WriteLine();

                    writer.WriteLine($"   Step {stepNumber++}: Complete the crafting process");
                    writer.WriteLine($"      Use the 'Craft' option in the game to finalize");
                    writer.WriteLine();

                    writer.WriteLine("💡 CRAFTING TIPS:");
                    writer.WriteLine($"   • This recipe yields {recipe.Result}");
                    writer.WriteLine($"   • Make sure you have enough inventory space for the result");
                    writer.WriteLine($"   • The quality of ingredients affects the final product");
                    writer.WriteLine($"   • Practice makes perfect!");
                    
                    writer.WriteLine();
                    writer.WriteLine("=".PadRight(60, '='));
                    writer.WriteLine("           HAPPY CRAFTING!");
                    writer.WriteLine("=".PadRight(60, '='));

                    writer.WriteLine();
                    writer.WriteLine($"Instructions generated on: {DateTime.Now}");
                    writer.WriteLine($"Recipe ID: {recipe.Id}");
                    writer.WriteLine($"File location: {fullPath}");
                }
                
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n✅ Successfully generated crafting instructions for {recipe.Name}!");
                Console.ResetColor();
                
                return fullPath;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n❌ Error generating instructions file: {ex.Message}");
                Console.ResetColor();
                return null;
            }
        }

        private static void DisplayPlayerInventory(Inventory inventory)
        {
            Console.WriteLine("=== Your Inventory ===");
            bool hasItems = false;
            
            foreach (var kvp in inventory.Contents.Where(k => k.Value > 0))
            {
                if (ItemRegistry.TryGet(Guid.Parse(kvp.Key), out var item) && item != null)
                {
                    Console.WriteLine($"  {kvp.Value} {item.Unit} {item.Name}");
                    hasItems = true;
                }
            }
            
            if (!hasItems)
            {
                Console.WriteLine("  (Empty)");
            }
            Console.WriteLine();
        }

        private static void DisplayRecipes(List<Recipe> recipes, Inventory playerInventory)
        {
            Console.WriteLine("=== Available Recipes ===");
            for (int i = 0; i < recipes.Count; i++)
            {
                var recipe = recipes[i];
                // Recipes are always yellow since player needs to get ingredients first
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"  {i + 1}. {recipe.Name} - {recipe.Result}");
                Console.ResetColor();
            }
            Console.WriteLine("  0. Exit");
            Console.WriteLine();
        }

        private static int GetRecipeChoice(List<Recipe> recipes)
        {
            while (true)
            {
                Console.Write("Select a recipe to request from trader (0 to exit): ");
                string? input = Console.ReadLine();
                
                if (int.TryParse(input, out int choice))
                {
                    if (choice >= 0 && choice <= recipes.Count)
                    {
                        return choice;
                    }
                }
                
                Console.WriteLine($"Invalid choice. Please enter a number between 0 and {recipes.Count}");
            }
        }
    }
}