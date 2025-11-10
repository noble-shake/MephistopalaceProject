using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Speed;
    public float LifeTime;

    /*
        Fire Transform
        Destination Transform
        Trigger By Key.
     */

    private void Start()
    {
        LifeTime = 10f;
    }


    public void SetProjectile(float _Speed)
    { 
        Speed = _Speed;
    }

    private void Update()
    {
        LifeTime -= Time.deltaTime;
        if (LifeTime < 0f) Destroy(gameObject);

        transform.position += transform.forward * Speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacles"))
        {
            other.GetComponent<ProjectilPath>().ActivateEvent();
            Destroy(gameObject);
        }
    }

}