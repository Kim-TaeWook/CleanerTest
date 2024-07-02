using System.Collections;
using UnityEngine;

public class InteractableStain : MonoBehaviour
{
    public bool playerInRange = false;  // 플레이어가 범위 내에 있는지 여부
    public float fadeDuration = 2.0f;   // 투명해지는 시간
    private Renderer objectRenderer;
    private Material objectMaterial;
    private Color originalColor;
    private bool isFading = false;
    private float currentFadeTime = 0f;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            objectMaterial = objectRenderer.material;
            originalColor = objectMaterial.color;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetMouseButton(0) && !isFading)
        {
            StartCoroutine(FadeOut());
        }
        else if (!Input.GetMouseButton(0))
        {
            StopCoroutine(FadeOut());
            isFading = false;
        }
    }

    private IEnumerator FadeOut()
    {
        isFading = true;
        Color currentColor = objectMaterial.color;

        while (currentFadeTime < fadeDuration)
        {
            if (!Input.GetMouseButton(0))
            {
                isFading = false;
                yield break;
            }

            currentFadeTime += Time.deltaTime;
            float alpha = Mathf.Lerp(originalColor.a, 0f, currentFadeTime / fadeDuration);
            currentColor.a = alpha;
            objectMaterial.color = currentColor;
            yield return null;
        }

        currentColor.a = 0f;
        objectMaterial.color = currentColor;
        Destroy(gameObject);
    }
}
