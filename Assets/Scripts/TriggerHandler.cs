using UnityEngine;

public class TriggerHandler : MonoBehaviour
{
    public GameObject laptop; // 노트북 오브젝트
    private Outline outline; // Outline 스크립트 참조
    private LaptopController laptopController; // LaptopController 스크립트 참조
    private bool isPlayerInRange = false; // 플레이어가 범위 내에 있는지 확인

    void Start()
    {
        outline = laptop.GetComponent<Outline>();
        laptopController = laptop.GetComponent<LaptopController>();

        // 초기 상태 설정
        outline.enabled = false;
        laptopController.SetLaptopUI(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            outline.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            outline.enabled = false;
            laptopController.SetLaptopUI(false);
        }
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            laptopController.SetLaptopUI(true);
        }
    }
}
