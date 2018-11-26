using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Bowgum.GraphQL.Authentication {
    public static class JWTTokenValidator {
        public static JwtSecurityToken ValidateAndDecode(string jwt, string secret) {
            var key = Encoding.ASCII.GetBytes(secret);
            var validationParameters = new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            try {
                var claimsPrincipal = new JwtSecurityTokenHandler()
                    .ValidateToken(jwt, validationParameters, out var rawValidatedToken);

                return (JwtSecurityToken)rawValidatedToken;
            }
            catch (SecurityTokenValidationException stvex) {
                throw new Exception($"Token failed validation: {stvex.Message}");
            }
            catch (ArgumentException argex) {
                throw new Exception($"Token was invalid: {argex.Message}");
            }
        }

    }
}