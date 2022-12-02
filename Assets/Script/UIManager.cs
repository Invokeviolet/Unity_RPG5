using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject TimePrefab; // 타이머 UI
    [SerializeField] public TextMeshProUGUI TimeText; // 타이머 텍스트

    [SerializeField] Canvas TITLECANVAS;
    [SerializeField] Canvas GENERALCANVAS;
    [SerializeField] Canvas LOGINCANVAS;
    [SerializeField] Canvas STORECANVAS;
    [SerializeField] Canvas GAMECLEARCANVAS;
    [SerializeField] Canvas GAMEOVERCANVAS;

    [Header("플레이어 정보")]
    [SerializeField] public TextMeshProUGUI Player_Lv;
    [SerializeField] public TextMeshProUGUI Player_ID;
    [SerializeField] public TextMeshProUGUI Player_Gold;
    [SerializeField] public TextMeshProUGUI Player_Potion;
    [SerializeField] public TextMeshProUGUI Player_Attack;

    [SerializeField] Slider Player_Exp;
    [SerializeField] Slider Player_HP;

    [Header("[INPUT UI]")]
    [SerializeField] GameObject objInputName = null; // 이름 입력 UI
    [SerializeField] TMP_InputField inputName = null; // 입력받은 이름
    
    [SerializeField] TextMeshProUGUI MapInfo;
    [SerializeField] TextMeshProUGUI MonsterInfo;

    [SerializeField] Button ReturnButton; // 나중에 삭제
    [SerializeField] Button BuyButton;
    [SerializeField] Button StartButton;


    // 싱글톤
    #region 싱글톤

    private static UIManager Instance = null;
    public static UIManager INSTANCE
    {
        get
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<UIManager>();
            }
            DontDestroyOnLoad(Instance.gameObject);
            return Instance;
        }
    }
    #endregion

    Action action;
        
    private void Awake()
    {
        action += BUYPOTION;
        BuyButton.onClick.AddListener(delegate () { action(); });

        TITLECANVAS.gameObject.SetActive(false);
        GENERALCANVAS.gameObject.SetActive(false);
        STORECANVAS.gameObject.SetActive(false);
        GAMECLEARCANVAS.gameObject.SetActive(false);
        GAMEOVERCANVAS.gameObject.SetActive(false);

        //TimeManager timeManager=FindObjectOfType<TimeManager>();
    }
    
    public void Start()
    {
        TITLESCENE();
    }

    //
    // SCENE UI
    #region SCENE UI

    public void TITLESCENE()
    {
        TITLECANVAS.gameObject.SetActive(true);//        
        GENERALCANVAS.gameObject.SetActive(false);
        STORECANVAS.gameObject.SetActive(false);
        GAMECLEARCANVAS.gameObject.SetActive(false);
        GAMEOVERCANVAS.gameObject.SetActive(false);

        // 시작 버튼을 누르면 Login 창 띄우기
        // Login 하면 인게임 씬으로 이동
        
    }
    public void GENERALSCENE()
    {
        GENERALCANVAS.gameObject.SetActive(true);
        MonsterInfo.gameObject.SetActive(false);
        TITLECANVAS.gameObject.SetActive(false);
        STORECANVAS.gameObject.SetActive(false);
        GAMECLEARCANVAS.gameObject.SetActive(false);
        GAMEOVERCANVAS.gameObject.SetActive(false);
        
    }
    public void LOGIN() 
    {
        LOGINCANVAS.gameObject.SetActive(true);
    }
    public void ACTIONSCENE()
    {
        GENERALCANVAS.gameObject.SetActive(true);
        MonsterInfo.gameObject.SetActive(true);
        TITLECANVAS.gameObject.SetActive(false);
        STORECANVAS.gameObject.SetActive(false);
        GAMECLEARCANVAS.gameObject.SetActive(false);
        GAMEOVERCANVAS.gameObject.SetActive(false);
        
    }

    public void STORESCENE()
    {
        GENERALCANVAS.gameObject.SetActive(false);
        TITLECANVAS.gameObject.SetActive(false);
        STORECANVAS.gameObject.SetActive(true);//        
        GAMECLEARCANVAS.gameObject.SetActive(false);
        GAMEOVERCANVAS.gameObject.SetActive(false);
        
    }

    public void GAMECLEARSCENE()
    {
        GENERALCANVAS.gameObject.SetActive(false);
        TITLECANVAS.gameObject.SetActive(false);
        STORECANVAS.gameObject.SetActive(false);
        GAMECLEARCANVAS.gameObject.SetActive(true);//        
        GAMEOVERCANVAS.gameObject.SetActive(false);
        
    }
    public void GAMEOVERSCENE()
    {
        GENERALCANVAS.gameObject.SetActive(false);
        TITLECANVAS.gameObject.SetActive(false);
        STORECANVAS.gameObject.SetActive(false);
        GAMECLEARCANVAS.gameObject.SetActive(false);
        GAMEOVERCANVAS.gameObject.SetActive(true);//        
        
    }
    public void YOUWINSCENE()
    {
        GENERALCANVAS.gameObject.SetActive(false);
        TITLECANVAS.gameObject.SetActive(false);
        STORECANVAS.gameObject.SetActive(false);
        GAMECLEARCANVAS.gameObject.SetActive(false);
        GAMEOVERCANVAS.gameObject.SetActive(false);
        
    }
    public void YOULOSESCENE()
    {
        GENERALCANVAS.gameObject.SetActive(false);
        TITLECANVAS.gameObject.SetActive(false);
        STORECANVAS.gameObject.SetActive(false);
        GAMECLEARCANVAS.gameObject.SetActive(false);
        GAMEOVERCANVAS.gameObject.SetActive(false);
        
    }

    #endregion

    

    //
    // GET INFO
    #region GET INFO
    public void GETPLAYERINFO(string Lv, string Gold, string Potion, string Attack, string ID) // 플레이어와 충돌해서 얻은 정보 출력
    {
        //PlayerInfo = GENERALCANVAS.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        //PlayerInfo.text = GameManager.INSTANCE.GetPlayer().GETMYINFO().ToString();
        Player_Lv.text = Lv.ToString();
        Player_Gold.text = Gold.ToString();
        Player_Potion.text = Potion.ToString();
        Player_Attack.text = Attack.ToString();
        Player_ID.text = ID.ToString();
    }
    public void GETMAPINFO(MAPINFO mapinfo) // 플레이어와 충돌해서 얻은 정보 출력
    {
        //MapInfo = GENERALCANVAS.gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        MapInfo.text = "MAPINFO " + mapinfo.ToString();
    }
    public void GETMOBINFO(GameObject mobinfo) // 플레이어와 충돌해서 얻은 정보 출력
    {   
        // 체력 업데이트 안됨     
        MonsterInfo.text = "LV " + mobinfo.GetComponent<Monster>().MOBINFO.ToString() + "\nHP " + mobinfo.GetComponent<Monster>().HP.ToString();         
    }
    #endregion

    //
    // Button UI
    #region Button UI
    public void BUYPOTION()
    {
        if (Player.INSTANCE.Unavailable == true) // 골드가 없을 때 물약구매 막기
        {
            Player.INSTANCE.SetPotion(0);
            Player.INSTANCE.SetGold(0);
        }
        else if (Player.INSTANCE.Unavailable == false)
        {
            Player.INSTANCE.SetPotion(1);
            Player.INSTANCE.SetGold(10);
        }

        Player_Gold.text = "GOLD " + Player.INSTANCE.GetGold().ToString();
        Player_Potion.text = "POTION " + Player.INSTANCE.GetPotion().ToString();
    }


    #endregion

    //------------------------------------------------------------------

    // 이름 입력 상자
    #region 이름 입력 상자

    public bool InputNameResult { get; private set; } = false;

    public void ShowInputName(bool show = true)
    {
        objInputName.SetActive(show);
    }

    public void OnClick_Confirm()
    {
        // 이름은 반드시 입력하자
        if (string.IsNullOrEmpty(inputName.text)) return;

        InputNameResult = true;
        PlayerPrefs.SetString("myname", inputName.text);
        SetPlayerName(inputName.text);
    }

    public void SetPlayerName(string name)
    {
        Player_ID.text = name; // 화면 UI에 표시하자
    }
    #endregion // 이름 입력 상자

    //------------------------------------------------------------------



}
