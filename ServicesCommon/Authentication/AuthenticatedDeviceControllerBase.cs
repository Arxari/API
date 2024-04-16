﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OpenShock.Common.OpenShockDb;

namespace OpenShock.ServicesCommon.Authentication;

[Authorize(AuthenticationSchemes = OpenShockAuthSchemas.DeviceToken)]
public class AuthenticatedDeviceControllerBase : OpenShockControllerBase, IActionFilter
{
    public Device CurrentDevice = null!;

    [NonAction]
    public void OnActionExecuting(ActionExecutingContext context)
    {
        CurrentDevice = ControllerContext.HttpContext.RequestServices.GetRequiredService<IClientAuthService<Device>>()
            .CurrentClient;
    }

    [NonAction]
    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}