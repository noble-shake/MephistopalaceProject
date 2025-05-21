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
public enum SkillActions
{ 
    TurnOver,
    
}