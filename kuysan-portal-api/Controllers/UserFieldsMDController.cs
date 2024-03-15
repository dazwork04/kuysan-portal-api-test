using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAPB1SLayerWebAPI.Models;
using SAPB1SLayerWebAPI.Models.SLayer;
using SAPB1SLayerWebAPI.Services;

namespace SAPB1SLayerWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserFieldsMDController : ControllerBase
    {
        private readonly UserFieldsMDService sqService;
        public UserFieldsMDController() => sqService = new();

        // GET USER FIELD
        [HttpPost("GetUserFieldsMD/{userId}/{companyDB}")]
        public async Task<IActionResult> GetUserFieldsMD(int userId, string companyDB, List<UserFieldParam> parameter) =>
            Ok(await sqService.GetUserFieldsMDAsync(userId, companyDB, parameter));

        
    }
}
