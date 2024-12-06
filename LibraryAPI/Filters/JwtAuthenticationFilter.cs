using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;
using Microsoft.IdentityModel.Tokens;

public class JwtAuthenticationFilter : IAuthenticationFilter
{
    public bool AllowMultiple => false;

    public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
    {
        if (context.ActionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any() ||
            context.ActionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
        {
            return;
        }

        var request = context.Request;
        var authorization = request.Headers.Authorization;

        if (authorization == null || authorization.Scheme != "Bearer")
        {
            context.ErrorResult = new AuthenticationFailureResult("Missing or invalid authorization header", request);
            return;
        }

        var token = authorization.Parameter;
        if (string.IsNullOrEmpty(token))
        {
            context.ErrorResult = new AuthenticationFailureResult("Missing token", request);
            return;
        }

        try
        {
            var principal = ValidateToken(token);
            context.Principal = principal;
        }
        catch (Exception ex)
        {
            context.ErrorResult = new AuthenticationFailureResult($"Invalid token: {ex.Message}", request);
        }
    }

    public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
    {
        return Task.FromResult(0);
    }

    private ClaimsPrincipal ValidateToken(string token)
    {
        var secretKey = Encoding.UTF8.GetBytes("1234567890123456");
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "yourIssuer",
            ValidateAudience = true,
            ValidAudience = "yourAudience",
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secretKey),
            ValidateLifetime = true, 
            ClockSkew = TimeSpan.Zero
        };

        var handler = new JwtSecurityTokenHandler();
        SecurityToken validatedToken;
        var principal = handler.ValidateToken(token, validationParameters, out validatedToken);
        return principal;
    }
}

public class AuthenticationFailureResult : IHttpActionResult
{
    private string ReasonPhrase;
    private HttpRequestMessage Request;

    public AuthenticationFailureResult(string reasonPhrase, HttpRequestMessage request)
    {
        ReasonPhrase = reasonPhrase;
        Request = request;
    }

    public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
    {
        var response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized)
        {
            RequestMessage = Request,
            ReasonPhrase = ReasonPhrase
        };
        return Task.FromResult(response);
    }
}