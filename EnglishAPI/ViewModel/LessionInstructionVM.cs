using EnglishAPI.Data;

namespace EnglishAPI.ViewModel
{
    public class LessionInstructionVM
    {
        public string title { get; set; }   
        public List<VocabVM>? vocabularies { get; set; }
        public List<GrammarVM>? grammars { get; set; }
    }
}
