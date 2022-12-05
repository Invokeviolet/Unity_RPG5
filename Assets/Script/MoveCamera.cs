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
        // �÷��̾��� ��ġ�� Ÿ�� ��ġ�� �ֱ�
        Target = FindObjectOfType<Player>();
        targetTransfotm = Target.GetComponent<Transform>();
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = targetTransfotm.position + CameraOffset;
    }
}
