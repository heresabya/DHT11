// Copyright (c) Microsoft. All rights reserved.

using Sensors.Dht;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HelloWorld
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const int piN = 4;
        private const int sensoR = 11;
        private GpioPin _pin1;
        private IDht _dht1;
        


        public MainPage()
        {
            InitializeComponent();
        }

        private async void ClickMe_Click(object sender, RoutedEventArgs e)
        {
            //HelloMessage.Text = "Hello, Windows 10 IoT Core!";
            GpioController controller = GpioController.GetDefault();
            if (controller != null)
            {                
                _pin1 = GpioController.GetDefault().OpenPin(piN, GpioSharingMode.Exclusive);
                _pin1.SetDriveMode(GpioPinDriveMode.Input);

                _dht1 = new Dht11(_pin1, GpioPinDriveMode.Input);

            }

            DhtReading reading = new DhtReading();            
            reading = await _dht1.GetReadingAsync().AsTask();
            Stopwatch stopwatch  = new Stopwatch();
            stopwatch.Start();

            while (stopwatch.Elapsed < TimeSpan.FromSeconds(60))
            {
                if (reading.IsValid)
                {
                    var Temperature = reading.Temperature;
                    var Humidity = reading.Humidity;
                    var messageDialog = new MessageDialog(string.Format("Temperature : {0} - Humidity : {1}", Temperature, Humidity));
                    Task.Delay(5000).Wait();
                    await messageDialog.ShowAsync();
                }

                stopwatch.Stop();                              
                                
            }               

        }
    }
}
