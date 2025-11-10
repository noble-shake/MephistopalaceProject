using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class LavaGrounds : MonoBehaviour
{
    [SerializeField] float OffsetX;
    [SerializeField] float OffsetZ;

    private void Start()
    {
        StartCoroutine(ScaleUp());
    }

    IEnumerator ScaleUp()
    {
        float curFlow = 0f;

        while (curFlow < 1f)
        {
            // 3초동안 OffsetX에 도달해야 한다.

            curFlow += (Time.deltaTime / 3f);
            if (curFlow > 1f) curFlow = 1f;
            transform.localScale = new Vector3(OffsetX * curFlow + 0.0001f, 1f, OffsetZ * curFlow + 0.0001f);
            yield return null;
        }



    }
}