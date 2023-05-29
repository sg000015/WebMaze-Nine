using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpEfx : MonoBehaviour
{

    [SerializeField] Transform mainPannel;
    [SerializeField] Text text;


    public void ActivePopup(string msg)
    {
        text.text = msg;
        gameObject.SetActive(true);
        StartCoroutine(OnPopupActive());
    }

    public void DisablePopup()
    {
        gameObject.SetActive(false);
    }


    IEnumerator OnPopupActive()
    {
        WaitForSeconds ws = new WaitForSeconds(0.02f);

        mainPannel.transform.localScale = Vector3.one * 0.7f;
        for (int i = 0; i < 7; i++)
        {
            mainPannel.transform.localScale += Vector3.one * 0.06f;
            yield return ws;
        };
        for (int i = 0; i < 2; i++)
        {
            mainPannel.transform.localScale -= Vector3.one * 0.06f;
            yield return ws;
        };
    }





}
