using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
// using System.Text.RegularExpressions;

namespace SubtitleUtil
{

    public class Card{
        protected int id;
        public int Id { 
            get { return id; } 
            set { id = value; } 
        }
        protected string text;
        public string Text { 
            get { return text; } 
            set { text = value; } 
        }
        public Card(int i, string s){
            id = i;
            text = s;
        }
        public bool isStartingWith(string s){
            return text.StartsWith(s);
        }
 
    }
    public class CardManager
    {
        internal List<Card> list;
        public List<Card> ActiveCards;
        public List<Card> InactiveCards;
        public CardManager(){
            list = new List<Card>();
            ActiveCards = new List<Card>();
            InactiveCards = new List<Card>();
        }
        public void Add(Card c){
            list.Add(c);
        }
        public void GetListStaringWith(string s){
            ActiveCards.Clear();
            InactiveCards.Clear();
            foreach(Card c in list){
                InactiveCards.Add(c);
            }
            if (s.Length == 0) return;
            foreach(Card c in list){
                if (c.isStartingWith(s)){
                    ActiveCards.Add(c);
                    InactiveCards.Remove(c);
                }
            }            
        }
        public List<int> GetActiveCardIds(){
            List<int> ids = new List<int>();
            foreach(Card c in ActiveCards){
                ids.Add(c.Id);
            }
            return ids;
        }
        public List<int> GetInactiveCardIds(){
            List<int> ids = new List<int>();
            foreach(Card c in InactiveCards){
                ids.Add(c.Id);
            }
            return ids;
        }

    }

    public class Subtitle
    {
        public Subtitle()
        {
        }

        public float StartTime;
        public float EndTime;
        public string Text;
    }

    public class SubtitleManager
    {

        protected int id;
        public int Id { 
            get { return id; } 
            set { id = value; } 
        }
        internal List<Subtitle> subtitleList;
        // public List<Subtitle> subtitleList;
        public SubtitleManager(int _id)
        {
            id = _id;
            subtitleList = new List<Subtitle>();
        }

        public void Dump(){
            Console.WriteLine(id);
            foreach(Subtitle s in subtitleList){
                Console.WriteLine(s.StartTime);
                Console.WriteLine(s.EndTime);
                Console.WriteLine(s.Text);
            }
        }

        public virtual void LoadAnnotation(Stream s)
        {
            StreamReader sr = new StreamReader(s);
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] ele = line.Split('\t');
                Subtitle subtitle = new Subtitle();
                subtitle.StartTime = float.Parse(ele[0]);
                subtitle.EndTime = float.Parse(ele[1]);
                subtitle.Text = ele[2];
                // subtitle.TextLength = txt.Length;
                subtitleList.Add(subtitle);
            }
        }

        public string GetSubtitleByPosition(float currentPosition){
            for (int i = 0; i < subtitleList.Count; i++)
            {
                float st = subtitleList[i].StartTime;
                float et = subtitleList[i].EndTime;
                if (st <= currentPosition && currentPosition < et){
                    return subtitleList[i].Text;
                }
            }
            //指定時刻に字幕がない            
            return "";
        }

    }
}

