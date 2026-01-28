using System;
using UnityEngine;
public interface IRuntimeValue<T> where T : struct, IConvertible
{
    //값 확인
    T MaxValue { get; }
    T CurrentValue { get; }
    float RegenerationPerTime { get; }

    //상태확인
    bool IsFull();          //가득차있나?
    bool IsEmpty();         //비어있나?
    float GetRatio();       //현재 current / max  

    //값변경
    //void Decrease(T amount);    //amount만큼 감소
    //void Increase(T  amount);   //amout만큼 증가
    void Regeneration(float deltaTime); //런타임 시간에 의한 증가
    //void Reset();               //초기화

    //UI 친화
    void Subscribe(Action<T> action);   //OnValueChange 구독
    void Unsubscribe(Action<T> action); //OnValueChange 구독해지
}

//런타임중 변하는 데이터(HP, Stemina등 한계치가 있는 값)
public class RuntimeValue<T> : ValueBase<T>, IRuntimeValue<T>
    where T : struct, IConvertible
{
    [SerializeField] private T _max;
    [SerializeField] private T _current;
    [SerializeField] private float _regen;

    private float _regenAccumulator;

    public T MaxValue => _max;
    public T CurrentValue => _current;
    public float RegenerationPerTime => _regen;

    public RuntimeValue(T max, T current = default, float regen = 0f)
    {
        Init(max, current, regen);
    }

    public void Init(T max, T current = default, float regen = 0f)
    {
        SetMax(max);

        if (ToDouble(current) <= 0.00001)
            SetCurrent(_max);
        else
            SetCurrent(current);

        SetRegen(regen);
        _regenAccumulator = 0f;
    }

    #region State Check
    public bool IsFull()
    {
        return EqualsValue(_current, _max);
    }

    public bool IsEmpty()
    {
        return ToDouble(_current) <= 0;
    }

    public float GetRatio()
    {
        double max = ToDouble(_max);

        if (max <= 0.00001)
            return 0f;

        return (float)(ToDouble(_current) / max);
    }
    #endregion

    #region Max
    public void SetMax(T value)
    {
        T clamped = ClampMinZero(value);

        if (EqualsValue(_max, clamped))
            return;

        _max = clamped;

        // current 자동 보정
        SetCurrent(_current);
    }
    #endregion

    #region Current
    public void SetCurrent(T value)
    {
        double clamped = Math.Clamp(
            ToDouble(value),
            0,
            ToDouble(_max));

        T newValue = FromDouble(clamped);

        if (EqualsValue(_current, newValue))
            return;

        _current = newValue;
        Notify(_current);
    }

    public void Reset()
    {
        SetCurrent(default);
    }

    public void Decrease(T amount)
    {
        SetCurrent(FromDouble(ToDouble(_current) - ToDouble(amount)));
    }

    public void Increase(T amount)
    {
        SetCurrent(FromDouble(ToDouble(_current) + ToDouble(amount)));
    }
    #endregion

    #region Regen
    public void SetRegen(float value)
    {
        float clamped = Mathf.Max(0f, value);

        if (Mathf.Approximately(_regen, clamped))
            return;

        _regen = clamped;
    }

    public void Regeneration(float deltaTime)
    {
        if (IsFull() || _regen <= 0f)
            return;

        _regenAccumulator += _regen * deltaTime;

        if (typeof(T) == typeof(float))
        {
            SetCurrent(FromDouble(ToDouble(_current) + _regen * deltaTime));
            return;
        }

        if (_regenAccumulator < 1f)
            return;

        int add = Mathf.FloorToInt(_regenAccumulator);
        _regenAccumulator -= add;

        SetCurrent(FromDouble(ToDouble(_current) + add));
    }
    #endregion
}