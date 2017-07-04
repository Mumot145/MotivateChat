using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MotivationAdmin.Models
{
    public class RandomGenerator
    {
        private static Random random = new Random();
        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
