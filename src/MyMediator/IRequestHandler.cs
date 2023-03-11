﻿namespace MyMediator
{
    public interface IRequestHandler<in TRequest> where TRequest : IRequest
    {
        void Handle(TRequest request);
    }

    public interface IRequestHandler<in TRequest, out TResponse> 
        where TRequest : IRequest<TResponse>
        where TResponse : IResponse
    {
        TResponse Handle();
    }
}