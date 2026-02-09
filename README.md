# ProgramII - Crafting System

## Project Information
- **Course:** PROG 305-01 Programming II
- **Author:** Connor Lonergan
- **Term:** Fall 2026

## 📊 UML Class Diagram

```plantuml
@startuml
' Crafting Engine UML Class Diagram
' For github.com/Zencode22/ProgramII
' Created with DeepSeek & Proton Lumo AI assistance
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

title ProgramII - Crafting Engine Class Diagram\nPROG 305-01 Programming II - Fall 2026

note left
  <b>Author:</b> Connor Lonergan
  <b>Course:</b> PROG 305-01 Programming II
  <b>Term:</b> Fall 2026
  <b>AI Assistance:</b> DeepSeek
  <b>Disclaimer:</b> Educational project
  <b>Folder Structure:</b>
  - Root: Core classes
  - Test/: TestData class
  - Entities/: Person, Player, Trader
  - Utilities/: ParseUtils class
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
    #Person(string)
}

class Player {
    +Player(string)
}

class Trader {
    +Trader(string)
}

' Test (in Test/ folder)
class TestData {
    +static CreateStarterKitRecipe() : Recipe
    +static LoadTestRecipes() : List<Recipe>
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
    +static Main(string[]) : void
}

note top of Program
  /*
  * Craft System
  * Connor Lonergan
  * Application created in PROG 305-01 Programming II
  * Fall 2026
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

' Test Data Relationships
TestData ..> Recipe : creates
TestData ..> RecipeCatalog : uses
TestData ..> GameItems : references
Program ..> TestData : uses

' Utility Usage (indirect)
TestData ..> ParseUtils : may use
Program ..> ParseUtils : may use

' Program Relationships
Program ..> RecipeCatalog : uses
Program ..> Inventory : uses
Program ..> GameItems : references
Program ..> Player : creates

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

note right of Person
    Abstract base class
    Initializes with starter items:
    - 10 Milk
    - 2 Chocolate Chips
    - 1 Sugar
end note

note bottom of TestData
    Temporary test class
    Creates starter recipes
    Located in Test/ folder
end note

note bottom of ParseUtils
    Utility class for parsing
    Located in Utilities/ folder
end note
@enduml
note bottom of GameItems
    Static initializer registers
    all items automatically
end note
@enduml
