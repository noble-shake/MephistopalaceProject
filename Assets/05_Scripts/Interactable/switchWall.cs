using UnityEngine;
using UnityEngine.Playables;

public class switchWall : MonoBehaviour
{
    bool isActivate;
    [SerializeField] PlayableDirector CutscenePlayer;
    [SerializeField] GameObject Orb1;
    [SerializeField] GameObject Orb2;

    private void Start()
    {
        isActivate = false;
    }

    private void Update()
    {
        if (Orb1.activeSelf == false && Orb2.activeSelf == false)
        {
            if (isActivate) return;
            isActivate = true;
            CutscenePlayer.Play();
        }
    }
}