using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Input;
using Acr.UserDialogs;
using FinalBPM.View;
using FinalBPM.ViewModel.FinalBPM.ViewModel;
using Plugin.BluetoothLE;
using ReactiveUI;

namespace FinalBPM.ViewModel
{
    public class ScanViewModel : Infrastructure.ViewModel
    {
        IDisposable scan;


        public ScanViewModel()
        {
            UserDialogs.Instance.Alert("Select device 'HeartBeat' to begin measurement process, if Bluetooth capability is not enabled, press 'Toggle Adapter' to enable, or 'Open Settings' to view device settings");
            UserDialogs.Instance.Alert("Welcome! This demo App allows users to take, log, and observe blood pressure and heart-rate measurements");

            this.Devices = new ObservableCollection<ScanResultViewModel>();

            CrossBleAdapter
                .Current
                .WhenStatusChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(_ => this.RaisePropertyChanged(nameof(this.Title)));

            this.SelectDevice = ReactiveCommand.CreateFromTask<ScanResultViewModel>(async x =>
            {
                this.scan?.Dispose();
                await App.Current.MainPage.Navigation.PushAsync(new DevicePage
                {
                   BindingContext = new DeviceViewModel(x.Device)
                });
            });

            this.OpenSettings = ReactiveCommand.Create(() =>
            {
                if (CrossBleAdapter.Current.Features.HasFlag(AdapterFeatures.OpenSettings))
                {
                    CrossBleAdapter.Current.OpenSettings();
                }
                else
                {
                    UserDialogs.Instance.Alert("Cannot open bluetooth settings");
                }
            });

            this.ToggleAdapterState = ReactiveCommand.Create(
                () =>
                {
                    if (CrossBleAdapter.Current.CanControlAdapterState())
                    {
                        var poweredOn = CrossBleAdapter.Current.Status == AdapterStatus.PoweredOn;
                        CrossBleAdapter.Current.SetAdapterState(!poweredOn);
                    }
                    else
                    {
                        UserDialogs.Instance.Alert("Cannot change bluetooth adapter state");
                    }
                }
            );

            this.ScanToggle = ReactiveCommand.Create(
                () =>
                {
                    if (this.IsScanning)
                    {
                        this.scan?.Dispose();
                        this.IsScanning = false;
                    }
                    else
                    {
                        this.Devices.Clear();

                        this.IsScanning = true;
                        this.scan = CrossBleAdapter
                            .Current
                            .Scan()
                            .Buffer(TimeSpan.FromSeconds(1))
                            .ObserveOn(RxApp.MainThreadScheduler)
                            .Subscribe(results =>
                            {
                                foreach (var result in results)
                                    this.OnScanResult(result);
                            });
                    }
                },
                CrossBleAdapter
                    .Current
                    .WhenStatusChanged()
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Select(x => x == AdapterStatus.PoweredOn)
            );
        }


        public override void OnDeactivated()
        {
            base.OnDeactivated();
            this.scan?.Dispose();
        }


        public ICommand ScanToggle { get; }
        public ICommand OpenSettings { get; }
        public ICommand ToggleAdapterState { get; }
        public ICommand SelectDevice { get; }
        public ObservableCollection<ScanResultViewModel> Devices { get; }


        public string Title => $"{CrossBleAdapter.Current.DeviceName} ({CrossBleAdapter.Current.Status})";


        bool scanning;
        public bool IsScanning
        {
            get => this.scanning;
            private set => this.RaiseAndSetIfChanged(ref this.scanning, value);
        }


        void OnScanResult(IScanResult result)
        {
            var dev = this.Devices.FirstOrDefault(x => x.Uuid.Equals(result.Device.Uuid));
            if (dev != null)
            {
                dev.TrySet(result);
            }
            else
            {
                dev = new ScanResultViewModel();
                dev.TrySet(result);
                this.Devices.Add(dev);
            }
        }
    }
}
