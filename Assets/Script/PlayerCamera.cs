using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // �÷��̾� �Ӹ� ���� ��ġ
    // �÷��̾ �ٶ� ������ ������

    [SerializeField] public Vector3 CameraOffset;
    public Player Target;
    public Transform target;

    private void Start()
    {
        Target = FindObjectOfType<Player>();
        target = Player.INSTANCE.PlayerPos.transform;
    }

    void Update()
    {
        transform.position = target.transform.position + CameraOffset + transform.right * 0.6f;
        transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0f));
    }
}
