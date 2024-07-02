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
        // �ʱ� �÷��̾� ������ ���� (�÷��̾� 1)
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
        originalScale = heldObject.transform.localScale; // ���� scale �� ����
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
            heldObject.transform.localScale = originalScale; // ���� scale �� ����

            if (dropPosition != null)
            {
                heldObject.transform.position = dropPosition.position;
            }
            else
            {
                // ��� ��ġ�� �������� ���� ���, �÷��̾� �������� ���
                Transform playerTransform = holdPosition.parent;
                Vector3 defaultDropPosition = playerTransform.position + playerTransform.forward * 2.0f + Vector3.down * 1.0f;
                heldObject.transform.position = defaultDropPosition;
            }

            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }

            // �ݶ��̴��� ��� ��Ȱ��ȭ �� ��Ȱ��ȭ�Ͽ� �浹 ���� �ذ�
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
