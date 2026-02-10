/*
 * Craft System
 * Connor Lonergan
 * Application created in PROG 305-01 Programming II
 * Fall 2026
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CraftingEngine.Entities;

namespace CraftingEngine
{
    public static class Program
    {
        private static readonly Random _random = new Random();
        
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Crafting System ===");
            Console.WriteLine("Welcome to the Crafting Engine!");
            Console.WriteLine("You'll need to ask the trader for ingredients first.\n");
            
            // Load all available recipes
            var allRecipes = RecipeCatalog.LoadStarterRecipes();
            var availableRecipes = new List<Recipe>(allRecipes);
            
            // Randomly select one recipe to give the trader ingredients for
            var recipeForTrader = allRecipes[_random.Next(allRecipes.Count)];
            
            var player = new Player("Connor");
            var trader = new Trader("Merchant", recipeForTrader);

            Console.WriteLine($"The trader specializes in various recipes.\n");

            bool continuePlaying = true;
            bool hasCrafted = false;

            while (continuePlaying && availableRecipes.Count > 0 && !hasCrafted)
            {
                Console.WriteLine("\n" + new string('=', 40));
                DisplayPlayerInventory(player.Inventory);
                DisplayRecipes(availableRecipes, player.Inventory);

                var recipeChoice = GetRecipeChoice(availableRecipes);
                
                if (recipeChoice == 0) // Exit option
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
                var confirm = Console.ReadLine()?.Trim().ToUpper();
                
                if (confirm == "Y")
                {
                    Console.WriteLine($"\nAsking trader for ingredients to make {selectedRecipe.Name}...");
                    
                    // Check if trader has all required ingredients
                    bool traderHasAllIngredients = true;
                    foreach (var ing in selectedRecipe.Ingredients)
                    {
                        var traderAmount = trader.Inventory.GetAmount(ing.Item.Id.ToString());
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
                        
                        // Transfer ingredients from trader to player
                        foreach (var ing in selectedRecipe.Ingredients)
                        {
                            // Remove from trader
                            trader.Inventory.Remove(ing.Item.Id.ToString(), ing.Amount);
                            // Add to player
                            player.Inventory.Add(ing.Item.Id.ToString(), ing.Amount);
                        }
                        
                        Console.WriteLine($"You received the ingredients for {selectedRecipe.Name}!");
                        
                        // Now attempt to craft
                        Console.WriteLine("\nNow attempting to craft...");
                        if (selectedRecipe.Craft(player.Inventory))
                        {
                            var res = selectedRecipe.Result;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"SUCCESS! You crafted {res.Amount} {res.Item.Unit} {res.Item.Name}");
                            Console.ResetColor();
                            
                            // Player has crafted successfully - program will end
                            hasCrafted = true;
                            Console.WriteLine("\nCongratulations! You successfully crafted an item!");
                        }
                        
                        // Remove this recipe from available options since it was attempted
                        availableRecipes.Remove(selectedRecipe);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Trader says: \"I don't have all the ingredients you need for that recipe.\"");
                        Console.ResetColor();
                        
                        // Remove this recipe from available options since trader doesn't have ingredients
                        availableRecipes.Remove(selectedRecipe);
                        
                        // Loop back to start (continue the loop)
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
                    
                    // Loop back to start
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

        private static void DisplayPlayerInventory(Inventory inventory)
        {
            Console.WriteLine("=== Your Inventory ===");
            bool hasItems = false;
            
            foreach (var kvp in inventory.Contents.Where(k => k.Value > 0))
            {
                if (ItemRegistry.TryGet(Guid.Parse(kvp.Key), out var item))
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
                var input = Console.ReadLine();
                
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