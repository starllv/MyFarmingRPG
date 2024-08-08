public enum AnimationName
{
    idleUp,
    idleDown,
    idleLeft,
    idleRight,
    walkUp,
    walkDown,
    walkLeft,
    walkRight,
    runUp,
    runDown,
    runLeft,
    runRight,
    useToolUp,
    useToolDown,
    useToolLeft,
    useToolRight,
    swingToolUp,
    swingToolDown,
    swingToolLeft,
    swingToolRight,
    liftToolUp,
    liftToolDown,
    liftToolLeft,
    liftToolRight,
    holdToolUp,
    holdToolDown,
    holdToolLeft,
    holdToolRight,
    pickUp,
    pickDown,
    pickLeft,
    pickRight,
    count
}

public enum CharacterPartAnimator
{
    body,
    arms,
    hair,
    tool,
    hat,
    count
}

public enum PartVariantColour
{
    none,
    count
}

public enum PartVariantType
{
    none,
    carry,
    hoe,
    pickaxe,
    axe,
    scythe,
    wateringCan,
    count
}

public enum ToolEffect
{
    none,
    watering    
}

public enum Direction
{
    up,
    down,
    left,
    right,
    none
}

public enum ItemType
{
    Seed,
    Commodity,
    Water_tool,
    Hoeing_tool,
    Chopping_tool,
    Breaking_tool,
    Reaping_tool,
    Collecting_tool,
    Reapable_scenary,
    Furniture,
    none,
    count
}

public enum InventoryLocation 
{
    player,
    chest,
    count
}