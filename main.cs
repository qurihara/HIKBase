using System;
using System.Collections.Generic;
using SubtitleUtil;
using System.IO;

class Program
{
    static void Main()
    {
        //基本構造
        //unity側で、読み上げる札の配列がある。それは札のIDと読み上げ音声がペアになっている。
        //こちら側で、札のIDとその読み上げアノテーションデータが関連づけられたSubtitleManagerが取り出される。
        //unity側でaudiosourceが再生される。
        //どこかのUpdate関数で1フレームごとにaudiosource.timeが取得される。
        //string readOut = SubtitleManager.GetSubtitle(audiosource.time)により、その時点での既読み上げ音声テキストが得られる。例："おお"

        //Cardクラスは、IDと読み上げ音声テキストが関連付けられている
        //CardManagerは、場にある全てのCardが登録されており、List<Card> cList = GetListStaringWith(string s)によって、から始まるカードのリストが得られる
        //cListに含まれるCardのIDを取得し、それに対応するUnity側の札オブジェクトのモグラを表示する。
        //cListに含まれないCardのIDを取得し、それに対応するUnity側の札オブジェクトのモグラを非表示にする。

        List<SubtitleManager> cardList = new List<SubtitleManager>();
        CardManager cManager = new CardManager();

        //1つのSubtitleManagerが1つの札に対応する。

        SubtitleManager sManager1 = new SubtitleManager(1);
        sManager1.LoadAnnotation(File.OpenRead("test.txt"));
        sManager1.Dump();
        cardList.Add(sManager1);
        cManager.Add(new Card(1,"おおえやまいくののみちのとほければまだふみもみずあまのはしだて"));

        SubtitleManager sManager2 = new SubtitleManager(2);
        sManager2.LoadAnnotation(File.OpenRead("test2.txt"));
        sManager2.Dump();
        cardList.Add(sManager2);
        cManager.Add(new Card(2,"おおこやまいくののみちのとほければまだふみもみずあまのはしだて"));

        //以下はunityでaudiosource.playOneShot()後にどこかのUpdate関数で呼び出される想定
        float currentTime = 0.2f;
        // string readout = sManager1.GetSubtitle(currentTime);
        string readout = "おおえ";
  
        cManager.GetListStaringWith(readout);
        List<int> activeCardIds = cManager.GetActiveCardIds();
        Console.WriteLine("active:");
        foreach(int i in activeCardIds){
            Console.WriteLine(i);
        }
        List<int> inactiveCardIds = cManager.GetInactiveCardIds();
        Console.WriteLine("inactive:");
        foreach(int i in inactiveCardIds){
            Console.WriteLine(i);
        }
    }
}

