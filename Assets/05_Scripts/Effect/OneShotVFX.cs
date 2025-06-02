using UnityEngine;

public class OneShotVFX : MonoBehaviour
{
    public float lifeTime;

    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if(lifeTime < 0f) Destroy(gameObject);
    }
}