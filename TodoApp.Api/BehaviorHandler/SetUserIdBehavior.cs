using MediatR;
using TodoApp.Common.Interfaces;

namespace TodoApp.Api.BehaviorHandler;

public class SetUserIdBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SetUserIdBehavior(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("userId");
        
        if (userIdClaim is not null && Guid.TryParse(userIdClaim.Value, out var userId))
        {
            var userIdProperty = typeof(TRequest).GetProperty("UserId");
            if (userIdProperty != null && userIdProperty.PropertyType == typeof(Guid))
            {
                userIdProperty.SetValue(request, userId);
            }
        }

        return await next();
    }
}
