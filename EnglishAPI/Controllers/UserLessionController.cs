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
        [HttpPut("comment")]
        public async Task<IActionResult> UpdateComment(UserLessionVM vm)
        {
            var uid = User.FindFirst("Id").Value;

            var userLession = await _ctx.UserLessions.FirstOrDefaultAsync(a => a.LessionId == vm.LessionID && a.UserId.ToString() == uid);
            if (userLession == null)
            {
                return BadRequest("sai roi nha");
            }
            else
            {
                userLession.CommentDate = DateOnly.FromDateTime(DateTime.Now);
                userLession.Comment = vm.Comment;

                _ctx.Update(userLession);
                await _ctx.SaveChangesAsync();
                
                return Ok();
            }
            
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
                        CompleteDate = DateOnly.FromDateTime(DateTime.Now),
                        Status = "fail"
                    };
                    if(vm.Score >= 1000)
                    {
                        lession.Status = "pass";
                    }
                    _ctx.Add(lession);
                    await _ctx.SaveChangesAsync();
                }
                else
                {
                    if (lession.HighScore <= vm.Score)
                    {
                        if(vm.Score >= 1000)
                        {
                            lession.Status = "pass";
                        }
                        lession.HighScore = vm.Score;
                        lession.CompleteDate = DateOnly.FromDateTime(DateTime.Now);
                    }
                    await _ctx.SaveChangesAsync();
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
