using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MidWare.Extention
{
    public class Helper
    {
        public static int Ratings()
        {
            Random random = new Random();
            return random.Next(1, 5);
        }

        public static int RandomNumber()
        {
            Random random = new Random();
            return random.Next(1, 10);
        }

        public static string Badge()
        {
            Random random = new Random();
            string[] arr2 = { "Excellent", "Rising Start", "Star" };
            var index = random.Next(1, 3);
            return arr2[index];
        }
    }
}
