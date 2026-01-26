using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using VidyoClient;
using VidyoConnector.Model;
using VidyoConnector.ViewModel;
using static VidyoClient.Connector;

namespace VidyoMicrophone.ViewModel
{
    class VidyoMicrophoneViewModel : VidyoConnectorViewModel
    {
        private object DataContext = null;
        LocalMicrophoneModel micToSelect = null;

        public VidyoMicrophoneViewModel(object Context)
        {
            DataContext = Context;
        }

        public void Init()
        {
            micToSelect = ((VidyoConnectorViewModel)DataContext).GetSelectedLocalMicrophone();
            SelectedMicrophoneName = micToSelect.DisplayName;
            VolumeText = micToSelect.Object.GetVolume().ToString();
            AutoGainText = micToSelect.Object.GetAutoGain() ? "On" : "Off";
            SetVolume = SetAutoGain = false;
        }

        public bool Apply()
        {
            if (string.IsNullOrEmpty(micToSelect.DisplayName))
            {
                MessageBox.Show("Invalid microphone selected.", "Vidyo Microphone");
                return false;
            }

            if (SetVolume == false && SetAutoGain == false)
            {
                MessageBox.Show("No changes to apply.", "Vidyo Microphone");
                return false;
            }

            if (SetVolume)
            {
                if (int.TryParse(VolumeText, out int volume) && volume >= 0 && volume <= 100)
                {
                    micToSelect.Object.SetVolume((uint)long.Parse(VolumeText));
                }
                else
                {
                    MessageBox.Show("Volume must be an integer between 0 and 100.", "Invalid Value");
                    return false;
                }
            }

            if (SetAutoGain)
            {
                if (AutoGainText.Equals("On", StringComparison.OrdinalIgnoreCase) || AutoGainText.Equals("Off", StringComparison.OrdinalIgnoreCase))
                {
                    bool isAutoGain = AutoGainText.Equals("On", StringComparison.OrdinalIgnoreCase);
                    if(micToSelect.Object.SetAutoGain(isAutoGain) == false) {
                        MessageBox.Show("Failed to set.", "Set Auto Gain");
                    }
                }
                else
                {
                    MessageBox.Show("AutoGainText must be either 'On' or 'Off'.", "Invalid Value");
                    return false;
                }
            }
                return true;
        }

        private string _selectedMicrophoneName;
        public string SelectedMicrophoneName
        {
            get { return _selectedMicrophoneName; }
            set
            {
                _selectedMicrophoneName = value;
                OnPropertyChanged();
            }
        }

        private string _autoGainText;
        public string AutoGainText
        {
            get { return _autoGainText; }
            set
            {
                _autoGainText = value;
                OnPropertyChanged();
            }
        }

        private string _volumeText;
        public string VolumeText
        {
            get { return _volumeText; }
            set
            {
                _volumeText = value;
                OnPropertyChanged();
            }
        }

        private bool _setAutoGain;
        public bool SetAutoGain
        {
            get { return _setAutoGain; }
            set { _setAutoGain = value; OnPropertyChanged(); }
        }

        private bool _setVolume;
        public bool SetVolume
        {
            get { return _setVolume; }
            set { _setVolume = value; OnPropertyChanged(); }
        }
    }
}