using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Threading.Tasks;
using System.Media;
using System.Windows.Media;
using System.Diagnostics;
using System.Speech.Recognition;

namespace PC81_BatteryTest
{


    public static class General
    {


        //インスタンス変数の宣言
        public static SoundPlayer player = null;
        public static SoundPlayer soundPass = null;
        public static SoundPlayer soundFail = null;
        public static SoundPlayer soundOperator = null;
        public static SoundPlayer soundModel = null;
        public static SoundPlayer soundOpecode = null;
        public static SoundPlayer soundAlarm = null;
        public static SoundPlayer soundKuru = null;
        public static SoundPlayer soundCutin = null;
        public static SoundPlayer soundWavePass = null;
        public static SoundPlayer soundSerialLabel = null;

        static General()
        {
            //オーディオリソースを取り出す
            General.soundPass = new SoundPlayer(@"Resources\Pass.wav");
            General.soundFail = new SoundPlayer(@"Resources\Fail.wav");
            General.soundOperator = new SoundPlayer(@"Resources\Operator.wav");
            General.soundModel = new SoundPlayer(@"Resources\Model.wav");
            General.soundOpecode = new SoundPlayer(@"Resources\Opecode.wav");
            General.soundAlarm = new SoundPlayer(@"Resources\Alarm.wav");
            General.soundKuru = new SoundPlayer(@"Resources\Kuru.wav");
            //General.soundCutin = new SoundPlayer(@"Resources\enshutsu01.wav");
            //General.soundSerialLabel = new SoundPlayer(@"Resources\BGM_Label.wav");
        }







        //**************************************************************************
        //WAVEファイルを再生する
        //引数：なし
        //戻値：なし
        //**************************************************************************  

        //WAVEファイルを再生する（非同期で再生）
        public static void PlaySound(SoundPlayer p)
        {
            //再生されているときは止める
            if (player != null)
                player.Stop();

            //waveファイルを読み込む
            player = p;
            //最後まで再生し終えるまで待機する
            player.Play();
        }

        public static void PlaySoundLoop(SoundPlayer p)
        {
            //再生されているときは止める
            if (player != null)
                player.Stop();

            //waveファイルを読み込む
            player = p;
            //最後まで再生し終えるまで待機する
            player.PlayLooping();
        }

        //再生されているWAVEファイルを止める
        public static void StopSound()
        {
            if (player != null)
            {
                player.Stop();
                player.Dispose();
                player = null;
            }
        }

        public static void ResetViewModel()//TODO:
        {

            //ViewModel OK台数、NG台数、Total台数の更新
            State.VmTestStatus.OkCount = State.Setting.TodayOkCount.ToString() + "台";
            State.VmTestStatus.NgCount = State.Setting.TodayNgCount.ToString() + "台";

            State.VmMainWindow.EnableOtherButton = true;



            Flags.Testing = false;


            //テーマ透過度を元に戻す
            State.VmMainWindow.ThemeOpacity = State.CurrentThemeOpacity;

        }

        public static void Init周辺機器()//TODO:
        {
            Flags.Initializing周辺機器 = true;

            //マルチメータの初期化
            Task.Run(() =>
            {
                while (true)
                {
                    Flags.Hioki3228 = Hioki323x.Init();
                    if (Flags.Hioki3228) break;
                    Thread.Sleep(400);
                }

                Hioki323x.SetDcVolt(Hioki323x.DCV_Range.R20V);
                while (true)
                {
                    Hioki323x.GetDcVolt();
                    var data = Hioki323x.VoltData;
                    State.VmTestStatus.VolData = data.ToString("F4") + "V";
                    if (data >= State.VmTestStatus.VolSpec)
                    {
                        State.VmTestStatus.ColorVol = Brushes.DodgerBlue;
                    }
                    else
                    {
                        State.VmTestStatus.ColorVol = Brushes.HotPink;
                    }
                    //Thread.Sleep(100);
                }


            });

            //マイクの初期化
            Task.Run(() =>
            {
                Flags.Mic = false;
                while (true)
                {

                    Flags.Mic = SpeechRecognition.Init();
                    if (Flags.Mic) break;

                    Thread.Sleep(1000);
                }
            });

            Task.Run(() =>
            {
                while (true)
                {
                    Flags.AllOk周辺機器接続 = (Flags.Hioki3228 && Flags.Mic);
                    if (Flags.AllOk周辺機器接続 || Flags.StopInit周辺機器) break;
                    Thread.Sleep(400);

                }
                Flags.Initializing周辺機器 = false;
            });


        }


    }

}

