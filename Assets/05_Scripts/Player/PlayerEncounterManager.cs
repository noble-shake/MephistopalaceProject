using JetBrains.Annotations;
using UnityEngine;

public class PlayerEncounterManager : MonoBehaviour
{
    private PlayerManager playerManager;
    [SerializeField] public WeaponTriggerJudge LeftWeapon;
    [SerializeField] public WeaponTriggerJudge RightWeapon;
    [HideInInspector] public ItemObject temporaryItem;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    #region Encounter Animation Triggers
    public void OnEncounterAttackCollider()
    {
        if (LeftWeapon != null)
        {
            LeftWeapon.WeaponTrail.SetActive(true);
            LeftWeapon.WeaponCollider.enabled = true;
        }

        if (RightWeapon != null)
        {
            RightWeapon.WeaponTrail.SetActive(true);
            RightWeapon.WeaponCollider.enabled = true;
        }

    }

    public void OffEncounterAttackCollider()
    {
        if (LeftWeapon != null)
        {
            LeftWeapon.WeaponTrail.SetActive(false);
            LeftWeapon.WeaponCollider.enabled = false;
        }

        if (RightWeapon != null)
        {
            RightWeapon.WeaponTrail.SetActive(false);
            RightWeapon.WeaponCollider.enabled = false;
        }
    }

    public void OnEncounterAttackDone()
    {

        playerManager.locomotor.rigid.useGravity = true;
        playerManager.isAttack = false;
    }

    public void OnEarnItem()
    {
        if (temporaryItem != null)
        {
            if (InventoryManager.Instance.EarnItem(temporaryItem.ItemInfo))
            {
                Destroy(temporaryItem.gameObject);
            }
        }

        playerManager.isItemEarnAction = false;

    }

    [HideInInspector] private Transform FireTrs;
    public void EncounterFirePlay(Transform _FireTrs)
    {
        FireTrs = _FireTrs;
        playerManager.animator.animator.Play("EncounterFire");
    }


    public void OnEncounterFire()
    {
        if (playerManager.characterType != CharacterType.Magician) return;
        GameObject Projectile = Instantiate(ResourceManager.Instance.VFXResources[VFXName.MagicianEncounterFireBall].VFXPrefab);
        Projectile.transform.position = RightWeapon.WeaponVFXTransform.position;
        Projectile.transform.LookAt(FireTrs);

        FireTrs = null;

    }
    #endregion
}
