using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeSceneManager : MonoBehaviour
{
    // ΩÃ±€≈Ê
    #region ΩÃ±€≈Ê

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
    public bool Npc { get; set; } = false;

    public void Start()
    {
        //GameObject.Find("START").GetComponentInChildren<Text>().text = "START_text";        
        TITLESCENE();
    }

    public void TITLESCENE() // ≈∏¿Ã∆≤
    {
        SceneManager.LoadScene("00_TITLE_Scene");
        UIManager.INSTANCE.TITLESCENE();
        GameManager.INSTANCE.myPlayerInGame = false;
        Title = true;
        Ingame = false;
        Action = false;
        Store = false;
        Npc = false;
    }

    public void INGAMESCENE() // ¿Œ∞‘¿”
    {
        SceneManager.LoadScene("01_INGAME_Scene");
        UIManager.INSTANCE.GENERALSCENE();
        UIManager.INSTANCE.OFFQUESTION();
        GameManager.INSTANCE.myPlayerInGame = true;
        GameManager.INSTANCE.myPlayerAction = false;
        Title = false;
        Ingame = true;
        Action = false;
        Store = false; 
        Npc = false;
    }

    public void STORESCENE() // ªÛ¡°
    {
        SceneManager.LoadScene("02_STORE_Scene");
        UIManager.INSTANCE.STORESCENE();
        UIManager.INSTANCE.OFFQUESTION();
        GameManager.INSTANCE.myPlayerInGame = false;
        GameManager.INSTANCE.myPlayerAction = false;
        Title = false;
        Ingame = false;
        Action = false;
        Store = true;
        Npc = false;
    }


    public void ACTIONFORESTSCENE() // ¿¸≈ı - Ω£
    {
        SceneManager.LoadScene("03_ACTION_FOREST_Scene");        
        UIManager.INSTANCE.ACTIONSCENE();
        UIManager.INSTANCE.OFFQUESTION();
        GameManager.INSTANCE.myPlayerInGame = false;
        GameManager.INSTANCE.myPlayerAction = true;

        Player.INSTANCE.ActionPlayerPos.transform.position = new Vector3(2000, 1f, 0);

        Title = false;
        Ingame = false;
        Action = true;
        Store = false;
        Npc = false;
    }
    public void ACTIONSWAMPSCENE() // ¿¸≈ı - ¥À
    {
        SceneManager.LoadScene("04_ACTION_SWAMP_Scene");
        UIManager.INSTANCE.ACTIONSCENE();
        UIManager.INSTANCE.OFFQUESTION();
        GameManager.INSTANCE.myPlayerInGame = false;
        GameManager.INSTANCE.myPlayerAction = true;

        Player.INSTANCE.ActionPlayerPos.transform.position = new Vector3(2000, 1f, 0); 

        Title = false;
        Ingame = false;
        Action = true;
        Store = false;
        Npc = false;
    }
    public void ACTIONGROUNDSCENE() // ¿¸≈ı - ∂•
    {
        SceneManager.LoadScene("05_ACTION_GROUND_Scene");
        UIManager.INSTANCE.ACTIONSCENE();
        UIManager.INSTANCE.OFFQUESTION();
        GameManager.INSTANCE.myPlayerInGame = false;
        GameManager.INSTANCE.myPlayerAction = true;

        Player.INSTANCE.ActionPlayerPos.transform.position = new Vector3(2000, 1f, 0);

        Title = false;
        Ingame = false;
        Action = true;
        Store = false;
        Npc = false;
    }

    public void ACTIONBOSSSCENE() // ¿¸≈ı - ∫∏Ω∫
    {
        SceneManager.LoadScene("06_ACTION_BOSS_Scene");
        UIManager.INSTANCE.GENERALSCENE();
        UIManager.INSTANCE.OFFQUESTION();
        GameManager.INSTANCE.myPlayerInGame = false;
        GameManager.INSTANCE.myPlayerAction = true;

        Player.INSTANCE.ActionPlayerPos.transform.position = new Vector3(2000, 1f, 0);

        Title = false;
        Ingame = false;
        Action = true;
        Store = false;
        Npc = false;
    }
    public void NPCSCENE() 
    {
        SceneManager.LoadScene("07_NPC_Scene");
        UIManager.INSTANCE.GENERALSCENE();
        GameManager.INSTANCE.myPlayerInGame = false;
        GameManager.INSTANCE.myPlayerAction = true;
        Title = false;
        Ingame = false;
        Action = false;
        Store = false;
        Npc = true;
    }
    public void GAMEOVERSECNE() 
    {
        SceneManager.LoadScene("09_GAMEOVER_Scene");
        UIManager.INSTANCE.GENERALSCENE();
        GameManager.INSTANCE.myPlayerInGame = false;
    }
    public void LOADINGSECNE()
    {
        SceneManager.LoadScene("10_LOADING_Scene");
        GameManager.INSTANCE.myPlayerInGame = false;
    }

    static public void Quit() // ∞‘¿” ¡æ∑·
    {
        Application.Quit();
    }

}
