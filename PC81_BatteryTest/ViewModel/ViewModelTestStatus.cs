using Microsoft.Practices.Prism.Mvvm;
using System.Windows.Media;

namespace PC81_BatteryTest
{
    public class ViewModelTestStatus : BindableBase
    {




        //判定表示のプロパティ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

        //判定表示　PASS or FAIL
        private string _Decision;
        public string Decision
        {
            get { return _Decision; }
            set { SetProperty(ref _Decision, value); }
        }




        private Brush _Color判定;
        public Brush Color判定
        {
            get { return _Color判定; }
            set { SetProperty(ref _Color判定, value); }
        }


        private Brush _ColorVol;
        public Brush ColorVol
        {
            get { return _ColorVol; }
            set { SetProperty(ref _ColorVol, value); }
        }

        //ステータス表示部のプロパティ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

        private string _OkCount;
        public string OkCount
        {
            get { return _OkCount; }
            set { SetProperty(ref _OkCount, value); }
        }

        private string _NgCount;
        public string NgCount
        {
            get { return _NgCount; }
            set { SetProperty(ref _NgCount, value); }
        }








        //その他のプロパティ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■



        //作業者へのメッセージ
        private string _Message;
        public string Message
        {
            get { return _Message; }
            set { SetProperty(ref _Message, value); }
        }








        private Brush _Color3228;
        public Brush Color3228
        {
            get { return _Color3228; }
            set { SetProperty(ref _Color3228, value); }
        }

        private Brush _ColorMic;
        public Brush ColorMic
        {
            get { return _ColorMic; }
            set { SetProperty(ref _ColorMic, value); }
        }


        private Brush _ColorVoice;
        public Brush ColorVoice
        {
            get { return _ColorVoice; }
            set { SetProperty(ref _ColorVoice, value); }
        }

        private bool _SetOpecode;
        public bool SetOpecode
        {
            get { return _SetOpecode; }

            set
            {
                SetProperty(ref _SetOpecode, value);

                if (!value)
                {
                    Opecode = "";
                }

            }
        }



        private bool _SetDc;
        public bool SetDc
        {
            get { return _SetDc; }

            set
            {
                SetProperty(ref _SetDc, value);
            }
        }

        private bool _SetKeyWord;
        public bool SetKeyWord
        {
            get { return _SetKeyWord; }

            set
            {
                SetProperty(ref _SetKeyWord, value);

                if (!value)
                {
                    KeyWord = "";
                }

            }
        }
    

        private string _Opecode;
        public string Opecode
        {
            get { return _Opecode; }
            set { SetProperty(ref _Opecode, value); }
        }

        private string _DcYear;
        public string DcYear
        {
            get { return _DcYear; }
            set { SetProperty(ref _DcYear, value); }
        }

        private string _DcMonth;
        public string DcMonth
        {
            get { return _DcMonth; }
            set { SetProperty(ref _DcMonth, value); }
        }
        private string _KeyWord;
        public string KeyWord
        {
            get { return _KeyWord; }
            set { SetProperty(ref _KeyWord, value); }
        }



        private string _VolData;
        public string VolData
        {
            get { return _VolData; }
            set { SetProperty(ref _VolData, value); }
        }


        private double _VolSpec;
        public double VolSpec
        {
            get { return _VolSpec; }
            set {
                SetProperty(ref _VolSpec, value);
                MessVolSpec = "合格範囲 " + VolSpec.ToString("F2")  + "V以上";
            }
        }

        private string _MessVolSpec;
        public string MessVolSpec
        {
            get { return _MessVolSpec; }
            set { SetProperty(ref _MessVolSpec, value); }
        }

        private double _VoiceSpec;
        public double VoiceSpec
        {
            get { return _VoiceSpec; }
            set
            {
                SetProperty(ref _VoiceSpec, value);
                MessVoiceSpec = "音声認識レベル閾値 " + VoiceSpec.ToString("F0") + "％以上";
            }
        }

        private string _MessVoiceSpec;
        public string MessVoiceSpec
        {
            get { return _MessVoiceSpec; }
            set { SetProperty(ref _MessVoiceSpec, value); }
        }

        private string _音声認識率;
        public string 音声認識率
        {
            get { return _音声認識率; }
            set { SetProperty(ref _音声認識率, value); }
        }

    }
}
