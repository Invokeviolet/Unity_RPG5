using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Player Target;
    public Transform targetTransfotm;
    [SerializeField] public Vector3 CameraOffset;

    private void Start()
    {
        // 플레이어의 위치를 타겟 위치에 넣기
        Target = FindObjectOfType<Player>();
        targetTransfotm = Target.GetComponent<Transform>();
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = targetTransfotm.position + CameraOffset;
    }
}
