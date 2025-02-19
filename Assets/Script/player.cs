using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed = 5f; // 이동 속도
    public Camera mainCamera; // 카메라 참조
    public Vector2 backgroundSize = new Vector2(30f, 30f); // 배경 크기
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool facingRight = true;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private bool isNearDesk = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // 스프라이트 렌더러 가져오기
        animator = GetComponent<Animator>(); // 애니메이터 가져오기

        // 회전 고정 (Z축 회전 방지)
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (isNearDesk && Input.GetKeyDown(KeyCode.F))
        {
            SceneManager.LoadScene("SampleScene"); // 씬 이름을 실제 씬 이름으로 변경
        }
        // 플레이어 이동 입력 받기
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // 애니메이션 제어 (이동 중일 때만 실행)
        bool isMoving = moveInput.magnitude > 0; // 입력 값이 0보다 크면 이동 중
        animator.SetBool("isRunning", isMoving);

        // 좌우 방향 전환 (회전 없이 이미지 반전)
        if (moveInput.x < 0)
        {
            spriteRenderer.flipX = false;
            facingRight = true;
        }
        else if (moveInput.x > 0)
        {
            spriteRenderer.flipX = true;
            facingRight = false;
        }

        // 카메라 이동 제한
        if (mainCamera != null)
        {
            float camHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
            float camHalfHeight = mainCamera.orthographicSize;

            float minX = -backgroundSize.x / 2 + camHalfWidth;
            float maxX = backgroundSize.x / 2 - camHalfWidth;
            float minY = -backgroundSize.y / 2 + camHalfHeight;
            float maxY = backgroundSize.y / 2 - camHalfHeight;

            float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
            float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);

            mainCamera.transform.position = new Vector3(clampedX, clampedY, mainCamera.transform.position.z);
        }
    }

    void FixedUpdate()
    {
        // Rigidbody2D를 이용한 이동
        rb.velocity = moveInput * speed;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Desk")) // 컴퓨터 오브젝트에 "Desk" 태그 추가해야 함
        {
            isNearDesk = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Desk"))
        {
            isNearDesk = false;
        }
    }
}