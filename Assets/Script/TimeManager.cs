using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    //const int CurTime = 0;
    //const int MaxTime = 86400;


    // 시, 분, 초 
    int H = 0;
    int M = 0;
    int S = 0;

    public string zero = "0";
    public string h = "";
    public string m = "";
    public string s = "";

    // 플레이 타임 표시 또는 실제 시간 표시    
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
        // 시 -> 00~11 까지 표시 / 12가 될때마다 0으로 초기화
        // 11:59:59 에서 1초 추가 될 경우 00:00:00 으로 초기화
        // 한자리수일때 앞에 0 붙여서 출력
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

        // 분 -> 00~59 까지 표시 / 60이 될때마다 0으로 초기화 -> 시 1 추가
        if (M < 10) 
        {
            m = zero + M.ToString();
            if (M >= 60)
            {
                M = 0;
                H += 1;
            }
        }

        // 초 -> 00~59 까지 표시 / 60이 될때마다 0으로 초기화 -> 분 1 추가
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
