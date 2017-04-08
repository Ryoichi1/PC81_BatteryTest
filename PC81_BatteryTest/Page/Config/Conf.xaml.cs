using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PC81_BatteryTest
{
    /// <summary>
    /// Config.xaml の相互作用ロジック
    /// </summary>
    public partial class Conf
    {
        private NavigationService naviTheme;
        Uri uriThemePage = new Uri("Page/Config/Theme.xaml", UriKind.Relative);


        public Conf()
        {
            InitializeComponent();
            naviTheme = FrameTheme.NavigationService;
            FrameTheme.NavigationUIVisibility = NavigationUIVisibility.Hidden;


            TabMenu.SelectedIndex = 0;


            // オブジェクト作成に必要なコードをこの下に挿入します。
        }




        private void TabTheme_Loaded(object sender, RoutedEventArgs e)
        {
            naviTheme.Navigate(uriThemePage);
        }





    }
}
