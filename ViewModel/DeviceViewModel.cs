using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Linq;
using System.Windows.Input;
using Acr.UserDialogs;
using FinalBPM.Model;
using Plugin.BluetoothLE;
using ReactiveUI;
using System.Reactive.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace FinalBPM.ViewModel
{
    namespace FinalBPM.ViewModel
    {
        public class DeviceViewModel : Infrastructure.ViewModel
        {
            readonly IDevice device;


            public DeviceViewModel(IDevice device)
            {
                IDisposable watcher;
                this.device = device;
                UserDialogs.Instance.Alert("Once connected user may press 'Measure!' to start the HeartBeat ");
                UserDialogs.Instance.Alert("User may press 'Connect' for a one-time measurement, or 'Pair Device' for future measurements");

                this.device
                    .WhenStatusChanged()
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(status =>
                    {
                        switch (status)
                        {
                            case ConnectionStatus.Connecting:
                                this.ConnectText = "Cancel Connection";
                                break;

                            case ConnectionStatus.Connected:
                                this.ConnectText = "Disconnect";
                                break;

                            case ConnectionStatus.Disconnected:
                                this.ConnectText = "Connect";
                                try
                                {
                                    this.GattCharacteristics.Clear();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex);
                                }

                                break;
                        }
                    });

                this.SelectCharacteristic = ReactiveCommand.Create<GattCharacteristicViewModel>(x => x.Select());

                this.ConnectionToggle = ReactiveCommand.Create(() =>
                {
                    // don't cleanup connection - force user to d/c
                    if (this.device.Status == ConnectionStatus.Disconnected)
                    {
                        this.device.Connect();
                        UserDialogs.Instance.Alert("Successfully Connected!");
                        ReadyToMeasure = true;

                    }
                    else
                    {
                        this.device.CancelConnection();
                        ReadyToMeasure = false;
                    }
                });
                this.MeasureToggle = ReactiveCommand.CreateFromTask(
                    async x =>
                    {
                        var bytestwo = new byte[] {0x30};
                        var bytes = new byte[] { 0x31 };
                        if (this.device.Status == ConnectionStatus.Connected)
                        {
                            this.characteristics = await this.device
                                .GetCharacteristicsForService(Constants.ScratchServiceUuid)
                                .Take(5)
                                .Timeout(Constants.OperationTimeout)
                                .ToArray()
                                .ToTask();
                        }

                        if (LED)
                        {
                            var t1 = this.characteristics[0].Write(bytes).Timeout(Constants.OperationTimeout).ToTask();
                        }
                        else
                        {
                            var t1 = this.characteristics[0].Write(bytestwo).Timeout(Constants.OperationTimeout).ToTask();
                        }

                        LED = !LED;
                        /*await this.device
                            .WriteCharacteristic(
                                Constants.ScratchServiceUuid,
                                Constants.ScratchCharacteristicUuid1,
                                new byte[] {0x31}
                            )
                            .ToTask();*/
                    }
                );
                this.PairToDevice = ReactiveCommand.Create(() =>
                {
                    if (!this.device.Features.HasFlag(DeviceFeatures.PairingRequests))
                    {
                        UserDialogs.Instance.Toast("Pairing is not supported on this platform");
                    }
                    else if (this.device.PairingStatus == PairingStatus.Paired)
                    {
                        UserDialogs.Instance.Toast("Device is already paired");
                    }
                    else
                    {
                        this.device
                            .PairingRequest()
                            .Subscribe(x =>
                            {
                                var txt = x ? "Device Paired Successfully" : "Device Pairing Failed";
                                UserDialogs.Instance.Toast(txt);
                                this.RaisePropertyChanged(nameof(this.PairingText));
                            });
                    }
                });

               
            }

            public ICommand MeasureToggle { get; }
            public ICommand ConnectionToggle { get; }
            public ICommand PairToDevice { get; }
            public ICommand RequestMtu { get; }
            public ICommand SelectCharacteristic { get; }
            IGattCharacteristic[] characteristics;

            public string Name => this.device.Name ?? "Unknown";
            public Guid Uuid => this.device.Uuid;
            public string PairingText => this.device.PairingStatus == PairingStatus.Paired ? "Device Paired" : "Pair Device";
            public ObservableCollection<Group<GattCharacteristicViewModel>> GattCharacteristics { get; } = new ObservableCollection<Group<GattCharacteristicViewModel>>();
            public bool LED;
            public string basic = "test";
            string connectText = "Connect";
            public string ConnectText
            {
                get => this.connectText;
                private set => this.RaiseAndSetIfChanged(ref this.connectText, value);
            }


            private bool measureReady;
            public bool ReadyToMeasure
            {
                get => measureReady;
                private set => this.RaiseAndSetIfChanged(ref this.measureReady, value);

            }
            int rssi;
            public int Rssi
            {
                get => this.rssi;
                private set => this.RaiseAndSetIfChanged(ref this.rssi, value);
            }
        }
    }

}
