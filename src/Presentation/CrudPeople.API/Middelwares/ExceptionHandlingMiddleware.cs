using CrudPeople.CoreDomain.Contracts.Base.Commands;
using ElasticLogger;
using System.Net;
namespace CrudPeople.API.Middelwares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUnitOfWork _unitOfWork;
        public ExceptionHandlingMiddleware(RequestDelegate next, IUnitOfWork unitOfWork)
        {
            _next = next;
            _unitOfWork = unitOfWork;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
                    if (!_unitOfWork.Disposed)
                { 
                    await _unitOfWork.CommitAsync(); 
                }
            }
            catch (ArgumentException ex)
            {
                await _unitOfWork.RollBackAsync();

                LogCall.LogException(ex);
                await HandleExceptionAsync(context, ex,HttpStatusCode.BadRequest);
            }
            catch(CrudPeople.CoreDomain.Helper.CrudPeopleException ex)
            {
                await _unitOfWork.RollBackAsync();

                LogCall.LogException(ex); 
                if (ex.ExceptionType==CoreDomain.Enums.ExceptionType.Validation)
                await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);
                if(ex.ExceptionType==CoreDomain.Enums.ExceptionType.NotFound)
                {
                    await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound);

                }
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackAsync();
                LogCall.LogException(ex);
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                context.Response.StatusCode,
                Message = "Internal Server Error. Please try again later.",
                Detailed = exception.Message // Optional: Include detailed error message
            };

            return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
        }
    }
}