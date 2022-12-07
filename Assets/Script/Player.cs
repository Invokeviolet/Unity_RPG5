using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.U2D.Path.GUIFramework;


public class Player : MonoBehaviour
{
    // �̱���
    #region �̱���

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

    // �÷��̾� �̵� ���ǵ�
    public float PlayerMoveSpeed;
    // �÷��̾� ������ٵ�
    private Rigidbody PlayerRB;
    // �÷��̾� ��ġ �޾ƿ���
    private Transform SaveMyPos { get; set; }

    // �÷��̾� ���� 
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

    // ����
    public int GetLevel()
    {
        return PLAYERLEVEL;
    }
    public void SetLevel(int level)
    {
        this.PLAYERLEVEL = level;
    }
    // ü��
    public int GetHP()
    {
        return PLAYERCURHP;
    }
    public void SetHP(int hp)
    {
        this.PLAYERCURHP = hp;
    }
    // ����ġ
    public int GetEXP()
    {
        return PLAYERCUREXP;
    }
    public void SetEXP(int exp)
    {
        this.PLAYERCUREXP = exp;
    }
    // ���ݷ�
    public int GetAttackPower()
    {
        return PLAYERATTACKPOWER;
    }
    public void SetAttackPower(int attackpower)
    {
        this.PLAYERATTACKPOWER = attackpower;
    }
    // ���ݹ���
    public int SetAttackRange()
    {
        return PLAYERATTACKRANGE;
    }
    public void SetAttackRange(int attackrange)
    {
        this.PLAYERATTACKRANGE = attackrange;
    }
    // ����
    public int GetPotion()
    {
        return POTION;
    }
    public void SetPotion(int potion)
    {
        this.POTION += potion;
    }
    // ���
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
    // �����̴��� ���� �ʿ� 
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
        // �����̴��� ���� �ʿ� 
        MyHP = PLAYERCURHP.ToString() + " / " + PLAYERMAXHP.ToString();
        MyEXP = PLAYERCUREXP.ToString() + PLAYERMAXEXP.ToString();

        // �ΰ��� �������� �̵��� ������. / � �˸�â�� ���� �ʾ��� ��
        if ((GameManager.INSTANCE.myPlayerInGame == true) && (GameManager.INSTANCE.IsWindowOpen == false))
        {
            PlayerMoveToIngame();
        }
        else if ((GameManager.INSTANCE.myPlayerAction == true) && (GameManager.INSTANCE.IsWindowOpen == false))
        {
            PlayerMoveToAction();
            // �ΰ��Ӿ� �÷��̾ �̵��Ǵ� ����
            PlayerAction();
        }
        PlayerCharge();
        UpdatePlayerInfo();

    }

    private void UpdatePlayerInfo() // ���߿� �Ⱦ��� ����
    {
        // �÷��̾� ���� ������Ʈ ���� - ������ ��,��
        UIManager.INSTANCE.GETPLAYERINFO(MyLV, MyAttack);
    }



    //
    // �÷��̾� ����ġ �� ������
    #region �÷��̾� ����ġ �� ������
    public void ExpUpdate(int exp) // ���� ��� ������ ȣ��
    {
        UIManager.INSTANCE.Player_Exp.value += exp;
        if (PLAYERCUREXP >= PLAYERMAXEXP)
        {
            // ����ġ�� �ִ��� �� ������
            PLAYERLEVEL++;
        }
    }
    #endregion
    //


    //
    // �÷��̾� �ΰ��� �̵�
    #region �÷��̾� �ΰ��� �̵�
    void PlayerMoveToIngame()
    {
        // WASD Ű�� �̵�
        if (Input.GetKeyDown((KeyCode.T))) // ��
        {
            IngamePlayerPos.transform.position += Vector3.forward;
            if (IngamePlayerPos.transform.position.z >= 9)
            {
                IngamePlayerPos.transform.position = new Vector3(IngamePlayerPos.transform.position.x, 0.5f, 9);
            }
        }
        else if (Input.GetKeyDown((KeyCode.F))) // �� 
        {
            IngamePlayerPos.transform.position += Vector3.left;
            if (IngamePlayerPos.transform.position.x <= 0)
            {
                IngamePlayerPos.transform.position = new Vector3(0, 0.5f, IngamePlayerPos.transform.position.z);
            }
        }
        else if (Input.GetKeyDown((KeyCode.G))) // ��
        {
            IngamePlayerPos.transform.position += Vector3.back;
            if (IngamePlayerPos.transform.position.z <= 0)
            {
                IngamePlayerPos.transform.position = new Vector3(IngamePlayerPos.transform.position.x, 0.5f, 0);
            }
        }
        else if (Input.GetKeyDown((KeyCode.H))) // ����
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
    // �÷��̾� ������ �̵�
    #region �÷��̾� ������ �̵�
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

    // �÷��̾� ������ �̵��� �� ���� ����
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
    // �÷��̾� ����
    #region �÷��̾� ����
    void PlayerAction()
    {
        if (Input.GetKeyDown((KeyCode.Space))) // ����
        {
            GameManager.INSTANCE.GetMonster().HitMonster(10);
            UIManager.INSTANCE.StaminarCheck();
            // ���¹̳ʷ� ���� Ƚ���� ������ �� �ʿ䰡 ����
            //attackCount++;
        }
    }
    #endregion
    //


    // �÷��̾� ü��
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
            if (PLAYERCURHP >= PLAYERMAXHP) // HP�� �̹� �������� �� ���� ��� ����
            {
                PLAYERCURHP = PLAYERMAXHP;
                POTION = curPotion;
            }
        }
    }
    #endregion



    //
    // �÷��̾� ��ġ ���� ���
    #region �÷��̾� ��ġ ���� ���

    private void OnCollisionEnter(Collision collision)
    {
        // �ʿ� �ִ� ��ť��� �浹�ϸ� �� ���� ���
        if (collision.gameObject.TryGetComponent<Cube>(out cube) == true) // (collision.collider.CompareTag("Cube"))
        {
            Debug.Log(cube.MAPINFO);
            UIManager.INSTANCE.GETMAPINFO(cube.MAPINFO); // UI�� �� ���� ���
        }
        if (collision.collider.CompareTag("Store"))
        {
            // �������� �̵��Ұ��� ����� â ����
            // OK ������ ���� ���� �̵�
            UIManager.INSTANCE.ONQUESTION();

            // ���, ���� ���� ���
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
            // �浹�� ���� ��ü�� ���̵��� ���� ������ �������� �ٸ��� ����
            // �浹�� �ʿ� ���� �Ѿ�� ���� �ٸ��� ����            
            SCENEMANAGER.ACTIONFORESTSCENE();

            UIManager.INSTANCE.GETMOBINFO(collider.gameObject); // UI�� ���� ���� ���
        }

    }

    #endregion
    //


    //
    // �÷��̾� Ÿ�� �� ����
    #region �÷��̾� Ÿ�� �� ����
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
        // 1 �� �� �÷��̾� ���ٴ� â ����
        Invoke("UIManager.INSTANCE.RESULTSCENE()", 1f);

    }
    #endregion

    // ���� ������ HP ����, ���� ������ ���� HP ����

}
