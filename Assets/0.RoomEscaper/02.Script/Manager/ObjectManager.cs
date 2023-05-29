using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{

    [SerializeField] GameObject coinObject;
    [SerializeField] GameObject coinEfx;
    [SerializeField] GameObject coinEfxUI;

    [SerializeField] List<GameObject> coinList = new List<GameObject>();
    [SerializeField] List<GameObject> UIcoinList = new List<GameObject>();



    void Start()
    {
        StartCoroutine(CreateUICoin());

        GameManager.Inst.OnStatusUpdate += () =>
        {
            autoGet = 60 - GameManager.Inst.dataManager.UPGRADE_COINTIME * 5;
            if (GameManager.Inst.dataManager.UPGRADE_COINTIME > 5)
            {
                delay = 5;
            }
        };
    }


    int autoGet = 60;
    float autoTimer = 0;
    float timer = 0;
    float UITimer = 0;

    public int delay = 10;

    void Update()
    {
        CreateCoin();
    }

    void CreateCoin()
    {
        timer += Time.deltaTime;
        autoTimer += Time.deltaTime;
        if (timer > delay)
        {
            if (coinList.Count < 10)
            {
                GameObject go = Instantiate(coinObject, new Vector3(Random.Range(-5.0f, 3.0f), Random.Range(0.5f, 1.5f), Random.Range(-1.5f, 5.0f)), Quaternion.identity);
                coinList.Add(go);
                timer -= delay;
            }
        }

        if (autoTimer > autoGet)
        {
            if (coinList.Count > 0)
            {
                GetCoin(coinList[Random.Range(0, coinList.Count)]);
                autoTimer = 0;
            }
        }

    }

    IEnumerator CreateUICoin()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(10, 70));
            UIcoinList[Random.Range(0, UIcoinList.Count)].SetActive(true);
        }
    }



    public void GetCoin(GameObject go)
    {
        SoundManager.Inst.PlaySfx(3);
        coinList.Remove(go);
        Instantiate(coinEfx, go.transform.position, Quaternion.identity);

        int coin = Random.Range(1, 50) + GameManager.Inst.dataManager.UPGRADE_COIN * 10;
        GameManager.Inst.dataManager.COIN += coin;

        GameManager.Inst.uiManager.GetCoin(coin);

        Destroy(go);
    }


    public void GetUICoin(GameObject go)
    {
        SoundManager.Inst.PlaySfx(2);
        Instantiate(coinEfxUI, go.transform.position, Quaternion.identity);
        int coin = Random.Range(100, 500) + GameManager.Inst.dataManager.UPGRADE_COIN * 10;
        GameManager.Inst.dataManager.COIN += coin;
        GameManager.Inst.uiManager.GetCoin(coin);
        go.SetActive(false);
    }

}
