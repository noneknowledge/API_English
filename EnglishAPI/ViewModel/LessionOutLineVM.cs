using EnglishAPI.Data;

namespace EnglishAPI.ViewModel
{
    public class LessionOutLineVM
    {
        public int LessionId { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? Vietnamese { get; set; }
        public List<CommentVM> Comments { get; set; }
        public List<TopRankVM> topRank {  get; set; }
        public bool canComment { get; set; }
        public bool canTest {  get; set; }
        public UserScoreVM userScore { get; set; }


    }
}
