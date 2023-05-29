using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    float timer = 0;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up);
        // timer += Time.deltaTime;

        // if (timer > 60)
        // {
        //     GameManager.Inst.objectManager.GetCoin(this.gameObject);
        // }
    }


    void OnTriggerEnter()
    {
        GameManager.Inst.objectManager.GetCoin(this.gameObject);
    }



}
