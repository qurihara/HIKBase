using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

using System.Text.RegularExpressions;

namespace Subtitle
{
    public static class Util
    {
   //- voice	人の声
   //- music	音楽
   //- mix_music	音楽＋人の声
   //- other	その他物音
   //- sil	無音

        public static bool Mlf2Son(string mlf,out string result)
        {
            result = "";
            StringBuilder sb = new StringBuilder();
            string[] lines = mlf.Split('\n');
            int count = 1;
            bool first = true;
            foreach (string line in lines)
            {
                if (first)
                {
                    first = false;
                    continue;
                }
                if (line.StartsWith("#")) continue;
                string[] ele = line.Split('\t');
                if (ele.Length != 3) continue;
                TimeSpan st = new TimeSpan(long.Parse(ele[0]));
                TimeSpan et = new TimeSpan(long.Parse(ele[1]));
                char[] trm = { '\r' };
                ele[2] = ele[2].TrimEnd(trm);
                switch (ele[2])
                {
                    case "voice":
                    case "mix_music":
                        string s = GetTime(st);
                        string e = GetTime(et);
                        sb.Append(count.ToString("0000") + "\t" + s + "\t" + e + "\t" + ele[2] + "\n");
                        count++;
                        break;
                    default:
                        break;
                }
            }
            result = sb.ToString();
            return true;
        }
        private static string GetTime(TimeSpan t)
        {
            return t.Hours.ToString("00") + ":" + t.Minutes.ToString("00") + ":" + t.Seconds.ToString("00") + ":" + ((int)(t.Milliseconds/10)).ToString("00");
        }

        public static bool Mlf2Srt(string mlf,int marginBefore, out string result)
        {
            result = "";
            StringBuilder sb = new StringBuilder();
            string[] lines = mlf.Split('\n');
            int count = 1;
            TimeSpan pEt = TimeSpan.MinValue;
            foreach (string line in lines)
            {
                if (line.StartsWith("#")) continue;
                string[] ele = line.Split('\t');
                if (ele.Length != 3) continue;
                TimeSpan st = new TimeSpan(long.Parse(ele[0]));
                TimeSpan minus = new TimeSpan(0, 0, 0, 0, marginBefore);
                if (st > minus) st = st.Subtract(minus);
                TimeSpan et = new TimeSpan(long.Parse(ele[1]));

                char[] chars = {'\r','\n' };
                string label = ele[2].Trim(chars);
                switch (label)
                {
                    case "an":
                    case "voice":
                    case "mix_music":

                        if (pEt > st)
                        {
                            st = pEt;
                            if (st >= et) continue;
                        }
                        pEt = et;

                        string s = GetTimeSrt(st);
                        string e = GetTimeSrt(et);
                        sb.Append(count.ToString() + "\n");
                        sb.Append(s + " --> " + e + "\n");
                        sb.Append(label + "\n\n");
                        count++;
                        break;
                    default:
                        break;
                }
            }
            result = sb.ToString();
            return true;
        }
        private static string GetTimeSrt(TimeSpan t)
        {
            return t.Hours.ToString("00") + ":" + t.Minutes.ToString("00") + ":" + t.Seconds.ToString("00") + "," + t.Milliseconds.ToString("000");
        }

        public static bool MlfSimplify(string mlf, out string result)
        {
            List<string> stList = new List<string>();
            List<string> etList = new List<string>();
            List<string> labelList = new List<string>();
            StringBuilder sb = new StringBuilder();

            string[] lines = mlf.Split('\n');
            bool first = true;
            foreach (string line in lines)
            {
                if (line.StartsWith("#"))
                {
                    sb.Append(line + "\n");
                    continue;
                }
                string[] ele = line.Split('\t');
                if (ele.Length < 3) continue;
                char[] chars = { '\r', '\n' };
                string label = ele[2].Trim(chars);
                int prevCount = stList.Count - 1;
                if (!first && labelList[prevCount].Equals(label) && etList[prevCount].Equals(ele[0]))
                {
                    etList[prevCount] = ele[1];
                }
                else
                {
                    first = false;
                    stList.Add(ele[0]);
                    etList.Add(ele[1]);
                    labelList.Add(label);
                }
            }
            for (int i = 0; i < stList.Count; i++)
            {
                sb.Append(stList[i] + "\t" + etList[i] + "\t" + labelList[i] + "\n");
            }
            result = sb.ToString();
            return true;
        }

