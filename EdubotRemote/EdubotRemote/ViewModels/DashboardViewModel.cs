using Plugin.BLE.Abstractions.Contracts;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace EdubotRemote.ViewModels
{
    public class DashboardViewModel : BindableBase, INavigationAware
    {
        private IAdapter _btAdapter;
        private IDevice _btDevice;
        private ICharacteristic _characteristic;
        private string _selectedCommand;

        public DashboardViewModel(IAdapter btAdapter)
        {
            _btAdapter = btAdapter;
        }

        public string ArrowCircleDown => "\uf0ab";
        public Color ArrowCircleDownColor => GetColor("s");
        public string ArrowCircleLeft => "\uf0a8";
        public Color ArrowCircleLeftColor => GetColor("a");
        public string ArrowCircleRight => "\uf0a9";
        public Color ArrowCircleRightColor => GetColor("d");
        public string ArrowCircleUp => "\uf0aa";
        public Color ArrowCircleUpColor => GetColor("w");
        public ICommand DirectionalCommand => new DelegateCommand<string>(async (val) => await SendDataAsync(val));
        public string LDRValue { get; set; }
        public string StopCircle => "\uf28d";
        public Color StopCircleColor => GetColor("x");

        public async Task ConnectToDeviceAsync(IDevice bleDevice)
        {
            await _btAdapter.ConnectToDeviceAsync(bleDevice);
            var service = await bleDevice.GetServiceAsync(new Guid("0000ffe0-0000-1000-8000-00805f9b34fb"));

            if (service != null)
            {
                //Read Write Property
                var characteristic = await service.GetCharacteristicAsync(new Guid("0000ffe1-0000-1000-8000-00805f9b34fb"));

                if (characteristic != null)
                {
                    _characteristic = characteristic;
                }
            }
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            _btDevice = parameters["Device"] as IDevice;
            ConnectToDeviceAsync(_btDevice);
        }

        private Color GetColor(string command)
        {
            return command == _selectedCommand ? Color.Red : Color.Black;
        }
        private async Task SendDataAsync(string value)
        {
            await _characteristic?.WriteAsync(Encoding.ASCII.GetBytes(value));

            _selectedCommand = value;
            UpdateControlColors();
        }

        private void UpdateControlColors()
        {
            RaisePropertyChanged(nameof(ArrowCircleDownColor));
            RaisePropertyChanged(nameof(ArrowCircleUpColor));
            RaisePropertyChanged(nameof(ArrowCircleLeftColor));
            RaisePropertyChanged(nameof(ArrowCircleRightColor));
            RaisePropertyChanged(nameof(StopCircleColor));
        }
    }
}
