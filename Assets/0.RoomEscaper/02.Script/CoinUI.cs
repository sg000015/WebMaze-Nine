using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CoinUI : MonoBehaviour
{
    void OnMouseDown()
    {
        GameManager.Inst.objectManager.GetUICoin(this.gameObject);
    }

    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }

}
