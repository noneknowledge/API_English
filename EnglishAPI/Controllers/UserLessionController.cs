using EnglishAPI.Data;
using EnglishAPI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.X86;

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

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateScore(UserLessionVM vm)
        {
            var uidStr = User.FindFirst("Id").Value;
            if (int.TryParse(uidStr, out var id)) {
                var lession = await _ctx.UserLessions.FirstOrDefaultAsync(a => a.UserId == id && a.LessionId == vm.LessionID);
                if (lession == null)
                {
                    lession = new UserLession()
                    {
                        UserId = id,
                        LessionId = vm.LessionID,
                        HighScore = vm.Score,
                        CompleteDate = DateOnly.FromDateTime(DateTime.Now)
                    };
                }
                else
                {
                    if (lession.HighScore <= vm.Score)
                    {
                        lession.HighScore = vm.Score;
                        lession.CompleteDate = DateOnly.FromDateTime(DateTime.Now);
                    }
                }

                return Ok(lession);
            }
            else
            {
                return BadRequest("Lỗi mã người dùng vui lòng đăng nhập lại hoặc thử lại sau");
            }
           
        }
    }
}
