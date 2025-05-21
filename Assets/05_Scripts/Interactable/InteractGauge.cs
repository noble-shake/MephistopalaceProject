using UnityEngine;
using UnityEngine.UI;

public class InteractGauge : MonoBehaviour
{
    [SerializeField] Image interactionGauge;
    InteractObject interactObject;
    float delay;

    private void Start()
    {
        interactionGauge.fillAmount = 0f;
        delay = 0f;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        delay -= Time.deltaTime;
        if (delay < 0f)
        {
            delay = 0f;
        }
        transform.LookAt(Camera.main.transform);
    }

    public void Interact()
    {
        if (delay > 0f) return;

        interactionGauge.fillAmount += Time.deltaTime / 1.25f;
        if (interactionGauge.fillAmount >= 1f)
        {
            interactionGauge.fillAmount = 0f;
            delay = 2f;
            interactObject = GetComponentInParent<InteractObject>();
            interactObject.InteractEvent();
        } 
    }

    public void Cancel()
    {
        interactionGauge.fillAmount = 0f;
    }
}