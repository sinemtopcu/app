using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Test2
{
    class Card
    {
        public int Rank { get; set; }
        public char Suit { get; set; }

        public Card() { }

        public Card(int p1, char p2)
        {
            Rank = p1;
            Suit = p2;
        }
    }

    class Hand
    {
        public List<Card> card = new List<Card>();
        const string pattern = @"\W+";

        public Hand() { }

        public Hand(string p1)
        {
            seperate(p1);
        }

        public void seperate(string s)
        {
            CardRanks cr = new CardRanks();

            foreach (string sub in Regex.Split(s, pattern))
            {
                string[] x = Regex.Split(sub, "(?=[HCDS])");
                Card c = new Card(cr.getRank(x[0]), x[1][0]);
                card.Add(c);
            }
            card.Sort((x, y) => x.Rank.CompareTo(y.Rank));
        }
    }

    class Player
    {
        string _name;
        public Hand _hand;

        public Player() { }

        public Player(string p1, string p2)
        {
            _name = p1;
            _hand = new Hand(p2);
        }

        public string GetName()
        {
            return _name;
        }

        public void SetName(string name)
        {
            _name = name;
        }
    }

    class CardRanks
    {
        Dictionary<string, int> cardRanks = new Dictionary<string, int>();

        public CardRanks() { CreateRankings(); }

        public void addRank(string _rank, int val)
        {
            cardRanks.Add(_rank, val);
        }

        public int getRank(string _rank)
        {
            cardRanks.TryGetValue(_rank, out int val);
            return val;
        }

        public void CreateRankings()
        {
            int i = 0;
            addRank("A", 14);
            addRank("K", 13);
            addRank("Q", 12);
            addRank("J", 11);
            for (i = 10; i > 1; i--)
            {
                addRank(i.ToString(), i);
            }
        }
    }
    class Poker
    {
        Dictionary<string, Player> users = new Dictionary<string, Player>();
        string winner = "";

        public Poker()
        {

        }

        public void AddPlayer(string name, string hand)
        {
            Player p = new Player(name, hand);
            users.Add(name, p);
        }

        public Player GetPlayer(string name)
        {
            return users[name];
        }

        private bool CheckFlush()
        {
            List<string> fList = new List<string>();
            foreach (KeyValuePair<string, Player> g in users)
            {
                if (g.Value._hand.card[0].Suit == g.Value._hand.card[1].Suit
                    && g.Value._hand.card[0].Suit == g.Value._hand.card[2].Suit
                    && g.Value._hand.card[0].Suit == g.Value._hand.card[3].Suit
                    && g.Value._hand.card[0].Suit == g.Value._hand.card[4].Suit)
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
                            if (users[nam]._hand.card[j].Rank > users[winner]._hand.card[j].Rank)
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

        private bool Check3OfAKind()
        {
            int j = 0, z = 0;
            Dictionary<string, int> fList = new Dictionary<string, int>();
            foreach (KeyValuePair<string, Player> g in users)
            {
                for (j = 4; j >= 2; j--)
                {
                    if (g.Value._hand.card[j].Rank == g.Value._hand.card[j - 1].Rank
                        && g.Value._hand.card[j - 1].Rank == g.Value._hand.card[j - 2].Rank)
                    {
                        fList.Add(g.Key, g.Value._hand.card[j].Rank);
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
                                if (users[winner]._hand.card[j].Rank == fList[winner])
                                    j = j - 3;
                                //if the value equals to 3ofakind iterate 3 indice
                                tmp = users[winner]._hand.card[j].Rank;
                                for (z = pntr; z >= 0; z--)
                                {
                                    if (users[sub.Key]._hand.card[z].Rank == fList[sub.Key])
                                        z = z - 3;
                                    if (users[sub.Key]._hand.card[z].Rank > tmp)
                                    {
                                        winner = sub.Key;
                                        j = -1;
                                        break;
                                    }
                                    else if (tmp > users[sub.Key]._hand.card[z].Rank)
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

        public bool CheckOnePair()
        {
            int j = 0, z = 0;
            Dictionary<string, int> fList = new Dictionary<string, int>();
            foreach (KeyValuePair<string, Player> g in users)
            {
                for (j = 4; j >= 1; j--)
                {
                    if (g.Value._hand.card[j].Rank == g.Value._hand.card[j - 1].Rank)
                    {
                        fList.Add(g.Key, g.Value._hand.card[j].Rank);
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
                                if (users[winner]._hand.card[j].Rank == fList[winner])
                                    j = j - 2;
                                //if the value equals to 3ofakind iterate 3 indice
                                tmp = users[winner]._hand.card[j].Rank;
                                for (z = pntr; z >= 0; z--)
                                {
                                    if (users[sub.Key]._hand.card[z].Rank == fList[sub.Key])
                                        z = z - 2;
                                    if (users[sub.Key]._hand.card[z].Rank > tmp)
                                    {
                                        winner = sub.Key;
                                        j = -1;
                                        break;
                                    }
                                    else if (tmp > users[sub.Key]._hand.card[z].Rank)
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

        public bool CheckHighCard()
        {
            int j = 0, equal = 0;
            for (j = 4; j >= 0; j--)
            {
                foreach (KeyValuePair<string, Player> g in users)
                {
                    if (winner == "")
                    {
                        winner = g.Key;
                    }
                    else
                    {
                        if (g.Value._hand.card[j].Rank > users[winner]._hand.card[j].Rank)
                        {
                            winner = g.Key; equal = 0;
                        }
                        else if (g.Value._hand.card[j].Rank == users[winner]._hand.card[j].Rank)
                        {
                            equal = 1;
                        }
                    }
                }
                if (equal == 0 && winner != "")
                {
                    break;
                }
                else if (equal == 1) { winner = ""; }
            }
            return true;
        }

        public string FindWinner()
        {
            if (CheckFlush())
                return winner;
            else if (Check3OfAKind())
                return winner;
            else if (CheckOnePair())
                return winner;
            else if (CheckHighCard())
                return winner;
            return winner;
        }
    }
}
