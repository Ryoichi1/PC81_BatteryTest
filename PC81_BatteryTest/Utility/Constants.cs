using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC81_BatteryTest
{
    public static  class Constants
    {


        //パラメータファイルのパス
        public const string DataRootPath = @"C:\PC81_BattTest_DATA";

        public static readonly string filePath_Configuration = Path.Combine(DataRootPath, @"Configuration.config");

        //検査データフォルダのパス
        public static readonly string DataFolderPath = Path.Combine(DataRootPath, @"検査データ\");

    

    }
}
