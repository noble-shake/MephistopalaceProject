using UnityEngine;

[System.Serializable]
public enum VFXType
{ 
    OneShotEffector, // Loop가 끝나면 자동으로 사라질 VFX
    OneShotVFX, // 일정 시간 이후 사라질 Loop 형 VFX
    VFX,  // 지속적으로 표시할 VFX, 트리거에 의해 사라질 VFX
}

[System.Serializable]
public enum VFXName
{ 
    BuffEffectA,
    BuffEffectB,
    KnightSlash,
    EnemyAttackAlarmEffect,
    EnemyEvadeAlarmEffect,
    KnightChargingBladeA,
    KnightChargingBladeB,
    PlayerCounterAttack,
    SlimbAttackEffect,
    SkeletonAttackEffect,
    MagicianEncounterFireBall,
    KnightEffectA,
    KnightEffectB,
}

[CreateAssetMenu(fileName ="Effect 1", menuName = "Palace/Effect")]
public class EffectScriptableObject : ScriptableObject
{
    public VFXType vfxType;
    public VFXName Name;
    public float LifeTime;
    public GameObject VFXPrefab;

    public GameObject GetVFXInstance(float Lifetime = 0f)
    {
        GameObject vfxPrefab = Instantiate(VFXPrefab);
        switch (vfxType)
        {
            default:
            case VFXType.OneShotEffector:
            case VFXType.VFX:
                return vfxPrefab;
            case VFXType.OneShotVFX:
                vfxPrefab.GetComponent<OneShotVFX>().lifeTime = Lifetime;
                return vfxPrefab;
        }
    }
}