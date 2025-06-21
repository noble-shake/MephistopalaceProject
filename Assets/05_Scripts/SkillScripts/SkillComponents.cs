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
}