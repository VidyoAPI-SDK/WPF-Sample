using System;
using System.Reflection;
using System.Windows;
using VidyoClient;
using VidyoConnector.Listeners;
using VidyoConnector.ViewModel;

namespace VidyoConnector.Listeners
{
    class ConnectionPropertiesListener : ListenerBase, Connector.IRegisterConnectionPropertiesEventListener        
    {
        public ConnectionPropertiesListener(VidyoConnectorViewModel viewModel) : base(viewModel) { }

        public void OnConnectionPropertiesChanged(ConnectorConnectionProperties connectionProperties)
        {
            ViewModel.OnConnectionPropertiesChanged(connectionProperties);
        }
    }
}