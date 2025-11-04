using TeleportFX;
using UnityEngine;

public class Showcase : MonoBehaviour
{
    public Animator animator;
    public Transform Head;
    public Transform Body;

    public bool isDisappear;


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