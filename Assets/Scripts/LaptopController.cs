using UnityEngine;

public class LaptopController : MonoBehaviour
{
    public GameObject laptopUI; // Laptop UI 오브젝트
    private StarterAssets.StarterAssetsInputs starterAssetsInputs;

    void Start()
    {
        starterAssetsInputs = FindObjectOfType<StarterAssets.StarterAssetsInputs>();
        // 초기 상태 설정
        SetLaptopUI(false);
    }

    // Laptop UI 활성화/비활성화 메서드
    public void SetLaptopUI(bool isActive)
    {
        if (laptopUI != null)
        {
            laptopUI.SetActive(isActive);
            // UI가 활성화되면 커서 잠금 해제 및 커서 보이기
            if (starterAssetsInputs != null)
            {
                starterAssetsInputs.SetCursorState(!isActive);
            }
        }
    }
}