        private static string anyLabel = "<any>";
        // recall: tgt = lab, src = mlf, precision: tgt = mlf, src = lab
        public static string EvalRecallPrecision(string prefix, string tgt, string src, int margin) 
        {
            StringBuilder sb = new StringBuilder();
            List<string> labelList = GetLabelList(src);

            foreach (string tgtLabel in labelList)
            {

                Dictionary<long, string> tgtDic = GetEvalDic(tgt,tgtLabel, 0);
                Dictionary<long, string> srcDic = GetEvalDic(src, tgtLabel, margin);

                int sum = 0;
                int correct = 0;
                foreach (long time in tgtDic.Keys)
                {
                    sum++;
                    string key;
                    if (srcDic.TryGetValue(time, out key))
                    {
                        if (key.Equals(tgtDic[time]) || key.Equals(anyLabel))
                        {
                            correct++;
                        }
                    }
                }
                float val = 0;
                if (sum > 0) val = (float)correct / (float)sum;
                sb.Append(prefix + "," +  tgtLabel + "," + val.ToString() + "," + correct.ToString() + "," + sum.ToString() + "\n");

                tgtDic.Clear();
                srcDic.Clear();
                tgtDic = null;
                srcDic = null;
            }
            return sb.ToString();

        }

        private static List<string> GetLabelList(string mlf)
        {

            List<string> list = new List<string>();

            string[] lines = mlf.Split('\n');
            foreach (string line in lines)
            {
                char[] sepa = { '\t', ' ' };
                string[] ele = line.Split(sepa);
                if (ele.Length < 3) continue;
                char[] chars = { '\r', '\n' };
                string label = ele[2].Trim(chars);
                if (!list.Contains(label)) list.Add(label);
            }
            return list;
        }
        private static Dictionary<long, string> GetEvalDic(string mlf,string tgtLabel, int marginMsec)
        {
            // 1 frame = 10ms
            int margin = marginMsec / 10;
            Dictionary<long, string> dic = new Dictionary<long, string>();

            StringBuilder sb = new StringBuilder();

            string[] lines = mlf.Split('\n');
            foreach (string line in lines)
            {
                char[] sepa = { '\t', ' ' };
                string[] ele = line.Split(sepa);
                if (ele.Length < 3) continue;
                char[] chars = { '\r', '\n' };
                string label = ele[2].Trim(chars);
                if (label.Equals(tgtLabel))
                {
                    long st = (long)(long.Parse(ele[0]) * 1f / 100000.0f);
                    long et = (long)(long.Parse(ele[1]) * 1f / 100000.0f);

                        //elif trange.find('frame')==0 : # [100ns] --> [frame] 
                        //t_index = 1/100000.0

                    for (long i = st; i <= et; i++)
                    {
                        if (!dic.ContainsKey(i))
                        {
                            dic.Add(i, label);
                        }
                    }
                    long ss = st - margin;
                    if (ss < 0) ss = 0;
                    for (long i = ss; i < st + margin; i++)
                    {
                        if (!dic.ContainsKey(i))
                        {
                            dic.Add(i, anyLabel);
                        }
                        else
                        {
                            dic[i] = anyLabel;
                        }
                    }
                    long ee = et - margin;
                    if (ee < 0) ee = 0;
                    for (long i = ee; i < et + margin; i++)
                    {
                        if (!dic.ContainsKey(i))
                        {
                            dic.Add(i, anyLabel);
                        }
                        else
                        {
                            dic[i] = anyLabel;
                        }
                    }
                }

            }
            return dic;
        }


        public static bool WinSub2Lab(string winSubPath, string labPath, bool interpolateOhterLabel)
        {
            SubtitleManager sm = new SubtitleManager();
            sm.LoadWINSub(File.OpenRead(winSubPath));
            if (File.Exists(labPath)) File.Delete(labPath);
            sm.DumpMlf(File.OpenWrite(labPath), true, Path.GetFileName(labPath), interpolateOhterLabel);
            return true;
        }

