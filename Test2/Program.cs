using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test2
{
    class Program
    {
        static void Main(string[] args)
        {
            Poker p = new Poker();
            p.AddPlayer("sinem", "QS, 10S, KC, JC, QH");
            p.AddPlayer("sinan", "7H, 8H, 9H, 10H, 3H");
            p.AddPlayer("feon", "QS, 10D, 8C, JC, QH");
            string winner = p.FindWinner();
            Console.WriteLine(winner);
            Console.ReadLine();
        }
    }
}
