using TMPro;
using UnityEngine;

public class DamageUI : MonoBehaviour
{
    float curFlow;
    [SerializeField] TMP_Text damage;
    public int SetDamage { set { damage.text = value.ToString(); } }

    private void OnEnable()
    {
        curFlow = 1f;
    }

    private void Update()
    {
        transform.LookAt(Camera.main.transform);
        curFlow -= Time.deltaTime;
        transform.position += Vector3.up * Time.deltaTime / 3f;
        if (curFlow < 0f)
        {
            curFlow = 0f;
            gameObject.SetActive(false);
        }
    }
}
