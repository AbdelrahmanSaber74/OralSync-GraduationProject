using SharedClassLibrary.DTOs;

namespace SharedClassLibrary.Contracts
{
    public interface IErrorResponseBuilder
    {
        Task BuildBadRequestAsync(int statusCode, string message);
        Task BuildNotFoundAsync(int statusCode, string message);
    }


}
