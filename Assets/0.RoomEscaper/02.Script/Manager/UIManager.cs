using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Manager<UIManager>
{



    [Header("Title")]
    [SerializeField] CanvasGroup titleGroup;

    [Header("InGame")]
    [SerializeField] CanvasGroup gameUIGroup;
    [SerializeField] Text[] rubyText;
    [SerializeField] Text[] coinText;
    [SerializeField] Text coinGetText;

    [Header("Shop")]
    [SerializeField] CanvasGroup shopGroup;
    [SerializeField] GameObject shopUI;
    [SerializeField] Image[] shopMenuTab;
    [SerializeField] GameObject[] shopContent;
    [SerializeField] Color32 shopMenuSelectedColor = new Color32(255, 186, 186, 255);



    protected override void Start()
    {
        base.Start();
        GameManager.Inst.OnStartGame += () => { gameUIGroup.gameObject.SetActive(true); };
        GameManager.Inst.OnTitleMenu += () => { gameUIGroup.gameObject.SetActive(false); };

        GameManager.Inst.OnGetMoney += () =>
        {
            rubyText[0].text = GameManager.Inst.dataManager.RUBY.ToString();
            rubyText[1].text = GameManager.Inst.dataManager.RUBY.ToString();
            coinText[0].text = GameManager.Inst.dataManager.COIN.ToString();
            coinText[1].text = GameManager.Inst.dataManager.COIN.ToString();
        };

        InitHintMenu();

        GameManager.Inst.OnGetMoney.Invoke();
    }




    #region TITLE

    public void Bt_StartGame()
    {
        StartGame();
    }

    void StartGame()
    {
        titleGroup.interactable = false;
        StartCoroutine(AlphaFadeOut(titleGroup, 1.0f, () =>
        {
            GameManager.Inst.OnStartGame();
        }));
    }

    public void Bt_ExitGame()
    {
        GameManager.Inst.OnTitleMenu();
        StartCoroutine(AlphaFadeIn(titleGroup, 1.0f, () =>
        {
            titleGroup.interactable = true;
        }));
    }

    #endregion



    #region Shop


    enum ShopMenu
    {
        Store, Upgrade, Hint, Answer, Achievement
    }

    ShopMenu currentMenu = ShopMenu.Store;

    public void Bt_EnterShop()
    {
        EnterShop();
    }
    void EnterShop()
    {
        shopUI.SetActive(true);
        GameManager.Inst.OnCanMove(false);
        InitShopMenu(currentMenu);
    }

    public void Bt_ExitShop()
    {
        ExitShop();
    }
    void ExitShop()
    {
        shopUI.SetActive(false);
        GameManager.Inst.OnCanMove(true);
    }


    public void Bt_ShopMenu(int index)
    {
        if (index == (int)currentMenu) { return; }
        if (index > (int)ShopMenu.Achievement) { return; }

        InitShopMenu((ShopMenu)index);
    }

    void InitShopMenu(ShopMenu menu)
    {
        shopMenuTab[(int)currentMenu].color = Color.white;
        shopMenuTab[(int)menu].color = shopMenuSelectedColor;

        shopContent[(int)currentMenu].SetActive(false);
        shopContent[(int)menu].SetActive(true);

        currentMenu = menu;

        InitShopContent(menu);
    }

    void InitShopContent(ShopMenu menu)
    {
        switch (menu)
        {
            case ShopMenu.Store:
                UpdateStore();
                break;
            case ShopMenu.Upgrade:
                UpdateUpgrade();
                break;
            case ShopMenu.Hint:
                UpdateHint();
                break;
            case ShopMenu.Answer:
                UpdateAnswer();
                break;
        }
    }

    #endregion


    #region Store

    //todo 캐릭터관련

    [Header("Store")]
    [SerializeField] Text[] storePriceText;
    string[] storeNames = { "힌트코인", "신비의보석" };
    int[] storePrices = { 1000, 10000, };

    void UpdateStore()
    {
        for (int i = 0; i < storeNames.Length; i++)
        {
            storePriceText[i].text = (GameManager.Inst.dataManager.GetStore(i) * storePrices[i]).ToString();
        }
    }

    public void Bt_BuyStore(int index)
    {
        if (GameManager.Inst.dataManager.COIN < GameManager.Inst.dataManager.GetStore(index) * storePrices[index])
        {
            SoundManager.Inst.PlaySfx(0);
            PopupManager.Inst.Notice("소지금이 부족합니다");
            return;
        }

        GameManager.Inst.dataManager.COIN -= GameManager.Inst.dataManager.GetStore(index) * storePrices[index];
        GameManager.Inst.dataManager.SetStore(index, GameManager.Inst.dataManager.GetStore(index) + 1);
        SoundManager.Inst.PlaySfx(2);

        switch (index)
        {
            case 0: GameManager.Inst.dataManager.HINTCOIN++; break;
            case 1: GameManager.Inst.dataManager.RUBY++; break;
        }
        PopupManager.Inst.Notice($"{storeNames[index]}를 구입하였습니다");
        GameManager.Inst.OnStatusUpdate.Invoke();

        InitShopContent(ShopMenu.Store);
    }

    #endregion


    #region Upgrade

    [Header("Upgrade")]
    [SerializeField] Text[] upgradeNameText;
    [SerializeField] Text[] upgradeInfoText;
    [SerializeField] Text[] upgradePriceText;
    string[] upgradeNames = { "신속의비서", "탄력의비서", "귀중한보석", "마법의보석" };
    int[] upgradeMaxLevels = { 5, 5, 10, 10 };
    int[] upgradePrices = { 2500, 2500, 1000, 2000 };

    void UpdateUpgrade()
    {
        for (int i = 0; i < upgradeNames.Length; i++)
        {
            upgradeNameText[i].text = $"{upgradeNames[i]} ({GameManager.Inst.dataManager.GetUpgrade(i)}/{upgradeMaxLevels[i]})";
            upgradePriceText[i].text = (GameManager.Inst.dataManager.GetUpgrade(i) * upgradePrices[i]).ToString();
        }
        upgradeInfoText[2].text = $"코인 획득량이 +{GameManager.Inst.dataManager.UPGRADE_COIN * 10}만큼 증가합니다";
        upgradeInfoText[3].text = $"{65 - GameManager.Inst.dataManager.UPGRADE_COINTIME * 5}초마다 자동으로 코인을 획득합니다";
    }

    public void Bt_BuyUpgrade(int index)
    {
        if (GameManager.Inst.dataManager.GetUpgrade(index) >= upgradeMaxLevels[index])
        {
            SoundManager.Inst.PlaySfx(0);
            PopupManager.Inst.Notice("최대레벨입니다");
            return;
        }
        if (GameManager.Inst.dataManager.COIN < GameManager.Inst.dataManager.GetUpgrade(index) * upgradePrices[index])
        {
            SoundManager.Inst.PlaySfx(0);
            PopupManager.Inst.Notice("소지금이 부족합니다");
            return;
        }

        GameManager.Inst.dataManager.COIN -= GameManager.Inst.dataManager.GetUpgrade(index) * upgradePrices[index];
        GameManager.Inst.dataManager.SetUpgrade(index, GameManager.Inst.dataManager.GetUpgrade(index) + 1);
        SoundManager.Inst.PlaySfx(2);
        PopupManager.Inst.Notice("업그레이드 완료");
        GameManager.Inst.OnStatusUpdate.Invoke();

        InitShopContent(ShopMenu.Upgrade);
    }

    #endregion



    #region Hint

    [Header("Hint")]

    [SerializeField] InputField hintInputField;
    [SerializeField] Text hintTitle;
    [SerializeField] Text hintCoinCount;
    [SerializeField] Text[] hintText;
    [SerializeField] Button[] hintBtn;
    [SerializeField] GameObject answerItem;

    int currentHintIndex = -1;

    string[] questionTitleList = { "튜토리얼", "알파벳", "도형", "빈칸", "미로", "색상", "키패드", "회전", "대칭", "숫자표",
                                   "문자해독","문자해독2","문자해독3","부호해독","부호해독2","부호해독3","손글씨해독","손글씨해독2",
                                   "주기해독","주기해독2","끝말잇기","글자","동물","방법","시","" };
    string[,] questionHintList = { { "문제의 제목은 튜토리얼입니다", "상점에서 힌트를 확인할 수 있습니다", "상단의 '정답'탭을 눌러주세요" },
                                   { "커다란 알파벳을 찾아보세요", "돋보기는 찾다를 의미합니다", "돋보기는 F in D 입니다" },
                                   { "점과 선은 불가능 합니다", "입체(3D)형태로 만들어봅니다", "동그라미로 이루어진 입체" },
                                   { "빈칸을 채워야합니다", "숫자는 노노그램을 의미합니다", "정답은 한자입니다" },
                                   { "미로의 길을 찾아야합니다", "빛나는 두 지점을 연결합니다", "정답은 경로를 확인하면 알 수 있습니다" },
                                   { "색상하나당 알파벳 하나입니다", "우측은 흐릿한 색상을 제외한 알파벳입니다", "포털사이트를 나타냅니다" },
                                   { "숫자의 규칙을 찾아야합니다", "두 숫자를 더하면 규칙이 나타납니다", "정답은 10개의 경우의수를 가집니다" },
                                   { "회전된 모양을 확인합니다", "숫자를 알파벳으로 치환합니다", "정답은 한글입니다" },
                                   { "공통으로 들어갈 글자를 찾습니다", "정답은 한글자입니다", "왼쪽의 단어는 모두 왼쪽에 위치합니다(피+?, 비상+? 등)" },
                                   { "숫자의 규칙을 찾아야합니다", "정답은 THE ANSWER에 해당하는 숫자(9자리)입니다", "숫자표에서 1은 1개있습니다" },
                                   { "각 단어는 하나의 글자를 뜻합니다", "단어를 영어로 변환합니다", "첫번째 예시는 L+O+V+E 입니다" },
                                   { "각 단어는 하나의 자/모음을 뜻합니다", "단어의 첫번째 자음에 주목합니다", "첫번째 예시는 ㄹ+ㅓ+ㅂ+ㅡ 입니다" },
                                   { "각 단어는 하나의 글자를 뜻합니다", "단어의 숫자에 주목합니다", "첫번째 예시는 T+O+Y입니다" },
                                   { "각 단어는 하나의 부호를 뜻합니다", "장점(長)과 단점(短)은 길고 짧음을 의미합니다", "모스부호로 표현합니다" },
                                   { "각 단어는 하나의 부호를 뜻합니다", "단어의 발음의 길고 짧음을 확인합니다", "모스부호로 표현합니다" },
                                   { "각 단어는 하나의 부호를 뜻합니다", "곰세마리 노래박자의 길고 짧음을 확인합니다", "모스부호로 표현합니다" },
                                   { "각 알파벳은 하나의 모양을 뜻합니다", "첫번째 예시의 Bird는 새 입니다", "점자표를 확인합니다" },
                                   { "각 알파벳들은 하나의 글자를 뜻합니다", "점자표를 확인합니다", "키보드의 위치를 확인합니다"  },
                                   { "각 단어는 특정한 알파벳을 뜻합니다", "주기율표를 확인합니다", "존재하지 않는 알파벳을 찾습니다" },
                                   { "주기율표를 확인합니다", "1번부터 10번까지 해당하는 원소를 찾습니다", "높이를 통해 구분을 짓습니다(수소=1)" },
                                   { "두 사람은 커플입니다", "사로 시작하는 로맨틱한 말이 있습니다", "A는 한방단어를 사용했습니다" },
                                   { "각 한자는 하나의 자/모음을 뜻합니다", "첫번째 문장을 해석하면 '글자를 잘 살피자' 입니다", "두번째 획부터 확인합니다" },
                                   { "넌센스입니다", "크기는 숫자를 의미합니다", "첫번째 동물은 조류에 속합니다" },
                                   { "지문의 내용을 해석해야 합니다", "문제 안에는 문이 6개 존재하고 있습니다", "이것이 무엇인지 알아내야합니다" },
                                   { "지문의 내용에서 글자를 추출해야 합니다", "+3, +1, +4, +1, +5, +9, +2 ...", "문, 제, 의, 정, 답, 을, 알 ..." },
                                   { "", "", "" },
                                   { "", "", "" },


                                   };

    public void ResetHintData()
    {
        PlayerPrefs.SetInt(questionTitleList[0], 4);
        for (int i = 1; i < questionTitleList.Length; i++)
        {
            PlayerPrefs.SetInt(questionTitleList[i], 0);
        }
    }

    void UpdateHint()
    {
        hintCoinCount.text = $"× {GameManager.Inst.dataManager.HINTCOIN}";
    }

    void InitHintMenu()
    {
        hintInputField.onEndEdit.AddListener((t) => { OnSubmitHint(t); });
    }


    void OnSubmitHint(string text)
    {
        if (string.IsNullOrEmpty(hintInputField.text))
        {
            return;
        }
        hintInputField.text = string.Empty;
        CheckHint(text);
    }

    void CheckHint(string title)
    {
        for (int i = 0; i < questionTitleList.Length; i++)
        {
            if (string.Equals(questionTitleList[i], title, System.StringComparison.Ordinal))
            {
                LoadHint(i);
                return;
            }
        }
        PopupManager.Inst.Notice("해당하는 문제가 없습니다");
    }

    void LoadHint(int index)
    {
        currentHintIndex = index;
        hintTitle.text = questionTitleList[index];
        int hintCount = PlayerPrefs.GetInt(questionTitleList[index]);

        //정답구입버튼
        answerItem.SetActive(hintCount == 3);

        for (int i = 0; i < 3; i++)
        {
            if (i < hintCount)
            {
                hintText[i].text = questionHintList[index, i];
                hintBtn[i].interactable = false;
            }
            else
            {
                hintText[i].text = "보유중인 힌트가 없습니다";
                hintBtn[i].interactable = true;
            }
        }
    }

    public void Bt_PurchaseHint()
    {
        if (currentHintIndex < 0) { return; }   //문제 미선택
        if (PlayerPrefs.GetInt(questionTitleList[currentHintIndex]) >= 3) { return; }    //힌트3개구입완료

        UnityEngine.Events.UnityAction action = () =>
        {
            PurchaseHint();
        };
        PopupManager.Inst.NoticeChoice("힌트를 구입하시겠습니까?", action);
    }

    void PurchaseHint()
    {
        if (GameManager.Inst.dataManager.HINTCOIN < 1)
        {
            PopupManager.Inst.Notice("힌트코인이 부족합니다");
            return;
        }

        GameManager.Inst.dataManager.HINTCOIN--;
        PlayerPrefs.SetInt(questionTitleList[currentHintIndex], PlayerPrefs.GetInt(questionTitleList[currentHintIndex]) + 1);
        SoundManager.Inst.PlaySfx(2);

        InitShopContent(currentMenu);
        LoadHint(currentHintIndex);
    }


    public void Bt_PurchaseAnswer()
    {
        if (currentHintIndex < 0) { return; }   //문제 미선택

        UnityEngine.Events.UnityAction action = () =>
        {
            PurchaseAnswer();
        };
        PopupManager.Inst.NoticeChoice("정답 및 해설을 구입하시겠습니까?\n<size=45><color=#FF4200>(신비의보석 필요)</color></size>", action);
    }

    void PurchaseAnswer()
    {
        if (GameManager.Inst.dataManager.RUBY < 1)
        {
            PopupManager.Inst.Notice("신비의 보석이 부족합니다");
            return;
        }

        GameManager.Inst.dataManager.RUBY--;
        PlayerPrefs.SetInt(questionTitleList[currentHintIndex], 4);
        SoundManager.Inst.PlaySfx(2);

        InitShopContent(currentMenu);
        LoadHint(currentHintIndex);
    }


    #endregion



    #region Answer

    [SerializeField] GameObject answer;
    [SerializeField] Text[] answerTexts;

    string[] questionAnswerList = { "Gamestart", "Nine", "Sphere", "九", "구", "Goo", "09,18,27...", "아홉", "9", "999999999",
                                    "Cafe","Iced Americano","Latte","1000","10","50","Lion", "Butterfly","Bus", "Chopstick",
                                    "해질녘", "4", "양", "목숨", "Diamond", ""     };
    string[] questionExplainList = { "미궁을 시작하시려면 메인화면 하단의 정답 입력창에 [Gamestart]을 입력해주세요",
                                     "왼쪽의 그림은 D안에 F가 들어있다는 의미를 영어로 치환하여 Find를 뜻합니다. 같은 방법을 적용하여 오른쪽의 정답은 Nine입니다",
                                     "우측의 단어는 왼쪽의 그림으로 이루어진 입체(3D)형태를 나타내는 단어입니다. 따라서, 정답은 원으로 이루어진 입체도형인 구(Sphere)입니다",
                                     "문제의 빈칸은 노노그램을 의미합니다. 각줄의 숫자가 5라면 해당하는 줄에 5개의 색칠이 필요한것이며 (1 1 1)과 같이 띄어쓰기가 적용되어있을경우 색칠또한 띄어서 칠하라는 의미를 가집니다. 규칙에 맞게 색칠을 하면 결과적으로 九의 모양을 확인할 수 있습니다",
                                     "미로의 빛나는 두 지점을 겹치는 경로 없이 최단거리로 연결을 한다면, 지나갔던 길 사이에 주황색으로 크게 '구'라는 글씨를 발견할 수 있습니다",
                                     "위에서부터 각각 포털사이트의 로고색상(Naver,Daum,Google)을 의미합니다. 흐릿한 부분을 제외하고 알파벳으로 나타내면 Goo라는 결과를 확인할 수 있습니다",
                                     "칸안의 각각의 숫자를 서로 더하면, 왼쪽위에서부터 1(1+0), 2(1+1), 3(2+1) ... 를 확인할 수 있습니다. 따라서 마지막에 올 숫자는 9이며, 두자리의 합으로 나타내기에 (09,18,27,36... 90)중 하나를 입력하면 정답입니다",
                                     "각각의 숫자를 알파벳으로 치환하면 15=O, 20=T, 1=A 입니다. 이후 회전 및 위치에 맞추어서 알파벳을 대입하면 한글로 '아홉' 이라는 문구를 만들어낼 수 있습니다",
                                     "피구, 비상구, 구미호, 구성원 등 각각의 단어는 모두 공통된 글씨 '구'를 가집니다. 따라서 정답은 구 입니다",
                                     "숫자표는 모두 45칸으로 자세히 살펴보면 1이 1개, 2가 2개, 3이 3개 ... 8이 8개있는것을 확인할 수 있습니다. 따라서 THE ANSWER에 들어갈 9개의 숫자는 모두 9입니다",
                                     "주어진 단어를 영어로 바꾼뒤에 맨앞의 알파벳을 추출합니다. 첫번째의 경우 인생(Life)+의견(Opinion)+승리(Victory)+감정(Emotion)=LOVE, 따라서 정답은 C(andy)+A(ngel)+F(ool)+E(xercise) = Cafe 입니다",
                                     "주어진 단어의 첫번째 자음에 해당하는 순서의 자/모음을 추출합니다. 달의 경우 첫번쨰의 자음인 ㄷ의 순서는 3이므로 달에서 3번째에 해당하는 자음은 ㄹ 입니다. 달(3)+넋(2)+마법(5)+늘(2)=러브, 마찬가지로 문제의 마당(5)+나룻배(2)+당근(3)+솜사탕(7)=아아 를 확인할 수 있습니다. 따라서, 아아를 의미하는 아이스아메리카노가 정답입니다",
                                     "주어진 단어에 담겨진 숫자에 해당하는 순서의 자/모음을 추출하여 180도 뒤짚은 다음 알파벳으로 치환합니다. 마도사의 경우 숫자4(사)를 포함하기에 4번쨰에 해당하는 자/모음인 ㅗ를 추출하여 180도 뒤집으면 T임을 확인할 수 있습니다. 마도사(4)+일본(1)+해삼(3) = ㅗㅇㅅ → TOY 입니다. 따라서, 오목(5)+보일러(1)+각오(5)+노이즈(2)+고3(3) = ㄱㅂㅗㅗ3 → LATTE 입니다",
                                     "주어진 단어의 장단점을 파악하여 장(長)은 선, 단(短)은 점으로 치환하여 모스부호로 해석하면 각각 C,J,S라는 결과를 얻을 수 있습니다. 이를 영타로 변환하면 천(1000)이라는 정답을 얻을 수 있습니다",
                                     "주어진 단어의 발음을 확인하여 긴것은 선, 짧은것은 점으로 치환하여 모스부호로 해석하면 YEOL이라는 결과를 얻을 수 있습니다. 이를 다시한번 발음하면 열(10)이라는 정답을 얻을 수 있습니다",
                                     "주어진 노래 가사의 박자를 확인하여 긴것은 선, 짧은것은 점으로 치환하여 모스부호로 해석하면 E,O,O라는 결과를 얻을 수 있습니다. 이를 발음에 해당하는 숫자로 치환하면 2×5×5 = 50 이라는 정답을 얻을 수 있습니다",
                                     "주어진 알파벳을 점자표에서 찾아낸다음 모양을 추출합니다. 이후 모양을 서로 붙여둔 상태에서 표기된 부분을 확인하면 한글로 이루어진 단어를 확인할 수 있습니다. 따라서 정답은 사자입니다",
                                     "주어진 알파벳을 키보드에서 찾아낸 다음 키보드를 2x3의 모양으로 분리하여 점자를 도출해냅니다. 이후 점자표에서 해당하는 알파벳을 확인하면 n,a,b,i를 확인할 수 있습니다. 이후, 단어의 발음을 통해 나비가 정답임을 확인할 수 있습니다",
                                     "주어진 원자들을 주기율표를 통해 모두 하나 혹은 두개의 알파벳으로 치환합니다. 이후 A~Z에 해당되지 않는 알파벳을 추려내면 j,m,q,t를 확인할 수 있으며, 한글로 변환 후(ㅓ,ㅡ,ㅂ,ㅅ) 애너그램을 통해 버스를 도출해낼 수 있습니다.",
                                     "주기율표를 확인한 다음, 1부터 10번에(수소~네온) 해당하는 원소를 순서대로 찾아냅니다. 이후, 층별로 구분을 지어보면 1351356665임을 확인할 수 있습니다. 이를 음계로 변환하면 젓가락송을 유추할 수 있습니다. 이후, 해당하는 가사의 답변인 젓가락이 정답임을 알 수 있습니다",
                                     "커플사이에서 사로 시작하는 낭만적인 단어를 추론하여 사랑해라는 답변을 도출해냅니다. 이후에, A의 말로 게임이 종료되었음으로 한방단어를 사용하였음을 유추할 수 있습니다. 따라서, 해로 시작하는 대표적인 한방단어인 해질녘이 정답임을 알 수 있습니다",
                                     "한자의 모양을 살펴본 뒤에, 예시문장과 비교하면 한자의 첫번째 획을 제외할경우 한글과 매칭이 된다는 것을 확인할 수 있습니다. 따라서 아래의 한자에서 1획을 제외한 상태로 문장을 해석하면 '코끼리 다리 수는' 라는 결과를 얻을 수 있습니다. 따라서 정답은 4 입니다",
                                     "세상에서 가장 큰 동물은 넌센스로 백조(100조)임을 알아낼 수 있습니다. 100조보다 400억배 큰 숫자는 4자이며, 두번째 별이 사자를 의미하는 것을 알 수 있습니다. 또한 이보다 2500배 큰 숫자를 확인하면 1양이며, 정답이 양이라는 것을 확인할 수 있습니다",
                                     "문장을 해석하면 먼저, 입구에서 모든방향의 문을 찾아가야합니다. 해당문제에서 '문'이라는 글씨를 모두 찾아 입구에서 연결하면 木형태가 나타남을 확인할 수 있습니다. 또한, 지문의 남은 내용을 확인하면 이것이 의미하는 것은 숨이라는 것을 알아챌 수 있습니다(숨은그림, 숨죽여, 숨소리없이 등). 이후 두개의 결론을 더하여 목숨이라는 정답을 도출해낼 수 있습니다",
                                     "먼저 첫번째로 작가명을 확인하여, 파이(pi)의 자리수 순서로 글자를 도출합니다(+3(3),+1(4),+4(8),+1(9),+5(14) ... = '문제의정답을 알고 싶다면 파이를 세지말고 피로 시작하는 수열을 파악하라'). 피로 시작하는 대표적인 수열은 피보나치 수열이기에, 이후 피보나치 수열에 해당하는 순서의 글자를 추출합니다(1,1,2,3,5, ... = '하하 이문제의 정답은 계절위'). 이후 본문에서 계절에 해당하는 단어(봄,여름,가을,겨울)를 찾은 뒤에 위에있는 단어를 순서대로 연결하면 '아,몬드,가죽,으면'이라는 결과를 확인하여, 다이아몬드가 정답임을 확인합니다",
                                     "",
                                     };

    [SerializeField] GameObject[] answerObjectList;

    void UpdateAnswer()
    {
        for (int i = 0; i < questionTitleList.Length; i++)
        {
            if (PlayerPrefs.GetInt(questionTitleList[i]) >= 4)
            {
                answerObjectList[i].SetActive(true);
            }
        }
    }

    public void DisplayAnswer(int index)
    {
        SoundManager.Inst.PlaySfx(2);
        answer.SetActive(true);
        answerTexts[0].text = questionTitleList[index];
        answerTexts[1].text = questionExplainList[index];
        answerTexts[2].text = "정답 : " + questionAnswerList[index];
    }


    #endregion


    IEnumerator AlphaFadeIn(CanvasGroup group, float duration, System.Action callback = null)
    {
        float time = 0f;
        float speed = (duration != 0) ? 1 / duration : 1;

        while (time < duration)
        {
            time += Time.deltaTime;
            group.alpha += Time.deltaTime * speed;
            yield return null;
        }
        group.alpha = 1;
        callback();
    }

    IEnumerator AlphaFadeOut(CanvasGroup group, float duration, System.Action callback = null)
    {
        float time = 0f;
        float speed = (duration != 0) ? 1 / duration : 1;

        while (time < duration)
        {
            time += Time.deltaTime;
            group.alpha -= Time.deltaTime * speed;
            yield return null;
        }
        group.alpha = 0;
        callback();
    }


    IEnumerator AlphaFadeOut(Text text, float duration)
    {
        float time = 0f;
        float speed = (duration != 0) ? 1 / duration : 1;

        Color color = text.color;
        color.a = 1;

        while (time < duration)
        {
            time += Time.deltaTime;
            color.a -= Time.deltaTime * speed;
            text.color = color;
            yield return null;
        }
        color.a = 0;
        text.color = color;
    }



    Coroutine GetCoinCoroutine = null;
    public void GetCoin(int num)
    {
        if (GetCoinCoroutine != null)
        {
            StopCoroutine(GetCoinCoroutine);
        }
        coinGetText.text = $"+{num}";
        GetCoinCoroutine = StartCoroutine(AlphaFadeOut(coinGetText, 3.0f));

    }

}
