using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinEfx : MonoBehaviour
{

    void OnEnable()
    {
        Destroy(this.gameObject, 2.0f);
    }



}
