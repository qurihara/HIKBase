using System;
using Subtitle;
using System.IO;

class Program
{
    static void Main()
    {
        SubtitleManager sManager = new SubtitleManager();
        sManager.LoadSrt(File.OpenRead("news.srt"));
        sManager.Dump(Console.OpenStandardOutput());

    }
}

