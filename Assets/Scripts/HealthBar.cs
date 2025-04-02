using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Color healthBarColor;
    [SerializeField] private Slider slider;

    public void InitializeHealthBar(float newValue)
    {
        slider.maxValue = newValue;
        slider.value = newValue;
        slider.image.color = healthBarColor;
    }

    public void ChangeHealthValue(float newValue)
    {
        slider.value = newValue;   
    }
}
