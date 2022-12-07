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

    [SerializeField] Player ActionPlayerPrefab;
    // ���� ������ �޾ƿ���
    [SerializeField] Monster MonsterParentPrefab;
    [SerializeField] Monster MonsterPrefab;

    GameObject MAPCUBE = null;
    Player ActionPlayer;
    Monster ActionMobParents = null;
    Monster ActionMob;

    // ���� ��
    private int CurActionMobCount = 0;
    private int MaxActionMobCount = 10;


    // �ϴ� ������ �� �� ť�긦 ����
    // ���Ŀ� �׼� �� ť�� ��ũ��Ʈ�� ���� ���� ������ �İų�,

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
