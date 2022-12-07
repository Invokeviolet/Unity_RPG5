using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner_ACTION : MonoBehaviour
{
    // 싱글톤
    #region 싱글톤

    private static MapSpawner_ACTION Instance = null;
    public static MapSpawner_ACTION INSTANCE
    {
        get
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<MapSpawner_ACTION>();
            }

            return Instance;
        }
    }
    #endregion

    // 맵 지면 생성
    [SerializeField] GameObject MapCubePrefab;

    [SerializeField] Player ActionPlayerPrefab;
    // 몬스터 프리팹 받아오기
    [SerializeField] Monster MonsterParentPrefab;
    [SerializeField] Monster MonsterPrefab;

    GameObject MAPCUBE = null;
    Player ActionPlayer;
    Monster ActionMobParents = null;
    Monster ActionMob;

    // 몬스터 수
    private int CurActionMobCount = 0;
    private int MaxActionMobCount = 10;


    // 일단 통으로 된 맵 큐브를 생성
    // 추후에 액션 맵 큐브 스크립트를 따로 만들어서 함정을 파거나,

    Vector3 vec3;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        vec3 = new Vector3(-2000,0,0);
    }
    void Start()
    {
        MAPCUBE = Instantiate(MapCubePrefab, transform.position, Quaternion.identity,transform);
        ActionPlayer = Instantiate(ActionPlayerPrefab, vec3, Quaternion.identity, transform);
        ActionMobParents = Instantiate(MonsterParentPrefab, transform.position, Quaternion.identity, transform);
        ActionMob = Instantiate(MonsterPrefab, transform.position, Quaternion.identity, transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
