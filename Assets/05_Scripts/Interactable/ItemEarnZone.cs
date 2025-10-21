using UnityEngine;

public class ItemEarnZone : InteractZone
{
    [HideInInspector] PlayerManager player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(LayerEnum.Player.ToString()))
        {
            player = other.gameObject.GetComponent<PlayerManager>();
            if (player == null) return;
            player.encounter.temporaryItem = interactOwner.GetComponent<ItemObject>();
            interacted = true;
            interactGauge.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(LayerEnum.Player.ToString()))
        {
            player = other.gameObject.GetComponent<PlayerManager>();
            if (player == null) return;
            player.encounter.temporaryItem = null;
            interacted = false;
            interactGauge.gameObject.SetActive(false);
        }
    }
}