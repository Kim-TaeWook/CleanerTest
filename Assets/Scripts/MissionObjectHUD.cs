using UnityEngine;
using TMPro;

public class MissionObjectHUD : MonoBehaviour
{
    public TextMeshProUGUI currentObjectText;
    public TextMeshProUGUI currentDecalText;
    public int totalObjects;
    public int totalDecals;

    void Start()
    {
        UpdateHUD();
    }

    public void UpdateHUD()
    {
        currentObjectText.text = totalObjects.ToString("D2");
        currentDecalText.text = totalDecals.ToString("D2");
    }

    public void DecreaseObjectCount()
    {
        if (totalObjects > 0)
        {
            totalObjects--;
            UpdateHUD();
        }
    }

    public void DecreaseDecalCount()
    {
        if (totalDecals > 0)
        {
            totalDecals--;
            UpdateHUD();
        }
    }
}
