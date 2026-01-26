using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Windows;
using System.ComponentModel;
using VidyoConnector.ViewModel;
using VidyoMicrophone.ViewModel;
using VidyoConferenceModeration.ViewModel;
using System.Windows.Controls;

namespace VidyoConnector
{
    /// <summary>
    /// Interaction logic for VidyoMicrophoneOptions.xaml
    /// </summary>
    public partial class VidyoMicrophoneOptions : Window
    {
        VidyoMicrophoneViewModel microphoneView;
        public VidyoMicrophoneOptions(object Context)
        {
            InitializeComponent();
            microphoneView = new VidyoMicrophoneViewModel(Context);
            DataContext = microphoneView;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen; 
        }

        public void Init()
        {
            ((VidyoMicrophoneViewModel)DataContext).Init();
        }

        private void ButtonApplyMicrophoneOptions_Click(object sender, RoutedEventArgs e)
        {
            if (((VidyoMicrophoneViewModel)DataContext).Apply() == true) {
                this.Visibility = Visibility.Hidden;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            e.Cancel = true;
        }
        private void VolumeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if ((int.TryParse(textBox.Text, out int volume) && volume >= 0 && volume <= 100) == false)
            {
                MessageBox.Show("Volume must be an integer between 0 and 100. Reset to default value - 100", "Invalid Value");
                textBox.Text = "100";
            }
        }

        private void AutoGainTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if ((textBox.Text.Equals("On", StringComparison.OrdinalIgnoreCase) || textBox.Text.Equals("Off", StringComparison.OrdinalIgnoreCase)) == false)
            {
                MessageBox.Show("AutoGainText must be either 'On' or 'Off'. Reset to default value - On", "Invalid Value");
                textBox.Text = "On";
            }
        }
    }
}
