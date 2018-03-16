using Android.App;
using Android.Widget;
using Android.OS;
using Android.Bluetooth;
using Java.IO;
using Java.Util;

using System;
using System.Collections.Generic;
using System.IO;
using Android.Content;
using Android.Views;
using Android.Util;

namespace BluetoothPrintingApp
{
    [Activity(Label = "BluetoothPrintingApp", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var _address = Intent.GetStringExtra("Address");

            string mAddress = _address == null ? "00:2A:0A:03:8A:E0" : _address;

            //string mAddress = "00:02:0A:03:8A:E0";
            MobilePrinter mobilePrinter = new MobilePrinter(mAddress);

            mobilePrinter.Connect();

            Button printBtn = FindViewById<Button>(Resource.Id.PrintBtn);
            EditText text = FindViewById<EditText>(Resource.Id.UserInputTxt);

            Button selectPrinterBtn = FindViewById<Button>(Resource.Id.ConnectPrinterBtn);
            selectPrinterBtn.Click += SelectPrinterBtn_Click;

            //Button scanBtn = FindViewById<Button>(Resource.Id.ScanBtn);
            //ListView PairedListView = FindViewById<ListView>(Resource.Id.pairedDevicesList);


            printBtn.Click += delegate
            {
                try
                {
                    if (text.Text.Length > 0)
                    {
                        Toast roast = Toast.MakeText(this, "Printing. Please be patient", ToastLength.Long);
                        roast.Show();
                        mobilePrinter.PrintText(text.Text + "\n");
                    }
                    else
                    {
                        Toast toast = Toast.MakeText(this, "Please input some texts", ToastLength.Long);
                        toast.Show();
                    }
                }
                catch (Exception ex)
                {
                    Toast boast = Toast.MakeText(this, "Bluetooth printer or bluetooth has not been switch on,please make sure the printer and bluetooth is switch on and restart the app", ToastLength.Long);
                    boast.Show();

                }
            };
        }

        private void SelectPrinterBtn_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(SearchDeviceActivity));
        }
    }
}


