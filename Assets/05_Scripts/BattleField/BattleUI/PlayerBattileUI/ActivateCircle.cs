using UnityEngine;

public class ActivateCircle : MonoBehaviour
{
    [SerializeField] private ParticleSystem circleObject;

    private void Start()
    {
        circleObject = GetComponent<ParticleSystem>();
    }
}