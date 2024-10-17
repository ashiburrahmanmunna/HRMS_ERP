using GTERP.Models.Letter;

namespace GTERP.Interfaces.Letter
{
    public interface IAbsentLetterRepository
    {
        public string Print(int? id, string letterType, string type = "pdf");
        public string AllLetter(AllLetter AllLettermodel);
    }
}
