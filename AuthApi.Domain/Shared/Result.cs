namespace AuthApi.Domain.Shared;

public record Result<T>
{
  public bool Success { get; init; }
  public T? Value { get; init; }
  public List<string>? Errors { get; init; }
  public static Result<T> SuccessResult(T value) => new()
  {
    Success = true,
    Value = value
  };
  public static Result<T> Failure(params string[] errors) => new()
  {
    Errors = [.. errors]
  };

  public static Result<T> Failure(IEnumerable<string> errors) => new()
  {
    Errors = [.. errors]
  };
}

public record Result
{
  public bool Success { get; init; }
  public List<string>? Errors { get; init; }
  public static Result SuccessResult() => new()
  {
  };
  public static Result Failure(params string[] errors) => new()
  {
    Errors = [.. errors]
  };

  public static Result Failure(IEnumerable<string> errors) => new()
  {
    Errors = [.. errors]
  };
}