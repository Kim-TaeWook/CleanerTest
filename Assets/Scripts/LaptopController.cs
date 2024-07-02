using UnityEngine;

public class LaptopController : MonoBehaviour
{
    public GameObject laptopUI; // Laptop UI ������Ʈ
    private StarterAssets.StarterAssetsInputs starterAssetsInputs;

    void Start()
    {
        starterAssetsInputs = FindObjectOfType<StarterAssets.StarterAssetsInputs>();
        // �ʱ� ���� ����
        SetLaptopUI(false);
    }

    // Laptop UI Ȱ��ȭ/��Ȱ��ȭ �޼���
    public void SetLaptopUI(bool isActive)
    {
        if (laptopUI != null)
        {
            laptopUI.SetActive(isActive);
            // UI�� Ȱ��ȭ�Ǹ� Ŀ�� ��� ���� �� Ŀ�� ���̱�
            if (starterAssetsInputs != null)
            {
                starterAssetsInputs.SetCursorState(!isActive);
            }
        }
    }
}
