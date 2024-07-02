using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleButtonActions : MonoBehaviour
{
    // Ư�� ������Ʈ�� Ȱ��ȭ�ϴ� �޼���
    public void ActivateObject(GameObject targetObject)
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true);
        }
    }

    // Ư�� ������Ʈ�� ��Ȱ��ȭ�ϴ� �޼���
    public void DeactivateObject(GameObject targetObject)
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
    }

    // �� ��ȯ �޼���
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // ���� ���� �޼���
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        // ����Ƽ �����Ϳ��� �÷��� ��� �� ���� ����
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
