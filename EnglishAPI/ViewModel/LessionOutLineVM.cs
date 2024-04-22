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
        public List<UserLession> UserLessions { get; set; }
        public bool canComment { get; set; }
        public bool canTest {  get; set; }


    }
}
