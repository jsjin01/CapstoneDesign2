using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    public Joystick joystick;  // Joystick을 추가할 변수

    void Update()
    {
#if UNITY_EDITOR
        // 개발 환경에서만 키보드 입력을 통한 이동 가능
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.position += new Vector3(moveX, moveY, 0);
#endif

        Vector3 moveDirection = new Vector3(joystick.Horizontal, joystick.Vertical, 0).normalized;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}



