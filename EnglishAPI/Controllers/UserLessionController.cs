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
    [Authorize]
    public class UserLessionController : ControllerBase
    {
        private readonly EnglishWebContext _ctx;

        public UserLessionController(EnglishWebContext ctx)
        {
            _ctx = ctx;
        }

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

        [HttpGet("testoutline/{lessionId}")]
        public async Task<IActionResult> GetTestOutline(int lessionId)
        {
            var uid = User.FindFirst("Id").Value;
            var response = new TestOutLineVm();
            var doneVocab = _ctx.UserProgresses.Where(a => a.UserId.ToString() == uid && a.Vocab!.LessionId == lessionId).Count();
            var doneSentence = _ctx.UserProgresses.Where(a => a.UserId.ToString() == uid && a.Sentence!.LessionId == lessionId).Count();
            var doneReading = _ctx.UserProgresses.Where(a => a.UserId.ToString() == uid && a.Reading!.LessionId == lessionId).Count();

            var totalVocab = _ctx.Vocabularies.Where(a => a.LessionId == lessionId).Count();
            var totalSentence = _ctx.Sentences.Where(a=>a.LessionId == lessionId).Count();
            var totalReading = _ctx.Readings.Where(a => a.LessionId == lessionId).Count();

            response.doneReading = doneReading;
            response.doneSentence = doneSentence;
            response.doneVocab = doneVocab;
            response.totalVocab = totalVocab;
            response.totalReading=totalReading;
            response.totalSentence = totalSentence;
            return Ok(response);
        }

        [HttpGet("vocab/{lessionId}/{state}")]
        public async Task<IActionResult> GetVocab(int lessionId,string state)
        {
            var uid = User.FindFirst("Id").Value;
            var doneVocab = _ctx.UserProgresses.Where(a => a.UserId.ToString() == uid && a.Vocab!.LessionId == lessionId);
            if (state == "keep")
            {
                
      
                var leftOver = _ctx.Vocabularies.Where(a => a.LessionId == lessionId).Skip(doneVocab.Count());
                return Ok(leftOver);
            }
            else
            {
                _ctx.RemoveRange(doneVocab);
                await _ctx.SaveChangesAsync();
                var leftOver = _ctx.Vocabularies.Where(a => a.LessionId == lessionId);
                return Ok(leftOver);
            }
           
        }

        [HttpGet("sentence/{lessionId}/{state}")]
        public async Task<IActionResult> GetSentence(int lessionId, string state)
        {
            var uid = User.FindFirst("Id").Value;
            var doneSentence = _ctx.UserProgresses.Where(a => a.UserId.ToString() == uid && a.Sentence!.LessionId == lessionId);
            if (state == "keep")
            {
               
                var leftOver = _ctx.Sentences.Where(a => a.LessionId == lessionId).Skip(doneSentence.Count());
                return Ok(leftOver);
            }
            else
            {
                _ctx.RemoveRange(doneSentence);
                await _ctx.SaveChangesAsync();
                var leftOver = _ctx.Sentences.Where(a => a.LessionId == lessionId);
                return Ok(leftOver);
            }
           
        }

        [HttpGet("reading/{lessionId}/{state}")]
        public async Task<IActionResult> GetReading(int lessionId, string state)
        {
            var uid = User.FindFirst("Id").Value;
            var doneReading = _ctx.UserProgresses.Where(a => a.UserId.ToString() == uid && a.Reading!.LessionId == lessionId);
            if (state == "keep")
            {
                
                var leftOver = _ctx.Readings.Where(a => a.LessionId == lessionId).Skip(doneReading.Count());
                return Ok(leftOver);
            }
            else
            {
                _ctx.RemoveRange(doneReading);
                await _ctx.SaveChangesAsync();
                var leftOver = _ctx.Readings.Where(a => a.LessionId == lessionId);
                return Ok(leftOver);
            }
       
        }


        [HttpPost("vocab/update")]
        public async Task<IActionResult> UpdateVocab(UpdateSpecificVM parameters)
        {
            var uid = int.Parse(User.FindFirst("Id").Value);
            var vocab = await _ctx.Vocabularies.FirstOrDefaultAsync(a => a.VocabId == parameters.quesId);
            if (vocab == null) return BadRequest();
            
            var UserVocab = new UserProgress() { IsTrue =  parameters.isTrue, VocabId= parameters.quesId,UserId = uid };
            _ctx.Add(UserVocab);
            await _ctx.SaveChangesAsync();
            return Created();
        }

        [HttpPost("sentence/update")]
        public async Task<IActionResult> UpdateSentence(UpdateSpecificVM parameters)
        {
            var uid = int.Parse(User.FindFirst("Id").Value);
            var sentence = await _ctx.Sentences.FirstOrDefaultAsync(a => a.SenId == parameters.quesId);
            if (sentence == null) return BadRequest();

            var UserSen = new UserProgress() { IsTrue = parameters.isTrue, SentenceId = parameters.quesId, UserId = uid };
            _ctx.Add(UserSen);
            await _ctx.SaveChangesAsync();
            return Created();
        }

        [HttpPost("reading/update")]
        public async Task<IActionResult> UpdateReading(UpdateSpecificVM parameters)
        {
            var uid = int.Parse(User.FindFirst("Id").Value);
            var reading = await _ctx.Readings.FirstOrDefaultAsync(a => a.ReadId == parameters.quesId);
            if (reading == null) return BadRequest();

            var UserReading = new UserProgress() { IsTrue = parameters.isTrue, ReadingId = parameters.quesId, UserId = uid, AdditionalAnswer = parameters.additional };
            _ctx.Add(UserReading);
            await _ctx.SaveChangesAsync();
            return Created();
        }


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

public sealed class UpdateSpecificVM
{
   public string isTrue {  get; set; }
    public int quesId { get; set; }
    public string? additional { get; set; } 
}