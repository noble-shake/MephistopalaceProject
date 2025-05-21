using JetBrains.Annotations;
using UnityEngine;

public class PlayerEncounterManager : MonoBehaviour
{
    private PlayerManager playerManager;
    [SerializeField] WeaponTriggerJudge Weapon;
    [HideInInspector] public ItemObject temporaryItem;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    #region Encounter Animation Triggers
    public void OnEncounterAttackCollider()
    {
        Weapon.WeaponTrail.SetActive(true);
        Weapon.WeaponCollider.enabled = true;
    }

    public void OffEncounterAttackCollider()
    {
        Weapon.WeaponTrail.SetActive(false);
        Weapon.WeaponCollider.enabled = false;
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
    #endregion
}
