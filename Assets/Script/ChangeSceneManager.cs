using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeSceneManager : MonoBehaviour
{
    // �̱���
    #region �̱���

    private static ChangeSceneManager Instance = null;
    public static ChangeSceneManager INSTANCE
    {
        get
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<ChangeSceneManager>();
            }
            return Instance;
        }
    }
    #endregion



    public bool Title { get; set; } = false;
    public bool Ingame { get; set; } = false;
    public bool Action { get; set; } = false;
    public bool Store { get; set; } = false;

    public void Start()
    {
        //GameObject.Find("START").GetComponentInChildren<Text>().text = "START_text";        
        TITLESCENE();
    }

    public void TITLESCENE() // Ÿ��Ʋ
    {
        SceneManager.LoadScene("00_TITLE_Scene");
        UIManager.INSTANCE.TITLESCENE();
        Title = true;
        Ingame = false;
        Action = false;
        Store = false;
    }

    public void INGAMESCENE() // �ΰ���
    {
        SceneManager.LoadScene("01_INGAME_Scene");
        UIManager.INSTANCE.GENERALSCENE();
        Title = false;
        Ingame = true;
        Action = false;
        Store = false;
    }

    public void STORESCENE() // ����
    {
        SceneManager.LoadScene("02_STORE_Scene");
        UIManager.INSTANCE.STORESCENE();
        Title = false;
        Ingame = false;
        Action = false;
        Store = true;
    }

    public void ACTIONSCENE() // ����
    {
        SceneManager.LoadScene("03_ACTION_Scene");        
        UIManager.INSTANCE.ACTIONSCENE();
        Title = false;
        Ingame = false;
        Action = true;
        Store = false;
    }

    public void BOSSSCENE() // ����
    {
        SceneManager.LoadScene("04_BOSS_Scene");
        UIManager.INSTANCE.GENERALSCENE();
        Title = false;
        Ingame = false;
        Action = true;
        Store = false;
    }

    static public void Quit() // ���� ����
    {
        Application.Quit();
    }

}
