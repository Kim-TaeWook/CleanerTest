using UnityEngine;

public interface IInteractable
{
    void Interact();
}

public class PickupableItem : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        // ������ ��� ���ɿ��� �̵��ϴ� ����
        // �� �κ��� ���� AI���� ó���մϴ�.
    }
}
