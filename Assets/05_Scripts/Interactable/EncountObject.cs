using UnityEngine;

public class EncountObject : InteractObject
{
    bool isActivated;
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
        if (isActivated) return;

        if (other.gameObject.layer == LayerMask.NameToLayer(LayerEnum.Player.ToString()))
        {
            ActivateEvent();
            isActivated = true;


        }

    }


}
