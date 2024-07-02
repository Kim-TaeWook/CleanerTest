using UnityEngine;
using UnityEngine.SceneManagement;

public class NewSceneCursorController : MonoBehaviour
{
    void Awake()
    {
        // SceneManager�� sceneLoaded �̺�Ʈ�� �̺�Ʈ �ڵ鷯�� �߰��մϴ�.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // SceneManager�� sceneLoaded �̺�Ʈ���� �̺�Ʈ �ڵ鷯�� �����մϴ�.
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���� �ε�� �� Ŀ�� ��� ���¸� �����մϴ�.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
