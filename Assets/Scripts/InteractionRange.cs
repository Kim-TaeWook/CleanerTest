using UnityEngine;

public class InteractionRange : MonoBehaviour
{
    public float interactionRadius = 2.0f;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }

    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionRadius);
        foreach (var hitCollider in hitColliders)
        {
            IInteractable interactable = hitCollider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
                break;
            }
        }
    }
}
