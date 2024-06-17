using System.ComponentModel;

namespace VisualSharp;

public class ObservableValue<T>:INotifyPropertyChanged
{

public delegate void ObservableValueChangedEventHandler(object sender);
    T value;
    public T Value
    {
        get=>value;
        set
        {
            this.value = value;
            PropertyChanged?.Invoke(this, new(nameof(Value)));
            OnValueChanged?.Invoke(this);
        }
    }

    public ObservableValue(T value)=>this.value = value;

    public event ObservableValueChangedEventHandler OnValueChanged;
    public event PropertyChangedEventHandler PropertyChanged;

    public static implicit operator T(ObservableValue<T> observableValue)=>observableValue.Value;

    public static implicit operator ObservableValue<T>(T value)=>new(value);
}