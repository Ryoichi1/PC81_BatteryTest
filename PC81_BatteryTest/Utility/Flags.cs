using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PC81_BatteryTest
{
    public static class Flags
    {
        public static bool OtherPage { get; set; }

        //試験開始時に初期化が必要なフラグ
        public static bool StopInit周辺機器 { get; set; }
        public static bool Initializing周辺機器 { get; set; }
        public static bool Testing { get; set; }


        public static bool FlagStop音声認識 { get; set; }

        //周辺機器ステータス
        private static bool _Hioki3228;
        public static bool Hioki3228
        {
            get { return _Hioki3228; }
            set
            {
                _Hioki3228 = value;
                State.VmTestStatus.Color3228 = value ? Brushes.DodgerBlue : Brushes.OrangeRed;
            }
        }


        private static bool _Mic;
        public static bool Mic
        {
            get { return _Mic; }
            set
            {
                _Mic = value;
                State.VmTestStatus.ColorMic = value ? Brushes.DodgerBlue : Brushes.OrangeRed;
            }
        }

        public static bool AllOk周辺機器接続 { get; set; }



    }
}
