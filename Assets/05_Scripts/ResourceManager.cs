using UnityEngine;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour
{ 
    public static ResourceManager Instance;

    [Header("Portraits")]
    public Sprite KnightPortrait;
    public Sprite DualBladePortrait;
    public Sprite MagicianPortrait;

    public Sprite KnightIcon;
    public Sprite DualBladeIcon;
    public Sprite MagicianIcon;

    [Header("ScriptableObjects")]
    public List<ItemScriptableObject> KeyItemResources;
    public List<ItemScriptableObject> ItemResources;
    public Dictionary<ItemNames, ItemScriptableObject> ItemDictionary;

    public List<PlayerCharacterScriptableObject> PlayerResources;
    public List<EnemyScriptableObject> EnemyResources;

    [Header("Enemy Pools")]
    private Dictionary<int, List<EnemyScriptableObject>> EnemyPools;

    [Header("VFX Effect")]
    public List<EffectScriptableObject> EffectScriptableObjects;
    public Dictionary<VFXName, EffectScriptableObject> VFXResources;

    [Header("Item")]
    public ItemObject DropItemPrefab;

    [Header("ETC")]
    [SerializeField] DamageUI DamageUIPrefab;
    public List<DamageUI> DamageUIPool;

    private void Awake()
    {
        if (Instance == null) { Instance = this; } else { Destroy(gameObject); }
    }

    private void Start()
    {
        EnemyPools = new Dictionary<int, List<EnemyScriptableObject>>();

        foreach (EnemyScriptableObject enemyRef in EnemyResources)
        {
            int minPool = enemyRef.MinLevel;
            int maxPool = enemyRef.MaxLevel;
            for (int idx = minPool; idx <= maxPool; idx++)
            { 
                if(!EnemyPools.ContainsKey(idx)) EnemyPools[idx] = new List<EnemyScriptableObject>();
                EnemyPools[idx].Add(enemyRef);
            }
        }

        VFXResources = new Dictionary<VFXName, EffectScriptableObject>();
        foreach (EffectScriptableObject e in EffectScriptableObjects)
        {
            VFXResources[e.Name] = e;
        }

        ItemDictionary = new Dictionary<ItemNames, ItemScriptableObject>();
        foreach (ItemScriptableObject i in ItemResources)
        {
            ItemDictionary[i.Name] = i;
        }

        DamageUIPooling();
    }

    public List<EnemyManager> GetEnemies(EnemyScriptableObject refer, int numbEnemies)
    { 
        List<EnemyManager> entries = new List<EnemyManager>();

        int level = Random.Range(refer.minPool, refer.maxPool + 1);
        var referEnemy = Instantiate(refer.CharacterPrefab).GetComponent<EnemyManager>();
        referEnemy.GetComponent<EnemyManager>().status.StatInitialize(refer.GetEnemyStatChange());
        referEnemy.GetComponent<EnemyManager>().status.Level = level;
        referEnemy.GetComponent<EnemyManager>().status.EnemyStatAdjust();
        referEnemy.phaser.identityType = Identifying.Enemy;
        entries.Add(referEnemy);

        if (refer.isUnique) return entries;

        for (int idx = 0; idx < numbEnemies; idx++)
        {
            int tier = Random.Range(refer.minPool, refer.maxPool + 1);
            List<EnemyScriptableObject> enemies = EnemyPools[tier];
            int target = Random.Range(0, enemies.Count);
            var enemy = Instantiate(enemies[target].CharacterPrefab);
            enemy.GetComponent<EnemyManager>().status.StatInitialize(enemies[target].GetEnemyStatChange());
            enemy.GetComponent<EnemyManager>().status.Level = tier;
            enemy.GetComponent<EnemyManager>().status.EnemyStatAdjust();
            entries.Add(enemy.GetComponent<EnemyManager>());
        }

        return entries;
    }

    public ItemScriptableObject GetItemResource(ItemNames name)
    {
        foreach (ItemScriptableObject itemObject in ItemResources)
        {
            if (itemObject.Name == name)
            {
                return itemObject;
            }
        }

        Debug.Log("Item Not Matched");
        return null;
    }

    public void ItemDropObjectSpawn(Transform trs, ItemNames name, EarnActionType actionType)
    {
        ItemObject itemObject = Instantiate(DropItemPrefab);
        itemObject.transform.position = trs.position;
        itemObject.GetItemInfo(ItemDictionary[name], actionType);
        return;
    }

    public void ItemDropObjectSpawn(Transform trs, ItemScriptableObject _item, EarnActionType actionType)
    {
        ItemObject itemObject = Instantiate(DropItemPrefab);
        itemObject.transform.position = trs.position + Vector3.up;
        itemObject.GetItemInfo(_item, actionType);
        return;
    }

    //[SerializeField] DamageUI DamageUIPrefab;
    //public List<DamageUI> DamageUIPool;
    private void DamageUIPooling()
    {
        DamageUIPool = new List<DamageUI>();

        for (int i = 0; i< 64; i++)
        {
            DamageUI damageUI = Instantiate(DamageUIPrefab, transform);
            damageUI.gameObject.SetActive(false);
            DamageUIPool.Add(damageUI);
        }
        
    }

    public void GetDamageUI(int _damage, Vector3 _Position)
    {
        for (int i = 0; i < 64; i++)
        {
            if (DamageUIPool[i].gameObject.activeSelf) continue;

            DamageUIPool[i].gameObject.SetActive(true);
            DamageUIPool[i].SetDamage = _damage;
            DamageUIPool[i].transform.position= _Position;
            return;
        }
    }
}