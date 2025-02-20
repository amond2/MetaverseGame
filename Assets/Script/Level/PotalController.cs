using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    // 플레이어가 포탈 영역 안에 있는지 여부를 저장할 변수
    private bool playerInPortal = false;

    // 포탈에 플레이어가 들어왔을 때 호출됨, 플레이어의 태그 이용
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerInPortal = true;
        }
    }

    // 플레이어가 포탈 영역을 벗어났을 때 호출됨
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerInPortal = false;
        }
    }

    // 매 프레임 호출되는 Update 함수에서 입력을 확인
    private void Update()
    {
        // 플레이어가 포탈 안에 있고, 스페이스바를 누른 경우
        if (playerInPortal && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("(디버깅) 포탈에 들어갑니다");
            // // 전환할 씬의 이름 또는 빌드 인덱스를 넣습니다.
            // SceneManager.LoadScene("NextSceneName");
        }
    }
}