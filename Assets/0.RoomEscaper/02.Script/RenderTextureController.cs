using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTextureController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Inst.OnStartGame += OffChild;
        GameManager.Inst.OnTitleMenu += OnChild;
    }

    void OnChild()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    void OffChild()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
