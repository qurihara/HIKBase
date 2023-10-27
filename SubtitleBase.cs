using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
// using System.Drawing;

using System.Text.RegularExpressions;

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

        // public void GetMidTime()
        // {
        //     TimeSpan mt = EndTime.Subtract(StartTime);
        //     MidTime = StartTime.Add(new TimeSpan(mt.Ticks / 2));
        // }
        // public TimeSpan StartTime;
        // public TimeSpan EndTime;
        public float StartTime;
        public float EndTime;
        // public TimeSpan MidTime;
        // public System.Drawing.Rectangle Bounds;
        // public System.Drawing.RectangleF NomalBounds4_3;
        //public System.Drawing.RectangleF NomalBounds16_9;
        public string Text;
        // public int TextLength;
        // public string ImageFilePath;

        // public float GetReadableSpeed(float readableCharsPerMin)
        // {
        //     float result = 1f;
        //     TimeSpan dura = EndTime.Subtract(StartTime);
        //     if (File.Exists(ImageFilePath))
        //     {
        //         FileInfo fo = new FileInfo(ImageFilePath);
        //         long bmpSize = fo.Length;
        //         double length = 0.761766152 + 0.002459347 * (double)bmpSize;
        //         double neededSeconds = length / (double)readableCharsPerMin * 60d;
        //         result = (float)(dura.TotalSeconds / neededSeconds);
        //     }
        //     return result;
        // }
    }

    public class SubtitleManager
    {

        protected int id;
        public int Id { 
            get { return id; } 
            set { id = value; } 
        }
        // public TimeSpan mainDuration;
        // public TimeSpan subtitleDuration;
        // protected TimeSpan duration;
        // public TimeSpan Duration
        // {
        //     get { return duration; }
        //     set { duration = value; }
        // }

        // public enum Timing
        // {
        //     StartTime,
        //     MidTime,
        //     EndTime
        // }
        // protected Timing timing = Timing.MidTime;

        // public double ClockRatio
        // {
        //     get { return ClockRatio; }
        //     set { clockRatio = value; }
        // }
        // protected double clockRatio = 1d;
        // internal bool isActive = false;
        internal List<Subtitle> subtitleList;
        // public List<Subtitle> subtitleList;
        // internal int currentPosition;
        // protected TimeSpan currentStartTime;
        // //public event EventHandler SubtitleShown;
        // public event SubtitleShownEventHandler SubtitleShown;

        // public delegate void SubtitleShownEventHandler(object sender, SubtitleEventArgs e);
        // public class SubtitleEventArgs : EventArgs
        // {
        //     public SubtitleEventArgs(Subtitle s)
        //     {
        //         Subtitle = s;
        //     }
        //     public Subtitle Subtitle;
        // }

        // public SubtitleManager(Timing timing)
        //     : this()
        // {
        //     this.timing = timing;
        // }
        public SubtitleManager(int _id)
        {
            id = _id;
            subtitleList = new List<Subtitle>();
            // currentPosition = 0;
            // currentStartTime = new TimeSpan(0);

            // duration = new TimeSpan(0);
            // mainDuration = new TimeSpan(0);
            // subtitleDuration = new TimeSpan(0);
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

        // public virtual void LoadSON(Stream s)
        // {
        //     Regex reg = new System.Text.RegularExpressions.Regex(@"\d\d\d\d\t");
        //     StreamReader sr = new StreamReader(s);
        //     while (!sr.EndOfStream)
        //     {
        //         string line = sr.ReadLine();
        //         Match m = reg.Match(line);
        //         if (m.Success)// continue;
        //         //if (line.StartsWith("Display_Area"))
        //         {
        //             //string[] ele = line.Split('\t');
        //             //string bnds = ele[1].Replace("(", "").Replace(")", "");
        //             //string[] bnd = bnds.Split(' ');
        //             //int x = int.Parse(bnd[0]);
        //             //int y = int.Parse(bnd[1]);
        //             //int w = int.Parse(bnd[2])-x;
        //             //int h = int.Parse(bnd[3])-y;
        //             //System.Drawing.Rectangle r = new System.Drawing.Rectangle(x, y, w, h);                    

        //             //string line2 = sr.ReadLine();
        //             string line2 = line;
        //             string[] ele2 = line2.Split('\t');
        //             string[] ele3 = ele2[1].Split(':');
        //             TimeSpan st = new TimeSpan(0, int.Parse(ele3[0]), int.Parse(ele3[1]), int.Parse(ele3[2]), int.Parse(ele3[3]) * 10);
        //             string[] ele4 = ele2[2].Split(':');
        //             TimeSpan et = new TimeSpan(0, int.Parse(ele4[0]), int.Parse(ele4[1]), int.Parse(ele4[2]), int.Parse(ele4[3]) * 10);
        //             string txt = ele2[3];
        //             Subtitle subtitle = new Subtitle();
        //             //subtitle.Bounds = r;
        //             subtitle.StartTime = st;
        //             subtitle.EndTime = et;

        //             subtitle.GetMidTime();
        //             //TimeSpan mt = et.Subtract(st);
        //             //subtitle.MidTime = st.Add(new TimeSpan(mt.Ticks / 2));

        //             subtitle.Text = txt;
        //             subtitle.TextLength = txt.Length;
        //             subtitleList.Add(subtitle);
        //         }
        //     }

        //     //currentPosition = 0;
        //     //UpdateCurrentStartTime();
        //     isActive = true;
        // }

        // public virtual void LoadWINSub(Stream s)
        // {
        //     StreamReader sr = new StreamReader(s);
        //     while (!sr.EndOfStream)
        //     {
        //         char[] sepa = { '\t', ' ' };
        //         string line = sr.ReadLine();
        //         string[] ele = line.Split(sepa);//'\t');
        //         int x = int.Parse(ele[5]);
        //         int y = int.Parse(ele[6]);
        //         int w = int.Parse(ele[3]);
        //         int h = int.Parse(ele[4]);
        //         System.Drawing.Rectangle r = new System.Drawing.Rectangle(x, y, w, h);                    

        //         string[] ele3 = ele[1].Split(':');
        //         TimeSpan st = new TimeSpan(0, int.Parse(ele3[0]), int.Parse(ele3[1]), int.Parse(ele3[2]), int.Parse(ele3[3]) * 10);
        //         string[] ele4 = ele[2].Split(':');
        //         TimeSpan et = new TimeSpan(0, int.Parse(ele4[0]), int.Parse(ele4[1]), int.Parse(ele4[2]), int.Parse(ele4[3]) * 10);
        //         //string txt = ele2[3];
        //         Subtitle subtitle = new Subtitle();
        //         subtitle.ImageFilePath = ele[0];
        //         // subtitle.Bounds = r;
        //         subtitle.StartTime = st;
        //         subtitle.EndTime = et;

        //         // subtitle.NomalBounds4_3 = new RectangleF((float)(r.X) / 720f, (float)(r.Y) / 480f, (float)(r.Width) / 720f, (float)(r.Height) / 480f);
        //         //subtitle.NomalBounds16_9 = new RectangleF((float)(r.X) / 720f, (float)(r.Y) / 405f, (float)(r.Width) / 720f, (float)(r.Height) / 405f);

        //         if (st >= et) continue;
        //         subtitle.GetMidTime();
        //         //TimeSpan mt = et.Subtract(st);
        //         //subtitle.MidTime = st.Add(new TimeSpan(mt.Ticks / 2));

        //         //subtitle.Text = txt;
        //         //subtitle.TextLength = txt.Length;
        //         subtitleList.Add(subtitle);
        //     }

        //     currentPosition = 0;
        //     UpdateCurrentStartTime();
        //     isActive = true;
        // }

        // public virtual void LoadSrt(Stream s)
        // {
        //     StreamReader sr = new StreamReader(s);
        //     while (!sr.EndOfStream)
        //     {
        //         string num = sr.ReadLine();
        //         string timeS = sr.ReadLine();
        //         string sub = "";
        //         char[] tm = { '\n' };
        //         while (!sr.EndOfStream)
        //         {
        //             string li = sr.ReadLine();
        //             if (li == "") break;
        //             sub += li + "\n";
        //         }
        //         sub = sub.TrimEnd(tm);

        //         string[] sep = { " --> " };
        //         string[] time = timeS.Split(sep,StringSplitOptions.None);
        //         char[] sepa = { ':', ',' };
        //         string[] ele3 = time[0].Split(sepa);
        //         string[] ele4 = time[1].Split(sepa);

        //         TimeSpan st = new TimeSpan(0, int.Parse(ele3[0]), int.Parse(ele3[1]), int.Parse(ele3[2]), int.Parse(ele3[3]));
        //         TimeSpan et = new TimeSpan(0, int.Parse(ele4[0]), int.Parse(ele4[1]), int.Parse(ele4[2]), int.Parse(ele4[3]));
        //         Subtitle subtitle = new Subtitle();
        //         subtitle.StartTime = st;
        //         subtitle.EndTime = et;
        //         subtitle.Text = sub;

        //         if (st >= et) continue;
        //         subtitle.GetMidTime();
        //         subtitleList.Add(subtitle);
        //     }

        //     currentPosition = 0;
        //     UpdateCurrentStartTime();
        //     isActive = true;
        // }

        //現在は相対座標NormalBoundsを使っているので変換は不要になったのでつかっていない．
        //public void ConvertResolution(int srcWidth, int srcHeight, int destWidth, int destHeight)
        //{
        //    float aspX = (float)destWidth / (float)srcWidth;
        //    float aspY = (float)destHeight / (float)srcHeight;
        //    foreach (Subtitle subtitle in subtitleList)
        //    {
        //        subtitle.Bounds.X = (int)((float)subtitle.Bounds.X * aspX);
        //        subtitle.Bounds.Y = (int)((float)subtitle.Bounds.Y * aspY);
        //        subtitle.Bounds.Width = (int)((float)subtitle.Bounds.Width * aspX);
        //        subtitle.Bounds.Height = (int)((float)subtitle.Bounds.Height * aspY);
        //    }
        //}

        // public void ConvertSubtitleList(double mainVideoSpeed, double subtitleSpeed)
        // {
        //     TimeSpan current = new TimeSpan(0);
        //     TimeSpan newCurrent = new TimeSpan(0);
        //     foreach (Subtitle subtitle in subtitleList)
        //     {
        //         TimeSpan sDur = subtitle.StartTime.Subtract(current);
        //         TimeSpan dur = subtitle.EndTime.Subtract(subtitle.StartTime);
        //         TimeSpan newS = new TimeSpan((long)((double)sDur.Ticks / mainVideoSpeed));
        //         TimeSpan newD = new TimeSpan((long)((double)dur.Ticks / subtitleSpeed));
        //         current = subtitle.EndTime;

        //         newCurrent = newCurrent.Add(newS);
        //         subtitle.StartTime = newCurrent;
        //         newCurrent = newCurrent.Add(newD);
        //         subtitle.EndTime = newCurrent;
        //         subtitle.GetMidTime();
        //     }
        // }

        // public TimeSpan ConvertPlayPositionFromModifiedTimeToStandardTime(TimeSpan modifiedPlayPosition,double mainVideoSpeed,double subtitleSpeed)
        // {
        //     TimeSpan current = new TimeSpan(0);
        //     TimeSpan newCurrent = new TimeSpan(0);
        //     foreach (Subtitle subtitle in subtitleList)
        //     {
        //         TimeSpan sDur = subtitle.StartTime.Subtract(current);
        //         TimeSpan dur = subtitle.EndTime.Subtract(subtitle.StartTime);
        //         TimeSpan newS = new TimeSpan((long)((double)sDur.Ticks / mainVideoSpeed));
        //         TimeSpan newD = new TimeSpan((long)((double)dur.Ticks / subtitleSpeed));

        //         TimeSpan offM = new TimeSpan((long)((double)modifiedPlayPosition.Subtract(newCurrent).Ticks * mainVideoSpeed));
        //         newCurrent = newCurrent.Add(newS);
        //         if (modifiedPlayPosition < newCurrent) return current.Add(offM);
        //         current = subtitle.StartTime;

        //         TimeSpan offS = new TimeSpan((long)((double)modifiedPlayPosition.Subtract(newCurrent).Ticks * subtitleSpeed));
        //         newCurrent = newCurrent.Add(newD);
        //         if (modifiedPlayPosition < newCurrent) return current.Add(offS);
        //         current = subtitle.EndTime;
        //     }
        //     return current;
        // }
        // public TimeSpan ConvertPlayPositionFromStandardTimeToModifiedTime(TimeSpan standardPlayPosition, double mainVideoSpeed, double subtitleSpeed)
        // {
        //     TimeSpan current = new TimeSpan(0);
        //     TimeSpan newCurrent = new TimeSpan(0);
        //     foreach (Subtitle subtitle in subtitleList)
        //     {
        //         if (standardPlayPosition < subtitle.StartTime)
        //         {
        //             TimeSpan offM = new TimeSpan((long)((double)standardPlayPosition.Subtract(current).Ticks / mainVideoSpeed));
        //             return newCurrent.Add(offM);
        //         }
        //         TimeSpan sDur = subtitle.StartTime.Subtract(current);
        //         TimeSpan dur = subtitle.EndTime.Subtract(subtitle.StartTime);
        //         TimeSpan newS = new TimeSpan((long)((double)sDur.Ticks / mainVideoSpeed));
        //         TimeSpan newD = new TimeSpan((long)((double)dur.Ticks / subtitleSpeed));

        //         newCurrent = newCurrent.Add(newS);
        //         if (standardPlayPosition < subtitle.EndTime)
        //         {
        //             TimeSpan offS = new TimeSpan((long)((double)standardPlayPosition.Subtract(subtitle.StartTime).Ticks / subtitleSpeed));
        //             return newCurrent.Add(offS);
        //         }
        //         //current = subtitle.StartTime;
        //         newCurrent = newCurrent.Add(newD);
        //         current = subtitle.EndTime;

        //     }
        //     return newCurrent;
        // }

        // public bool Skip(TimeSpan playPosition, double mainVideoSpeed, double subtitleSpeed,out TimeSpan newPlayPosition)
        // {
        //     newPlayPosition = new TimeSpan(0);
        //     TimeSpan current = new TimeSpan(0);
        //     TimeSpan newCurrent = new TimeSpan(0);
        //     foreach (Subtitle subtitle in subtitleList)
        //     {
        //         TimeSpan sDur = subtitle.StartTime.Subtract(current);
        //         TimeSpan dur = subtitle.EndTime.Subtract(subtitle.StartTime);
        //         TimeSpan newS = new TimeSpan((long)((double)sDur.Ticks / mainVideoSpeed));
        //         TimeSpan newD = new TimeSpan((long)((double)dur.Ticks / subtitleSpeed));

        //         newCurrent = newCurrent.Add(newS);
        //         if (playPosition < newCurrent)
        //         {
        //             newPlayPosition = newCurrent;// ConvertPlayPositionFromStandardTimeToModifiedTime(subtitle.EndTime, mainVideoSpeed, subtitleSpeed);
        //             return true;
        //             //return false;
        //         }
        //         current = subtitle.StartTime;
        //         newCurrent = newCurrent.Add(newD);
        //         if (playPosition < newCurrent)
        //         {
        //             newPlayPosition = newCurrent;// ConvertPlayPositionFromStandardTimeToModifiedTime(subtitle.EndTime, mainVideoSpeed, subtitleSpeed);
        //             return true;
        //         }
        //         current = subtitle.EndTime;
        //     }
        //     return false;
        // }
        // public bool SkipBack(TimeSpan playPosition, double mainVideoSpeed, double subtitleSpeed, out TimeSpan newPlayPosition)
        // {
        //     newPlayPosition = new TimeSpan(0);
        //     TimeSpan current = new TimeSpan(0);
        //     TimeSpan newCurrent = new TimeSpan(0);
        //     foreach (Subtitle subtitle in subtitleList)
        //     {
        //         TimeSpan sDur = subtitle.StartTime.Subtract(current);
        //         TimeSpan dur = subtitle.EndTime.Subtract(subtitle.StartTime);
        //         TimeSpan newS = new TimeSpan((long)((double)sDur.Ticks / mainVideoSpeed));
        //         TimeSpan newD = new TimeSpan((long)((double)dur.Ticks / subtitleSpeed));

        //         TimeSpan prevNewCurrent = newCurrent;
        //         newCurrent = newCurrent.Add(newS);
        //         current = subtitle.StartTime;
        //         newCurrent = newCurrent.Add(newD);
        //         if (playPosition < newCurrent)
        //         {
        //             newPlayPosition = prevNewCurrent;
        //             return true;
        //         }
        //         current = subtitle.EndTime;
        //     }
        //     return false;
        // }
        // public bool SkipBackPlusOffset(TimeSpan playPosition, double mainVideoSpeed, double subtitleSpeed,int offsetSecond, out TimeSpan newPlayPosition)
        // {
        //     newPlayPosition = new TimeSpan(0);
        //     TimeSpan current = new TimeSpan(0);
        //     TimeSpan newCurrent = new TimeSpan(0);
        //     foreach (Subtitle subtitle in subtitleList)
        //     {
        //         TimeSpan sDur = subtitle.StartTime.Subtract(current);
        //         TimeSpan dur = subtitle.EndTime.Subtract(subtitle.StartTime);
        //         TimeSpan newS = new TimeSpan((long)((double)sDur.Ticks / mainVideoSpeed));
        //         TimeSpan newD = new TimeSpan((long)((double)dur.Ticks / subtitleSpeed));

        //         TimeSpan prevNewCurrent = newCurrent;
        //         newCurrent = newCurrent.Add(newS);
        //         current = subtitle.StartTime;
        //         newCurrent = newCurrent.Add(newD);
        //         if (playPosition < newCurrent)
        //         {
        //             TimeSpan outS = prevNewCurrent.Subtract(new TimeSpan(0, 0, offsetSecond));
        //             if (outS.Ticks < 0) outS = new TimeSpan(0);
        //             newPlayPosition = outS;
        //             return true;
        //         }
        //         current = subtitle.EndTime;
        //     }
        //     return false;
        // }


        // public void Dump(Stream s)
        // {
        //     StreamWriter sw = new StreamWriter(s);
        //     TimeSpan previousEnd = new TimeSpan(0);
        //     sw.WriteLine("#,Duration,MainDuration,SubtitleDuration");
        //     sw.WriteLine("#," + this.duration.TotalMilliseconds.ToString() + "," + mainDuration.TotalMilliseconds.ToString() + "," + subtitleDuration.TotalMilliseconds.ToString());
        //     sw.WriteLine("#No, Silence, Utterance, Length, Bmpsize,PngSize,PngPath"); // legend
        //     int i = 0;
        //     Subtitle st;
        //     TimeSpan sDuration;
        //     TimeSpan mDuration;
        //     for (i = 0; i < subtitleList.Count; i++)
        //     {
        //         st = subtitleList[i];
        //         sDuration = st.StartTime.Subtract(previousEnd);
        //         mDuration = st.EndTime.Subtract(st.StartTime);

        //         long f1 = 0;
        //         if (File.Exists(st.ImageFilePath))
        //         {
        //             FileInfo fo = new FileInfo(st.ImageFilePath);
        //             f1 = fo.Length;
        //         }
        //         long f2 = 0;
        //         string pngPath = st.ImageFilePath + ".png";
        //         if (File.Exists(pngPath))
        //         {
        //             FileInfo fo = new FileInfo(pngPath);
        //             f2 = fo.Length;
        //         }

        //         if (mDuration.Ticks > 0 && sDuration.Ticks > 0)
        //         {
        //             sw.WriteLine(i.ToString() + "," + sDuration.TotalMilliseconds.ToString() + "," + mDuration.TotalMilliseconds.ToString() + "," + st.TextLength.ToString() + "," + f1.ToString() + "," + f2.ToString() + "," + pngPath);
        //             previousEnd = st.EndTime;
        //         }
        //     }
        //     if (this.duration.Ticks > 0)
        //     {
        //         TimeSpan lastSilence = this.duration.Subtract(previousEnd);
        //         sw.WriteLine(i.ToString() + "," + lastSilence.TotalMilliseconds.ToString());
        //     }
        //     sw.Close();
        // }

        // public bool GetStats()
        // {
        //     if (duration.Ticks == 0) return false;
        //     if (subtitleList.Count == 0) return false;

        //     TimeSpan silenceSum = new TimeSpan(0);
        //     TimeSpan utteranceSum = new TimeSpan(0);
        //     TimeSpan previousEnd = new TimeSpan(0);
        //     foreach (Subtitle s in subtitleList)
        //     {
        //         TimeSpan addS = s.StartTime.Subtract(previousEnd);
        //         TimeSpan addU = s.EndTime.Subtract(s.StartTime);
        //         silenceSum = silenceSum.Add(addS);
        //         utteranceSum = utteranceSum.Add(addU);
        //         previousEnd = s.EndTime;
        //     }
        //     TimeSpan lastS = duration.Subtract(previousEnd);
        //     silenceSum = silenceSum.Add(lastS);

        //     mainDuration = silenceSum;
        //     subtitleDuration = utteranceSum;

        //     return true;

        // }

        //private static string templatePath = @"C:\Users\qurihara\Documents\My Dropbox\MovieQ\avs_scripts\template.avs";
        //public void DampAVS(Stream s, float videoSpeed, float subtitleSpeed, float fps)
        //{
        //    StringBuilder sb = new StringBuilder();

        //    //header

        //    //body
        //    TimeSpan previousEnd = new TimeSpan(0);
        //    int previousEndFrame = 0;
        //    int startFrame = 0;
        //    int endFrame = 0;
        //    for (int i = 0; i < subtitleList.Count; i++)
        //    {
        //        Subtitle st = subtitleList[i];
        //        startFrame = (int)(st.StartTime.TotalSeconds * (double)fps);
        //        endFrame = (int)(st.EndTime.TotalSeconds * (double)fps);
        //        if (startFrame > 0 && endFrame > 0 && startFrame < endFrame)
        //        {
        //            sb.Append("tmp = Trim(src," + previousEndFrame.ToString() + "," + (startFrame - 1).ToString() + ")\n");
        //            sb.Append("tmp = tmp.ChangePlaySpeed(" + videoSpeed.ToString() + ")\n");
        //            sb.Append("result = result ++ tmp\n");
        //            sb.Append("tmp = Trim(src," + startFrame.ToString() + "," + (endFrame - 1).ToString() + ")\n");
        //            sb.Append("tmp = tmp.ChangePlaySpeed(" + subtitleSpeed.ToString() + ")\n");
        //            sb.Append("result = result ++ tmp\n");

        //            previousEndFrame = endFrame;
        //        }
        //    }
        //    sb.Append("tmp = Trim(src," + previousEndFrame.ToString() + ",FrameCount(src)-1)\n"); // check
        //    sb.Append("tmp = tmp.ChangePlaySpeed(" + videoSpeed.ToString() + ")\n");
        //    sb.Append("result = result ++ tmp\n");


        //    //footer
        //    //sb.Append("\nreturn result\n");

        //    StreamReader sr = new StreamReader(templatePath);
        //    string result = sr.ReadToEnd();
        //    sr.Close();
        //    result = result.Replace("<body>", sb.ToString());
        //    StreamWriter sw = new StreamWriter(s);
        //    sw.Write(result);
        //    sw.Close();
        //}

        // private static string GetPath(string pathBase, float videoSpeed, float subtitleSpeed,int splitMinutes, int num,bool overlayOriginalSubtitle,bool overlaySubtitleCenter,bool overlayPreviousSubtitleCenter,bool overlayPreviousSubtitleUpper)
        // {
        //     string subtitleStatus = "n";
        //     if (overlayOriginalSubtitle || overlaySubtitleCenter || overlayPreviousSubtitleCenter || overlayPreviousSubtitleUpper)
        //     {
        //         subtitleStatus = "";
        //         if (overlayOriginalSubtitle) subtitleStatus += "o";
        //         if (overlaySubtitleCenter) subtitleStatus += "c";
        //         if (overlayPreviousSubtitleCenter) subtitleStatus += "p";
        //         if (overlayPreviousSubtitleUpper) subtitleStatus += "u";
        //     }
        //     return pathBase + "[" + videoSpeed.ToString() + "_" + subtitleSpeed.ToString() + "_" + subtitleStatus + "_" + splitMinutes.ToString() + "_" + num.ToString("00") + "].avs";
        // }
        // public void DumpAVSSplit(string pathBase, float videoSpeed, float subtitleSpeed, int splitMinutes, string avsTemplate, bool overlaySpeed, bool overlaySubtitleImage, bool omitCreatePng, bool overlayPreviousSubtitleImage, bool overlayPreviousSubtitleImageUpper, bool overlayOriginalSubtitle,bool setSubtitleSpeedByReadingRate,float readingRate,bool addLogo)
        // {

        //     int num = 1;
        //     int nextPos = splitMinutes * 60;
        //     string path = GetPath(pathBase, videoSpeed, subtitleSpeed,splitMinutes, num,overlayOriginalSubtitle,overlaySubtitleImage,overlayPreviousSubtitleImage,overlayPreviousSubtitleImageUpper);

        //     StringBuilder sb = new StringBuilder();

        //     //header

        //     //body
        //     TimeSpan previousEnd = new TimeSpan(0);
        //     //int previousEndFrame = 0;
        //     //int startFrame = 0;
        //     //int endFrame = 0;
        //     double previousEndFrame = 0d;
        //     double startFrame = 0d;
        //     double endFrame = 0d;
        //     Subtitle previousSubtitle = null;

        //     for (int i = 0; i < subtitleList.Count; i++)
        //     {
        //         Subtitle st = subtitleList[i];
        //         if (i > 0) previousSubtitle = subtitleList[i - 1];
        //         startFrame = st.StartTime.TotalSeconds;// (int)(st.StartTime.TotalSeconds * (double)fps);
        //         endFrame = st.EndTime.TotalSeconds;// (int)(st.EndTime.TotalSeconds * (double)fps);

        //         double sTest = Math.Floor(startFrame * 30d) - 1d;
        //         double fTest = Math.Floor(endFrame * 30d) - 1d;
        //         if (sTest < 0 || fTest < 0) continue;

        //         if (startFrame > 0 && endFrame > 0 && startFrame < endFrame)
        //         {
        //             if (st.StartTime.TotalSeconds > nextPos)
        //             {
        //                 //int endF = (int)(nextPos * fps);
        //                 double endF = (double)nextPos;

        //                 OutputMainVideo(sb, previousEndFrame, endF, videoSpeed, overlaySpeed, overlaySubtitleImage, previousSubtitle, omitCreatePng, overlayPreviousSubtitleImage, overlayOriginalSubtitle);
        //                 //sb.Append("tmp = Trim(src,Floor(" + previousEndFrame.ToString() + "*fps),Floor(" + endF.ToString() + "*fps)-1)\n");
        //                 //sb.Append("tmp = tmp.ChangePlaySpeed(" + videoSpeed.ToString() + ")\n");
        //                 ////sb.Append("tmp = tmp.Subtitle(\"x" + videoSpeed.ToString() + "\")\n");
        //                 //OverlaySpeed(overlaySpeed, sb, videoSpeed);
        //                 //sb.Append("result = result ++ tmp\n");

        //                 //write
        //                 WriteAVS(path, sb.ToString(),avsTemplate,addLogo);
        //                 sb = new StringBuilder();
        //                 previousEndFrame = endF;// (int)(nextPos * fps);
        //                 nextPos += splitMinutes * 60;
        //                 num++;
        //                 //path = GetPath(pathBase, videoSpeed, subtitleSpeed,splitMinutes, num);
        //                 path = GetPath(pathBase, videoSpeed, subtitleSpeed, splitMinutes, num, overlayOriginalSubtitle, overlaySubtitleImage, overlayPreviousSubtitleImage, overlayPreviousSubtitleImageUpper);

        //             }
        //             else
        //             {
        //                 if (st.EndTime.TotalSeconds > nextPos)
        //                 {
        //                     //int endF = (int)(nextPos * fps);
        //                     double endF = (int)nextPos;
        //                     OutputMainVideo(sb, previousEndFrame, startFrame, videoSpeed, overlaySpeed, overlaySubtitleImage, previousSubtitle, omitCreatePng, overlayPreviousSubtitleImage, overlayOriginalSubtitle);
        //                     //sb.Append("tmp = Trim(src,Floor(" + previousEndFrame.ToString() + "*fps),Floor(" + startFrame.ToString() + "*fps)-1)\n");
        //                     //sb.Append("tmp = tmp.ChangePlaySpeed(" + videoSpeed.ToString() + ")\n");
        //                     ////sb.Append("tmp = tmp.Subtitle(\"x" + videoSpeed.ToString() + "\")\n");
        //                     //OverlaySpeed(overlaySpeed, sb, videoSpeed);
        //                     //sb.Append("result = result ++ tmp\n");

        //                     OutputSubtitle(sb, startFrame, endF, subtitleSpeed, overlaySpeed, overlaySubtitleImage, st, previousSubtitle, omitCreatePng, overlayPreviousSubtitleImage, overlayPreviousSubtitleImageUpper, setSubtitleSpeedByReadingRate, readingRate, overlayOriginalSubtitle);
        //                     //sb.Append("tmp = Trim(src,Floor(" + startFrame.ToString() + "*fps),Floor(" + endF.ToString() + "*fps)-1)\n");
        //                     //sb.Append("tmp = tmp.ChangePlaySpeed(" + subtitleSpeed.ToString() + ")\n");
        //                     ////sb.Append("tmp = tmp.Subtitle(\"x" + subtitleSpeed.ToString() + "\")\n");
        //                     //OverlaySpeed(overlaySpeed, sb, subtitleSpeed);
        //                     //sb.Append("result = result ++ tmp\n");

        //                     //write
        //                     WriteAVS(path, sb.ToString(),avsTemplate,addLogo);
        //                     sb = new StringBuilder();
        //                     previousEndFrame = endF;// (int)(nextPos * fps);
        //                     nextPos += splitMinutes * 60;
        //                     num++;
        //                     //path = GetPath(pathBase, videoSpeed, subtitleSpeed,splitMinutes, num);
        //                     path = GetPath(pathBase, videoSpeed, subtitleSpeed, splitMinutes, num, overlayOriginalSubtitle, overlaySubtitleImage, overlayPreviousSubtitleImage, overlayPreviousSubtitleImageUpper);

        //                     OutputSubtitle(sb, endF, endFrame, subtitleSpeed, overlaySpeed, overlaySubtitleImage, st, previousSubtitle, omitCreatePng, overlayPreviousSubtitleImage, overlayPreviousSubtitleImageUpper, setSubtitleSpeedByReadingRate, readingRate, overlayOriginalSubtitle);
        //                     //sb.Append("tmp = Trim(src,Floor(" + endF.ToString() + "*fps),Floor(" + endFrame.ToString() + "*fps)-1)\n");
        //                     //sb.Append("tmp = tmp.ChangePlaySpeed(" + subtitleSpeed.ToString() + ")\n");
        //                     ////sb.Append("tmp = tmp.Subtitle(\"x" + subtitleSpeed.ToString() + "\")\n");
        //                     //OverlaySpeed(overlaySpeed, sb, subtitleSpeed);
        //                     //sb.Append("result = result ++ tmp\n");

        //                 }
        //             }

        //             OutputMainVideo(sb, previousEndFrame, startFrame, videoSpeed, overlaySpeed, overlaySubtitleImage, previousSubtitle, omitCreatePng, overlayPreviousSubtitleImage, overlayOriginalSubtitle);
        //             //sb.Append("tmp = Trim(src,Floor(" + previousEndFrame.ToString() + "*fps),Floor(" + startFrame.ToString() + "*fps)-1)\n");
        //             //sb.Append("tmp = tmp.ChangePlaySpeed(" + videoSpeed.ToString() + ")\n");
        //             //OverlaySpeed(overlaySpeed, sb, videoSpeed);
        //             ////sb.Append("tmp = tmp.Subtitle(\"x" + videoSpeed.ToString() + "\")\n");
        //             //sb.Append("result = result ++ tmp\n");

        //             OutputSubtitle(sb, startFrame, endFrame, subtitleSpeed, overlaySpeed, overlaySubtitleImage, st, previousSubtitle, omitCreatePng, overlayPreviousSubtitleImage, overlayPreviousSubtitleImageUpper, setSubtitleSpeedByReadingRate, readingRate, overlayOriginalSubtitle);
        //             //sb.Append("tmp = Trim(src,Floor(" + startFrame.ToString() + "*fps),Floor(" + endFrame.ToString() + "*fps)-1)\n");
        //             //sb.Append("tmp = tmp.ChangePlaySpeed(" + subtitleSpeed.ToString() + ")\n");
        //             ////sb.Append("tmp = tmp.Subtitle(\"x" + subtitleSpeed.ToString() + "\")\n");
        //             //OverlaySpeed(overlaySpeed, sb, subtitleSpeed);
        //             //sb.Append("result = result ++ tmp\n");

        //             previousEndFrame = endFrame;
        //         }
        //     }
        //     sb.Append("tmp = Trim(src,Floor(" + previousEndFrame.ToString() + "*fps),FrameCount(src)-1)");
        //     if (true)//overlaySubtitleImage)
        //     {
        //         sb.Append(".ConvertToRGB32()");
        //     }
        //     sb.Append("\n");

        //     //sb.Append("tmp = tmp.ChangePlaySpeed(" + videoSpeed.ToString() + ")\n");
        //     if (videoSpeed <= 10f)
        //     {
        //         sb.Append("tmp = tmp.ChangePlaySpeed(" + videoSpeed.ToString() + ")\n");
        //     }
        //     else
        //     {
        //         float ssp = videoSpeed / 10f;
        //         sb.Append("tmp = ChangePlaySpeed(ChangePlaySpeed(tmp," + ssp.ToString() + "),10)\n");
        //     }
        //     OverlaySpeed(overlaySpeed, sb, videoSpeed);
        //     OverlayPreviousSubtitleImage(overlayPreviousSubtitleImage, sb, previousSubtitle, omitCreatePng, true, overlayOriginalSubtitle);
        //     //sb.Append("tmp = tmp.Subtitle(\"x" + videoSpeed.ToString() + "\")\n");
        //     sb.Append("result = result ++ tmp\n");


        //     //footer
        //     //sb.Append("\nreturn result\n");

        //     WriteAVS(path, sb.ToString(),avsTemplate,addLogo);
        // }
        // private void OverlaySpeed(bool overlaySpeed,StringBuilder sb, float speed)
        // {
        //     if (overlaySpeed)
        //     {
        //         sb.Append("tmp = tmp.Subtitle(\"x" + speed.ToString() + "\")\n");
        //     }
        // }
        // private void OutputMainVideo(StringBuilder sb, double t1, double t2, float speed, bool overlaySpeed, bool overlaySubtitleImage, Subtitle previousSubtitle, bool omitCreatePng, bool overlayPreviousSubtitleImage, bool overlayOriginalSubtitle)
        // {
        //     if (t1 >= t2) return;
        //     sb.Append("tmp = Trim(src,Floor(" + t1.ToString() + "*fps),Floor(" + t2.ToString() + "*fps)-1)");//\n");
        //     if (true)//overlaySubtitleImage)
        //     {
        //         sb.Append(".ConvertToRGB32()");
        //     }
        //     sb.Append("\n");
        //     //sb.Append("tmp = tmp.ChangePlaySpeed(" + speed.ToString() + ")\n");
        //     if (speed <= 10f)
        //     {
        //         sb.Append("tmp = tmp.ChangePlaySpeed(" + speed.ToString() + ")\n");
        //     }
        //     else
        //     {
        //         float ssp = speed / 10f;
        //         sb.Append("tmp = ChangePlaySpeed(ChangePlaySpeed(tmp," + ssp.ToString() + "),10)\n");
        //     }
        //     OverlaySpeed(overlaySpeed, sb, speed);
        //     OverlayPreviousSubtitleImage(overlayPreviousSubtitleImage, sb, previousSubtitle, omitCreatePng, true, overlayOriginalSubtitle);
        //     //sb.Append("tmp = tmp.Subtitle(\"x" + videoSpeed.ToString() + "\")\n");
        //     sb.Append("result = result ++ tmp\n");
        // }
        // private void OutputSubtitle(StringBuilder sb, double t1, double t2, float speed, bool overlaySpeed, bool overlaySubtitleImage, Subtitle subtitle, Subtitle previousSubtitle, bool omitCreatePng, bool overlayPreviousSubtitleImage, bool overlayPreviousSubtitleImageUpper, bool setSubtitleSpeedByReadingRate,float readingRate,bool overlayOriginalSubtitle)
        // {
        //     if (t1 >= t2) return;
        //     if (setSubtitleSpeedByReadingRate)
        //     {
        //         float tmpSpeed = subtitle.GetReadableSpeed(readingRate);// 800f);
        //         if (tmpSpeed >= 8f) tmpSpeed = 8f;
        //         //if (tmpSpeed < speed) tmpSpeed = speed;    // 最低スピードを指定する場合。
        //         speed = tmpSpeed;
        //     }
        //     sb.Append("tmp = Trim(src,Floor(" + t1.ToString() + "*fps),Floor(" + t2.ToString() + "*fps)-1)");//\n");
        //     if (true)//overlaySubtitleImage)
        //     {
        //         sb.Append(".ConvertToRGB32()");
        //     }
        //     sb.Append("\n");
        //     if (speed <= 10f)
        //     {
        //         sb.Append("tmp = tmp.ChangePlaySpeed(" + speed.ToString() + ")\n");
        //     }
        //     else
        //     {
        //         float ssp = speed / 10f;
        //         sb.Append("tmp = ChangePlaySpeed(ChangePlaySpeed(tmp," + ssp.ToString() + "),10)\n");               
        //     }
        //     OverlaySpeed(overlaySpeed, sb, speed);
        //     //OverlayPreviousSubtitleImage(overlaySubtitleImage, sb, previousSubtitle, omitCreatePng, false,vga);
        //     OverlayPreviousSubtitleImage(overlayPreviousSubtitleImageUpper, sb, previousSubtitle, omitCreatePng, false, overlayOriginalSubtitle);
        //     OverlaySubtitleImage(overlaySubtitleImage, sb, subtitle, omitCreatePng, overlayOriginalSubtitle);
        //     //sb.Append("tmp = tmp.Subtitle(\"x" + videoSpeed.ToString() + "\")\n");
        //     sb.Append("result = result ++ tmp\n");
        // }
        // private void OverlaySubtitleImage(bool overlaySubtitleImage, StringBuilder sb, Subtitle subtitle, bool omitCreatePng, bool overlayOriginalSubtitle)
        // {
        //     OverlaySubtitleImageInside(overlaySubtitleImage, sb, subtitle, omitCreatePng, true, true, overlayOriginalSubtitle);
        // }
        // private void OverlayPreviousSubtitleImage(bool overlaySubtitleImage, StringBuilder sb, Subtitle subtitle, bool omitCreatePng, bool center, bool overlayOriginalSubtitle)
        // {
        //     if (subtitle != null)
        //         OverlaySubtitleImageInside(overlaySubtitleImage, sb, subtitle, true, center, false, false);
        // }

        // private void OverlaySubtitleImageInside(bool overlaySubtitleImage, StringBuilder sb, Subtitle subtitle, bool omitCreatePng, bool center, bool bold, bool overlayOriginalSubtitle)
        // {
        //     if (overlaySubtitleImage || overlayOriginalSubtitle)
        //     {
        //         string path = subtitle.ImageFilePath + ".png";
        //         if (!omitCreatePng)
        //         {
        //             // Size s = new Size(subtitle.Bounds.Width, subtitle.Bounds.Height);
        //             Bitmap b2 = new Bitmap(subtitle.ImageFilePath);                    
        //             Bitmap bm = new Bitmap(b2, s);
        //             //Color backC = Color.FromArgb(255, 0, 0, 255);
        //             for (int y = 0; y < bm.Height; y++)
        //             {
        //                 for (int x = 0; x < bm.Width; x++)
        //                 {
        //                     Color c = bm.GetPixel(x, y);
        //                     //if (c == backC)
        //                     if (c.A >= 128 && c.R < 128 && c.G < 128 && c.B >= 128)
        //                     {
        //                         bm.SetPixel(x, y, Color.Transparent);
        //                     }
        //                     //if (y == bm.Height - 1)
        //                     //{
        //                     //    bm.SetPixel(x, y, Color.Transparent);
        //                     //}
        //                 }
        //             }
        //             bm.Save(path, System.Drawing.Imaging.ImageFormat.Png);
        //             b2.Dispose();
        //             bm.Dispose();                    
        //         }
        //         if (File.Exists(path))
        //         {
        //             //# ロゴの読み込みと変数への代入（アルファチャンネル付きの PNG 画像を RGB32 デコード）
        //             //logo = ImageSource("C:\Users\qurihara\Desktop\work\win24h_000005.bmp", pixel_type="RGB24").ConvertToRGB32()
        //             //sb.Append("logo = ImageSource(\"" + path + "\", pixel_type=\"RGB32\")\n");//.ConvertToRGB32()\n");

        //             // sb.Append("sw = Floor(src.Width()* " + subtitle.NomalBounds4_3.Width.ToString() + ")\n");
        //             sb.Append("sw = (sw > 4) ?sw:4\n");
        //             // sb.Append("sh = Floor(src.Height()* " + subtitle.NomalBounds4_3.Height.ToString() + ")\n");
        //             sb.Append("sh = (sh > 4) ?sh:4\n");
        //             sb.Append("logo = ImageSource(\"" + path + "\", pixel_type=\"RGB32\").BilinearResize(sw,sh)\n");//.ConvertToRGB32()\n");
        //             //sb.Append("logo = ImageSource(\"" + path + "\", pixel_type=\"RGB32\").PointResize(sw,sh)\n");//.ConvertToRGB32()\n");
        //             //sb.Append("logo = ImageSource(\"" + path + "\", pixel_type=\"RGB32\").Spline16Resize(sw,sh)\n");//.ConvertToRGB32()\n");
        //             //sb.Append("logo = ImageSource(\"" + path + "\", pixel_type=\"RGB32\").LanczosResize(sw,sh)\n");//.ConvertToRGB32()\n");
        //             //sb.Append("logo = ImageSource(\"" + path + "\", pixel_type=\"RGB32\").BlackmanResize(sw,sh)\n");//.ConvertToRGB32()\n");
        //             //sb.Append("logo = ImageSource(\"" + path + "\", pixel_type=\"RGB32\").BicubicResize(sw,sh)\n");//.ConvertToRGB32()\n");
        //             //sb.Append("logo = ImageSource(\"" + path + "\", pixel_type=\"RGB32\").GaussResize(sw,sh)\n");//.ConvertToRGB32()\n");
                    
        //             //# Layer で main の上に logo を追加
        //             //src = Layer(src, logo, "add" , x=0, y=120, level=255) # level を調節して半透明に
        //             //sb.Append("tmp = Layer(tmp,logo,\"add\",x=0, y=120, level=255)\n");

        //             int level = 255;
        //             if (!bold) level = 128;
        //             //int newY = subtitle.Bounds.Y;
        //             //int newX = subtitle.Bounds.X;
        //             //string yfac = "0.5";
        //             float yfactor = 0.5f;
        //             if (overlaySubtitleImage)
        //             {
        //                 //int 
        //                 //newY = 0;
        //                 //int newX = subtitle.Bounds.X;

        //                 //int wid = 320;
        //                 //int hei = 240;
        //                 //if (vga)
        //                 //{
        //                 //    wid = 640;
        //                 //    hei = 480;
        //                 //}
        //                 //int 
        //                 //newX = wid / 2 - subtitle.Bounds.Width / 2;
        //                 if (center)
        //                 {
        //                     //newY = hei / 2 - subtitle.Bounds.Height / 2;
        //                 }
        //                 else
        //                 {
        //                     //newY = (int)((double)hei * 0.375d) - subtitle.Bounds.Height / 2;
        //                     //yfac = " * 0.375 -";
        //                     //yfac = "0.385";
        //                     yfactor = 0.385f;
        //                 }
        //             }
        //             //sb.Append("tmp = Layer(tmp,logo,\"add\",x=" + newX.ToString() + ", y=" + newY.ToString() + ", level=" + level.ToString() + ")\n");

        //             //yfactor -= subtitle.NomalBounds4_3.Height / 2f;
        //             //if (overlayOriginalSubtitle) yfactor = subtitle.NomalBounds4_3.Y;
        //             //sb.Append("tmp = Layer(tmp,logo,\"add\",x=Floor(tmp.Width() *" + subtitle.NomalBounds4_3.X.ToString() + "), y=Floor(tmp.Height() *" + yfactor + "), level=" + level.ToString() + ")\n");
                    
        //             //sb.Append("rx = (tmp.Height()/tmp.Width() == 0.75) ? " + subtitle.NomalBounds4_3.X.ToString() + " : " + subtitle.NomalBounds16_9.X.ToString() + "\n");
        //             // sb.Append("rx = 0.5 * (1-" + subtitle.NomalBounds4_3.Width.ToString() + ")\n");
        //             if (overlayOriginalSubtitle)
        //             {
        //                 // sb.Append("ry = " + subtitle.NomalBounds4_3.Y.ToString() + "\n");
        //                 sb.Append("tmp = Layer(tmp,logo,\"add\",x=Floor(tmp.Width() * rx), y=Floor(tmp.Height() * ry), level=" + level.ToString() + ")\n");
        //             }
        //             else
        //             {
        //                 // sb.Append("ry = 0.5 * " + subtitle.NomalBounds4_3.Height.ToString() + "\n");
        //                 sb.Append("tmp = Layer(tmp,logo,\"add\",x=Floor(tmp.Width() * rx), y=Floor(tmp.Height() * (" + yfactor + "-ry)), level=" + level.ToString() + ")\n");
        //             }

        //             //int newY = 240 / 2 - subtitle.Bounds.Height / 2; 
        //             ////int newX = subtitle.Bounds.X;
        //             //int newX = 320 / 2 - subtitle.Bounds.Width / 2;
        //             //if (center)
        //             //{
        //             //}
        //             //else
        //             //{
        //             //    newX = 0;
        //             //}
        //             //sb.Append("tmp = Layer(tmp,logo,\"add\",x=" + newX.ToString() + ", y=" + newY.ToString() + ", level=" + level.ToString() + ")\n");
        //         }
        //     }

        // }

        // private void WriteAVS(string path, string body,string avsTemplate, bool addLogo)
        // {
        //     if (addLogo)
        //     {
        //         body += "\nresult = Layer(result,blogo,\"add\",x=result.Width() - 190, y=result.Height() - 26, level=60)";

        //     }
        //     string result = avsTemplate;
        //     result = result.Replace("#<body>", body);

        //     System.Reflection.Assembly ass = System.Reflection.Assembly.GetEntryAssembly();
        //     string exeDir = Path.GetDirectoryName(ass.Location);
        //     result = result.Replace("#<exepath>", exeDir);

        //     using (StreamWriter sw = new StreamWriter(path))
        //     {
        //         sw.Write(result);
        //         sw.Close();
        //     }
        // }

        // public virtual void SeekNextSubtitle(TimeSpan current)
        // {
        //     current = ConvertToInnerTime(current);
        //     currentPosition = 0;
        //     currentStartTime = new TimeSpan(0);
        //     for (int i = 0; i < subtitleList.Count; i++)
        //     {
        //         currentPosition = i;
        //         UpdateCurrentStartTime();

        //         if (currentStartTime >= current)
        //         {
        //             isActive = true;
        //             return;
        //         }
        //     }
        //     //全部の字幕終了
        //     currentPosition = 0;
        //     currentStartTime = new TimeSpan(0);
        //     isActive = false;
        //     return;

        // }
        // private void IncrementCurrentPosition()
        // {
        //     currentPosition++;
        //     if (currentPosition >= subtitleList.Count)
        //     {
        //         currentPosition = 0;
        //         currentStartTime = new TimeSpan(0);
        //         isActive = false;
        //     }
        //     else
        //     {
        //         UpdateCurrentStartTime();
        //     }
        // }

        // internal void UpdateCurrentStartTime()
        // {
        //     switch (timing)
        //     {
        //         case Timing.StartTime:
        //             currentStartTime = subtitleList[currentPosition].StartTime;
        //             break;
        //         case Timing.MidTime:
        //             currentStartTime = subtitleList[currentPosition].MidTime;
        //             break;
        //         case Timing.EndTime:
        //             currentStartTime = subtitleList[currentPosition].EndTime;
        //             break;
        //     }
        //     //TimeSpan ts = subtitleList[currentPosition].EndTime.Subtract(subtitleList[currentPosition].StartTime);
        //     //currentStartTime = subtitleList[currentPosition].StartTime.Add(new TimeSpan(ts.Ticks / 2));
        // }

        // private TimeSpan ConvertToInnerTime(TimeSpan ts)
        // {
        //     long t = ts.Ticks;
        //     t = (long)(t * clockRatio);
        //     return new TimeSpan(t);
        // }


        // public void DumpCMAVS(string pathBase, string srcTxt)
        // {
        //     bool vga = false;
        //     string avsTemplate = Properties.Resources.cm_template;
        //     avsTemplate = avsTemplate.Replace("#<src>", srcTxt);

        //     System.Reflection.Assembly ass = System.Reflection.Assembly.GetEntryAssembly();
        //     string exeDir = Path.GetDirectoryName(ass.Location);
        //     avsTemplate = avsTemplate.Replace("#<exepath>", exeDir);

        //     Random r = new Random(DateTime.Now.Millisecond);
        //     int maxSubIndex = (int)(subtitleList.Count * 0.95);

        //     string path = pathBase + "[Trailer_" + System.DateTime.Now.Ticks.ToString() +"].avs";

        //     StringBuilder sb = new StringBuilder();

        //     //header

        //     //body
        //     TimeSpan previousEnd = new TimeSpan(0);
        //     double startFrame = 0d;
        //     double endFrame = 0d;


        //     Subtitle stl = subtitleList[r.Next(maxSubIndex)];
        //     startFrame = stl.StartTime.TotalSeconds;
        //     endFrame = stl.EndTime.TotalSeconds;
        //     OutputSubtitleOnly(3f,sb, startFrame, endFrame, 1, false, true, stl, null, true,vga);

        //     for (int i = 0; i < 20; i++)
        //     {

        //         Subtitle st = subtitleList[r.Next(maxSubIndex)];
        //         startFrame = st.StartTime.TotalSeconds;
        //         endFrame = st.EndTime.TotalSeconds;

        //         double sTest = Math.Floor(startFrame * 30d) - 1d;
        //         double fTest = Math.Floor(endFrame * 30d) - 1d;
        //         if (sTest < 0 || fTest < 0) continue;

        //         if (startFrame > 0 && endFrame > 0 && startFrame < endFrame)
        //         {
        //             OutputSubtitle(sb, startFrame, endFrame, 1f + (float)i/6f, false, true, st, null,true,false,false,false,0f,false);
        //         }
        //     }

        //     stl = subtitleList[r.Next(maxSubIndex)];
        //     startFrame = stl.StartTime.TotalSeconds;
        //     endFrame = stl.EndTime.TotalSeconds;
        //     OutputSubtitleOnly(3f,sb, startFrame, endFrame, 1, false, true, stl, null, true,vga);

        //     //footer

        //     WriteAVS(path, sb.ToString(), avsTemplate,false);
        // }

//tmp = BlankClip(result,Floor(3*fps))
//logo = ImageSource("C:\Users\qurihara\Desktop\toystory_again\wmv\TOYSTORY\VIDEO\sub\TOYSTORY.Title1_000436.bmp.png", pixel_type="RGB32")
//tmp = Layer(tmp,logo,"add",x=66, y=104, level=255)
//result = result ++ tmp
        // private void OutputSubtitleOnly(float sec, StringBuilder sb, double t1, double t2, float speed, bool overlaySpeed, bool overlaySubtitleImage, Subtitle subtitle, Subtitle previousSubtitle, bool omitCreatePng,bool vga)
        // {
        //     sb.Append("tmp = BlankClip(result,Floor(" + sec.ToString() + "*fps))\n");
        //     //sb.Append("tmp = tmp.ChangePlaySpeed(" + speed.ToString() + ")\n");
        //     if (speed <= 10f)
        //     {
        //         sb.Append("tmp = tmp.ChangePlaySpeed(" + speed.ToString() + ")\n");
        //     }
        //     else
        //     {
        //         float ssp = speed / 10f;
        //         sb.Append("tmp = ChangePlaySpeed(ChangePlaySpeed(tmp," + ssp.ToString() + "),10)\n");
        //     }

        //     OverlaySubtitleImage(overlaySubtitleImage, sb, subtitle, omitCreatePng,false);
        //     sb.Append("result = result ++ tmp\n");
        // }

        // public float GetMainSpeedFromDurationAndSubtitleSpeed(float subtitleSpeed, TimeSpan newDuration)
        // {
        //     double sp = mainDuration.TotalMilliseconds / (newDuration.TotalMilliseconds - subtitleDuration.TotalMilliseconds/(double)subtitleSpeed);
        //     return (float)sp;
        // }
        // public float GetSubtitleSpeedFromDurationAndMainSpeed(float mainSpeed, TimeSpan newDuration)
        // {
        //     double sp = subtitleDuration.TotalMilliseconds / (newDuration.TotalMilliseconds - mainDuration.TotalMilliseconds / (double)mainSpeed);
        //     return (float)sp;
        // }


        // public void DumpSubtitleTestAVS(string pathBase, string srcTxt)
        // {
        //     //bool vga = false;

        //     Random r = new Random(DateTime.Now.Millisecond);
        //     int maxSubIndex = (int)(subtitleList.Count * 0.95);

        //     string id = System.DateTime.Now.Ticks.ToString();

        //     for (float speed = 1f; speed <= 10f; speed = speed + 0.5f)
        //     {
        //         //string avsTemplate = Properties.Resources.cm_template;
        //         string avsTemplate = Properties.Resources.avsTemplate;
        //         avsTemplate = avsTemplate.Replace("#<src>", srcTxt);

        //         System.Reflection.Assembly ass = System.Reflection.Assembly.GetEntryAssembly();
        //         string exeDir = Path.GetDirectoryName(ass.Location);
        //         avsTemplate = avsTemplate.Replace("#<exepath>", exeDir);

        //         string path = pathBase + "[SubtitleTest_" + speed.ToString() + "_" + id + "].avs";

        //         StringBuilder sb = new StringBuilder();

        //         //header

        //         //body
        //         TimeSpan previousEnd = new TimeSpan(0);
        //         double startFrame = 0d;
        //         double endFrame = 0d;


        //         //Subtitle stl = subtitleList[r.Next(maxSubIndex)];
        //         //startFrame = stl.StartTime.TotalSeconds;
        //         //endFrame = stl.EndTime.TotalSeconds;
        //         //OutputSubtitleOnly(3f, sb, startFrame, endFrame, 1, false, true, stl, null, true, vga);

        //         for (int i = 0; i < 20; i++)
        //         {

        //             Subtitle st = subtitleList[r.Next(maxSubIndex)];
        //             startFrame = st.StartTime.TotalSeconds;
        //             endFrame = st.EndTime.TotalSeconds;

        //             double sTest = Math.Floor(startFrame * 30d) - 1d;
        //             double fTest = Math.Floor(endFrame * 30d) - 1d;
        //             if (sTest < 0 || fTest < 0) continue;

        //             if (startFrame > 0 && endFrame > 0 && startFrame < endFrame)
        //             {
        //                 OutputSubtitle(sb, startFrame, endFrame,speed, false, true, st, null, true, false, false, false, 0f,false);
        //             }
        //         }

        //         //stl = subtitleList[r.Next(maxSubIndex)];
        //         //startFrame = stl.StartTime.TotalSeconds;
        //         //endFrame = stl.EndTime.TotalSeconds;
        //         //OutputSubtitleOnly(3f, sb, startFrame, endFrame, 1, false, true, stl, null, true, vga);

        //         //footer

        //         WriteAVS(path, sb.ToString(), avsTemplate,false);
        //     }

        // }

        // public void DumpMlf(Stream s)
        // {
        //     DumpMlf(s, false, "",false);
        // }
        // public void DumpMlf(Stream s,bool lab,string labHeader, bool interpolateOtherLabel)
        // {
        //     StreamWriter sw = new StreamWriter(s);
        //     TimeSpan previousEnd = new TimeSpan(0);


        //     if (lab)
        //     {
        //         sw.WriteLine(labHeader);
        //     }
        //     else
        //     {
        //         sw.WriteLine("#!MLF!#");
        //     }

        //     TimeSpan prevend = new TimeSpan(0);
        //     foreach (Subtitle sub in subtitleList)
        //     {
        //         if (interpolateOtherLabel)
        //         {
        //             sw.WriteLine(prevend.Ticks.ToString() + "\t" + sub.StartTime.Ticks.ToString() + "\tother");
        //             prevend = sub.EndTime;
        //         }
        //         sw.WriteLine(sub.StartTime.Ticks.ToString() + "\t" + sub.EndTime.Ticks.ToString() + "\tvoice");
        //     }

        //     if (lab)
        //     {
        //         sw.WriteLine(".");
        //     }
        //     sw.Close();
        // }

    //     public void DumpContinuousAVS(string pathBase, string srcTxt,List<float> speedList, int stepMilliSecond)
    //     {
    //         string avsTemplate = Properties.Resources.avsTemplate;
    //         avsTemplate = avsTemplate.Replace("#<src>", srcTxt);

    //         System.Reflection.Assembly ass = System.Reflection.Assembly.GetEntryAssembly();
    //         string exeDir = Path.GetDirectoryName(ass.Location);
    //         avsTemplate = avsTemplate.Replace("#<exepath>", exeDir);


    //         string path = pathBase + "_cont_.avs";

    //         StringBuilder sb = new StringBuilder();


    //         //body
    //         double startFrame = 0d;
    //         double endFrame = 0d;

    //         //long totalMillisecond = speedList.Count * stepMilliSecond;
    //         for (int i = 0; i < speedList.Count; i++)
    //         {
    //             startFrame = (double)(i * stepMilliSecond) / 1000d;
    //             endFrame = (double)((i+1) * stepMilliSecond) / 1000d;

    //             OutputMainVideo(sb, startFrame, endFrame, speedList[i], true, false, null, true, false, false);
    //         }

    //         //footer

    //         WriteAVS(path, sb.ToString(), avsTemplate, false);
    //     }


    }
}

