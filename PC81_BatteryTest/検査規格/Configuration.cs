using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC81_BatteryTest
{
    public class Configuration
    {

        public string 日付 { get; set; }
        public int TodayOkCount { get; set; }
        public int TodayNgCount { get; set; }
        public string PathTheme { get; set; }
        public double OpacityTheme { get; set; }

        public string KeyWord { get; set; }
        public double VolSpec { get; set; }
        public double VoiceSpec { get; set; }

        public string Opecode { get; set; }

        public string Dc { get; set; }


    }
}
