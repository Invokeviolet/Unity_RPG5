using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner_ACTION : MonoBehaviour
{
    // �̱���
    #region �̱���

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

    // �� ���� ����
    [SerializeField] GameObject MapCubePrefab;
    // ���� ������ �޾ƿ���
    [SerializeField] Monster MonsterParentPrefab;
    [SerializeField] Monster MonsterPrefab;
    
    // �ϴ� ������ �� �� ť�긦 ����
    // ���Ŀ� �׼� �� ť�� ��ũ��Ʈ�� ���� ���� ������ �İų�,
    
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
