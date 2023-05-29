using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class TutorialManager : Manager<TutorialManager>
{

    public bool SKIP;

    [SerializeField] GameObject tutorialBlack;
    [SerializeField] TitleRenderPlayer renderPlayer;


    [DllImport("__Internal")]
    public static extern void OnLoadedUnityInstance();


    protected override void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        OnLoadedUnityInstance();
#endif

        if (SKIP)
        {
            EndTutorial();
            return;
        }

        if (GameManager.Inst.dataManager.TUTORIAL > 0)
        {
            EndTutorial();
        }
        else
        {
            Init();
        }
    }




    [Header("TutorialObjects")]

    [SerializeField] RawImage render;

    [SerializeField] GameObject cursor;
    [SerializeField] GameObject jewel;

    void Init()
    {
        cursor.SetActive(true);
        StartCoroutine(GlitterEfx(render));
        GameManager.Inst.OnClickCharacter += Tutorial1;
        progressNum++;
    }


    IEnumerator GlitterEfx(RawImage rawImage)
    {
        byte value = 255;
        WaitForSeconds ws = new WaitForSeconds(0.02f);
        while (true)
        {
            for (int i = 0; i < 30; i++)
            {
                value -= 2;
                rawImage.color = new Color32(value, value, value, value);
                yield return ws;
            }
            for (int i = 0; i < 30; i++)
            {
                value += 2;
                rawImage.color = new Color32(value, value, value, value);
                yield return ws;
            }
        }
    }



    int progressNum = 0;

    public void CheckProgress()
    {
        switch (progressNum)
        {
            case 1:
                Tutorial1();
                break;
            case 2:
                Tutorial2();
                break;
        }
    }


    void Tutorial1()
    {
        GameManager.Inst.OnClickCharacter -= Tutorial1;
        render.color = Color.white;
        StopAllCoroutines();
        progressNum++;
        cursor.SetActive(false);

        Tutorial2();
    }

    void Tutorial2()
    {
        PopupManager.Inst.DialogueMessage(new string[]{"저..안녕하세요..저는 힌트요정이에요-!",
                                                        "만약 문제를 풀어나가는게 힘드시다면 언제든지 찾아와주세요!",
                                                        "제가 도움이 될 수 있을거에요-!"});

        GameManager.Inst.OnEndDialogue += Tutorial3;
    }

    void Tutorial3()
    {
        GameManager.Inst.OnEndDialogue -= Tutorial3;
        renderPlayer.ForceMaterialEfx(new Color32(255, 180, 180, 255));

        PopupManager.Inst.DialogueMessage(new string[]{"딱..딱히 걱정되서 찾아온 건 아니지만...",
                                                        "그래도, 첫 만남이니까 선물을 드릴게요!"});
        GameManager.Inst.OnEndDialogue += Tutorial4;
    }


    void Tutorial4()
    {
        renderPlayer.ForceMaterialEfx(Color.white);
        GameManager.Inst.OnEndDialogue -= Tutorial4;

        SoundManager.Inst.PlaySfx(2);
        jewel.SetActive(true);
        GameManager.Inst.dataManager.RUBY++;
        GameManager.Inst.dataManager.TUTORIAL = 1;
        EndTutorial();
    }


    void EndTutorial()
    {
        tutorialBlack.SetActive(false);
    }





}
