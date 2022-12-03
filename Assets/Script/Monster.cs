using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{

    public bool IsDead { get; set; }

    MOBINFO mobInfo;
    // 좌표 받아오기
    public Vector3 pos { get; set; }
    // 색상 받아오기
    public Color prefabCol { get; set; }  
    // 몬스터 받아오기
    public string monster { get; set; }
    // 체력 받아오기
    public int HP { get; set; }
    // 경험치 받아오기
    public int EXP { get; set; }

    // 몹 정보 
    public MOBINFO MOBINFO { get { return mobInfo; } set { mobInfo = value; } }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            AttackPlayer();
        }
    }
    // 몬스터 공격 조건 : 플레이어가 공격하면 1초후 랜덤 데미지 만큼 공격
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

    // 공격받으면 들어온 데미지만큼 HP 감소
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
        // 2초 뒤 몬스터 잡았다는 창 띄우기
        Invoke("UIManager.INSTANCE.RESULTSCENE()", 2f);

    }

    // 대전 게임
    // 플레이어 공격 범위 안에 있는 모든 몬스터에게 공격 가능

    // 생성
    public void Init(Vector3 pos, MOBINFO info)
    {
        // 큐브마다 랜덤한 몬스터 출력
        this.pos = pos;
        this.mobInfo = info;

        // 내 컴포넌트의 Meterial을 가져와서 색상 변경
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
