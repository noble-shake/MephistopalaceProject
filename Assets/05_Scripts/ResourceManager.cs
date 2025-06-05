using UnityEngine;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour
{ 
    public static ResourceManager Instance;

    [Header("Portraits")]
    public Sprite KnightPortrait;
    public Sprite DualBladePortrait;
    public Sprite MagicianPortrait;

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
    }

    public List<EnemyManager> GetEnemies(EnemyScriptableObject refer, int numbEnemies)
    { 
        List<EnemyManager> entries = new List<EnemyManager>();

        var referEnemy = Instantiate(refer.CharacterPrefab).GetComponent<EnemyManager>();
        referEnemy.GetComponent<EnemyManager>().status.StatInitialize(refer.GetEnemyStatChange());
        referEnemy.phaser.identityType = Identifying.Enemy;
        entries.Add(referEnemy);

        for (int idx = 0; idx < numbEnemies; idx++)
        {
            int tier = Random.Range(refer.minPool, refer.maxPool + 1);
            List<EnemyScriptableObject> enemies = EnemyPools[tier];
            int target = Random.Range(0, enemies.Count);
            var enemy = Instantiate(enemies[target].CharacterPrefab);
            enemy.GetComponent<EnemyManager>().status.StatInitialize(enemies[target].GetEnemyStatChange());
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
        itemObject.transform.position = trs.position;
        itemObject.GetItemInfo(_item, actionType);
        return;
    }
}