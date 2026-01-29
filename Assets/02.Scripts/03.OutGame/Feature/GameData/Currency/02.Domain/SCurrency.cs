using System;
public readonly struct SCurrency
{
    public readonly float Value;

    public SCurrency( float value)
    {
        // 유효성 검사
        if (value < 0)
        {
            throw new Exception("Currency값은 0보다 작을 수 없습니다.");
            // 이런 잘못된 데이터가 들어왔다는 것은 여러가지 부작용이 생길수있다.
            // 게임 플레이 도중에 그 부작용을 느끼는 것보다
            // 애초에 시작단계에서 에러를 뱉어버리는게 유지보수 면에서 편한다.
        }
        Value = value;
    }

    // 연산자 오버라이딩 : 객체간의 연산자(+,-, >, <)할때 암시적으로 호출되는 메서드

    // 1. 재화끼리 더하기
    public static SCurrency operator +(SCurrency currency1, SCurrency currency2)
    {
        return new SCurrency(currency1.Value + currency2.Value);
    }

    // 2. 재화끼리 빼기
    public static SCurrency operator -(SCurrency a, SCurrency b)
    {
        return new SCurrency(a.Value - b.Value);
    }

    // 3. 비교 연산자들
    public static bool operator >=(SCurrency a, SCurrency b)
    {
        return a.Value >= b.Value;
    }

    public static bool operator <=(SCurrency a, SCurrency b)
    {
        return a.Value <= b.Value;
    }

    public static bool operator >(SCurrency a, SCurrency b)
    {
        return a.Value > b.Value;
    }

    public static bool operator <(SCurrency a, SCurrency b)
    {
        return a.Value < b.Value;
    }

    // double → Currency 암시적 변환    
    public static implicit operator SCurrency(float value)
    {
        return new SCurrency(value);
    }

    // Currency -> double 암시적 변환
    public static explicit operator float(SCurrency currency)
    {
        return currency.Value;
    }

    // ToString이란 객체를 문자열로 변환될때 암시적으로 호출되는 메서드인데..
    // 이걸 개조(메서드 오버라이)해서 특정 포맷으로 문자 변환되게끔 강제한다.
    public override string ToString()
    {
        return Utils.FormattedString(Value);
    }
}