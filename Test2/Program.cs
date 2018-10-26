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
            p.AddPlayer("sinem", "KS, QS, 10S, 9S, 8S");
            p.AddPlayer("sinan", "KD, QD, 10D, 9D, 8D");
            p.AddPlayer("feon", "QS, 10D, 8C, JC, QH");
            string winner = p.FindWinner();
            Console.WriteLine(winner);
            Console.ReadLine();
        }
    }
}
