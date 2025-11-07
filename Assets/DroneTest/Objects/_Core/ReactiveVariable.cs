using System;

public class ReactiveVariable<T>
{
    public event Action<T, T> eChanged;

    private T value;

    public ReactiveVariable(T value)
    {
        this.value = value;
    }

    public T Value
    {
        get => this.value;
        set
        {
            T oldValue = this.value;
            this.value = value;
            if (!value.Equals(oldValue))
            {
                eChanged?.Invoke(oldValue, this.value);
            }
        }
    }

}
