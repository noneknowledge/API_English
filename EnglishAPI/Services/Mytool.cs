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
    }
}
