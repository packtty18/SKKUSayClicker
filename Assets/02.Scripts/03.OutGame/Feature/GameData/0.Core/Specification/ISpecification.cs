
//형식을 체크하는 인터페이스
public interface ISpecification<T>
{
    bool IsSatisfiedBy(T entity);
    string ErrorMessage { get; }
}