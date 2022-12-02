using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    //const int CurTime = 0;
    //const int MaxTime = 86400;


    // ��, ��, �� 
    int H = 0;
    int M = 0;
    int S = 0;

    public string zero = "0";
    public string h = "";
    public string m = "";
    public string s = "";

    // �÷��� Ÿ�� ǥ�� �Ǵ� ���� �ð� ǥ��    
    float time = 0;

    private void Awake()
    {        
        h = H.ToString();
        m = M.ToString();
        s = S.ToString();
        //h = zero + H.ToString();
        //m = zero + M.ToString();
        //s = zero + S.ToString();
    }

    private void Start()
    {

    }

    private void Update()
    {
        time = Time.deltaTime;
        //h = (H * Time.deltaTime).ToString();
        //m = (M * Time.deltaTime).ToString();
        s = (S * time).ToString();
    }

    void GameTimer()
    {
        UIManager.INSTANCE.TimeText.text = h + ":" + m + ":" + s;
    }

    private void StartTime()
    {
        // �� -> 00~11 ���� ǥ�� / 12�� �ɶ����� 0���� �ʱ�ȭ
        // 11:59:59 ���� 1�� �߰� �� ��� 00:00:00 ���� �ʱ�ȭ
        // ���ڸ����϶� �տ� 0 �ٿ��� ���
        if (H < 10) 
        {
            h = zero + H.ToString();
            if (H >= 12)
            {
                H = 0;
                M = 0;
                S = 0;
            }
        }

        // �� -> 00~59 ���� ǥ�� / 60�� �ɶ����� 0���� �ʱ�ȭ -> �� 1 �߰�
        if (M < 10) 
        {
            m = zero + M.ToString();
            if (M >= 60)
            {
                M = 0;
                H += 1;
            }
        }

        // �� -> 00~59 ���� ǥ�� / 60�� �ɶ����� 0���� �ʱ�ȭ -> �� 1 �߰�
        if (S < 10) 
        {
            s = zero + S.ToString();
            if (S >= 60)
            {
                S = 0;
                M += 1;
            }
        }
        
    }
}
