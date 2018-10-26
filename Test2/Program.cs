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
            p.AddPlayer("sinem", "4H, 6D, 6D, 8D, QC");
            p.AddPlayer("sinan", "KH, 10S, 10D, 2H, 9H");
            p.AddPlayer("feon", "QS, 10H, 10C, JC, 3H");
            string winner = p.FindWinner();
            Console.WriteLine(winner);
            Console.ReadLine();
        }
    }
}
