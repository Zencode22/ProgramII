namespace CraftingEngine.Entities
{
    public sealed class Trader : Person
    {
        public Trader(string name, Recipe recipeForTrader) : base(name, isTrader: true)
        {
            // Add ingredients for the selected recipe (double the required amount)
            foreach (var ingredient in recipeForTrader.Ingredients)
            {
                Inventory.Add(ingredient.Item.Id.ToString(), ingredient.Amount * 2);
            }
            
            // Also add some extra common items
            Inventory.Add(GameItems.Milk.Id.ToString(), 5m);
            Inventory.Add(GameItems.Water.Id.ToString(), 3m);
        }
    }
}