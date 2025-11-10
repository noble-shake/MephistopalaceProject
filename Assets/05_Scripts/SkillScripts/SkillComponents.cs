// Skill Action Components

[System.Serializable]
public enum SkillGroup
{
    Attack,
    Support,
    None, // Only Used In TurnOver.
}

[System.Serializable]
public enum CommandGroup
{
    Attack,
    Support,
    Command,
    None,
}

[System.Serializable]
public enum ProcessType
{ 
    Enable,
    Disable,
    OnlyParry,
    OnlyEvade,
}

[System.Serializable]
public enum SkillActions
{ 
    TurnOver,
    DoubleSlash,
    SlimbCombo01,
    SlimbCombo02,
    ChargingAttack,
    SkeletonCombo01,
    SkeletonCombo02,
    SkeletonCombo03,
    KnightCombo01,
    KnightCombo02,
    KnightCombo03,
    KnightCombo04,
    DragonCombo01,
    DragonCombo02,
    DragonCombo03,
    DragonCombo04,
    DragonCombo05,
    KnightSpecial,
    DualBladeCombo01,
    DualBladeCombo02,
    DualBladeSpecial,
    MagicianCombo01,
    MagicianCombo02,
    MagicianSpecial,
    KnightBuff,
    DualBladeBuff,
    MagicianBuff,
    SmallHPConsume,
    MiddleHPConsume,
    BigHPConsume,
    SmallAPConsume,
    MiddleAPConsume,
    BigAPConsume,
}