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
        //string readOut = SubtitleManager.GetSubtitleByPosition(audiosource.time)により、その時点での既読み上げ音声テキストが得られる。例："おお"

        //Cardクラスは、IDと和歌テキストが関連付けられている
        //CardManagerは、場にある全てのCardが登録されており、List<Card> cList = GetListStaringWith(string s)によって、から始まるカードのリストが得られる
        //cListに含まれるCardのIDを取得し、それに対応するUnity側の札オブジェクトのモグラを表示する。
        //cListに含まれないCardのIDを取得し、それに対応するUnity側の札オブジェクトのモグラを非表示にする。

        List<SubtitleManager> readoutList = new List<SubtitleManager>();
        CardManager cManager = new CardManager();

        //1つのSubtitleManagerが1つの札に対応する。音声読み上げにともなうアノテーションを管理する。
        //1つのCardが1つの札に対応する。場にある札の状態を管理する。

        SubtitleManager sManager;
        //札8
        sManager = new SubtitleManager(8);
        sManager.LoadAnnotation(File.OpenRead("008wagai.txt"));
        sManager.Dump();
        readoutList.Add(sManager);
        cManager.Add(new Card(8,"わがいほはみやこのたつみしかぞすむよをうぢやまとひとはいふなり"));

        //札11
        sManager = new SubtitleManager(11);
        sManager.LoadAnnotation(File.OpenRead("011watanoharaya.txt"));
        sManager.Dump();
        readoutList.Add(sManager);
        cManager.Add(new Card(11,"わたのはらやそしまかけてこぎいでぬとひとにはつげよあまのつりぶね"));

        //札20
        sManager = new SubtitleManager(20);
        sManager.LoadAnnotation(File.OpenRead("020wabi.txt"));
        sManager.Dump();
        readoutList.Add(sManager);
        cManager.Add(new Card(20,"わびぬればいまはたおなじなにはなるみをつくしてもあはむとぞおもふ"));

        //札38
        sManager = new SubtitleManager(38);
        sManager.LoadAnnotation(File.OpenRead("038wasura.txt"));
        sManager.Dump();
        readoutList.Add(sManager);
        cManager.Add(new Card(38,"わすらるるみをばおもはずちかひてしひとのいのちのをしくもあるかな"));

        //札54
        sManager = new SubtitleManager(54);
        sManager.LoadAnnotation(File.OpenRead("054wasure.txt"));
        sManager.Dump();
        readoutList.Add(sManager);
        cManager.Add(new Card(54,"わすれじのゆくすゑまではかたければけふをかぎりのいのちともがな"));

        //札76
        sManager = new SubtitleManager(76);
        sManager.LoadAnnotation(File.OpenRead("076watanoharako.txt"));
        sManager.Dump();
        readoutList.Add(sManager);
        cManager.Add(new Card(76,"わたのはらこぎいでてみればひさかたのくもゐにまがふおきつしらなみ"));

        //札92
        sManager = new SubtitleManager(92);
        sManager.LoadAnnotation(File.OpenRead("092wagaso.txt"));
        sManager.Dump();
        readoutList.Add(sManager);
        cManager.Add(new Card(92,"わがそではしほひにみえぬおきのいしのひとこそしらねかわくまもなし"));

        int seikaiCardId = -1;

        //以下はunityで札1に対応するaudiosource.playOneShot()後にどこかのUpdate関数で呼び出される想定

        // float t = audiosource.time;

        foreach(SubtitleManager sm in readoutList){
            for(float t = 0f;t<4f;t=t+0.1f){
                Console.WriteLine("time: " + t);
                string readout = sm.GetSubtitleByPosition(t);
                Console.WriteLine("readout: " + readout);

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

                if (activeCardIds.Count == 1){
                    seikaiCardId = activeCardIds[0];
                    break;
                }
                Console.WriteLine("----");
            }
            if (seikaiCardId >= 0){
                Console.WriteLine("Seikai card identified: " + seikaiCardId);
            }else{
                Console.WriteLine("Seikai doesn't exist: " + seikaiCardId);
            }
        }


        // string readout = "おおこ";
  
        // cManager.GetListStaringWith(readout);
        // List<int> activeCardIds = cManager.GetActiveCardIds();
        // Console.WriteLine("active:");
        // foreach(int i in activeCardIds){
        //     Console.WriteLine(i);
        // }
        // List<int> inactiveCardIds = cManager.GetInactiveCardIds();
        // Console.WriteLine("inactive:");
        // foreach(int i in inactiveCardIds){
        //     Console.WriteLine(i);
        // }
    }
}

