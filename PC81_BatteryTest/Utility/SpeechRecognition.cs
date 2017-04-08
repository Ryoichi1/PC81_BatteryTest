using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PC81_BatteryTest
{
    public static class SpeechRecognition
    {
        public static SpeechRecognitionEngine sre;
        public static double 一致率;


        static SpeechRecognition()
        {
            sre = new SpeechRecognitionEngine();

        }

        public static bool Init()
        {
            try
            {
                //入力ソース（既定のマイク）
                sre.SetInputToDefaultAudioDevice();//マイクが接続されていないとここで例外が発生する
                return true;
            }
            catch
            {
                return false;
            }

        }

        public static void set()
        {
            //イベント登録
            sre.RecognizeCompleted += (object sender, RecognizeCompletedEventArgs e) =>
            {

                if (e.Result == null)
                {
                    一致率 = 0;
                }
                else
                {
                    一致率 = e.Result.Confidence * 100;
                }
                State.VmTestStatus.音声認識率 = "音声一致率 " + 一致率.ToString("F0") + "%";
                Flags.FlagStop音声認識 = true;



            };

            //中途半端なワードを認識した場合
            sre.SpeechRecognitionRejected += (se, e) =>
            {

            };





        }

        public static void 音声認識()
        {
            try
            {
                State.VmTestStatus.音声認識率 = "音声一致率 ----";
                //語彙登録
                //Choices words = new Choices(new string[] { "", "", "", "" });

                //GrammarBuilderインスタンス
                GrammarBuilder gb = new GrammarBuilder(State.VmTestStatus.KeyWord);

                //GrammarBuilderインスタンスに語彙を追加
                //gb.Append(words);

                //Grammarインスタンスを作成
                Grammar g = new Grammar(gb);
                //Grammarインスタンスをロード
                sre.LoadGrammar(g);
                //Console.Write("認識開始" + "\n");

                //入力ソース（既定のマイク）
                sre.SetInputToDefaultAudioDevice();//マイクが接続されていないとここで例外が発生する


                Flags.FlagStop音声認識 = false;
                sre.RecognizeAsync(RecognizeMode.Single);//非同期で認識開始

                while (true)
                {
                    if (Flags.FlagStop音声認識)
                    {
                        sre.RecognizeAsyncCancel();
                        sre.RecognizeAsyncStop();
                        break;
                    }

                }
            }
            catch
            {
                MessageBox.Show("マイク接続異常\r\nアプリケーションを閉じます");
                Environment.Exit(0);

            }


        }

    }
}
