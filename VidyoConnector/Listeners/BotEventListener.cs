using System;
using System.Reflection;
using System.Windows;
using VidyoClient;
using VidyoConnector.Listeners;
using VidyoConnector.ViewModel;

namespace VidyoConnector.Listeners
{
    public class BotEventListener : ListenerBase, Connector.IRegisterBotEventListener
    {
        public BotEventListener(VidyoConnectorViewModel viewModel) : base(viewModel) { }
        public void OnBotJoined(Connector.ConnectorBotInfo info)
        {
            ViewModel.OnBotJoined(info);
        }
        public void OnBotLeft(Connector.ConnectorBotInfo info)
        {
            ViewModel.OnBotLeft(info);
        }
    }
}