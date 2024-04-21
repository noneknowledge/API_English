using System.Text;

namespace EnglishAPI.Services
{
    public class Mytool
    {
        public static string GetRandom(int length = 5)
        {
            var pattern = @"1234567890QAZWSXEDCRFVTGBYHNUJMIKLOPqazwsxedcrfvtgbyhn@#$%";
            var rd = new Random();
            var sb = new StringBuilder();
            for (int i = 0; i < length; i++)
                sb.Append(pattern[rd.Next(0, pattern.Length)]);

            return sb.ToString();
        }
        public static List<T> ShuffleList<T>(List<T> lst)
        { 
            Random rand = new Random();

            for (int i =  - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                var temp = lst[i];
                lst[i] = lst[j];
                lst[j] = temp;
            }
            return lst;
        }
    }
}
