using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionManager : MonoBehaviour
{
    public static ProgressionManager Instance;

    [SerializeField] private int neededExpirience;
    [SerializeField] private int currentExpirience;
    private int currentLevel;

    [SerializeField] private Slider expirienceSlider;
    [SerializeField] private TextMeshProUGUI expirienceText;

    private void Awake()
    {
        Instance = this;
        SetSliderAndTextExpirienceValues();
    }

    public bool IsEnoughLevel(int requiredLevel)
    {
        return currentLevel >= requiredLevel;
    }

    private void SetSliderAndTextExpirienceValues()
    {
        expirienceSlider.maxValue = neededExpirience;
        expirienceSlider.value = currentExpirience;

        expirienceText.text = "EXPIRIENCE: " + currentExpirience + "/" + neededExpirience;
    }

    public void GetExpirience(int expirienceGained)
    {
        currentExpirience += expirienceGained;
        if(currentExpirience >= neededExpirience)
        {
            currentLevel++;
            currentExpirience = 0;
            neededExpirience += 20;
            SetSliderAndTextExpirienceValues();
        }
    }
}
