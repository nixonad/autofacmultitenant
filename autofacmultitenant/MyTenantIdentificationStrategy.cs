using System;
using System.Linq;
using Autofac.Multitenant;
using Microsoft.AspNetCore.Http;

namespace autofacmultitenant
{
  internal class MyTenantIdentificationStrategy : ITenantIdentificationStrategy
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MyTenantIdentificationStrategy(IHttpContextAccessor httpContextAccessor) {
      _httpContextAccessor = httpContextAccessor;
    }

    public Boolean TryIdentifyTenant(out Object tenantId) {
      if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null) {
        //System.Security.Claims.Claim tenantIdClaim = _httpContextAccessor.HttpContext.User.Claims.SingleOrDefault((claim) => claim.Type == "https://blahblah/tenantid");
        Console.WriteLine(_httpContextAccessor.HttpContext.User.Claims.Count());
        tenantId = "1";
        return true;
      }
      tenantId = null;
      return false;
    }
  }
}