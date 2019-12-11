using EdubotRemote.Views;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace EdubotRemote.ViewModels
{
    public class ConnectViewModel : BindableBase, INavigationAware
    {
        private INavigationService _navigationService;
        private IAdapter _btAdapter;
        private IDevice _btDevice;

        public ICommand TapGestureRecognizerTappedCommand { get; set; }

        private bool _isScanning;

        public bool IsScanning
        {
            get { return _isScanning; }
            set { SetProperty(ref _isScanning, value); }
        }

        private string _statusText;

        public string StatusText
        {
            get { return _statusText; }
            set { SetProperty(ref _statusText, value); }
        }

        public ConnectViewModel(INavigationService navigationService, IAdapter btAdapter)
        {
            _navigationService = navigationService;
            _btAdapter = btAdapter;
            _btAdapter.DeviceDiscovered += OnDeviceDiscovered;
            TapGestureRecognizerTappedCommand = new DelegateCommand(OnTapGestureRecognizerTapped);
        }

        private void OnDeviceDiscovered(object sender, DeviceEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Device.Name) && e.Device.Name.Contains("BT05"))
            {
                _btDevice = e.Device;
                StatusText = "Connecting...";
                NavigationParameters p = new NavigationParameters
                {
                    { "Device", _btDevice }
                };

                _navigationService.NavigateAsync(new Uri(nameof(DashboardView), UriKind.Relative), p);
            }
        }

        private async void OnTapGestureRecognizerTapped()
        {
            IsScanning = !IsScanning;

            if (_isScanning)
            {
                StatusText = "Searching...";
                await _btAdapter.StartScanningForDevicesAsync();
            }
            else
            {
                await _btAdapter.StopScanningForDevicesAsync();
                StatusText = "Tap to scan";
            }
        }


        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (_btDevice != null) _btAdapter.DisconnectDeviceAsync(_btDevice);
            StatusText = "Tap to scan.";
            IsScanning = false;
        }
    }
}
