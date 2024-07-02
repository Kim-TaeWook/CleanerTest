using UnityEngine;

public class TriggerHandler : MonoBehaviour
{
    public GameObject laptop; // ��Ʈ�� ������Ʈ
    private Outline outline; // Outline ��ũ��Ʈ ����
    private LaptopController laptopController; // LaptopController ��ũ��Ʈ ����
    private bool isPlayerInRange = false; // �÷��̾ ���� ���� �ִ��� Ȯ��

    void Start()
    {
        outline = laptop.GetComponent<Outline>();
        laptopController = laptop.GetComponent<LaptopController>();

        // �ʱ� ���� ����
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
