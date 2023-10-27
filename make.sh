mcs /target:library /out:Subtitle.DLL SubtitleBase.cs 
mcs /out:hik.exe /reference:Subtitle.DLL main.cs

#mono hik.exe