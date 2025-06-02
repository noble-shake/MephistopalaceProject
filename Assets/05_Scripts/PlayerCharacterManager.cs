using System.Collections.Generic;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

[System.Serializable]
public enum SwitchingDirection
{ 
    Previous,
    Next,
}

public class PlayerCharacterManager : MonoBehaviour
{
    public static PlayerCharacterManager Instance;
    public PlayerManager CurrentPlayer;
    [SerializeField] private List<PlayerStatUI> statUI;
    
    // 소유하고 있는 캐릭터를 나타낸다.
    public Dictionary<CharacterType, PlayerManager> Playables;

    private void Awake()
    {
        if (Instance == null) { Instance = this; } else { Destroy(gameObject); }
    }

    private void Start()
    {
        Playables = new Dictionary<CharacterType, PlayerManager>();
        GameBegin();
    }

    private void GameBegin()
    {
        var characterInfo = ResourceManager.Instance.PlayerResources[(int)CharacterType.Knight];
        PlayerManager player = Instantiate(characterInfo.CharacterPrefab).GetComponent<PlayerManager>();
        var container = characterInfo.GetStatChange();
        player.transform.position = new Vector3(0, 0, -2f);
        CurrentPlayer = player;
        CinemachineCore.GetVirtualCamera(0).Follow = player.transform;
        Playables[CurrentPlayer.characterType] = player;
        StartCoroutine(StatAdjust(player, container));
    }

    IEnumerator StatAdjust(PlayerManager player, StatContainer _container)
    {
        yield return null;
        foreach (var sUI in statUI)
        {
            if (!sUI.gameObject.activeSelf)
            {
                sUI.gameObject.SetActive(true);
                player.status.playerStatUI = sUI;
                break;
            }
        }
        player.status.StatInitialize(_container);

    }

    public void Switching(SwitchingDirection _direction)
    { 
        int ownCharacters = Playables.Count;
        if (ownCharacters == 1) return;
        int currentCharacter = (int)CurrentPlayer.characterType;
        currentCharacter--;
        int allCharacters = System.Enum.GetNames(typeof(CharacterType)).Length;


        if (currentCharacter < 0f)
        {
            currentCharacter = (allCharacters == ownCharacters ? allCharacters : ownCharacters) - 1;
        }
        else if (currentCharacter > ownCharacters -1)
        {
            currentCharacter = 0;
        }

        Vector3 CurrentPosition = CurrentPlayer.transform.position;
        CurrentPlayer.gameObject.SetActive(false);

        CurrentPlayer = Playables[(CharacterType)currentCharacter];
        CurrentPlayer.gameObject.SetActive(true);
        CurrentPlayer.transform.position = CurrentPosition;

        //Showcase Change.
        // Equip Change.
    }

    public void SpawnNewCharacter(CharacterType _characterType)
    {
        var characterInfo = ResourceManager.Instance.PlayerResources[(int)_characterType];
        PlayerManager player = Instantiate(characterInfo.CharacterPrefab).GetComponent<PlayerManager>();
        var container = characterInfo.GetStatChange();
        Playables[CurrentPlayer.characterType] = player;
        player.gameObject.SetActive(false);
        StartCoroutine(StatAdjust(player, container));
    }
}
