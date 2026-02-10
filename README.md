# ProgramII - Interactive Crafting System

## Project Information
- **Course:** PROG 305-01 Programming II
- **Author:** Connor Lonergan
- **Term:** Fall 2026
- **Version:** 2.0 - Interactive Trading & Crafting

## 📊 Updated UML Class Diagram

```plantuml
@startuml
' Interactive Crafting System UML Class Diagram
' For github.com/Zencode22/ProgramII
' Created with DeepSeek AI assistance
' Educational project for PROG 305-01 Programming II

skinparam class {
    BackgroundColor White
    BorderColor Black
    ArrowColor #007ACC
    AttributeFontColor Black
    AttributeFontSize 13
    MethodFontColor Black
    MethodFontSize 13
}

title ProgramII - Interactive Crafting System\nPROG 305-01 Programming II - Fall 2026

note left
  <b>Author:</b> Connor Lonergan
  <b>Course:</b> PROG 305-01 Programming II
  <b>Term:</b> Fall 2026
  <b>Version:</b> 2.0 - Interactive System
  <b>AI Assistance:</b> DeepSeek
  <b>Disclaimer:</b> Educational project
  <b>Game Flow:</b>
  1. Player starts with empty inventory
  2. Trader has random recipe ingredients
  3. Player requests ingredients from trader
  4. If trader has items: craft & end game
  5. If not: loop with remaining recipes
end note

' Enums
enum ItemCategory {
    Material
    Consumable
    Weapon
    Tool
    Quest
    Misc
}

enum Rarity {
    Common
    Uncommon
    Rare
    Epic
    Legendary
}

' Core Domain Classes
class Item {
    +Guid Id { get; init; }
    +string Name { get; init; }
    +string Description { get; init; }
    +string Unit { get; init; }
    +decimal BasePrice { get; private set; }
    +bool Stackable { get; init; }
    +int MaxStackSize { get; init; }
    +ItemCategory Category { get; init; }
    +Rarity Rarity { get; init; }
    --
    +Item(string, string, decimal, bool, int, ItemCategory, Rarity, string)
    +Equals(Item?) : bool
    +ApplyDiscount(decimal) : void
    +ToString() : string
}

class Quantity {
    +Item Item { get; }
    +decimal Amount { get; }
    --
    +Quantity(Item, decimal)
    +Equals(Quantity?) : bool
    +ToString() : string
}

class Ingredient {
    +Quantity Value { get; }
    +Item Item { get; }
    +decimal Amount { get; }
    --
    +Ingredient(Item, decimal)
    +ToString() : string
}

class Result {
    +Quantity Value { get; }
    +Item Item { get; }
    +decimal Amount { get; }
    --
    +Result(Item, decimal)
    +ToString() : string
}

class Recipe {
    +Guid Id { get; init; }
    +string Name { get; init; }
    +Result Result { get; init; }
    +IReadOnlyList<Ingredient> Ingredients { get; init; }
    +bool IsStarter { get; init; }
    --
    +Recipe(string, Result, IEnumerable<Ingredient>, bool)
    +CanCraft(Inventory) : bool
    +Craft(Inventory) : bool
    +ToString() : string
}

class Inventory {
    -Dictionary<string, decimal> _store
    +IReadOnlyDictionary<string, decimal> Contents { get; }
    --
    +Add(string, decimal) : void
    +Remove(string, decimal) : bool
    +Has(string, decimal) : bool
    +GetAmount(string) : decimal
}

' Static Registries
class ItemRegistry {
    -static Dictionary<Guid, Item> _lookup
    --
    +static Register(Item) : void
    +static TryGet(Guid, out Item?) : bool
}

class GameItems {
    +static readonly Item Milk
    +static readonly Item ChocolateChip
    +static readonly Item HotChocolate
    +static readonly Item Flour
    +static readonly Item Water
    +static readonly Item Yeast
    +static readonly Item Bread
    +static readonly Item Herb
    +static readonly Item HealingPotion
    +static readonly Item Sugar
    --
    -static Register(Item) : Item
}

class RecipeCatalog {
    +static LoadStarterRecipes() : List<Recipe>
}

' Entities (in Entities/ folder)
abstract class Person {
    +string Name { get; }
    +Inventory Inventory { get; }
    --
    #Person(string, bool)
}

class Player {
    +Player(string) : base(name, false)
}

class Trader {
    +Trader(string, Recipe) : base(name, true)
}

' Utilities (in Utilities/ folder)
class ParseUtils {
    +static ConvertStringToInteger(string) : int
    +static ConvertStringToDouble(string) : double
    +static ConvertStringToFloat(string) : float
    +static ConvertCsvToDecimalArray(string) : decimal[]
    +static TryConvertStringToInteger(string, out int) : bool
    +static NormaliseKey(string) : string
}

class Program {
    -static Random _random
    +static Main(string[]) : void
    -static DisplayPlayerInventory(Inventory) : void
    -static DisplayRecipes(List<Recipe>, Inventory) : void
    -static GetRecipeChoice(List<Recipe>) : int
}

note top of Program
  /*
  * Interactive Crafting System
  * Connor Lonergan
  * PROG 305-01 Programming II
  * Fall 2026
  *
  * Game Flow:
  * 1. Player starts with empty inventory
  * 2. Trader gets random recipe ingredients
  * 3. Player selects recipe to request
  * 4. Trader checks inventory
  * 5. If successful: craft & end
  * 6. If failed: remove recipe & loop
  */
end note

' Relationships
Ingredient --* Quantity : "1"
Result --* Quantity : "1"
Quantity --* Item : "1"
Recipe "1" *-- "1" Result : produces
Recipe "1" *-- "*" Ingredient : requires
Inventory -- "*" Item : stores (by Id)
RecipeCatalog ..> Recipe : creates
GameItems ..> Item : instantiates
GameItems ..> ItemRegistry : registers with

' Entity Inheritance
Person <|-- Player
Person <|-- Trader
Person --* Inventory : owns

' Program Relationships
Program ..> RecipeCatalog : uses
Program ..> Inventory : uses
Program ..> GameItems : references
Program ..> Player : creates
Program ..> Trader : creates with Recipe
Program ..> Random : uses for selection

' Utility Usage
Program ..> ParseUtils : may use

' Notes
note right of ItemRegistry
    Uses Guid as key for
    Item lookup
end note

note right of Inventory
    Stores items by their
    Guid string representation
end note

note bottom of GameItems
    Static initializer registers
    all items automatically
end note

note right of Player
    Starts with empty inventory
    Must get ingredients from trader
    to craft anything
end note

note right of Trader
    Constructor takes Recipe parameter
    Gets double ingredients for that recipe
    Also gets common items (Milk, Water)
    Used for trading with player
end note

note right of Program
    Main Game Loop Logic:
    1. Initialize with random trader recipe
    2. Display player inventory (empty)
    3. Show available recipes
    4. Player selects recipe
    5. Trader checks inventory
    6. If has all: transfer & craft → END
    7. If not: remove recipe → LOOP
    8. Repeat until success or no recipes
end note

@enduml