        private static string subtitleImagePath = "sub";
        private static string convExeName = "vsconv.exe";
        public static void ConvertSub(string workDir, string subBase)
        {
            System.Reflection.Assembly ass = System.Reflection.Assembly.GetEntryAssembly();
            string exeDir = Path.GetDirectoryName(ass.Location);

                //vsconv -f maestro -i "c:\tmp\字幕jimaku.idx" -o "c:\tmp\字幕jimaku"
                //vsconv -f winsubmux -i "c:\tmp\字幕jimaku.idx" -o "c:\tmp\字幕jimaku"

                string dir = Path.Combine(workDir, subtitleImagePath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = Path.Combine(exeDir, convExeName);// convExeName;
                p.StartInfo.Arguments = "-f winsubmux -i \"" + Path.Combine(workDir, subBase) + ".idx\" -o \"" + Path.Combine(dir, subBase) + "\"";
                p.Start();
                p.WaitForExit();
        }

        public static bool HasSrt(string workDir,string subBase)
        {
            string sP = Path.Combine(workDir, subBase) + ".srt";
            return File.Exists(sP);
        }
        public static bool HasMlf(string workDir, string subBase)
        {
            string sP = Path.Combine(workDir, subBase) + ".mlf";
            return File.Exists(sP);
        }
        public static bool HasIdxSub(string workDir, string subBase)
        {
            string sP1 = Path.Combine(workDir, subBase) + ".idx";
            string sP2 = Path.Combine(workDir, subBase) + ".sub";
            return (File.Exists(sP1) && File.Exists(sP2));
        }

        public static bool isSubtitleConverted(string workDir,string subBase)
        {
            string sP = Path.Combine(Path.Combine(workDir, subtitleImagePath), subBase) + ".sub";
            string dir = Path.Combine(workDir, subtitleImagePath);
            if (!Directory.Exists(dir)) return false;
            string[] files = Directory.GetFiles(dir, "*.png", SearchOption.TopDirectoryOnly);
            return (File.Exists(sP) && files.Length > 0);
        }

        public static void LoadSub(string workDir, string subBase, SubtitleManager sm)
        {
            //SubtitleManager 
            //sm = new SubtitleManager();
            //sm.LoadSON(File.OpenRead(Path.Combine(workDir, subBase) + ".son"));
            sm.LoadWINSub(File.OpenRead(Path.Combine(Path.Combine(workDir, subtitleImagePath), subBase) + ".sub"));
            //sm.ConvertResolution(720, 480, 320, 240);
            //if (vga)
            //{
            //    sm.ConvertResolution(720, 480, 640, 480);
            //}
            //else
            //{
            //    sm.ConvertResolution(720, 480, 320, 240);
            //}

        }
        public static void LoadSrt(string workDir, string subBase, SubtitleManager sm)
        {
            sm.LoadSrt(File.OpenRead(Path.Combine(workDir, subBase) + ".srt"));
        }
        public static void LoadMlf(string workDir, string subBase, SubtitleManager sm)
        {
            string sP = Path.Combine(workDir, subBase) + ".mlf";
            StreamReader sr = new StreamReader(sP);
            string mlf = sr.ReadToEnd();
            sr.Close();
            string son;
            string sS = Path.Combine(workDir, subBase) + ".son";
            Mlf2Son(mlf, out son);
            StreamWriter sw = new StreamWriter(sS);
            sw.Write(son);
            sw.Close();
            sm.LoadSON(File.OpenRead(sS));
        }

        public static void CreateAVS(string srcTxt, bool overlaySpeed, float mainMovieSpeed, float subtitleSpeed, int splitMinutes, string workDir, string subBase, string volume, bool overlaySubtitleImage, bool omitCreatePng, SubtitleManager sm, bool overlayPreviousSubtitleImage, bool overlayPreviousSubtitleImageUpper, bool overlayOriginalSubtitle,bool setSubtitleSpeedByReadingRate,float readingRate,bool addLogo)
        {
            ////可変速avs出力
            //SubtitleManager 
            //sm = new SubtitleManager();
            //sm.LoadSON(File.OpenRead(Path.Combine(workDir, subBase) + ".son"));

            //sm.LoadWINSub(File.OpenRead(Path.Combine(Path.Combine(workDir, subtitleImagePath), subBase) + ".sub"));

            //sm.ConvertResolution(720, 480, 320, 240);

            string avsTemp = Properties.Resources.avsTemplate;
            avsTemp = avsTemp.Replace("#<src>", srcTxt);
            sm.DumpAVSSplit(Path.Combine(workDir, volume), mainMovieSpeed, subtitleSpeed, splitMinutes, avsTemp, overlaySpeed, overlaySubtitleImage, omitCreatePng,overlayPreviousSubtitleImage,overlayPreviousSubtitleImageUpper,overlayOriginalSubtitle,setSubtitleSpeedByReadingRate,readingRate,addLogo);
            //Dump(sm);
        }

        public static void EncodeDir(string workDir,EncodeProfile profile)
        {
            string[] files = Directory.GetFiles(workDir, "*.avs", SearchOption.TopDirectoryOnly);
            foreach (string avs in files)
            {
                EncodeFile(avs, workDir, profile);
            }
        }

        public enum EncodeProfile
        {
            MiddleQVGAMP4,
            LowQVGAM4V,
            VGASwf, //エラーあり
            MiddleVGAMP4,
            MiddleQVGAWMV8, // あまり検証していない。
            MiddleVGAWMV8, // あまり検証していない。
        }
        private static string ffmpegPath = "ffmpeg.exe";
        //  Command0=""<%AppPath%>\cores\ffmpeg" -y -maxfr 30 -i "<%InputFile%>" -title "<%Title%>" -timestamp "<%TimeStamp%>" -bitexact -vcodec h264 -coder 1 -bufsize 128 -g 250 -s 320x240 -b 384 -acodec aac -ac 2 -ar 48000 -ab 64 -f mp4 "<%OutputFile%>.MP4""
        //  Command0=""<%AppPath%>\cores\ffmpeg" -y -maxfr 15 -i "<%InputFile%>" -title "<%Title%>" -timestamp "<%TimeStamp%>" -bitexact -vcodec h264 -coder 0 -bufsize 256 -g 250 -vlevel 13 -fixaspect -s 320x240 -b 192 -maxrate 768 -qmin 2 -qmax 51 -acodec aac -ac 2 -ar 48000 -ab 48 -f ipod "<%TemporaryFile%>.M4V""
        public static bool EncodeFile(string file,string workDir,EncodeProfile profile)
        {
            string filebase = Path.GetFileNameWithoutExtension(file);
            //string outFile = Path.Combine(workDir, filebase + ".MP4");
            string outFile = Path.Combine(workDir, filebase);// + ".M4V");
            string timestamp = DateTime.Now.ToString().Replace("/", "-");
            string ext = "";

            string arguments = "";
            switch (profile)
            {
                case EncodeProfile.MiddleQVGAMP4:
                    ext = ".mp4";
                    arguments = "-y -maxfr 30 -i \"" + file + "\" -title \"" + filebase + "\" -timestamp \"" + timestamp + "\" -bitexact -vcodec h264 -coder 1 -bufsize 128 -g 250 -s 320x240 -b 384 -acodec aac -ac 2 -ar 48000 -ab 64 -f mp4 \"" + outFile + ".mp4\"";
                    break;
                case EncodeProfile.LowQVGAM4V:
                    ext = ".M4V";
                    arguments = "-y -maxfr 15 -i \"" + file + "\" -title \"" + filebase + "\" -timestamp \"" + timestamp + "\" -bitexact -vcodec h264 -coder 0 -bufsize 256 -g 250 -vlevel 13 -fixaspect -s 320x240 -b 192 -maxrate 768 -qmin 2 -qmax 51 -acodec aac -ac 2 -ar 48000 -ab 48 -f ipod \"" + outFile + ".M4V\"";
                    break;
                case EncodeProfile.VGASwf:
                    ext = ".swf";
                    arguments = "-i \"" + file + "\" -title \"" + filebase + "\" -timestamp \"" + timestamp + "\" -b 1024k -r 24 -s 640x480 -ar 44100 -acodec mp3 \"" + outFile + ".swf\"";
                    break;
                case EncodeProfile.MiddleVGAMP4:
                    ext = ".mp4";
                    //arguments = "-y -maxfr 30 -i \"" + file + "\" -title \"" + filebase + "\" -timestamp \"" + timestamp + "\" -bitexact -vcodec h264 -coder 1 -bufsize 256 -g 250 -s 640x480 -b 1200 -acodec aac -ac 2 -ar 48000 -ab 64 -f mp4 \"" + outFile + ".MP4\"";
                    arguments = "-y -maxfr 30 -i \"" + file + "\" -title \"" + filebase + "\" -timestamp \"" + timestamp + "\" -bitexact -vcodec h264 -coder 1 -bufsize 256 -g 250 -s 640x480 -b 1200 -acodec aac -ac 2 -ar 48000 -ab 64 -f mp4 \"" + outFile + ".mp4\"";
                    break;
                case EncodeProfile.MiddleQVGAWMV8:
                    ext = ".wmv";
                    arguments = "-y -maxfr 30 -i \"" + file + "\" -title \"" + filebase + "\" -timestamp \"" + timestamp + "\" -bitexact -vcodec wmv2 -coder 1 -bufsize 12800k -g 250 -s 320x240 -b 384 -acodec mp3 -ac 2 -ar 48000 -ab 64 \"" + outFile + ".wmv\"";
                    break;
                case EncodeProfile.MiddleVGAWMV8:
                    ext = ".wmv";
                    arguments = "-y -maxfr 30 -i \"" + file + "\" -title \"" + filebase + "\" -timestamp \"" + timestamp + "\" -bitexact -vcodec wmv2 -coder 1 -bufsize 51200k -g 250 -s 640x480 -b 1200 -acodec mp3 -ac 2 -ar 48000 -ab 64 \"" + outFile + ".wmv\"";
                    break;
            }
            if (File.Exists(outFile + ext)) return false;

            System.Reflection.Assembly ass = System.Reflection.Assembly.GetEntryAssembly();
            string exeDir = Path.GetDirectoryName(ass.Location);

            System.Diagnostics.Process p = new System.Diagnostics.Process();

            List<System.Diagnostics.Process> list = new List<System.Diagnostics.Process>();
            list.Add(p);

            p.StartInfo.FileName = Path.Combine(exeDir, ffmpegPath);
            p.StartInfo.Arguments = arguments;// "-y -maxfr 15 -i \"" + file + "\" -title \"" + filebase + "\" -timestamp \"" + timestamp + "\" -bitexact -vcodec h264 -coder 0 -bufsize 256 -g 250 -vlevel 13 -fixaspect -s 320x240 -b 192 -maxrate 768 -qmin 2 -qmax 51 -acodec aac -ac 2 -ar 48000 -ab 48 -f ipod \"" + outFile + ".M4V\"";
            p.Start();
            while (!p.WaitForExit(100))//0))
            {
                KillProcess(list);
            }
            //p.WaitForExit();

            return KillProcess(list);

        }

        private static bool KillProcess(List<System.Diagnostics.Process> list)
        {
            //エラー報告プロセスのKILL

            // for vista
            System.Diagnostics.Process[] hProcesses = System.Diagnostics.Process.GetProcessesByName("WerFault");
            if (hProcesses.Length == 0)
            {
                //for XP
                hProcesses = System.Diagnostics.Process.GetProcessesByName("dwwin");
            }
            if (hProcesses.Length == 0)
            {
                //for 2000
                hProcesses = System.Diagnostics.Process.GetProcessesByName("DRWTSN32");
            }

            bool success = true;
            if (hProcesses.Length > 0) success = false;

            // string stPrompt = string.Empty;

            foreach (System.Diagnostics.Process hProcess in hProcesses)
            {
                try
                {
                    //stPrompt += hProcess.ProcessName + " " + hProcess.MainWindowTitle + System.Environment.NewLine;
                    if (!hProcess.CloseMainWindow())
                    {
                        hProcess.Kill();
                    }
                    hProcess.Close();
                    hProcess.Dispose();
                }catch{
                }
            }

            //MessageBox.Show(stPrompt);

            return success;
        }

        private static string rawMoviePath = "raw.avs";
        private static string rawMovieWithSubPath = "raw_sub.avs";
        private static string settingPath = "setting.txt";
        private static string ripExeName = "VSRip.exe";

        public static void GetSrcStringFromDVD(string root, out string srcTxt, string workDir, bool noCreateIdx,string subBase,bool addSubtitles)
        {
            //string 
            srcTxt = "";

            //DVDドライブから最もファイル容量の多いIFOを探す
            string dir = Path.Combine(root, "VIDEO_TS");
            string[] files = Directory.GetFiles(dir, "*.IFO", SearchOption.TopDirectoryOnly);

            long maxSize = 0;
            string maxSizeId = "";
            string maxSizeFile = "";
            bool first = true;
            foreach (string file in files)
            {
                string[] ele = Path.GetFileName(file).Split('_');
                string id = ele[1];
                FileInfo fo = new FileInfo(file);
                long f = fo.Length;
                if (first)
                {
                    first = false;
                    maxSize = f;
                    maxSizeId = id;
                    maxSizeFile = file;
                }
                else
                {
                    if (f > maxSize)
                    {
                        maxSize = f;
                        maxSizeId = id;
                        maxSizeFile = file;
                    }
                }
            }

            //src文字列生成：容量最大のIFOに対応するVOBを連結するavs文字列を生成
            string vobBase = "VTS_" + maxSizeId + "_*.VOB";
            string[] vobFiles = Directory.GetFiles(dir, vobBase, SearchOption.TopDirectoryOnly);

            string result = "";
            for (int i = 1; i < vobFiles.Length; i++)
            {
                result += "src" + i.ToString() + " = DirectShowSource(\"" + vobFiles[i] + "\")\n";
            }
            result += "src = ";
            for (int j = 1; j < vobFiles.Length; j++)
            {
                result += "src" + j.ToString();
                if (j < vobFiles.Length - 1)
                {
                    result += " ++ ";
                }
            }
            srcTxt = result;
            result += "\n#<body>\nreturn src\n";
            using (StreamWriter sw = new StreamWriter(Path.Combine(workDir, rawMoviePath)))
            {
                sw.Write(result);
                sw.Close();
            }

            //字幕抽出
            if (!noCreateIdx)
            {
                string settingTemplate = Properties.Resources.vsriptemp;
                settingTemplate = settingTemplate.Replace("<ifo>", maxSizeFile);
                //workDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), workDir);
                if (!Directory.Exists(workDir))
                {
                    Directory.CreateDirectory(workDir);
                }
                settingTemplate = settingTemplate.Replace("<outdir>", Path.Combine(workDir, subBase));
                using (StreamWriter sw = new StreamWriter(settingPath))
                {
                    sw.Write(settingTemplate);
                    sw.Close();
                }
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = ripExeName;
                p.StartInfo.Arguments = settingPath;
                p.Start();
                p.WaitForExit();
            }

            //字幕付き生動画出力
            string subS = "";
            if (addSubtitles)
            {
                subS = "src = src.VobSub(\"" + Path.Combine(workDir, subBase) + ".idx\")\n";
                srcTxt += "\n" + subS;
            }
            using (StreamWriter sw = new StreamWriter(Path.Combine(workDir, rawMovieWithSubPath)))
            {

                sw.Write(result.Replace("#<body>\n", subS + "#<body>\n"));
                sw.Close();
            }

        }

        public static void GetSrcStringFromMovie(string path, out string srcTxt,string workDir,bool addSubtitles)
        {
            srcTxt = "";
            string idxPath = Path.Combine(workDir, Path.GetFileNameWithoutExtension(path) + ".idx");

            //src文字列生成：動画から
            srcTxt = "src = DirectShowSource(\"" + path + "\")\n";
            //if (addSubtitles)
            //{
            //    srcTxt += "src = src.VobSub(\"" + idxPath + "\")\n";
            //}
        }


    }
}