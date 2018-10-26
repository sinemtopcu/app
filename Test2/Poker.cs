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
        Dictionary<string, int> fList = new Dictionary<string, int>();
        string winner = "";
        const int HAND_SIZE = 5;

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
            int i = 1, j = 0;
            bool tie = false;
            foreach (KeyValuePair<string, Player> g in users)
            {
                i = 1;
                foreach (Card c in g.Value._hand.card)
                {
                    if (g.Value._hand.card[0].Suit != c.Suit)
                    { i = 0; break; }
                }

                if (i == 1)
                    fList.Add(g.Key);
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
                        tie = true; //keep it to check 2 flush can have same rank maybe
                        //KS, QS, 10S, 9S, 8S
                        //KD, QD, 10D, 9D, 8D
                        for (j = HAND_SIZE - 1; j >= 0; j--)
                        {
                            if (users[winner]._hand.card[j].Rank > users[nam]._hand.card[j].Rank)
                            {
                                tie = false;
                                break;
                                //cards are already sorted, don't have to go through all elements
                            }
                            else if (users[winner]._hand.card[j].Rank < users[nam]._hand.card[j].Rank)
                            {
                                tie = false;
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

            if (tie)
                return false;
            return true;
        }

        private bool Check3OfAKind()
        {
            fList.Clear();
            foreach (KeyValuePair<string, Player> g in users)
            {
                /*
                    possible triplets - already sorted
                     0 , 1 , 2 , 3 , 4
                     9S, 8D, 8S, 8H, 7C
                     8D, 8S, 8H, 7C, 4C
                     KH, QH, 8D, 8S, 8H
                */
                if (g.Value._hand.card[1].Rank == g.Value._hand.card[3].Rank
                    || g.Value._hand.card[0].Rank == g.Value._hand.card[2].Rank
                    || g.Value._hand.card[2].Rank == g.Value._hand.card[4].Rank)
                {
                    fList.Add(g.Key, g.Value._hand.card[2].Rank);
                    break;
                }
            }
            return CheckKickers(3);
        }

        public bool CheckOnePair()
        {
            int j = 0;
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
            return CheckKickers(2);
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

        public bool CheckKickers(int nOfAKind)
        {
            int j = 0, z = 0;
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
                                    j = j - nOfAKind;
                                //if the value equals to nofakind iterate n indice
                                tmp = users[winner]._hand.card[j].Rank;
                                for (z = pntr; z >= 0; z--)
                                {
                                    if (users[sub.Key]._hand.card[z].Rank == fList[sub.Key])
                                        z = z - nOfAKind;
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
