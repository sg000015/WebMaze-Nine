using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleRenderPlayer : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerDownHandler
{
    [SerializeField] GameObject renderPlayer;

    public float rotSpeed = -0.5f;


    [SerializeField] Renderer[] renderers;

    void Start()
    {
        // GameManager.Inst.OnStartGame += () => { GetComponent<UnityEngine.UI.RawImage>().raycastTarget = false; };
        // GameManager.Inst.OnTitleMenu += () => { GetComponent<UnityEngine.UI.RawImage>().raycastTarget = true; };
        GameManager.Inst.OnStartGame += () => { GetComponent<UnityEngine.UI.RawImage>().enabled = false; };
        GameManager.Inst.OnTitleMenu += () => { GetComponent<UnityEngine.UI.RawImage>().enabled = true; };
    }


    byte clickCount = 0;

    void MaterialEfx()
    {
        if (GameManager.Inst.dataManager.TUTORIAL <= 0) { return; }

        SoundManager.Inst.PlaySfx(3);
        SoundManager.Inst.PlayVoiceSfx(Random.Range(0, 4));

        if (clickCount > 80) { return; }
        clickCount++;
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = new Color32(255, (byte)(255 - clickCount), (byte)(255 - clickCount), 255);
        }
    }

    public void ForceMaterialEfx(Color color)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = color;
        }
    }



    Vector2 curVec;
    Animator anim;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (anim == null)
        {
            anim = renderPlayer.GetComponent<Animator>();
        }

        anim.SetTrigger("smileFlag");
        MaterialEfx();

        GameManager.Inst.OnClickCharacter.Invoke();
    }


    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        curVec = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (curVec != Vector2.zero)
        {
            CheckDragValue(eventData.position - curVec);
        }
        curVec = eventData.position;
    }

    void CheckDragValue(Vector2 vec)
    {
        if (vec.x != 0)
        {
            renderPlayer.transform.Rotate(Vector3.up * vec.x * rotSpeed);
        }
    }

}
