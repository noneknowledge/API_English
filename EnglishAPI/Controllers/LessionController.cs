using EnglishAPI.Data;
using EnglishAPI.Services;
using EnglishAPI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnglishAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessionController : ControllerBase
    {
        private readonly EnglishWebContext _ctx;

        public LessionController(EnglishWebContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLession()
        {
            try
            {
                return Ok(await _ctx.Lessions.ToListAsync());
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
            
        }
        
        [HttpGet("outline/{id}")]
        public async Task<IActionResult> GetLessionOutLine(int id)
        {
            try
            {
                
                var lession = await _ctx.Lessions.FirstOrDefaultAsync(a => a.LessionId == id);
                if(lession != null)
                {
                    var lessionOutLine = new LessionOutLineVM();

                    lessionOutLine.LessionId = lession.LessionId;
                    lessionOutLine.Description = lession.Description;
                    lessionOutLine.Title = lession.Title;
                    lessionOutLine.Vietnamese = lession.Vietnamese;
                    lessionOutLine.Image = lession.Image;
                    lessionOutLine.Comments = await _ctx.UserLessions.Where(a => a.LessionId == id && a.Comment!.Length >0 ).Select(a=> new CommentVM
                    {
                       userName = a.User.UserName,
                       avatarImage = a.User.Avatar.Image,
                       comment = a.Comment,
                       commentDate = a.CommentDate,
                    }).ToListAsync();
                    lessionOutLine.canTest = false;
                    lessionOutLine.canComment = false;
                    lessionOutLine.topRank = await _ctx.UserLessions.Where(a=>a.LessionId == id).OrderByDescending(a => a.HighScore).ThenBy(a => a.CompleteDate).Take(5).Select(a=>new TopRankVM{ avatarImage= a.User.Avatar.Image, completeDate = a.CompleteDate, score = a.HighScore, userName= a.User.UserName}).ToListAsync();

                    if (User.Identity.IsAuthenticated)
                    {

                        var uid = User.FindFirst("Id").Value;
                        var userStatus  = await _ctx.UserLessions.FirstOrDefaultAsync(a => a.LessionId == id && a.UserId.ToString() == uid);
                        if (userStatus != null)
                        {
                            lessionOutLine.userScore = new UserScoreVM { completeDate = userStatus.CompleteDate, score = userStatus.HighScore };
                        }
                     
                        var preLessionId = _ctx.Lessions.Where(a => a.LessionId < id).OrderByDescending(a => a.LessionId).FirstOrDefault();
                        if (preLessionId == null)
                        {
                            lessionOutLine.canTest = true;
                        }
                        else
                        {
                            var preLession = await _ctx.UserLessions
                                .FirstOrDefaultAsync(a => a.LessionId == preLessionId.LessionId && a.UserId.ToString() == uid && a.Status.ToLower() == "pass");
                            if (preLession != null)
                            {
                                lessionOutLine.canTest = true;
                            }
                        }

                        var curULession = await _ctx.UserLessions
                            .FirstOrDefaultAsync(a => a.UserId.ToString() == uid && a.LessionId == id);

                        if (curULession != null && curULession.Status.ToLower() == "pass")
                        {
                            lessionOutLine.canComment = true;
                        }
                    }
                    return Ok(lessionOutLine);
                }
                else
                {
                    return BadRequest("Không tìm thấy lession này");
                }
                
            }
            catch (Exception e ) 
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet("instruction/{id}")]
        public async Task<IActionResult> GetLessionInstrucstion(int id)
        {
            try
            {

                var lession = await _ctx.Lessions.FirstOrDefaultAsync(a => a.LessionId == id);

                return Ok(lession);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetLession(int id)
        {
            var lession = await _ctx.Lessions.Include(a => a.Vocabularies).Include(a => a.Sentences).Include(a=>a.Readings).
                FirstOrDefaultAsync(a => a.LessionId == id);
            if(lession != null)
            {
                List<Vocabulary> vocab = lession.Vocabularies.Select(a => new Vocabulary() { Vocab = a.Vocab, Vietnamese = a.Vietnamese, VocabId = a.VocabId, Image = a.Image, LessionId = id ,WordClass=a.WordClass}).ToList();
                List<Reading> readings = lession.Readings.Select(a=>new Reading() { ReadId = a.ReadId, Paragraph = a.Paragraph, Question = a.Question, Answer = a.Answer, Question2 = a.Question2, Answer2 = a.Answer2, LessionId = a.LessionId }).ToList();
                List<Sentence> sentences = lession.Sentences.Select(a => new Sentence() { SenId = a.SenId, LessionId = a.LessionId, BlankSentence = a.BlankSentence, FillWord = a.FillWord, Hint = a.Hint, Vietnamese = a.Vietnamese }).ToList();

                
                lession.Vocabularies = vocab;
                lession.Readings = readings;
                lession.Sentences = sentences;

                return Ok(lession);
            }
            return BadRequest();
            
        }
    }
}
