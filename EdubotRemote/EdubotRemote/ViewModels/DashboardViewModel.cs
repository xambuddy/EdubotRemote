using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Prism;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;
using Xamarin.Forms;

namespace EdubotRemote.ViewModels
{
    public class DashboardViewModel : BindableBase, INavigationAware
    {
        private IAdapter _btAdapter;
        private IDevice _btDevice;
        private ICharacteristic _characteristic;

        public string LDRValue { get; set; }

        public ICommand DirectionalCommand => new DelegateCommand<string>(async (val) => await SendDataAsync(val));


        public DashboardViewModel(IAdapter btAdapter)
        {
            _btAdapter = btAdapter;
        }

        private async Task SendDataAsync(string value)
        {
            await _characteristic?.WriteAsync(Encoding.ASCII.GetBytes(value));
        }

        public string ArrowCircleUp => "\uf0aa";
        public string ArrowCircleLeft => "\uf0a8";
        public string ArrowCircleRight => "\uf0a9";
        public string ArrowCircleDown => "\uf0ab";
        public string StopCircle => "\uf28d";

        EventHandler<CharacteristicUpdatedEventArgs> valueUpdatedHandler;

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
    }
}
