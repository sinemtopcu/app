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
            p.AddPlayer("sinem", "KH, 8S, 5D, 2H, 9H");
            p.AddPlayer("sinan", "4H, 7D, 6D, 8D, QC");
            p.AddPlayer("feon", "3S, KH, 9D, 6H, 5H");
            string winner = p.FindWinner();
            Console.WriteLine(winner);
            Console.ReadLine();
        }
    }
}
