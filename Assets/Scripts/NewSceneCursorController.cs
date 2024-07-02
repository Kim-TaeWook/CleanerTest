using UnityEngine;
using UnityEngine.SceneManagement;

public class NewSceneCursorController : MonoBehaviour
{
    void Awake()
    {
        // SceneManager의 sceneLoaded 이벤트에 이벤트 핸들러를 추가합니다.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // SceneManager의 sceneLoaded 이벤트에서 이벤트 핸들러를 제거합니다.
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬이 로드될 때 커서 잠금 상태를 설정합니다.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
