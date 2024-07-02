using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleButtonActions : MonoBehaviour
{
    // 특정 오브젝트를 활성화하는 메서드
    public void ActivateObject(GameObject targetObject)
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true);
        }
    }

    // 특정 오브젝트를 비활성화하는 메서드
    public void DeactivateObject(GameObject targetObject)
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
    }

    // 씬 전환 메서드
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // 게임 종료 메서드
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        // 유니티 에디터에서 플레이 모드 중 게임 종료
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
