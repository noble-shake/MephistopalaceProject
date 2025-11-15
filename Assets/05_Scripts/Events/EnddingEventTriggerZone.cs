using UnityEngine;
using UnityEngine.Playables;

public class EndingEventTriggerZone : MonoBehaviour
{
    [SerializeField] PlayableDirector playable;
    InteractObject interact;
    Collider coll;

    private void Start()
    {
        coll = GetComponent<Collider>();
        interact = GetComponent<InteractObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(LayerEnum.Player.ToString()))
        {
            interact.TriggerEvent();
            SoundManager.Instance.ChangeSong((int)ThemeList.Ending);
            GameManager.Instance.OnStateChangeToCinematic();
            playable.Play();
            coll.enabled = false;
        }
    }

    public void OnGameContinue()
    {
        GameManager.Instance.OnStateChangeToEncounter();
    }
}