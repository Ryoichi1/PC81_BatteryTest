using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.ViewModel;
using System.Windows.Media;



namespace PC81_BatteryTest
{

    public class ViewModelMainWindow : BindableBase
    {

        public ViewModelMainWindow()
        {

        }




        private string _Theme;
        public string Theme
        {
            get { return _Theme; }
            set { SetProperty(ref _Theme, value); }
        }


        private double _ThemeOpacity;
        public double ThemeOpacity
        {
            get { return _ThemeOpacity; }
            set { SetProperty(ref _ThemeOpacity, value); }
        }





        private bool _EnableOtherButton;
        public bool EnableOtherButton
        {
            get { return _EnableOtherButton; }
            set { SetProperty(ref _EnableOtherButton, value); }
        }



        private int _TabIndex;
        public int TabIndex
        {

            get { return _TabIndex; }
            set { SetProperty(ref _TabIndex, value); }

        }












    }
}
