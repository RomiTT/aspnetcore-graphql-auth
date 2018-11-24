using System.Security.Claims;
using System.Threading.Tasks;
using aspnetcore_graphql_auth.GraphQL.Authentication;
using GraphQL.Server;
using GraphQL.Server.Transports.Subscriptions.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace aspnetcore_graphql_auth.GraphQL {
    public class WebSocketListener : IOperationMessageListener {
        AppSettings _appSettings;
        bool _isAuthenticated = false;

        public WebSocketListener(AppSettings appSettings) {
            _appSettings = appSettings;
        }

        public Task BeforeHandleAsync(MessageHandlingContext context) {
            if (context.Message.Type == MessageType.GQL_CONNECTION_INIT) {
                // Get connectionParams.
                var token = context.Message.Payload.Value<string>("token");
                if (string.IsNullOrEmpty(token) || !ValidateToken(token)) {
                    _isAuthenticated = false;
                    Log.Logger.Error("Terminate connection because of unauthorized access.");
                    return Task.FromException(new System.Exception("Terminate connection because of unauthorized access."));
                    //context.Terminated = true;
                    //return context.Terminate();
                }

                _isAuthenticated = true;
            }

            return Task.FromResult(true);
        }

        public Task HandleAsync(MessageHandlingContext context) {
            return Task.CompletedTask;
        }

        public Task AfterHandleAsync(MessageHandlingContext context) {
            return Task.CompletedTask;
        }

        protected bool ValidateToken(string token) {
            try {
                // var result = JWTTokenValidator.ValidateAndDecode(token, _appSettings.Secret);
                // Claim emailClaim = null;
                // foreach (var claim in result.Claims) {
                //     if (claim.Type == ClaimTypes.Email) {
                //         emailClaim = claim;
                //         break;
                //     }
                // }
                JWTTokenValidator.ValidateAndDecode(token, _appSettings.Secret);
                return true;
            }
            catch { }

            return false;
        }
    }
}