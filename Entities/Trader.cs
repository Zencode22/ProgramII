namespace CraftingEngine.Entities
{
    public sealed class Trader : Person
    {
        public Trader(string name) : base(name) { }

        // Trader‑specific logic (price markup, stock) belongs here.
    }
}