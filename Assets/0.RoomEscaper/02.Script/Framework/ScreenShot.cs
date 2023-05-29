using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScreenShot : MonoBehaviour
{
    public Camera cam;

    public string path;
    public int number;

    [ContextMenu("ScreenShot")]
    void CaptureScreen()
    {
        StartCoroutine(Capture());
    }

    IEnumerator Capture()
    {
        yield return new WaitForEndOfFrame();

        Texture2D screenTex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        Rect area = new Rect(0f, 0f, Screen.width, Screen.height);
        screenTex.ReadPixels(area, 0, 0);


        File.WriteAllBytes(path + number + ".png", screenTex.EncodeToPNG());
        Debug.Log(path + number + ".png");
        Destroy(screenTex);
        yield break;

        RenderTexture rt = new RenderTexture(1024, 768, 24);
        cam.targetTexture = rt;
        Texture2D screenShot = new Texture2D(1024, 768, TextureFormat.RGB24, false);
        Rect rec = new Rect(0, 0, screenShot.width, screenShot.height);
        cam.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, 1024, 768), 0, 0);
        screenShot.Apply();

        byte[] bytes = screenShot.EncodeToPNG();

        File.WriteAllBytes(path + number + ".png", bytes);
        Debug.Log(path + number + ".png");
    }

    // [ContextMenu("ASDA")]
    // void CaptureScreenVer2()
    // {

    //     number++;
    //     Texture2D screenTex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
    //     Rect area = new Rect(0f, 0f, Screen.width, Screen.height);
    //     screenTex.ReadPixels(area, 0, 0);
    //     File.WriteAllBytes(path + number + ".png", screenTex.EncodeToPNG());
    // }


    /*
    10-8.5
    11-8.75   
    12- 8.9375  0.1875 
    13- 8.984375    0.046875
    14- 8.99609375  0.01171875
    15- 8.9990234375    0.0029296875
    16- 8.999755859375  0.000732421875
    17- 8.99993896484375    0.00018310546875
    18- 8.99998474121094    0.0000457763671875
    19- 8.99999618530273    0.000011444091796875
    

    수열, 목숨, 마음의양식, 무지개,  , 엔딩
    20- 8.99999904632568    0.00000286102294921875
    21- 8.99999976158142    0.000000715255737304688
    22- 8.99999994039536    0.000000178813934326172
    23- 8.99999998509884    0.00000004.4703483581543
    24- 8.99999999          0.00000001.11758708953857
    25- 9.99999999          1


    0.00000000279396772384644
















    10~20
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


    21~30
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


    31~37
    8.99999976
    8.99999988
    8.99999994
    8.99999997
    8.99999998
    8.99999999
    9.99999999
*/
}
