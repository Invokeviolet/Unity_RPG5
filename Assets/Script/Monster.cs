using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{

    public bool IsDead { get; set; }

    MOBINFO mobInfo;
    // ��ǥ �޾ƿ���
    public Vector3 pos { get; set; }
    // ���� �޾ƿ���
    public Color prefabCol { get; set; }  
    // ���� �޾ƿ���
    public string monster { get; set; }
    // ü�� �޾ƿ���
    public int HP { get; set; }
    // ����ġ �޾ƿ���
    public int EXP { get; set; }

    // �� ���� 
    public MOBINFO MOBINFO { get { return mobInfo; } set { mobInfo = value; } }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            AttackPlayer();
        }
    }
    // ���� ���� ���� : �÷��̾ �����ϸ� 1���� ���� ������ ��ŭ ����
    public void AttackPlayer()
    {
        int rndDamage = Random.Range(0, 10);
        GameManager.INSTANCE.GetPlayer().SetHP(rndDamage);
    }

    /*IEnumerator AttackPlayer()
    {
        int rndDamage = Random.Range(0, 10);
        GameManager.INSTANCE.GetPlayer().SetHP(rndDamage);
        yield return new WaitForSeconds(1f);
    }*/

    // ���ݹ����� ���� ��������ŭ HP ����
    public void HitMonster(int damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            IMDEAD();
        }
    }
    public void IMDEAD()
    {
        IsDead = true;
        Destroy(this.gameObject);
        UIManager.INSTANCE.Check4WhoIsWin(false);
        // 2�� �� ���� ��Ҵٴ� â ����
        Invoke("UIManager.INSTANCE.RESULTSCENE()", 2f);

    }

    // ���� ����
    // �÷��̾� ���� ���� �ȿ� �ִ� ��� ���Ϳ��� ���� ����

    // ����
    public void Init(Vector3 pos, MOBINFO info)
    {
        // ť�긶�� ������ ���� ���
        this.pos = pos;
        this.mobInfo = info;

        // �� ������Ʈ�� Meterial�� �����ͼ� ���� ����
        Material myM = GetComponent<MeshRenderer>().material;

        if (info == MOBINFO.EASY) //0
        {
            // this.gameObject.tag = "Easy";
            prefabCol = Color.cyan;            
            HP = 50;
            EXP = 50;
        }
        if (info == MOBINFO.NORMAL) //1
        {
            // this.gameObject.tag = "Normal";
            prefabCol = Color.blue;            
            HP = 100;
            EXP = 100;
        }
        if (info == MOBINFO.HARD) //2
        {
            // this.gameObject.tag = "Hard";
            prefabCol = Color.magenta;            
            HP = 180;
            EXP = 180;
        }
        if (info == MOBINFO.BOSS) //3
        {
            // this.gameObject.tag = "Boss";
            prefabCol = Color.black;            
            HP = 300;
            EXP = 300;
        }

        myM.color = prefabCol;
    }
}
