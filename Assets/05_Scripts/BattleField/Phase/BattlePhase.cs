using UnityEngine;
[System.Serializable]
public enum PhaseType { Engage, Command, Attack, Support, Activate, Execute, Wait, Done }


public class BattlePhase : MonoBehaviour
{
    public string DisplayName;
    public Identifying identityType;
    public PhaseType CurrentPhase;
    public Transform SpawnPoint;
    public Transform AllocatedPoint;
    public bool isEngage;

    private void Start()
    {
        CurrentPhase = PhaseType.Engage;
    }

    public virtual void PhaseEngage()
    { 
    
    }

    public virtual void PhaseCommand()
    { 
    
    }

    public virtual void AttackCommand()
    { }
    public virtual void SupportCommand()
    { }

    public virtual void PhaseBuff()
    { 
    
    }

    public virtual void PhaseDbuff()
    { 
        
    }

    public virtual void PhaseExecute()
    { 
    
    }
    public virtual void PhaseDone()
    { 
        
    }
}