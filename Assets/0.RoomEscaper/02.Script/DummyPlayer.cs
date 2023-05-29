using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPlayer : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().SetBool("smileFlag", true);
    }


}
