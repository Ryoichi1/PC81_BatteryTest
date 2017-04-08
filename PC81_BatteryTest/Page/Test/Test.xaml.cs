using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;


namespace PC81_BatteryTest
{
    /// <summary>
    /// Test.xaml の相互作用ロジック
    /// </summary>
    public partial class Test
    {


        public Test()
        {
            this.InitializeComponent();


            // オブジェクト作成に必要なコードをこの下に挿入します。
            this.DataContext = State.VmTestStatus;

            (FindResource("Blink") as Storyboard).Begin();



            CheckUserInput();

            buttonKeyWordClear.Content = "クリア";
            tbKeyWord.Foreground = Brushes.White;
            tbKeyWord.IsReadOnly = true;

            tbOpecode.Foreground = Brushes.Gray;

            State.VmTestStatus.音声認識率 = "音声認識率 ---";


            /////////////////////////////////////////////////////////


            SpeechRecognition.set();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {

            //フォームの初期化
            //西暦コンボボックスの設定
            var thisYear = DateTime.Now.Year - 2000;
            comboBox西暦.Items.Add(thisYear.ToString());
            comboBox西暦.Items.Add((thisYear - 1).ToString());

            foreach (var m in Enumerable.Range(1, 12))
            {
                comboBox月.Items.Add((m * 4).ToString("D2"));
            }

            comboBox西暦.SelectedIndex = comboBox西暦.Items.IndexOf(State.VmTestStatus.DcYear);
            comboBox月.SelectedIndex = comboBox月.Items.IndexOf(State.VmTestStatus.DcMonth);
            State.VmTestStatus.SetDc = true;

            buttonKeyWordClear.Content = "クリア";





        }


        public async void CheckUserInput()
        {
            while (true)
            {
                await Task.Run(() =>
                {
                    while (true)
                    {

                        if (!State.VmTestStatus.SetOpecode)
                        {
                            State.VmTestStatus.Message = "工番を入力してください";
                        }
                        else if (!State.VmTestStatus.SetDc)
                        {
                            State.VmTestStatus.Message = "デートコードを入力してください";
                        }
                        else if (!State.VmTestStatus.SetKeyWord)
                        {
                            State.VmTestStatus.Message = "開始キーワードを入力してください";
                        }
                        else if (!Flags.AllOk周辺機器接続)
                        {
                            State.VmTestStatus.Message = "周辺機器の接続を確認してください";
                        }
                        else
                        {
                            State.VmTestStatus.Message = "音声認識中です・・・・";
                            CheckVol();
                            break;
                        }
                    }


                });
                Storyboard sb = Resources["SlideDecision"] as Storyboard;
                this.BeginStoryboard(sb);
                await Task.Delay(3000);
                State.VmTestStatus.Decision = "";

;
                
            }

        }






        private void tbOpecode_TextChanged(object sender, TextChangedEventArgs e)
        {


            if (State.VmTestStatus.Opecode.Length != 13) return;
            //以降は工番が正しく入力されているかどうかの判定
            if (System.Text.RegularExpressions.Regex.IsMatch(
                State.VmTestStatus.Opecode, @"^\d-\d\d-\d\d\d\d-\d\d\d$",
                System.Text.RegularExpressions.RegexOptions.ECMAScript))
            {

                State.VmTestStatus.SetOpecode = true;
                tbOpecode.Foreground = Brushes.White;
                tbOpecode.IsReadOnly = true;


            }
        }


        
        private void buttonOpeCodeClear_Click(object sender, RoutedEventArgs e)
        {
            Flags.FlagStop音声認識 = true;

            tbOpecode.Foreground = Brushes.Gray;
            State.VmTestStatus.SetOpecode = false;
            tbOpecode.IsReadOnly = false;
        }

        private void buttonKeyWordClear_Click(object sender, RoutedEventArgs e)
        {
            if (State.VmTestStatus.SetKeyWord)
            {
                Flags.FlagStop音声認識 = true;
                State.VmTestStatus.SetKeyWord = false;
                buttonKeyWordClear.Content = "登録";
                tbKeyWord.IsReadOnly = false;
                tbKeyWord.Foreground = Brushes.Gray;
            }
            else
            {
                if (State.VmTestStatus.KeyWord == "") return;
                State.VmTestStatus.SetKeyWord = true;
                buttonKeyWordClear.Content = "クリア";
                tbKeyWord.IsReadOnly = true;
                tbKeyWord.Foreground = Brushes.White;

            }



        }

        private void comboBox西暦_DropDownClosed(object sender, EventArgs e)
        {
            if (comboBox西暦.SelectedIndex == -1) return;
            State.VmTestStatus.DcYear = comboBox西暦.SelectedItem.ToString();

            if ( (comboBox西暦.SelectedIndex != -1) && (comboBox月.SelectedIndex != -1))
            {
                State.VmTestStatus.SetDc = true;
            }
        }

        private void comboBox月_DropDownClosed(object sender, EventArgs e)
        {
            if (comboBox月.SelectedIndex == -1) return;
            State.VmTestStatus.DcMonth = comboBox月.SelectedItem.ToString();

            if ((comboBox西暦.SelectedIndex != -1) && (comboBox月.SelectedIndex != -1))
            {
                State.VmTestStatus.SetDc = true;
            }
        }

        //メインルーチン
        public void CheckVol()
        {

            SpeechRecognition.音声認識();
            State.VmTestStatus.Decision = "";

            if (SpeechRecognition.一致率 < State.VmTestStatus.VoiceSpec) return;

            State.VmTestStatus.Message = "計測しています・・・";
            Measure();


        }

        private void Measure()
        {



            //TODO: 電圧表示の色を派手にする（計測中であることを明示）
            Thread.Sleep(1000);

            var data = Hioki323x.VoltData;
            if (data >= State.VmTestStatus.VolSpec)
            {
                WriteCsv(data.ToString("F2") + "V", true);
                State.VmTestStatus.Decision = "PASS";
                State.VmTestStatus.Color判定 = Brushes.DodgerBlue;
                General.PlaySound(General.soundPass);



            }
            else
            {

                State.VmTestStatus.Decision = "FAIL";
                State.VmTestStatus.Color判定 = Brushes.HotPink;
                General.PlaySound(General.soundFail);
            }


            State.VmTestStatus.音声認識率 = "音声一致率 ----";

        }

        private bool WriteCsv(string data, bool result)
        {
            try
            {
                string 工番 = State.VmTestStatus.Opecode;
                string filePath = Constants.DataFolderPath + 工番 + ".csv";
                string 判定 = result ? "OK" : "NG";
                //appendをtrueにすると，既存のファイルに追記
                //falseにすると，ファイルを新規作成する
                var append = System.IO.File.Exists(filePath) ? true : false;
                // 出力用のファイルを開く
                using (var sw = new System.IO.StreamWriter(filePath, append, Encoding.GetEncoding("Shift_JIS")))
                {
                    sw.WriteLine(System.DateTime.Now.ToString("yyyy年MM月dd日(ddd) HH時mm分ss秒") + "," + State.VmTestStatus.DcYear + State.VmTestStatus.DcMonth + "Ne," +
                                                              "規格値：" + State.VmTestStatus.VolSpec.ToString("F2") + "V以上," + data + "," + 判定);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }













    }
}
