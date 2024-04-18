using EnglishAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnglishAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvatarController : ControllerBase
    {
        private readonly EnglishWebContext _ctx;

        public AvatarController(EnglishWebContext ctx)
        {
            _ctx = ctx;    
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAvatar() 
        {
            try
            {
                var avatar = await _ctx.Avatars.ToListAsync();
                return Ok(avatar);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        [HttpPut]
        public async Task<IActionResult> ChangeAvatar(int AvatarID)
        {
            var avatar = _ctx.Avatars.FirstOrDefault(a=>a.AvatarId == AvatarID);
            return Ok(avatar);
        }
    }
}
