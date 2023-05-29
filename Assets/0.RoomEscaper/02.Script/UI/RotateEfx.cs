using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEfx : MonoBehaviour
{

    [SerializeField] float rotSpeed = 100;


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * rotSpeed * Time.deltaTime);
    }
}
