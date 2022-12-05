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
    // 몬스터 프리팹 받아오기
    [SerializeField] Monster MonsterParentPrefab;
    [SerializeField] Monster MonsterPrefab;
    
    // 일단 통으로 된 맵 큐브를 생성
    // 추후에 액션 맵 큐브 스크립트를 따로 만들어서 함정을 파거나,
    
    void Start()
    {
        Instantiate(MapCubePrefab, MapCubePrefab.transform.position, Quaternion.identity);
        Instantiate(MonsterParentPrefab, MonsterParentPrefab.transform.position, Quaternion.identity);
        Instantiate(MonsterPrefab, MonsterPrefab.transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
