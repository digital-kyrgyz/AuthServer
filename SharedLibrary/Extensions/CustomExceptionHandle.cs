using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using SharedLibrary.Dtos;
using SharedLibrary.Exceptions;

namespace SharedLibrary.Extensions;

public static class CustomExceptionHandle
{
    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(configure =>
        {
            configure.Run(async context =>
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (errorFeature != null)
                {
                    var exceptions = errorFeature.Error;
                    ErrorDto errorDto = null;
                    if (exceptions is CustomException)
                    {
                        errorDto = new ErrorDto(exceptions.Message, true);
                    }
                    else
                    {
                        errorDto = new ErrorDto(exceptions.Message, false);
                    }

                    var response = Response<NoDataDto>.Fail(errorDto, 500);
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                }
            });
        });
    }
}