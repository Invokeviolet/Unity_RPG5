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
    Transform IngamePlayerPos;
    public Transform ActionPlayerPos;
    Cube cube;
    Vector3 pos;
    float PlayerPosX;
    float PlayerPosZ;

    public bool IsDead { get; set; }

    // 플레이어 이동 스피드
    public float PlayerMoveSpeed;
    // 플레이어 리지드바디
    private Rigidbody PlayerRB;
    // 플레이어 위치 받아오기
    private Transform SaveMyPos { get; set; }

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
        IngamePlayerPos = GetComponent<Transform>();
        ActionPlayerPos = GetComponent<Transform>();

        IngamePlayerPos.transform.position = new Vector3(0, 0.5f, 0);
        
        SaveMyPos = IngamePlayerPos;

        PlayerPosX = IngamePlayerPos.transform.position.x;
        PlayerPosZ = IngamePlayerPos.transform.position.z;

        SCENEMANAGER = new ChangeSceneManager();

        PLAYERLEVEL = 1;
        PLAYERCURHP = 1 / PLAYERMAXHP;
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
        PlayerRB = GetComponent<Rigidbody>();
        if (PlayerRB == null)
        {
            this.gameObject.AddComponent<Rigidbody>();
        }
    }

    private void Update()
    {
        MyLV = "LV ." + PLAYERLEVEL.ToString();
        // MyID = ;
        //MyGold = GOLD.ToString();
        //MyPotion = POTION.ToString();
        MyAttack = PLAYERATTACKPOWER.ToString() + " / " + PLAYERATTACKRANGE.ToString();
        // 슬라이더로 변경 필요 
        MyHP = PLAYERCURHP.ToString() + " / " + PLAYERMAXHP.ToString();
        MyEXP = PLAYERCUREXP.ToString() + PLAYERMAXEXP.ToString();

        // 인게임 씬에서만 이동이 가능함. / 어떤 알림창도 뜨지 않았을 때
        if ((GameManager.INSTANCE.myPlayerInGame == true) && (GameManager.INSTANCE.IsWindowOpen == false))
        {
            PlayerMoveToIngame();
        }
        else if ((GameManager.INSTANCE.myPlayerAction == true) && (GameManager.INSTANCE.IsWindowOpen == false))
        {
            PlayerMoveToAction();
            // 인게임씬 플레이어가 이동되는 문제
            PlayerAction();
        }
        PlayerCharge();
        UpdatePlayerInfo();

    }

    private void UpdatePlayerInfo() // 나중에 안쓰면 삭제
    {
        // 플레이어 정보 업데이트 시점 - 전투씬 전,후
        UIManager.INSTANCE.GETPLAYERINFO(MyLV, MyAttack);
    }



    //
    // 플레이어 경험치 및 레벨업
    #region 플레이어 경험치 및 레벨업
    public void ExpUpdate(int exp) // 몬스터 잡는 곳에서 호출
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
    // 플레이어 인게임 이동
    #region 플레이어 인게임 이동
    void PlayerMoveToIngame()
    {
        // WASD 키로 이동
        if (Input.GetKeyDown((KeyCode.T))) // 앞
        {
            IngamePlayerPos.transform.position += Vector3.forward;
            if (IngamePlayerPos.transform.position.z >= 9)
            {
                IngamePlayerPos.transform.position = new Vector3(IngamePlayerPos.transform.position.x, 0.5f, 9);
            }
        }
        else if (Input.GetKeyDown((KeyCode.F))) // 왼 
        {
            IngamePlayerPos.transform.position += Vector3.left;
            if (IngamePlayerPos.transform.position.x <= 0)
            {
                IngamePlayerPos.transform.position = new Vector3(0, 0.5f, IngamePlayerPos.transform.position.z);
            }
        }
        else if (Input.GetKeyDown((KeyCode.G))) // 뒤
        {
            IngamePlayerPos.transform.position += Vector3.back;
            if (IngamePlayerPos.transform.position.z <= 0)
            {
                IngamePlayerPos.transform.position = new Vector3(IngamePlayerPos.transform.position.x, 0.5f, 0);
            }
        }
        else if (Input.GetKeyDown((KeyCode.H))) // 오른
        {
            IngamePlayerPos.transform.position += Vector3.right;
            if (IngamePlayerPos.transform.position.x >= 9)
            {
                IngamePlayerPos.transform.position = new Vector3(9, 0.5f, IngamePlayerPos.transform.position.z);
            }
        }

    }
    #endregion


    //
    //
    // 플레이어 전투씬 이동
    #region 플레이어 전투씬 이동
    void PlayerMoveToAction()
    {
        
        PlayerRB = this.gameObject.GetComponent<Rigidbody>();
        //float hAxis = Input.GetAxisRaw("Horizontal");
        //float vAxis = Input.GetAxisRaw("Vertical");

        Vector3 InputMoveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        PlayerRB.velocity = InputMoveDir * PlayerMoveSpeed;

        transform.LookAt(transform.position + InputMoveDir);
        transform.Translate(InputMoveDir);

        // LimitingPlayerMovement();
    }

    // 플레이어 전투씬 이동중 맵 범위 제한
    private void LimitingPlayerMovement()
    {
        if ((IngamePlayerPos.transform.position.z <= -24) && (IngamePlayerPos.transform.position.x <= -24))
        {
            IngamePlayerPos.transform.position = new Vector3(-24, IngamePlayerPos.transform.position.y, -24);
        }
        if ((IngamePlayerPos.transform.position.z >= -24) && (IngamePlayerPos.transform.position.x <= 24))
        {
            IngamePlayerPos.transform.position = new Vector3(-24, IngamePlayerPos.transform.position.y, 24);
        }
        if ((IngamePlayerPos.transform.position.z <= 24) && (IngamePlayerPos.transform.position.x >= -24))
        {
            IngamePlayerPos.transform.position = new Vector3(24, IngamePlayerPos.transform.position.y, -24);
        }
        if ((IngamePlayerPos.transform.position.z <= 24) && (IngamePlayerPos.transform.position.x <= 24))
        {
            IngamePlayerPos.transform.position = new Vector3(24, IngamePlayerPos.transform.position.y, 24);
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
            if (PLAYERCURHP >= PLAYERMAXHP) // HP가 이미 꽉차있을 때 물약 사용 막기
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
            UIManager.INSTANCE.ONQUESTION();

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
