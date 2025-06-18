namespace AuthApi.Dtos;

public record RegisterResult(bool Sucess, string? Message = null, Guid? UserId = null)
{
  public static RegisterResult Success(Guid id) => new(true, null, id);
  public static RegisterResult Failure(string message) => new(false, message);
}