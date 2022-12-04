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
    Transform PlayerPos;
    Cube cube;
    Vector3 pos;
    float PlayerPosX;
    float PlayerPosZ;

    public bool IsDead { get; set; }


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
        
        // �Է¹��� �г��� -> ID ������ �ѱ��
        // Ÿ��Ʋ ������ ���� ��ư ������ -> �α��� â ����
        // �α��� �ϸ� �� �г��Ӵ�� ���� ����
    }

    private void Update()
    {
        MyLV = "LV ." + PLAYERLEVEL.ToString() ;
        // MyID = ;
        MyGold = GOLD.ToString();
        MyPotion = POTION.ToString();
        MyAttack = PLAYERATTACKPOWER.ToString() + " / " + PLAYERATTACKRANGE.ToString();
        // �����̴��� ���� �ʿ� 
        MyHP = PLAYERCURHP.ToString() + " / " + PLAYERMAXHP.ToString();
        MyEXP = PLAYERCUREXP.ToString() + PLAYERMAXEXP.ToString();

        // �ΰ��� �������� �̵��� ������.
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
        // �÷��̾� ���� ������Ʈ ���� - ������ ��,��
        UIManager.INSTANCE.GETPLAYERINFO(MyLV, MyGold, MyPotion, MyAttack);
    }



    //
    // �÷��̾� ����ġ �� ������
    #region �÷��̾� ����ġ �� ������
    public void ExpUpdate(int exp) 
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
    // �÷��̾� �̵�
    #region �÷��̾� �̵�
    void PlayerMove()
    {
        // WASD Ű�� �̵�
        if (Input.GetKeyDown((KeyCode.W))) // ��
        {
            PlayerPos.transform.position += Vector3.forward;
            if (PlayerPos.transform.position.z >= 9)
            {
                PlayerPos.transform.position = new Vector3(PlayerPos.transform.position.x, 0.5f, 9);
            }
        }
        else if (Input.GetKeyDown((KeyCode.A))) // �� 
        {
            PlayerPos.transform.position += Vector3.left;
            if (PlayerPos.transform.position.x <= 0)
            {
                PlayerPos.transform.position = new Vector3(0, 0.5f, PlayerPos.transform.position.z);
            }
        }
        else if (Input.GetKeyDown((KeyCode.S))) // ��
        {
            PlayerPos.transform.position += Vector3.back;
            if (PlayerPos.transform.position.z <= 0)
            {
                PlayerPos.transform.position = new Vector3(PlayerPos.transform.position.x, 0.5f, 0);
            }
        }
        else if (Input.GetKeyDown((KeyCode.D))) // ����
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
            if (PLAYERCURHP>=PLAYERMAXHP) // HP�� �̹� �������� �� ���� ��� ����
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
            UIManager.INSTANCE.QUESTION();

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
