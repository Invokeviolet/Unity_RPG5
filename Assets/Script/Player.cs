using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


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
    bool IsDead { get; set; }


    // 플레이어 정보 
    private int PLAYERLEVEL = 1;
    private int PLAYERCURHP = 0;
    private int PLAYERMAXHP = 100;
    private int PLAYERCUREXP = 0;
    private int PLAYERMAXEXP = 1000;
    private int PLAYERATTACKPOWER = 1;
    private int PLAYERATTACKRANGE = 1;
    private int POTION = 0;
    private int GOLD = 0;
    public bool Unavailable = false;

    private void Awake()
    {
        monster = FindObjectOfType<Monster>();
        PlayerPos = GetComponent<Transform>();
        PlayerPos.transform.position = new Vector3(0, 0.5f, 0);

        PlayerPosX = PlayerPos.transform.position.x;
        PlayerPosZ = PlayerPos.transform.position.z;

        SCENEMANAGER = new ChangeSceneManager();

        PLAYERLEVEL = 1;
        PLAYERCURHP = PLAYERMAXHP;
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
            Unavailable = true;
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
        MyLV = "LV" + PLAYERLEVEL.ToString() ;
        // MyID = ;
        MyGold = "Gold " + GOLD.ToString();
        MyPotion = "Potion " + POTION.ToString();
        MyAttack = "Attack Power / Attack Range " + PLAYERATTACKPOWER.ToString();
        // 슬라이더로 변경 필요 
        MyHP = "HP / MAXHP . " + PLAYERCURHP.ToString() + " / " + PLAYERMAXHP.ToString() + " / " + PLAYERATTACKRANGE.ToString();
        MyEXP = "EXP / MAXEXP " + PLAYERCUREXP.ToString() + PLAYERMAXEXP.ToString();


        if (isBattle == true)
        {
            PlayerAction();
            UpdatePlayerInfo();
        }
        else
        {
            PlayerMove();
            PlayerCharge();
            UpdatePlayerInfo();
        }
    }

    private void UpdatePlayerInfo()
    {
        // 플레이어 정보 업데이트 시점 - 전투씬 전,후
        UIManager.INSTANCE.GETPLAYERINFO(myInfo);
    }
   

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

    void PlayerAction()
    {
        if (Input.GetKeyDown((KeyCode.Space))) // 공격
        {
            GameManager.INSTANCE.GetMonster().HitMonster(10);
            //attackCount++;
        }
        
    }
    void PlayerCharge() 
    {
        int curPotion = POTION;
        if (Input.GetKeyDown((KeyCode.I))) // 물약 사용
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

    // 인게임 씬에서 정보 출력
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
            SCENEMANAGER.STORESCENE(); // 상점 씬으로 이동
            // 골드, 물약 정보 출력
            //UIManager.INSTANCE.BUYPOTION(POTION, GOLD);
        }
        if (collision.collider.CompareTag("End"))
        {
            SCENEMANAGER.BOSSSCENE(); // 보스 전투 씬으로 이동
            // UIManager.INSTANCE.GETMOBINFO(); // 무조건 보스 정보 출력 / 보스프리팹을 따로 받아서 몬스터 스크립트 정보를 넣어주는 방법
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        // 맵에 있는 몬스터랑 충돌하면 몬스터 정보 출력
        if (collider.gameObject.TryGetComponent<Monster>(out monster) == true) //(collision.collider.CompareTag("Monster"))
        {
            Debug.Log(monster.MOBINFO);
            SCENEMANAGER.ACTIONSCENE(); // 액션 씬으로 이동                        
            UIManager.INSTANCE.GETMOBINFO(collider.gameObject); // UI에 몬스터 정보 출력
        }
        
    }

    // 공격 받으면 HP 감소, 물약 아이템 사용시 HP 증가

}
