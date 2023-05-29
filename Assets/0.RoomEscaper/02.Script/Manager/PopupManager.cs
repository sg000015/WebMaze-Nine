using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : Manager<PopupManager>
{

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    void Init()
    {
        InitDialogue();
    }


    [Header("Dialogue")]
    [SerializeField] Text dialogueText;
    [SerializeField] GameObject dialogue;
    [SerializeField] Button dialogueBtn;

    Coroutine DialogueDisplayCoroutine = null;


    void InitDialogue()
    {
        dialogueBtn.onClick.AddListener(() =>
        {
            currentPage++;
            if (currentPage >= maxPage)
            {
                dialogue.SetActive(false);
                GameManager.Inst.OnEndDialogue.Invoke();
            }
            else
            {
                if (DialogueDisplayCoroutine != null)
                {
                    StopCoroutine(DialogueDisplayCoroutine);
                }
                DialogueDisplayCoroutine = StartCoroutine(DisplayMessage(dialogueText, currentMsg[currentPage], dialogueBtn));
            }
        });
    }

    int currentPage = 0;
    int maxPage = 0;
    string[] currentMsg;
    public void DialogueMessage(string msg)
    {
        currentPage = 0;
        maxPage = 1;
        dialogue.SetActive(true);

        if (DialogueDisplayCoroutine != null)
        {
            StopCoroutine(DialogueDisplayCoroutine);
        }
        DialogueDisplayCoroutine = StartCoroutine(DisplayMessage(dialogueText, msg, dialogueBtn));
    }

    public void DialogueMessage(string[] msg)
    {
        currentPage = 0;
        maxPage = msg.Length;
        currentMsg = msg;
        dialogue.SetActive(true);

        if (DialogueDisplayCoroutine != null)
        {
            StopCoroutine(DialogueDisplayCoroutine);
        }
        DialogueDisplayCoroutine = StartCoroutine(DisplayMessage(dialogueText, currentMsg[currentPage], dialogueBtn));
    }


    IEnumerator DisplayMessage(Text _text, string message, Button button = null)
    {
        WaitForSeconds ws = new WaitForSeconds(0.03f);
        button.interactable = false;

        _text.text = string.Empty;
        for (int i = 0; i < message.Length; i++)
        {
            _text.text += message.Substring(i, 1);
            yield return ws;
        }
        button.interactable = true;
    }





    [Header("Notice")]
    [SerializeField] GameObject noticePopUp;
    [SerializeField] Text noticeMessage;

    [SerializeField] GameObject noticeChoicePopUp;
    [SerializeField] Text noticeChoiceMessage;
    [SerializeField] Button noticeChoiceBtn;

    public void Notice(string msg)
    {
        noticePopUp.SetActive(true);
        noticeMessage.text = msg;
    }


    public void NoticeChoice(string msg, UnityEngine.Events.UnityAction call)
    {
        SoundManager.Inst.PlaySfx(0);
        SoundManager.Inst.PlaySfx(0);
        noticeChoicePopUp.SetActive(true);
        noticeChoiceMessage.text = msg;

        noticeChoiceBtn.onClick.RemoveAllListeners();
        noticeChoiceBtn.onClick.AddListener(call);
    }



}
