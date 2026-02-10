namespace CraftingEngine.Entities
{
    public sealed class Player : Person
    {
        public Player(string name) : base(name, isTrader: false) { }
    }
}