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
    public List<ItemScriptableObject> ItemResources;
    public List<PlayerCharacterScriptableObject> PlayerResources;
    public List<EnemyScriptableObject> EnemyResources;

    private Dictionary<int, List<EnemyScriptableObject>> EnemyPools;



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
    }

    public List<EnemyManager> GetEnemies(EnemyScriptableObject refer, int numbEnemies)
    { 
        List<EnemyManager> entries = new List<EnemyManager>();
        entries.Add(Instantiate(refer.CharacterPrefab).GetComponent<EnemyManager>());

        for (int idx = 0; idx < numbEnemies; idx++)
        {
            int tier = Random.Range(refer.minPool, refer.maxPool + 1);
            List<EnemyScriptableObject> enemies = EnemyPools[tier];
            int target = Random.Range(0, enemies.Count);
            entries.Add(Instantiate(enemies[target].CharacterPrefab).GetComponent<EnemyManager>());
        }

        return entries;
    }
    
}