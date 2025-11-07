using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SliderView : MonoBehaviour
{
    [SerializeField] private Slider uiSlider;

    private ReactiveVariable<int> currentValue;


    public void Construct(ReactiveVariable<int> _currentValue)
    {
        currentValue = _currentValue;
    }


    private void OnEnable()
    {
        UpdateView(currentValue.Value);
        uiSlider.onValueChanged.AddListener(OnSliderChange);
    }

    private void OnDisable()
    {
        uiSlider.onValueChanged.RemoveListener(OnSliderChange);
    }


    private void OnSliderChange(float value)
    {
        currentValue.Value = (int)value;
    }

    private void UpdateView(float currentValue)
    {
        uiSlider.value = currentValue;
    }
}
