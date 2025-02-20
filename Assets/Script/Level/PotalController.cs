using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    // 전환할 씬 지정 가능
    [SerializeField] private string targetSceneName = "";
    
    // 플레이어가 포탈 영역 안에 있는지 여부를 저장할 변수
    private bool playerInPortal = false;

    // 포탈에 플레이어가 들어왔을 때 호출됨, 플레이어의 태그 이용
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerInPortal = true;
            Debug.Log("Player entered portal");
        }
    }

    // 플레이어가 포탈 영역을 벗어났을 때 호출됨
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerInPortal = false;
            Debug.Log("Player exited portal");
        }
    }

    // 매 프레임 호출되는 Update 함수에서 입력을 확인
    private void Update()
    {
        // 플레이어가 포탈 안에 있고, 스페이스바를 누른 경우
        if (playerInPortal && Input.GetKeyDown(KeyCode.Space))
        {
            // 전환할 씬을 로드
            SceneManager.LoadScene(targetSceneName);
            Debug.Log("Player moved other Scene");
        }
    }
}