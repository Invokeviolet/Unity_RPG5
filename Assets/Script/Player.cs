using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.U2D.Path.GUIFramework;


public class Player : MonoBehaviour
{
    // 싱글톤
    #region 싱글톤

    private static Player Instance = null;
    public static Player INSTANCE
    {
        get
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<Player>();
                if (Instance == null)
                {
                    Instance = new GameObject("Player").AddComponent<Player>();
                }
            }
            DontDestroyOnLoad(Instance.gameObject);
            return Instance;
        }
    }
    #endregion

    ChangeSceneManager SCENEMANAGER;
    Monster monster;
    Transform PlayerPos;
    Cube cube;
    Vector3 pos;
    float PlayerPosX;
    float PlayerPosZ;

    public bool IsDead { get; set; }


    // 플레이어 정보 
    private int PLAYERLEVEL = 1;
    private int PLAYERCURHP = 0;
    private int PLAYERMAXHP = 100;
    private int PLAYERCUREXP = 0;
    private int PLAYERMAXEXP = 1000;
    public int PLAYERATTACKPOWER = 1;
    private int PLAYERATTACKRANGE = 1;
    private int POTION = 0;
    private int GOLD = 0;
    public float CURSTAMINAR = 0;
    public float MAXSTAMINAR = 0.5f;
    
    public bool Shop4BuyAvailable = true;

    private void Awake()
    {
        monster = FindObjectOfType<Monster>();
        PlayerPos = GetComponent<Transform>();
        PlayerPos.transform.position = new Vector3(0, 0.5f, 0);

        PlayerPosX = PlayerPos.transform.position.x;
        PlayerPosZ = PlayerPos.transform.position.z;

        SCENEMANAGER = new ChangeSceneManager();

        PLAYERLEVEL = 1;
        PLAYERCURHP = 1/PLAYERMAXHP;
        PLAYERATTACKPOWER = 1;
        PLAYERATTACKRANGE = 1;
        PLAYERCUREXP = 0;
        POTION = 1;
        GOLD = 100;
    }

    //
    // Player Info Property
    #region Player Info Property

    // 레벨
    public int GetLevel()
    {
        return PLAYERLEVEL;
    }
    public void SetLevel(int level)
    {
        this.PLAYERLEVEL = level;
    }
    // 체력
    public int GetHP()
    {
        return PLAYERCURHP;
    }
    public void SetHP(int hp)
    {
        this.PLAYERCURHP = hp;
    }
    // 경험치
    public int GetEXP()
    {
        return PLAYERCUREXP;
    }
    public void SetEXP(int exp)
    {
        this.PLAYERCUREXP = exp;
    }
    // 공격력
    public int GetAttackPower()
    {
        return PLAYERATTACKPOWER;
    }
    public void SetAttackPower(int attackpower)
    {
        this.PLAYERATTACKPOWER = attackpower;
    }
    // 공격범위
    public int SetAttackRange()
    {
        return PLAYERATTACKRANGE;
    }
    public void SetAttackRange(int attackrange)
    {
        this.PLAYERATTACKRANGE = attackrange;
    }
    // 물약
    public int GetPotion()
    {
        return POTION;
    }
    public void SetPotion(int potion)
    {
        this.POTION += potion;        
    }
    // 골드
    public int GetGold()
    {
        if (GOLD <= 0)
        {
            GOLD = 0;
            Shop4BuyAvailable = false;
        }
        return GOLD;
    }
    public void SetGold(int gold)
    {
        this.GOLD -= gold;
               
    }
    #endregion


    public string GETMAPINFO()
    {
        string mapInfo = "MAP . " + cube.name + "\n";
        return mapInfo;
    }

    bool isBattle = false;

    string MyLV = "";
    string MyID = "";
    string MyGold = "";
    string MyPotion = "";
    string MyAttack = "";
    // 슬라이더로 변경 필요 
    string MyHP = "";
    string MyEXP = "";

    private void Start()
    {
        //StartCoroutine(TITLESTATE());
        
        // 입력받은 닉네임 -> ID 정보로 넘기기
        // 타이틀 씬에서 시작 버튼 누르면 -> 로그인 창 띄우기
        // 로그인 하면 그 닉네임대로 게임 실행
    }

    private void Update()
    {
        MyLV = "LV ." + PLAYERLEVEL.ToString() ;
        // MyID = ;
        MyGold = GOLD.ToString();
        MyPotion = POTION.ToString();
        MyAttack = PLAYERATTACKPOWER.ToString() + " / " + PLAYERATTACKRANGE.ToString();
        // 슬라이더로 변경 필요 
        MyHP = PLAYERCURHP.ToString() + " / " + PLAYERMAXHP.ToString();
        MyEXP = PLAYERCUREXP.ToString() + PLAYERMAXEXP.ToString();

        // 인게임 씬에서만 이동이 가능함.
        if (GameManager.INSTANCE.myPlayerInGame == true)
        {
            PlayerMove();           
        }
        else 
        {
            PlayerAction();            
        }
        PlayerCharge();
        UpdatePlayerInfo();

    }

    private void UpdatePlayerInfo()
    {
        // 플레이어 정보 업데이트 시점 - 전투씬 전,후
        UIManager.INSTANCE.GETPLAYERINFO(MyLV, MyGold, MyPotion, MyAttack);
    }



    //
    // 플레이어 경험치 및 레벨업
    #region 플레이어 경험치 및 레벨업
    public void ExpUpdate(int exp) 
    {
        UIManager.INSTANCE.Player_Exp.value += exp;
        if (PLAYERCUREXP >= PLAYERMAXEXP) 
        {
            // 경험치가 최대일 때 레벨업
            PLAYERLEVEL++;
        }
    }
    #endregion
    //


    //
    // 플레이어 이동
    #region 플레이어 이동
    void PlayerMove()
    {
        // WASD 키로 이동
        if (Input.GetKeyDown((KeyCode.W))) // 앞
        {
            PlayerPos.transform.position += Vector3.forward;
            if (PlayerPos.transform.position.z >= 9)
            {
                PlayerPos.transform.position = new Vector3(PlayerPos.transform.position.x, 0.5f, 9);
            }
        }
        else if (Input.GetKeyDown((KeyCode.A))) // 왼 
        {
            PlayerPos.transform.position += Vector3.left;
            if (PlayerPos.transform.position.x <= 0)
            {
                PlayerPos.transform.position = new Vector3(0, 0.5f, PlayerPos.transform.position.z);
            }
        }
        else if (Input.GetKeyDown((KeyCode.S))) // 뒤
        {
            PlayerPos.transform.position += Vector3.back;
            if (PlayerPos.transform.position.z <= 0)
            {
                PlayerPos.transform.position = new Vector3(PlayerPos.transform.position.x, 0.5f, 0);
            }
        }
        else if (Input.GetKeyDown((KeyCode.D))) // 오른
        {
            PlayerPos.transform.position += Vector3.right;
            if (PlayerPos.transform.position.x >= 9)
            {
                PlayerPos.transform.position = new Vector3(9, 0.5f, PlayerPos.transform.position.z);
            }
        }

    }
    #endregion
    //



    //
    // 플레이어 공격
    #region 플레이어 공격
    void PlayerAction()
    {
        if (Input.GetKeyDown((KeyCode.Space))) // 공격
        {
            GameManager.INSTANCE.GetMonster().HitMonster(10);
            UIManager.INSTANCE.StaminarCheck();
            // 스태미너로 공격 횟수에 제한을 둘 필요가 있음
            //attackCount++;
        }        
    }
    #endregion
    //


    // 플레이어 체력
    #region
    void PlayerCharge() 
    {
        int curPotion = POTION;
        if (Input.GetKeyDown((KeyCode.I)))
        {
            POTION -= 1;
            PLAYERCURHP += 10;

            if (POTION <= 0)
            {
                POTION = 0;            
            }
            if (PLAYERCURHP>=PLAYERMAXHP) // HP가 이미 꽉차있을 때 물약 사용 막기
            {
                PLAYERCURHP = PLAYERMAXHP;                
                POTION = curPotion;
            }
        }
    }
    #endregion



    //
    // 플레이어 위치 정보 출력
    #region 플레이어 위치 정보 출력

    private void OnCollisionEnter(Collision collision)
    {
        // 맵에 있는 맵큐브랑 충돌하면 맵 정보 출력
        if (collision.gameObject.TryGetComponent<Cube>(out cube) == true) // (collision.collider.CompareTag("Cube"))
        {
            Debug.Log(cube.MAPINFO);
            UIManager.INSTANCE.GETMAPINFO(cube.MAPINFO); // UI에 맵 정보 출력
        }
        if (collision.collider.CompareTag("Store"))
        {
            // 상점으로 이동할건지 물어보는 창 띄우기
            // OK 누르면 상점 으로 이동
            UIManager.INSTANCE.QUESTION();

            // 골드, 물약 정보 출력
            // UIManager.INSTANCE.BUYPOTION(POTION, GOLD);
        }
        if (collision.collider.CompareTag("End"))
        {
            SCENEMANAGER.NPCSCENE();            
        }
        
    }

    private void OnTriggerEnter(Collider collider)
    {        
        if (collider.gameObject.TryGetComponent<Monster>(out monster) == true) //(collision.collider.CompareTag("Monster"))
        {
            // Debug.Log(monster.MOBINFO);
            // 충돌한 몬스터 객체의 난이도에 따라 몬스터의 프리팹을 다르게 생성
            // 충돌한 맵에 따라 넘어가는 씬을 다르게 생성            
            SCENEMANAGER.ACTIONFORESTSCENE();
            
            UIManager.INSTANCE.GETMOBINFO(collider.gameObject); // UI에 몬스터 정보 출력
        }
        
    }

    #endregion
    //


    //
    // 플레이어 타격 및 죽음
    #region 플레이어 타격 및 죽음
    public void HitPlayer(int damage)
    {
        PLAYERCURHP -= damage;
        if (PLAYERCURHP <= 0)
        {
            IMDEAD();
        }
    }
    public void IMDEAD()
    {
        IsDead = true;
        UIManager.INSTANCE.Check4WhoIsWin(true);
        // 1 초 뒤 플레이어 졌다는 창 띄우기
        Invoke("UIManager.INSTANCE.RESULTSCENE()", 1f);

    }
    #endregion

    // 공격 받으면 HP 감소, 물약 아이템 사용시 HP 증가

}
