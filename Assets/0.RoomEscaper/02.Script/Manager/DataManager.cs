using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{

    void Awake()
    {
        PlayerPrefs.SetInt("튜토리얼", 4);  //첫번째 문제 

        ruby = PlayerPrefs.GetInt("RUBY", 0);
        coin = PlayerPrefs.GetInt("COIN", 0);
        hintCoin = PlayerPrefs.GetInt("HINTCOIN", 0);
        tutorial = PlayerPrefs.GetInt("TUTORIAL", 0);
        store_hint = PlayerPrefs.GetInt("STORE_HINT", Encode(1));
        store_ruby = PlayerPrefs.GetInt("STORE_RUBY", Encode(1));
        upgrade_speed = PlayerPrefs.GetInt("UPGRADE_SPEED", Encode(1));
        upgrade_jump = PlayerPrefs.GetInt("UPGRADE_JUMP", Encode(1));
        upgrade_coin = PlayerPrefs.GetInt("UPGRADE_COIN", Encode(1));
        upgrade_coinTime = PlayerPrefs.GetInt("UPGRADE_COINTIME", Encode(1));
    }


    [ContextMenu("ResetValue")]
    void ResetData()
    {
        PlayerPrefs.SetInt("RUBY", 0);
        PlayerPrefs.SetInt("COIN", 0);
        PlayerPrefs.SetInt("HINTCOIN", 0);
        PlayerPrefs.SetInt("TUTORIAL", 0);
        PlayerPrefs.SetInt("STORE_HINT", Encode(1));
        PlayerPrefs.SetInt("STORE_RUBY", Encode(1));
        PlayerPrefs.SetInt("UPGRADE_SPEED", Encode(1));
        PlayerPrefs.SetInt("UPGRADE_JUMP", Encode(1));
        PlayerPrefs.SetInt("UPGRADE_COIN", Encode(1));
        PlayerPrefs.SetInt("UPGRADE_COINTIME", Encode(1));

        UIManager.Inst.ResetHintData();
    }

    [ContextMenu("RichRich")]
    void RichRich()
    {
        COIN = 50000000;
        RUBY = 50000000;
        HINTCOIN = 1000;
    }


    void SaveData(string key, int value)
    {
        // return;
        PlayerPrefs.SetInt(key, value);
    }
    void SaveData(string key, string value)
    {
        // return;
        PlayerPrefs.SetString(key, value);
    }







    ///<summary>
    ///튜토리얼 여부
    ///</summary>
    public int TUTORIAL
    {
        get
        {
            return Decode(ruby);
        }
        set
        {
            ruby = Encode(value);
            SaveData("TUTORIAL", tutorial);
            GameManager.Inst.OnGetMoney.Invoke();
        }
    }
    private int tutorial;


    ///<summary>
    ///보석
    ///</summary>
    public int RUBY
    {
        get
        {
            return Decode(ruby);
        }
        set
        {
            ruby = Encode(value);
            SaveData("RUBY", ruby);
            GameManager.Inst.OnGetMoney.Invoke();
        }
    }
    private int ruby;

    ///<summary>
    ///보석
    ///</summary>
    public int COIN
    {
        get
        {
            return Decode(coin);
        }
        set
        {
            coin = Encode(value);
            SaveData("COIN", coin);
            GameManager.Inst.OnGetMoney.Invoke();
        }
    }
    private int coin;


    ///<summary>
    ///힌트코인
    ///</summary>
    public int HINTCOIN
    {
        get
        {
            return Decode(hintCoin);
        }
        set
        {
            hintCoin = Encode(value);
            SaveData("HINTCOIN", hintCoin);
            GameManager.Inst.OnGetMoney.Invoke();
        }
    }
    private int hintCoin;



    ///<summary>
    ///구매횟수
    ///0:힌트코인, 1:신비의보석, 
    ///</summary>
    public int GetStore(int num)
    {
        switch (num)
        {
            case 0: return STORE_HINT;
            case 1: return STORE_RUBY;
            default: return 0;
        }
    }


    ///<summary>
    ///구매횟수
    ///0:힌트코인, 1:신비의보석, 
    ///</summary>
    public void SetStore(int num, int value)
    {
        switch (num)
        {
            case 0: STORE_HINT = value; break;
            case 1: STORE_RUBY = value; break;
        }
    }



    ///<summary>
    ///힌트구매횟수
    ///</summary>
    public int STORE_HINT
    {
        get
        {
            return Decode(store_hint);
        }
        set
        {
            store_hint = Encode(value);
            SaveData("STORE_HINT", store_hint);
        }
    }
    private int store_hint;


    ///<summary>
    ///보석구매횟수
    ///</summary>
    public int STORE_RUBY
    {
        get
        {
            return Decode(store_ruby);
        }
        set
        {
            store_ruby = Encode(value);
            SaveData("STORE_RUBY", store_ruby);
        }
    }
    private int store_ruby;







    ///<summary>
    ///0:이동속도, 1:점프, 2:코인획득 3:코인시간
    ///</summary>
    public int GetUpgrade(int num)
    {
        switch (num)
        {
            case 0: return UPGRADE_SPEED;
            case 1: return UPGRADE_JUMP;
            case 2: return UPGRADE_COIN;
            case 3: return UPGRADE_COINTIME;
            default: return 0;
        }
    }


    ///<summary>
    ///0:이동속도, 1:점프, 2:코인획득 3:코인시간
    ///</summary>
    public void SetUpgrade(int num, int value)
    {
        switch (num)
        {
            case 0: UPGRADE_SPEED = value; break;
            case 1: UPGRADE_JUMP = value; break;
            case 2: UPGRADE_COIN = value; break;
            case 3: UPGRADE_COINTIME = value; break;
        }
    }





    ///<summary>
    ///이동속도 (1~5)
    ///</summary>
    public int UPGRADE_SPEED
    {
        get
        {
            return Decode(upgrade_speed);
        }
        set
        {
            upgrade_speed = Encode(value);
            SaveData("UPGRADE_SPEED", upgrade_speed);
        }
    }
    private int upgrade_speed;


    ///<summary>
    ///점프 (1~5)
    ///</summary>
    public int UPGRADE_JUMP
    {
        get
        {
            return Decode(upgrade_jump);
        }
        set
        {
            upgrade_jump = Encode(value);
            SaveData("UPGRADE_JUMP", upgrade_jump);
        }
    }
    private int upgrade_jump;



    ///<summary>
    ///코인획득량 증가
    ///</summary>
    public int UPGRADE_COIN
    {
        get
        {
            return Decode(upgrade_coin);
        }
        set
        {
            upgrade_coin = Encode(value);
            SaveData("UPGRADE_COIN", upgrade_coin);
        }
    }

    private int upgrade_coin;

    ///<summary>
    ///코인획득량 증가
    ///</summary>
    public int UPGRADE_COINTIME
    {
        get
        {
            return Decode(upgrade_coinTime);
        }
        set
        {
            upgrade_coinTime = Encode(value);
            SaveData("UPGRADE_COINTIME", upgrade_coinTime);
        }
    }

    private int upgrade_coinTime;




    int Encode(int num)
    {
        num *= 8;
        return num;
    }

    int Decode(int value)
    {
        value /= 8;
        return value;
    }

}
