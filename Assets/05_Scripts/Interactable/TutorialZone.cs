using UnityEngine;

public class TutorialZone : InteractObject
{
    [HideInInspector] private bool isTriggered;
    [SerializeField] TutorialUI tutorialUI;

    protected override void Start()
    {
        base.Start();
        GameManager.Instance.GameStop(0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTriggered) return;
        isTriggered = true;
        InputManager.Instance.UIInputBind();
        tutorialUI.gameObject.SetActive(true);
        tutorialUI.zone = this;
        GameManager.Instance.GameStop();
    }
}
