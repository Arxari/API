﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenShock.API.Models.Response;
using OpenShock.Common.Models;
using System.Net;

namespace OpenShock.API.Controller.Shares.Links;

public sealed partial class ShareLinksController
{
    /// <summary>
    /// Remove a shocker from a share link
    /// </summary>
    /// <param name="shareLinkId"></param>
    /// <param name="shockerId"></param>
    /// <response code="200">Successfully removed shocker</response>
    /// <response code="404">Share link or shocker does not exist</response>
    /// <response code="400">Shocker does not exist in share link</response>
    [HttpDelete("{shareLinkId}/{shockerId}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<BaseResponse<ShareLinkResponse>> ShareLinkShockerRemove([FromRoute] Guid shareLinkId, [FromRoute] Guid shockerId)
    {
        var exists = await _db.ShockerSharesLinks.AnyAsync(x => x.OwnerId == CurrentUser.DbUser.Id && x.Id == shareLinkId);
        if (!exists) return EBaseResponse<ShareLinkResponse>("Share link could not be found", HttpStatusCode.NotFound);

        var affected = await _db.ShockerSharesLinksShockers.Where(x => x.ShareLinkId == shareLinkId && x.ShockerId == shockerId)
            .ExecuteDeleteAsync();
        if (affected > 0)
            return new BaseResponse<ShareLinkResponse>
            {
                Message = "Successfully removed shocker"
            };

        return EBaseResponse<ShareLinkResponse>("Shocker does not exist in share link, consider adding a new one");
    }
}