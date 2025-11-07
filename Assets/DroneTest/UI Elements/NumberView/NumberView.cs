using TMPro;
using UnityEngine;

public class NumberView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI uiText;

    private ReactiveVariable<int> currentValue;


    public void Construct(ReactiveVariable<int> _currentValue)
    {
        currentValue = _currentValue;
    }

    private void OnEnable()
    {
        currentValue.eChanged += OnVariableChanged;  
        UpdateView(currentValue.Value);
    }

    private void OnDisable()
    {
        currentValue.eChanged -= OnVariableChanged; 
    }


    private void OnVariableChanged(int oldValue, int newValue)
    {
        Debug.Log(newValue);
        UpdateView(newValue);
    }

    private void UpdateView(int value)
    {
        uiText.text = value.ToString();
    }
}
