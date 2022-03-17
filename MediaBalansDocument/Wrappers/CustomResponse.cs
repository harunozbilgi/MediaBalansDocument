namespace MediaBalansDocument.Wrappers;

public class CustomResponse<T>
{
    public T Data { get; private set; }
    public string Messages { get; private set; }
    public bool IsSuccessful { get; private set; }
    public List<string> Errors { get; private set; }
    public static CustomResponse<T> Success(T data)
    {
        return new CustomResponse<T> { Data = data, IsSuccessful = true };
    }
    public static CustomResponse<T> Success(T data, string messages)
    {
        return new CustomResponse<T> { Data = data, Messages = messages, IsSuccessful = true };
    }
    public static CustomResponse<T> Fail(List<string> errors)
    {
        return new CustomResponse<T> { Errors = errors, IsSuccessful = false };
    }
    public static CustomResponse<T> Fail(string error)
    {
        return new CustomResponse<T> {  Errors = new List<string>() { error }, IsSuccessful = false };
    }
}
