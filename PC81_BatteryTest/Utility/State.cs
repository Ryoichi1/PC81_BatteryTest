using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;
using System.Windows;

namespace PC81_BatteryTest
{
    public class InputData
    {
        public INPUT_NAME name;
        public bool input;
    }


    public enum INPUT_NAME
    {

        SRC, SRH, RC, RH, CHSTON, MF, BON, PD, MENTE, CM, HM, EDM,/*CN26*/
        CIR, HIR, THW, ESC1, ESH1, ESON, ESOFF,/*TB1,CN10,CN11*/
        CLS, VB/*TB3*/
    }

    public class TestSpecs
    {
        public int Key;
        public string Value;
        public bool PowSw;

        public TestSpecs(int key, string value, bool powSW = true)
        {
            this.Key = key;
            this.Value = value;
            this.PowSw = powSW;

        }
    }

    public static class State
    {
        //データソース（バインディング用）
        public static ViewModelMainWindow VmMainWindow = new ViewModelMainWindow();
        public static ViewModelTestStatus VmTestStatus = new ViewModelTestStatus();


        public static List<InputData> InputDataList = new List<InputData>();


        //パブリックメンバ
        public static Configuration Setting { get; set; }

        public static string CurrDir { get; set; }

        public static string AssemblyInfo { get; set; }

        public static double CurrentThemeOpacity { get; set; }

        public static Uri uriErrInfoPage { get; set; }


        //リトライ履歴保存用リスト
        public static List<string> RetryLogList = new List<string>();


        public static List<TestSpecs> 日常点検項目 = new List<TestSpecs>()
        {


        };



        //個別設定のロード
        public static void LoadConfigData()
        {
            //Configファイルのロード
            Setting = Deserialize<Configuration>(Constants.filePath_Configuration);
            if (Setting.日付 != DateTime.Now.ToString("yyyyMMdd"))
            {
                Setting.日付 = DateTime.Now.ToString("yyyyMMdd");
                Setting.TodayOkCount = 0;
                Setting.TodayNgCount = 0;
            }


            VmMainWindow.Theme = Setting.PathTheme;
            VmMainWindow.ThemeOpacity = Setting.OpacityTheme;
            State.VmTestStatus.KeyWord = Setting.KeyWord;
            State.VmTestStatus.SetKeyWord = true;
            VmTestStatus.OkCount = Setting.TodayOkCount.ToString() + "台";
            VmTestStatus.NgCount = Setting.TodayNgCount.ToString() + "台";
            State.VmTestStatus.VolSpec = Setting.VolSpec;
            State.VmTestStatus.VoiceSpec = Setting.VoiceSpec;
            State.VmTestStatus.Opecode = Setting.Opecode;
            State.VmTestStatus.SetOpecode = true;
            State.VmTestStatus.DcYear = Setting.Dc.Substring(0, 2);
            State.VmTestStatus.DcMonth = Setting.Dc.Substring(2, 2);


        }


        //インスタンスをXMLデータに変換する
        public static bool Serialization<T>(T obj, string xmlFilePath)
        {
            try
            {
                //XmlSerializerオブジェクトを作成
                //オブジェクトの型を指定する
                System.Xml.Serialization.XmlSerializer serializer =
                    new System.Xml.Serialization.XmlSerializer(typeof(T));
                //書き込むファイルを開く（UTF-8 BOM無し）
                System.IO.StreamWriter sw = new System.IO.StreamWriter(xmlFilePath, false, new System.Text.UTF8Encoding(false));
                //シリアル化し、XMLファイルに保存する
                serializer.Serialize(sw, obj);
                //ファイルを閉じる
                sw.Close();

                return true;

            }
            catch
            {
                return false;
            }

        }

        //XMLデータからインスタンスを生成する
        public static T Deserialize<T>(string xmlFilePath)
        {
            System.Xml.Serialization.XmlSerializer serializer;
            using (var sr = new System.IO.StreamReader(xmlFilePath, new System.Text.UTF8Encoding(false)))
            {
                serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(sr);
            }
        }

        //********************************************************
        //個別設定データの保存
        //********************************************************
        public static bool Save個別データ()
        {
            try
            {
                //Configファイルの保存
                Setting.PathTheme = VmMainWindow.Theme;
                Setting.OpacityTheme = VmMainWindow.ThemeOpacity;

                if (State.VmTestStatus.SetOpecode)
                {
                    Setting.Opecode = State.VmTestStatus.Opecode;
                }

                if (State.VmTestStatus.SetDc)
                {
                    Setting.Dc = State.VmTestStatus.DcYear + State.VmTestStatus.DcMonth;
                }

                if (State.VmTestStatus.SetKeyWord)
                {
                    Setting.KeyWord = State.VmTestStatus.KeyWord;
                }



                Setting.VolSpec = State.VmTestStatus.VolSpec;
                Setting.VoiceSpec = State.VmTestStatus.VoiceSpec;

                Serialization<Configuration>(Setting, Constants.filePath_Configuration);

                return true;
            }
            catch
            {
                return false;

            }

        }







    }

}
