using System.Collections.Generic;

// 검증의 결과를 반환
public class ValidationResult
{
    public bool IsValid { get; }
    public List<string> Errors { get; } // 에러문구를 저장
    public string FirstError => Errors.Count > 0 ? Errors[0] : string.Empty;
    public string AllErrors => string.Join(", ", Errors); // 이것은 한번에 오류를 보여줄때 사용하는 변수. 현재 상황에서는 사용하지 않음

    public ValidationResult(bool isValid, List<string> errors)
    {
        IsValid = isValid;
        Errors = errors;
    }

    // 헬퍼 메서드
    public static ValidationResult Success()
    {
        return new ValidationResult(true, new List<string>());
    }

    public static ValidationResult Fail(string error)
    {
        return new ValidationResult(false, new List<string> { error });
    }

    public static ValidationResult Fail(List<string> errors)
    {
        return new ValidationResult(false, errors);
    }
}
