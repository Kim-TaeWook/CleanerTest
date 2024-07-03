using UnityEngine;

public class DestroyOnTriggerEnter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
}
