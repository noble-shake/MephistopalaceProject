using UnityEngine;

public class EnemyStatusManager : CharacterStatusManger
{
    private EnemyManager enemyManager;

    public override void HPChange(int _value)
    {

        ResourceManager.Instance.GetDamageUI(_value, transform.position + transform.forward * 1.5f + Vector3.up * 2f);
        HP += _value;

        if (HP >= MaxHP) HP = MaxHP;
        if (HP <= 0)
        {
            HP = 0;
            enemyManager.animator.animator.SetBool("isDead", true);
            EnemyPhase phaser = (EnemyPhase)enemyManager.phaser;
            phaser.teleportation.TeleportationState = TeleportFX.KriptoFX_Teleportation.TeleportationStateEnum.Disappear;
            phaser.teleportation.enabled = true;
            enemyManager.status.isDead = true;
        }
    }

    private void Start()
    {
        enemyManager = GetComponent<EnemyManager>();
    }
}
