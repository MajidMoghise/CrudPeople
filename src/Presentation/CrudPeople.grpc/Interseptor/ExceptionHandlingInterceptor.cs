using CrudPeople.CoreDomain.Contracts.Base.Commands;
using ElasticLogger;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace CrudPeople.grpc.Interseptor
{
    public class ExceptionHandlingInterceptor : Interceptor
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExceptionHandlingInterceptor(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                var result= await continuation(request, context);
                if (!_unitOfWork.Disposed)
                {
                    await _unitOfWork.CommitAsync();
                }
                return result;
            }
            catch (ArgumentException ex)
            {
                await _unitOfWork.RollBackAsync();

                LogCall.LogException(ex);
                throw new RpcException(new Status(StatusCode.Internal, "An unexpected error occurred"));

            }
            catch (CrudPeople.CoreDomain.Helper.CrudPeopleException ex)
            {
                await _unitOfWork.RollBackAsync();

                LogCall.LogException(ex);
                if (ex.ExceptionType == CoreDomain.Enums.ExceptionType.Validation)
                    throw new RpcException(new Status(StatusCode.Unavailable, ex.Message));
                if (ex.ExceptionType == CoreDomain.Enums.ExceptionType.NotFound)
                {
                    throw new RpcException(new Status(StatusCode.NotFound, ex.Message));

                }
                throw;
            }
            catch (RpcException ex)
            {
                await _unitOfWork.RollBackAsync();

                LogCall.LogException(ex);
                throw;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackAsync();

                LogCall.LogException(ex);
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }

        }
    }
}