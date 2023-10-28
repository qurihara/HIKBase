# HIKBase

UnityがインストールされているPCで、ターミナルを開き、

$ sh make.sh

でコンパイルされる。

$ mono hik.exe

でサンプルコードが実行される。

# 簡単な解説

SubtitleBase.cs

をUnityのプロジェクトにいれておくと（つまりAssetsの下のどこかに置いておくと？）、関連クラスが使えるようになる。

using SubtitleUtil;

で関連クラスが使えるようになる。

## Cardクラス

場に置いてある札１枚に対しCardクラスを1つインスタンス化する。句のIDと、その読みテキストを内部に持っている。最初に一度作るけど、あとは直接使うことはない。

## CardManagerクラス

場にあるCardをすべて格納するクラス。void GetListStaringWith(string s)によって、文字列sから始まるCardをすべて取得できる。この関数を実行後、
- List<int> GetActiveCardIds()
- List<int> GetInactiveCardIds()

の２つの関数により、「まだ正解の可能性のある札idのリスト」と「もう正解の可能性がない札idのリスト」が得られる。なぜGetListStartingWith関数の戻り値としてこれらリストを返さないかというと、ものすごく短時間に何回もGetListStaringWith(）が呼ばれる想定なので、そのたびにListオブジェクトをnewしなくてもよいようにしようと思ったから。これはメモリと実行時間の節約が目的。でも現状でList<int>を2つnewしているので、中途半端。次にリファクタリングするなら、GetActiveCardIds()とGetInactiveCardIds()でList<int>を毎回newしないようにしたいところ。

## SubtitleManagerクラス

一つの句の音声読み上げに対し、一つのSubtitleManagerクラスをインスタンス化する。

字幕ファイル（アノテーションファイル）を読み込んで、格納する。

読み上げ音声の再生位置 t を入力として、その再生位置ではどの文字まで読まれているかを返す　string GetSubtitleByPosition(float currentPosition)　が定義されている。これをAudioSourceの再生後Update関数の中で、AudioSource.timeで再生位置を取得し使う。

## Subltitleクラス

一つの字幕（アノテーション）区間を表現するクラス。開発者が使うことはない。SubtitleManagerクラスが内部で使うだけ。



