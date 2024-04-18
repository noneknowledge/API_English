using EnglishAPI.Data;
using EnglishAPI.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnglishAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLessionController : ControllerBase
    {
        private readonly EnglishWebContext _ctx;

        public UserLessionController(EnglishWebContext ctx) 
        {
            _ctx = ctx;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateScore(UserLessionVM vm)
        {
            var lession = await _ctx.UserLessions.FirstOrDefaultAsync(a=>a.UserId == vm.UID & a.LessionId == vm.LessionID);
            if (lession == null)
            {
                lession = new UserLession()
                {
                    UserId = vm.UID,
                    LessionId = vm.LessionID,
                    Comment = vm.Comment,
                    HighScore = vm.Score,
                };
            }

            return Ok(lession);
        }
    }
}
