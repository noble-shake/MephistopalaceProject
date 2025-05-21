using UnityEngine;

public class OneShotEffector : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;

    private void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (particle.isStopped)
        { 
            Destroy(gameObject);
        }
    }
}