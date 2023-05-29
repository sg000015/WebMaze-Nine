using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] Image image;


    [ContextMenu("Set")]
    void Ad()
    {
        for (int j = 0; j < 25; j++)
        {
            for (int i = 0; i < 40; i++)
            {
                Image img = Instantiate(image, transform);
                img.rectTransform.anchoredPosition = image.rectTransform.anchoredPosition
                                                        + Vector2.right * i * 20
                                                        - Vector2.up * j * 20;
            }
        }


    }
}
