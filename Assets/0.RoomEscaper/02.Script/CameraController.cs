using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] Transform playerTr;


    public Vector3 scrollMin = new Vector3(0, 1.3f, -2);
    public Vector3 scrollMax = new Vector3(5, 1.7f, -5);

    public Vector2 zoomMinMax = new Vector2(-30, 30);

    public float intensityWheel = 1;

    public float wheelSpeed;
    public float rotSpeedX;
    public float rotSpeedY;

    public Vector3 pos = new Vector3(0, 2, -3);
    public Vector3 rot = new Vector3(0, 180, 0);

    [SerializeField] Transform pivot;




    //todo 카메라 설정값에 따라 위치 부여, 공간방향이..
    // Update is called once per frame  
    void LateUpdate()
    {
        if (pivot == null) { return; }
        transform.position = pivot.position + new Vector3(pos.z * Mathf.Sin(rot.y * Mathf.Deg2Rad), pos.y, pos.z * Mathf.Cos(rot.y * Mathf.Deg2Rad));
        transform.eulerAngles = rot;

        // transform.localPosition = player.position + pos;

        MouseInput();

        pos.y = scrollMin.y + intensityWheel * (scrollMax.y - scrollMin.y) * 0.01f;
        pos.z = scrollMin.z + intensityWheel * (scrollMax.z - scrollMin.z) * 0.01f;

        // rot.x = scrollMin.x + intensityWheel * (scrollMax.x - scrollMin.x) * 0.01f;

    }



    public float sensivity = 1;
    Vector2 prePos;
    Vector2 curPos;
    void MouseInput()
    {
        if (Input.GetMouseButton(1))
        {
            curPos = Input.mousePosition;

            if (prePos != Vector2.zero)
            {
                if (curPos.y + sensivity < prePos.y)
                {
                    if (rot.x < scrollMax.x)
                    {
                        rot.x += rotSpeedX;
                    }
                }
                else if (curPos.y > prePos.y + sensivity)
                {
                    if (rot.x > scrollMin.x)
                    {
                        rot.x -= rotSpeedX;
                    }
                }

                if (curPos.x > prePos.x + sensivity)
                {
                    rot.y += rotSpeedY;
                }
                else if (curPos.x + sensivity < prePos.x)
                {
                    rot.y -= rotSpeedY;
                }
            }
            prePos = curPos;
        }
        else
        {
            prePos = Vector2.zero;
            curPos = Vector2.zero;
        }

        intensityWheel += Input.GetAxis("Mouse ScrollWheel") * wheelSpeed;
        intensityWheel = Mathf.Clamp(intensityWheel, 0, 100);

    }

}
