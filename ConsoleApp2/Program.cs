using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ConsoleApp2
{
    public struct Card
    {
        public int rank;
        public char suit;

        public Card(int p1, char p2)
        {
            rank = p1;
            suit = p2;
        }
    }

    public class Poker
    {
        public string winner = "";
        int cntUser = 0;
        public Poker() { }

        public Poker(int p1)
        {
            cntUser = p1;
        }

        public bool CheckFlush(Dictionary<string, Card[]> game)
        {
            List<string> fList = new List<string>();
            foreach (KeyValuePair<string, Card[]> g in game)
            {
                if (g.Value[0].suit == g.Value[1].suit
                    && g.Value[0].suit == g.Value[2].suit
                    && g.Value[0].suit == g.Value[3].suit
                    && g.Value[0].suit == g.Value[4].suit)
                {
                    fList.Add(g.Key);
                }
            }
            if (fList.Count > 0)
            {
                foreach (string nam in fList)
                {
                    if (winner == "")
                    {
                        winner = nam;
                    }
                    else
                    {
                        for (int j = 4; j >= 0; j--)
                        {
                            if (game[nam][j].rank > game[winner][j].rank)
                            {
                                winner = nam;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                winner = "";
                return false;
            }
            return true;
        }

        public bool Check3OfAKind(Dictionary<string, Card[]> game)
        {
            int j = 0, z = 0;
            Dictionary<string, int> fList = new Dictionary<string, int>();
            foreach (KeyValuePair<string, Card[]> g in game)
            {
                for (j = 4; j >= 2; j--)
                {
                    if (g.Value[j].rank == g.Value[j - 1].rank && g.Value[j - 1].rank == g.Value[j - 2].rank)
                    {
                        fList.Add(g.Key, g.Value[j].rank);
                        break;
                    }
                }
            }
            if (fList.Count > 0)
            {
                foreach (KeyValuePair<string, int> sub in fList)
                {
                    if (winner == "")
                    {
                        winner = sub.Key;
                    }
                    else
                    {
                        if (sub.Value > fList[winner])
                        {
                            winner = sub.Key;
                        }
                        else if (sub.Value == fList[winner])
                        {
                            //check the kickers
                            int tmp = 0, pntr = 4;
                            for (j = 4; j >= 0; j--)
                            {
                                if (game[winner][j].rank == fList[winner])
                                    j = j - 3;
                                //if the value equals to 3ofakind iterate 3 indice
                                tmp = game[winner][j].rank;
                                for (z = pntr; z >= 0; z--)
                                {
                                    if (game[sub.Key][z].rank == fList[sub.Key])
                                        z = z - 3;
                                    if (game[sub.Key][z].rank > tmp)
                                    {
                                        winner = sub.Key;
                                        j = -1;
                                        break;
                                    }
                                    else if (tmp > game[sub.Key][z].rank)
                                    {
                                        j = -1;
                                        break;
                                    }
                                    else
                                    {
                                        //if they're equal, set 2nd arrays pointer to where it lasted.
                                        pntr = z - 1;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                winner = "";
                return false;
            }
            return true;
        }

        public bool CheckOnePair(Dictionary<string, Card[]> game)
        {
            int j = 0, z = 0;
            Dictionary<string, int> fList = new Dictionary<string, int>();
            foreach (KeyValuePair<string, Card[]> g in game)
            {
                for (j = 4; j >= 1; j--)
                {
                    if (g.Value[j].rank == g.Value[j - 1].rank)
                    {
                        fList.Add(g.Key, g.Value[j].rank);
                        break;
                    }
                }
            }
            if (fList.Count > 0)
            {
                foreach (KeyValuePair<string, int> sub in fList)
                {
                    if (winner == "")
                    {
                        winner = sub.Key;
                    }
                    else
                    {
                        if (sub.Value > fList[winner])
                        {
                            winner = sub.Key;
                        }
                        else if (sub.Value == fList[winner])
                        {
                            //check the kickers
                            int tmp = 0, pntr = 4;
                            for (j = 4; j >= 0; j--)
                            {
                                if (game[winner][j].rank == fList[winner])
                                    j = j - 2;
                                //if the value equals to 3ofakind iterate 3 indice
                                tmp = game[winner][j].rank;
                                for (z = pntr; z >= 0; z--)
                                {
                                    if (game[sub.Key][z].rank == fList[sub.Key])
                                        z = z - 2;
                                    if (game[sub.Key][z].rank > tmp)
                                    {
                                        winner = sub.Key;
                                        j = -1;
                                        break;
                                    }
                                    else if (tmp > game[sub.Key][z].rank)
                                    {
                                        j = -1;
                                        break;
                                    }
                                    else
                                    {
                                        //if they're equal, set 2nd arrays pointer to where it lasted.
                                        pntr = z - 1;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                winner = "";
                return false;
            }
            return true;
        }

        public bool CheckHighCard(Dictionary<string, Card[]> game)
        {
            int j = 0, equal = 0;
            for (j = 4; j >= 0; j--)
            {
                foreach (KeyValuePair<string, Card[]> g in game)
                {
                    if (winner == "")
                    {
                        winner = g.Key;
                    }
                    else
                    {
                        if (g.Value[j].rank > game[winner][j].rank)
                        {
                            winner = g.Key; equal = 0;
                        }
                        else if (g.Value[j].rank == game[winner][j].rank)
                        {
                            equal = 1;
                        }
                    }
                }
                if (equal == 0 && winner != "") {
                    break;
                }else if (equal == 1) { winner = ""; }
            }
            return true;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            int i = 0, k = 0, numOfPlayers = 0;
            Poker _poker = new Poker();
            Dictionary<string, int> cardRanks = new Dictionary<string, int>();
            Dictionary<string, int> suitRanks = new Dictionary<string, int>();
            Dictionary<string, Card[]> users = new Dictionary<string, Card[]>();

            cardRanks.Add("A", 14);
            cardRanks.Add("K", 13);
            cardRanks.Add("Q", 12);
            cardRanks.Add("J", 11);
            for (i = 10; i > 1; i--)
            {
                cardRanks.Add(i.ToString(), i);
            }

            Console.Write("How many players? : ");
            numOfPlayers = Int32.Parse(Console.ReadLine());

            i = 0;
            String pattern = @"\W+";
            while (i < numOfPlayers)
            {
                Console.Write("{0} Player Name : ", i + 1); string player = Console.ReadLine();
                Console.Write("{0} Player Hand : ", i + 1); string hand = Console.ReadLine();

                k = 0;
                Card[] c = new Card[5];
                foreach (string sub in Regex.Split(hand, pattern))
                {
                    string[] x = Regex.Split(sub, "(?=[HCDS])");
                    cardRanks.TryGetValue(x[0], out c[k].rank);
                    c[k].suit = x[1][0];
                    k++;
                }
                Array.Sort<Card>(c, (x, y) => x.rank.CompareTo(y.rank));
                users.Add(player, c);
                i++;
            }

            if (_poker.CheckFlush(users))
                Console.WriteLine(_poker.winner);
            else if (_poker.Check3OfAKind(users))
                Console.WriteLine(_poker.winner);
            else if (_poker.CheckOnePair(users))
                Console.WriteLine(_poker.winner);
            else if (_poker.CheckHighCard(users))
                Console.WriteLine(_poker.winner);


            String name = Console.ReadLine();
        }
    }
}
