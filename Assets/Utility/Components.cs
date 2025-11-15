using UnityEngine;

[System.Serializable]
public enum LayerEnum
{ 
    Default = 0,
    TransparentFX = 1,
    IgnoreRaycast = 2,
    Water = 4,
    UI = 5,
    Player = 8,
    Enemy = 9,
    VisionSensor = 10,
    FootTrigger = 11,
    DamagableCollider = 12,
}

[System.Serializable]
public enum ContextType
{
    Global,
    Character,
    Inform,
    Battle,
}

[System.Serializable]
public enum ChracaterSpeaker
{
    Knight,
    DualBlade,
    Magician,
}

[System.Serializable]
public enum CharacterType
{
    Knight,
    DualBlade,
    Magician,
}

[System.Serializable]
public enum ItemType
{
    Key,
    Equip_Head,
    Equip_Armor,
    Equip_Weapon,
    Equip_Accessory,
    Equip_Boots,
    Consume,
}

[System.Serializable]
public enum EarnActionType
{ 
    forward,
    down
}

[System.Serializable]
public enum Identifying
{ 
    Player,
    Enemy,
}

[System.Serializable]
public enum EnemyType
{ 
    Gazer,
    Skeleton,
    MonterKnight,
    Dragon
}
