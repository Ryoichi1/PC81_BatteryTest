using System;
using System.IO.Ports;
using System.Threading;

namespace PC81_BatteryTest
{
    public static class Hioki323x
    {
        private const string ID_3227 = "HIOKI,323";//HIOKI,3237,0,V2.09

        //定数の宣言(クラスメンバーになります)
        public enum ErrorCode { Normal, ResponseErr, OverRange, DataErr, TimeoutErr, MeasErr, OpenErr, InitErr, SendErr, Other }
        public enum DCV_Range { R200mV, R2000mV, R20V, R200V, R1000V, 省略 }
        public enum ACV_Range { R2000mV, R20V, R200V, R700V, 省略 }

        //変数の宣言（インスタンスメンバーになります）
        private static SerialPort Port;
        private static string RecieveData;
        //private static byte statusByte;
        public static double VoltData { get; private set; }//計測したDCVデータ
        public static double AmpData { get; private set; }//計測したDCAデータ
        public static double ResData { get; private set; }//計測した抵抗値データ
        public static ErrorCode State3237 { get; set; }//Hioki3229のｽﾃｰﾀｽ
        public static DCV_Range DcRang { get; set; }



        //コンストラクタ
        static Hioki323x()
        {
            Port = new SerialPort();
        }


        //**************************************************************************
        //HIOKI3239 COMポートのオープン
        //引数：
        //戻値：bool
        //**************************************************************************
        public static bool Init()
        {
            Port = new SerialPort();

            try
            {
                //シリアルポート設定
                if (Port.IsOpen) return false;
                Port.PortName = "COM1";
                Port.BaudRate = 9600;
                Port.DataBits = 8;
                Port.DtrEnable = false;
                //Port.RtsEnable = true;
                Port.Parity = System.IO.Ports.Parity.None;
                Port.StopBits = System.IO.Ports.StopBits.One;
                Port.NewLine = "\r\n";
                Port.Open();

                Thread.Sleep(300);
                SendCommand("*IDN?");
                if (GetRecieveData() && RecieveData.Contains(ID_3227))
                {
                    SendCommand(":SYST:HEAD 0");
                    SendCommand(":SAMP:RATE FAST");
                    return true;
                }
                else
                {
                    Port.Close();
                    return false;
                }
            }
            catch
            {
                Port.Close();
                State3237 = ErrorCode.OpenErr;
                return false;
            }

        }


        //**************************************************************************
        //シリアルポートを閉じる
        //引数：
        //戻値：bool
        //**************************************************************************
        public static bool Close()
        {
            try
            {
                SendCommand(":INIT:CONT 1");//これないと3238次回立ち上げ時に制御できなくなる（工場出荷設定が必要になる）
                Thread.Sleep(300);
                if (Port.IsOpen) Port.Close();

                return true;
            }
            catch
            {
                State3237 = ErrorCode.Other;
                return false;
            }
        }

        //**************************************************************************
        //コマンドを送信する
        //引数：コマンド
        //戻値：bool
        //**************************************************************************
        private static bool SendCommand(string command, string parameter = null)
        {
            //コマンド送信前にステータスを初期化する
            State3237 = ErrorCode.Normal;

            try
            {
                Port.DiscardInBuffer();//データ送信前に受信バッファのクリア
                Port.WriteLine(command);//コマンド送信
                return true;
            }
            catch
            {
                State3237 = ErrorCode.SendErr;
                return false;
            }
            finally
            {
                Thread.Sleep(100);
            }
        }

        //**************************************************************************
        //受信データを読み取る
        //引数：指定時間（ｍｓｅｃ）
        //戻値：bool
        //**************************************************************************
        private static bool GetRecieveData(int time = 2000)
        {
            try
            {
                RecieveData = "";//念のため初期化
                Port.ReadTimeout = time;
                RecieveData = Port.ReadLine();

                return true;
            }
            catch
            {
                State3237 = ErrorCode.TimeoutErr;
                return false;
            }
        }




        //**************************************************************************
        //DC電圧の測定
        //引数：レンジを指定する電圧値
        //戻値：bool
        //**************************************************************************
        public static void SetDcVolt(DCV_Range range)
        {
            string レンジ;

            if (DcRang != range)
            {
                switch (range)
                {
                    case DCV_Range.R200mV:
                        DcRang = DCV_Range.R200mV;
                        レンジ = "199.999E-03";
                        break;
                    case DCV_Range.R2000mV:
                        DcRang = DCV_Range.R2000mV;
                        レンジ = "1.99999";
                        break;
                    case DCV_Range.R20V:
                        DcRang = DCV_Range.R20V;
                        レンジ = "19.9999";
                        break;
                    case DCV_Range.R200V:
                        DcRang = DCV_Range.R200V;
                        レンジ = "199.999";
                        break;
                    default:
                        レンジ = "19.9999";
                        break;
                }
                SendCommand("FUNC 'VOLTage:DC';VOLT:RANG " + レンジ);
                SendCommand(":INIT:CONT 0");
            }


        }

        public static bool GetDcVolt()
        {

            double buff = 0;
            try
            {

                SendCommand(":READ?");

                if (!GetRecieveData()) return false;

                return Double.TryParse(RecieveData, out buff);
            }
            catch
            {
                return false;
            }
            finally
            {
                VoltData = buff;
            }

        }
























    }
}
