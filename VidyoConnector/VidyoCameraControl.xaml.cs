using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using System.Windows.Input;
using VidyoClient;
using VidyoConnector.Commands;
using VidyoConnector.Listeners;
using VidyoConnector.ViewModel;
using static VidyoClient.Connector;
using VidyoConferenceModeration.Listeners;
using System.Windows.Media;
using VidyoConnector;
using System.Data;
using SearchUsersDialog;
using static VidyoClient.Participant;
using VidyoConnector.Model;
using static VidyoClient.CameraControlCapabilities;

namespace VidyoConnector
{
    /// <summary>
    /// Interaction logic for LocalCameraControl.xaml
    /// </summary>
    public partial class VidyoCameraControl : Window
    {
        LocalCameraModel camera;
        bool isNudge;
        public VidyoCameraControl()
        {
            InitializeComponent();
            camera = null;
            isNudge = false;
        }

        public void Init()
        {
            if (camera == null)
            {
                return;
            }


        }
        public void SetSelectedLocalCamera(LocalCameraModel c)
        {
            camera = c;
        }

        public new void Show()
        {
            if (camera == null)
            {
                return;
            }

            TextBoxCameraName.Text = camera.DisplayName;
            CameraControlCapabilities caps = camera.Object.GetControlCapabilities();

            CheckBoxpanTiltHasRubberBand.IsChecked = caps.panTiltHasRubberBand;
            CheckBoxpanTiltHasContinuousMove.IsChecked = caps.panTiltHasContinuousMove;
            CheckBoxpanTiltHasNudge.IsChecked = caps.panTiltHasNudge;

            CheckBoxzoomHasRubberBand.IsChecked = caps.zoomHasRubberBand;
            CheckBoxzooomHasContinuousMove.IsChecked = caps.zooomHasContinuousMove;
            CheckBoxzoomHasNudge.IsChecked = caps.zoomHasNudge;

            CheckBoxhasPhotoCapture.IsChecked = caps.hasPhotoCapture;
            CheckBoxhasViscaSupport.IsChecked = caps.hasViscaSupport;
            CheckBoxhasPresetSupport.IsChecked = caps.hasPresetSupport;

            RadioButtonNudge.IsEnabled = caps.panTiltHasNudge || caps.zoomHasNudge;
            RadioButtonContinuous.IsEnabled = caps.panTiltHasContinuousMove || caps.zooomHasContinuousMove;

            base.ShowDialog();
        }

        private void toggleButtons()
        {
            if(isNudge)
            {
                ButtonRight.IsEnabled = ButtonLeft.IsEnabled = ButtonUp.IsEnabled = ButtonDown.IsEnabled = CheckBoxpanTiltHasNudge.IsChecked.Value;
                ButtonZoomIn.IsEnabled = ButtonZoomOut.IsEnabled = CheckBoxzoomHasNudge.IsChecked.Value;
            }
            else
            {
                ButtonRight.IsEnabled = ButtonLeft.IsEnabled = ButtonUp.IsEnabled = ButtonDown.IsEnabled = CheckBoxpanTiltHasContinuousMove.IsChecked.Value;
                ButtonZoomIn.IsEnabled = ButtonZoomOut.IsEnabled = CheckBoxzooomHasContinuousMove.IsChecked.Value;
            }
            TextBoxTimeout.IsEnabled = !isNudge;
        }

        private CameraControlDirection GetDirection(int pan, int tilt, int zoom)
        {
            if(pan == -1)
            {
                return CameraControlDirection.CameracontroldirectionPanLeft;
            }

            if (pan == 1)
            {
                return CameraControlDirection.CameracontroldirectionPanRight;
            }

            if (tilt == -1)
            {
                return CameraControlDirection.CameracontroldirectionTiltDown;
            }

            if (tilt == 1)
            {
                return CameraControlDirection.CameracontroldirectionTiltUp;
            }

            if (zoom == -1)
            {
                return CameraControlDirection.CameracontroldirectionZoomOut;
            }

            if (zoom == 1)
            {
                return CameraControlDirection.CameracontroldirectionZoomIn;
            }

            return CameraControlDirection.CameracontroldirectionPanLeft;
        }

        private void sendPanTiltZoomCommand(int pan, int tilt, int zoom)
        {
            if(isNudge)
            {
                camera.Object.ControlPTZ(pan, tilt, zoom);
            }
            else
            {
                CameraControlDirection dir = GetDirection(pan, tilt, zoom);
                camera.Object.ControlPTZStart(dir, ulong.Parse(TextBoxTimeout.Text.ToString()));
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            e.Cancel = true;
        }

        private void RadioButtonNudgeMode_Checked(object sender, RoutedEventArgs e)
        {
            isNudge = true;
            toggleButtons();
        }

        private void RadioButtonContinousMode_Checked(object sender, RoutedEventArgs e)
        {
            isNudge = false;
            toggleButtons();
        }

        private void ButtonLeft_Click(object sender, RoutedEventArgs e)
        {
            sendPanTiltZoomCommand(-1, 0, 0);
        }

        private void ButtonRight_Click(object sender, RoutedEventArgs e)
        {
            sendPanTiltZoomCommand(1, 0, 0);
        }
        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            sendPanTiltZoomCommand(0, 1, 0);
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            sendPanTiltZoomCommand(0, -1, 0);
        }

        private void ButtonZoomIn_Click(object sender, RoutedEventArgs e)
        {
            sendPanTiltZoomCommand(0, 0, 1);
        }

        private void ButtonZoomOut_Click(object sender, RoutedEventArgs e)
        {
            sendPanTiltZoomCommand(0, 0, -1);
        }
    }
}
