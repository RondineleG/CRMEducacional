﻿namespace CRMEducacional.Core.Interfaces.UseCases;

public interface IUseCase<TRequest, TResponse>
{
    Task<CustomResult<TResponse>> Execute(TRequest request);
}