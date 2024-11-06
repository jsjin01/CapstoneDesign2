using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    public Joystick joystick;  // Joystick�� �߰��� ����

    void Update()
    {
#if UNITY_EDITOR
        // ���� ȯ�濡���� Ű���� �Է��� ���� �̵� ����
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.position += new Vector3(moveX, moveY, 0);
#endif

        Vector3 moveDirection = new Vector3(joystick.Horizontal, joystick.Vertical, 0).normalized;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}



