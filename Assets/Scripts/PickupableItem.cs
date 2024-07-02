using UnityEngine;

public interface IInteractable
{
    void Interact();
}

public class PickupableItem : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        // 물건을 들고 유령에게 이동하는 로직
        // 이 부분은 유령 AI에서 처리합니다.
    }
}
