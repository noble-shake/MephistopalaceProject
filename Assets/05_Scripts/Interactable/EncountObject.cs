using UnityEngine;

public class EncountObject : InteractObject
{
    protected override void Start()
    {
        base.Start();
    }

    public override void ActivateEvent()
    {
        base.ActivateEvent();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(LayerEnum.Player.ToString()))
        {
            ActivateEvent();
            Destroy(gameObject);
        }

    }


}
