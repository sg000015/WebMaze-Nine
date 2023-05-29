using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst;

    void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    #region Manager

    public UIManager uiManager;
    public ObjectManager objectManager;
    public DataManager dataManager;

    #endregion





    #region Action

    public bool CanMove = false;

    public Action<bool> OnCanMove = (b) => { GameManager.Inst.CanMove = b; };

    public Action OnStartGame = () =>
    {
        GameManager.Inst.OnCanMove(true);
        GameManager.Inst.OnStatusUpdate();
    };

    public Action OnTitleMenu = () => { GameManager.Inst.OnCanMove(false); };


    public Action OnClickCharacter = () => { };
    public Action OnEndDialogue = () => { };


    public Action OnGetMoney = () => { };

    public Action OnStatusUpdate = () => { };

    #endregion

}
