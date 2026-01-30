using System.Collections.Generic;

//여러개의 명세를 모두 체크
public class SpecificationValidator<T>
{
    private readonly List<ISpecification<T>> _specifications = new List<ISpecification<T>>();

    public SpecificationValidator<T> Add(ISpecification<T> specification)
    {
        _specifications.Add(specification);
        return this;
    }

    //전부 체크해서 한번에 내보내기
    public ValidationResult Validate(T value)
    {
        var errors = new List<string>();

        foreach (var spec in _specifications)
        {
            if (!spec.IsSatisfiedBy(value))
            {
                errors.Add(spec.ErrorMessage);
            }
        }

        return new ValidationResult(errors.Count == 0, errors);
    }

    // 첫 번째 실패 시 즉시 중단 (빠른 실패)
    public ValidationResult ValidateFast(T value)
    {
        foreach (var spec in _specifications)
        {
            if (!spec.IsSatisfiedBy(value))
            {
                return new ValidationResult(false, new List<string> { spec.ErrorMessage });
            }
        }

        return new ValidationResult(true, new List<string>());
    }
}