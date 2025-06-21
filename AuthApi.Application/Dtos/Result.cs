namespace AuthApi.Application.Dtos;

public record Result<T>
{
  public bool Success { get; init; }
  public string? Message { get; init; }
  public T? Value { get; init; }
  public static Result<T> SuccessResult(T value) => new() { Success = true, Value = value };
  public static Result<T> Failure(string message) => new() { Message = message };
}