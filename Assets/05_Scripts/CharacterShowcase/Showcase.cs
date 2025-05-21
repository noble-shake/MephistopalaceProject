using TeleportFX;
using UnityEngine;

public class Showcase : MonoBehaviour
{
    public Animator animator;
    public Transform Head;
    public Transform Body;

    public Unsacled_KriptoFX_Teleportation Effecter;
    public bool isDisappear;


    private void Start()
    {
        Effecter = GetComponent<Unsacled_KriptoFX_Teleportation>();
        Effecter.IsTeleportationFinished += () => TeleportationEffect(isDisappear);
    }

    public void TeleportationEffect(bool value)
    {
        if (value)
        {
            Disappear();
        }
    }

    public void Disappear()
    { 
        
    }

    public void OnHeadAnimation()
    { 
    
    }

    public void OnArmorAnimation()
    {

    }

    public void OnWeaponAnimation()
    {

    }

    public void OnAccessoryAnimation()
    {

    }

    public void OnBootsAnimation()
    {

    }

}