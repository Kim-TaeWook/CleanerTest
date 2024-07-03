using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    float throwForce = 600;
    Vector3 objectPos;
    float distance;
    Vector3 originalScale;

    public bool canHold = true;
    public GameObject item;
    public GameObject tempParent;
    public Camera playerCamera; // Add a reference to the player's camera
    public bool isHolding = false;
    private Outline outline;

    void Start()
    {
        outline = item.GetComponent<Outline>();
        if (outline == null)
        {
            Debug.LogWarning("Outline component is missing on the item.");
        }
        originalScale = item.transform.localScale; // Save the original scale
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHolding)
        {
            // Find the closest object in front of the camera
            Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.collider.gameObject;
                if (hitObject == item)
                {
                    distance = Vector3.Distance(item.transform.position, tempParent.transform.position);
                    if (distance <= 1f && canHold)
                    {
                        if (outline != null)
                        {
                            outline.enabled = true; // Enable outline
                        }
                    }
                    else
                    {
                        if (outline != null)
                        {
                            outline.enabled = false; // Disable outline
                        }
                    }
                }
                else
                {
                    if (outline != null)
                    {
                        outline.enabled = false; // Disable outline
                    }
                }
            }
        }

        // Check if is holding
        if (isHolding)
        {
            item.GetComponent<Rigidbody>().velocity = Vector3.zero;
            item.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            item.transform.SetParent(tempParent.transform);

            // Disable outline while holding
            if (outline != null)
            {
                outline.enabled = false;
            }

            if (Input.GetMouseButtonDown(1))
            {
                // Throw
                item.GetComponent<Rigidbody>().AddForce(tempParent.transform.forward * throwForce);
                isHolding = false;
            }
        }
        else
        {
            objectPos = item.transform.position;
            item.transform.SetParent(null);
            item.GetComponent<Rigidbody>().useGravity = true;
            item.transform.position = objectPos;

            // Restore the original scale
            item.transform.localScale = originalScale;
        }
    }

    void OnMouseDown()
    {
        if (distance <= 1f)
        {
            isHolding = true;
            item.GetComponent<Rigidbody>().useGravity = false;
            item.GetComponent<Rigidbody>().detectCollisions = true;

            if (outline != null)
            {
                outline.enabled = false; // Disable outline when holding
            }
        }
    }

    void OnMouseUp()
    {
        isHolding = false;
        item.GetComponent<Rigidbody>().useGravity = true;
        item.GetComponent<Rigidbody>().detectCollisions = true;

        if (distance <= 1f && canHold)
        {
            if (outline != null)
            {
                outline.enabled = true; // Re-enable outline if within range
            }
        }
    }
}
