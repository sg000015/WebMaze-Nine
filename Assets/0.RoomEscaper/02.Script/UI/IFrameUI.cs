using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFrameUI : MonoBehaviour
{
    public Vector2 start;
    public Vector2 end;

    public int density = 10;
    public int period = 10;


    void OnEnable()
    {
        StartCoroutine(MoveEffect());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator MoveEffect()
    {
        RectTransform rect = GetComponent<RectTransform>();

        WaitForSeconds ws = new WaitForSeconds(0.02f);

        rect.anchoredPosition = start;

        Vector2 value = (end - start) / density;


        while (true)
        {
            for (int i = 0; i < density; i++)
            {
                rect.anchoredPosition += value;
                yield return ws;
            }
            for (int i = 0; i < density; i++)
            {
                rect.anchoredPosition -= value;
                yield return ws;
            }
        }
    }
}
