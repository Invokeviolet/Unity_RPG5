using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


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


    // �÷��̾� ���� 
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
        MyLV = "LV" + PLAYERLEVEL.ToString() ;
        // MyID = ;
        MyGold = "Gold " + GOLD.ToString();
        MyPotion = "Potion " + POTION.ToString();
        MyAttack = "Attack Power / Attack Range " + PLAYERATTACKPOWER.ToString();
        // �����̴��� ���� �ʿ� 
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
        // �÷��̾� ���� ������Ʈ ���� - ������ ��,��
        UIManager.INSTANCE.GETPLAYERINFO(myInfo);
    }
   

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

    void PlayerAction()
    {
        if (Input.GetKeyDown((KeyCode.Space))) // ����
        {
            GameManager.INSTANCE.GetMonster().HitMonster(10);
            //attackCount++;
        }
        
    }
    void PlayerCharge() 
    {
        int curPotion = POTION;
        if (Input.GetKeyDown((KeyCode.I))) // ���� ���
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

    // �ΰ��� ������ ���� ���
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
            SCENEMANAGER.STORESCENE(); // ���� ������ �̵�
            // ���, ���� ���� ���
            //UIManager.INSTANCE.BUYPOTION(POTION, GOLD);
        }
        if (collision.collider.CompareTag("End"))
        {
            SCENEMANAGER.BOSSSCENE(); // ���� ���� ������ �̵�
            // UIManager.INSTANCE.GETMOBINFO(); // ������ ���� ���� ��� / ������������ ���� �޾Ƽ� ���� ��ũ��Ʈ ������ �־��ִ� ���
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        // �ʿ� �ִ� ���Ͷ� �浹�ϸ� ���� ���� ���
        if (collider.gameObject.TryGetComponent<Monster>(out monster) == true) //(collision.collider.CompareTag("Monster"))
        {
            Debug.Log(monster.MOBINFO);
            SCENEMANAGER.ACTIONSCENE(); // �׼� ������ �̵�                        
            UIManager.INSTANCE.GETMOBINFO(collider.gameObject); // UI�� ���� ���� ���
        }
        
    }

    // ���� ������ HP ����, ���� ������ ���� HP ����

}
