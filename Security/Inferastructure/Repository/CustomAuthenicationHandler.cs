﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Security.Inferastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Security.Inferastructure.Repository
{
    public class BasicAuthenticationOptions : AuthenticationSchemeOptions
    {

    }
    public class CustomAuthenicationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private ICustomAuthicationManager customAuthicationManager;

        public CustomAuthenicationHandler(
            IOptionsMonitor<BasicAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ICustomAuthicationManager customAuthicationManager) :
            base(options, logger, encoder, clock)
        {
            this.customAuthicationManager = customAuthicationManager;
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Unauthorized");

            string authorizationHeader = Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authorizationHeader))
                return AuthenticateResult.Fail("Unauthorized");

            if (!authorizationHeader.StartsWith("bearer", StringComparison.OrdinalIgnoreCase))
                return AuthenticateResult.Fail("Unauthorized");


            string token = authorizationHeader.Substring("bearer".Length).Trim();

            if (string.IsNullOrEmpty(token))
                return AuthenticateResult.Fail("Unauthorized");
            try
            {
                return ValidateToken(token);
            }
            catch (Exception ex)
            {

                return AuthenticateResult.Fail(ex.Message);
            }
        }
        private AuthenticateResult ValidateToken(string token)
        {
            var validatedToken = customAuthicationManager.Tokens.FirstOrDefault(t => t.Key == token);
            if (validatedToken.Key = null)
            {
                return AuthenticateResult.Fail("Unauthorized");
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,validatedToken.Value)
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new GenericPrincipal(identity,null);
            var ticket = new AuthenticationTicket(principal,Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }

    }
}
