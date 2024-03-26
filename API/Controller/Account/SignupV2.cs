﻿using Microsoft.AspNetCore.Mvc;
using OpenShock.API.Models.Requests;
using OpenShock.Common.Models;
using System.Net;
using Asp.Versioning;
using OpenShock.API.Services.Account;
using OpenShock.ServicesCommon.Errors;
using OpenShock.ServicesCommon.Problems;
using OpenShock.ServicesCommon.Services.Turnstile;

namespace OpenShock.API.Controller.Account;

public sealed partial class AccountController
{
    /// <summary>
    /// Signs up a new user
    /// </summary>
    /// <param name="body"></param>
    /// <param name="accountService"></param>
    /// <param name="turnstileService"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">User successfully signed up</response>
    /// <response code="400">Username or email already exists</response>
    [HttpPost("signup")]
    [ProducesSuccess]
    [ProducesProblem(HttpStatusCode.Conflict, "EmailOrUsernameAlreadyExists")]
    [ProducesProblem(HttpStatusCode.Forbidden, "InvalidTurnstileResponse")]
    [MapToApiVersion("2")]
    public async Task<IActionResult> SignUpV2([FromBody] SignUpV2 body,
        [FromServices] IAccountService accountService, [FromServices] ICloudflareTurnstileService turnstileService,
        CancellationToken cancellationToken)
    {
        if (APIGlobals.ApiConfig.Turnstile.Enabled)
        {
            var turnStile = await turnstileService.VerifyUserResponseToken(body.TurnstileResponse,
                HttpContext.Connection.RemoteIpAddress, cancellationToken);
            if (!turnStile.IsT0) return Problem(TurnstileError.InvalidTurnstile);
        }

        var creationAction = await accountService.Signup(body.Email, body.Username, body.Password);
        if (creationAction.IsT1)
            return Problem(SignupError.EmailAlreadyExists);

        return RespondSuccessSimple("Successfully signed up");
    }
}