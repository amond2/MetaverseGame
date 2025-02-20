using UnityEngine;

public class RollingObject : MonoBehaviour
{
    public float moveSpeed = 20f; // 이동 속도
    public float rotationSpeed = 240f; // 초당 회전 각도
    private Vector2 moveDirection;
    private bool isRolling = false;
    private float friction = 0.05f;

    // 현재 이동 속도를 나타내는 벡터 (슬라이딩 효과)
    private Vector2 velocity = Vector2.zero;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Object"))
        {
            // 플레이어와의 상대 방향 계산
            moveDirection = (transform.position - collision.transform.position).normalized;
            isRolling = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Object"))
        {
            isRolling = false;
        }
    }

    private void Update()
    {
        if (isRolling)
        {
            // 이동
            transform.position += (Vector3)(moveDirection * moveSpeed * Time.deltaTime);

            // 회전 (예: 진행 방향에 따라 회전)
            float angle = rotationSpeed * Time.deltaTime;
            // Z축 기준으로 회전
            transform.Rotate(0f, 0f, angle);
        }
        
        // // velocity의 크기가 매우 작아질 때까지 계속 이동 및 회전 시키고, 감쇠 효과 적용 -> 나중에 디테일 구현!
        // if (velocity.magnitude > 0.01f)
        // {
        //     // 이동: 매 프레임 현재 velocity에 따라 위치 변경 (미끄러지듯 이동)
        //     transform.position += (Vector3)(velocity * Time.deltaTime);
        //
        //     // 회전: 매 프레임 Z축 기준으로 회전
        //     transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        //
        //     // 감쇠: 매 프레임 velocity에 friction 계수를 곱해 속도를 감소시킵니다.
        //     velocity *= friction;
        // }
        // else
        // {
        //     // velocity가 거의 0이면 멈추도록 처리
        //     velocity = Vector2.zero;
        // }
    }
}