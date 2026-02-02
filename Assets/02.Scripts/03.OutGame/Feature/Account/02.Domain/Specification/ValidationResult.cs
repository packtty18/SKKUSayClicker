using System.Collections.Generic;

//검증의 결과를 반환
public class ValidationResult
{
    public bool IsValid { get; }
    public List<string> Errors { get; } //에러문구를 저장
    public string FirstError => Errors.Count > 0 ? Errors[0] : string.Empty;
    public string AllErrors => string.Join(", ", Errors);

    public ValidationResult(bool isValid, List<string> errors)
    {
        IsValid = isValid;
        Errors = errors;
    }
}