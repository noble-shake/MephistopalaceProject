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

    private void OnEnable()
    {
        //TeleportationEffect(true);

    }

    public void TeleportationEffect(bool value)
    {
        if (value)
        {
            Effecter.enabled = true;
        }
        else
        {
            Effecter.enabled = false;
            // Disappear();
        }
    }

    public void Disappear()
    {
        // gameObject.SetActive(false);  
    }

    public void OnHeadAnimation()
    {
        animator.Play("HeadEquip");
    }

    public void OnArmorAnimation()
    {
        animator.Play("ArmorEquip");
    }

    public void OnWeaponAnimation()
    {
        animator.Play("WeaponEquip");
    }

    public void OnAccessoryAnimation()
    {
        animator.Play("AccessoryEquip");
    }

    public void OnBootsAnimation()
    {
        animator.Play("BootsEquip");
    }

}