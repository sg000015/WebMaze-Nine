using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClick : MonoBehaviour
{

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SoundManager.Inst.PlayBgm(0);
            GetComponent<AudioSource>().enabled = true;
            Destroy(this);
        }
    }

    public int count;
    public double number = 0.5;
    public double result = 0;

    [ContextMenu("TEST")]
    void asdf()
    {
        result = 8;
        string str = "";
        string dis = "";
        for (int i = 0; i < count; i++)
        {
            dis += $"{number * 1.5}\n";
            result += number;
            number *= 0.5;
            result += number;
            number *= 0.5;
            str += $"{result}\n";
        }
        Debug.Log(str);
        Debug.Log(dis);
    }

    //8.5 + 8.8.875

    //!총 35문제
    /*
    8.5
    8.75
    8.875
    8.9375
    8.96875
    8.984375
    8.9921875
    8.99609375
    8.99804687
    8.99902343
    8.99951171
    8.99975585
    8.99987792
    8.99993896
    8.99996948
    8.99998474
    8.99999237
    8.99999618
    8.99999809
    8.99999904
    8.99999952
    8.99999976
    8.99999988
    8.99999994
    8.99999997
    8.99999998
    8.99999999
    9.99999999
*/


}
