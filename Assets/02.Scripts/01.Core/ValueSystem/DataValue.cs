using Sirenix.OdinInspector;
using System;
using UnityEngine;
public interface IDataValue<T> where T : struct, IConvertible
{
    T Value { get; }

    void Subscribe(Action<T> action);
    void Unsubscribe(Action<T> action);
}

//런타임동안 지속적으로 변하지 않는 데이터
public class DataValue<T> : ValueBase<T>, IDataValue<T> where T : struct, IConvertible
{
    [SerializeField] private T _value;
    [ShowInInspector] public T Value => _value;

    public DataValue(T value)
    {
        Init(value);
    }

    public void Init(T value)
    {
        Set(value);
    }

    public void Increase(T amount)
    {
        Set(FromDouble(ToDouble(_value) + ToDouble(amount)));
    }

    public void Decrease(T amount)
    {
        Set(FromDouble(ToDouble(_value) - ToDouble(amount)));
    }

    public void Set(T newValue)
    {
        T clamped = ClampMinZero(newValue);

        if (EqualsValue(_value, clamped))
            return;

        _value = clamped;
        Notify(_value);
    }
}