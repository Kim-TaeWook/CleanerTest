using System.Collections;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; set; }

    public bool onTarget;
    public GameObject selectedObject;

    public LayerMask interactableLayerMask;

    public Transform player1HoldPosition;
    public Transform player1DropPosition;
    public Transform player2HoldPosition;
    public Transform player2DropPosition;

    private Transform holdPosition;
    private Transform dropPosition;

    public GameObject heldObject;

    private Vector3 originalScale;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        // 초기 플레이어 포지션 설정 (플레이어 1)
        SetPlayerPositions(player1HoldPosition, player1DropPosition);
    }

    void Update()
    {
        if (heldObject != null)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                DropObject();
            }
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayerMask))
        {
            var selectionTransform = hit.transform;

            InteractableObject interactable = selectionTransform.GetComponent<InteractableObject>();
            if (interactable && interactable.playerInRange)
            {
                onTarget = true;
                selectedObject = interactable.gameObject;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    PickupObject(interactable.gameObject);
                }
            }
            else
            {
                onTarget = false;
            }
        }
        else
        {
            onTarget = false;
        }
    }

    private void PickupObject(GameObject obj)
    {
        heldObject = obj;
        originalScale = heldObject.transform.localScale; // 현재 scale 값 저장
        heldObject.transform.SetParent(holdPosition);
        heldObject.transform.localPosition = Vector3.zero;
        heldObject.transform.localRotation = Quaternion.identity;

        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        var outline = heldObject.GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false;
        }
    }

    private void DropObject()
    {
        if (heldObject != null)
        {
            heldObject.transform.SetParent(null);
            heldObject.transform.localScale = originalScale; // 원래 scale 값 복원

            if (dropPosition != null)
            {
                heldObject.transform.position = dropPosition.position;
            }
            else
            {
                // 드랍 위치가 설정되지 않은 경우, 플레이어 기준으로 드랍
                Transform playerTransform = holdPosition.parent;
                Vector3 defaultDropPosition = playerTransform.position + playerTransform.forward * 2.0f + Vector3.down * 1.0f;
                heldObject.transform.position = defaultDropPosition;
            }

            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }

            // 콜라이더를 잠시 비활성화 후 재활성화하여 충돌 문제 해결
            Collider col = heldObject.GetComponent<Collider>();
            if (col != null)
            {
                StartCoroutine(ReenableCollider(col));
            }

            heldObject = null;
        }
    }

    private IEnumerator ReenableCollider(Collider col)
    {
        col.enabled = false;
        yield return new WaitForSeconds(0.1f);
        col.enabled = true;
    }

    public void SetPlayerPositions(Transform holdPos, Transform dropPos)
    {
        holdPosition = holdPos;
        dropPosition = dropPos;
    }
}
