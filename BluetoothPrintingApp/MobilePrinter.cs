using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Bluetooth;

using Java.IO;
using Java.Util;

namespace BluetoothPrintingApp
{
    class MobilePrinter
    {

        private BluetoothSocket mSocket;
        private string _address = string.Empty;
        private static string UU_ID = "00001101-0000-1000-8000-00805f9b34fb";
        BluetoothAdapter mAdapter = BluetoothAdapter.DefaultAdapter;
        //private static List<Device> dlist;
        //public static List<Device> DList { get { return dlist; } }


        public MobilePrinter(string mAddress)
        {
            _address = mAddress;
        }

        public string Connect()
        {
            try
            {
                if (mAdapter == null)
                {
                    return "No Bluetooth Adapter Found";
                }

                if (!mAdapter.IsEnabled)
                {
                    Intent intent = new Intent(BluetoothAdapter.ActionRequestEnable);
                    Android.App.Application.Context.StartActivity(intent);
                }
            }
            catch (Exception ex)
            {
                return "Please check that Bluetooth permissions are granted and Bluetooth is turned on in the device!";
            }
            if(mAdapter.StartDiscovery())
            {
                BluetoothDevice mDevice = mAdapter.GetRemoteDevice(_address);

                if(mDevice == null)
                {
                    return "Unable to find printer. Please check for the printer address and try again";
                }

                mSocket = mDevice.CreateInsecureRfcommSocketToServiceRecord(UUID.FromString(UU_ID));

                try
                {
                    mSocket.Connect();
                }
                catch (Exception ex)
                {
                    return "Unable to pair with printer with the device";
                }
            }
            return string.Empty;
        }

        public void PrintText(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);

            if(!mSocket.IsConnected)
            {
                mSocket.Connect();
            }

            mSocket.OutputStream.Write(buffer, 0, buffer.Length);

            Java.Lang.Thread.Sleep(1000);
        }

        public void Close()
        {
            mSocket.Close();
            mSocket.Dispose();
        }
        //public void SearchForDevices()
        //{
        //    if(mAdapter.StartDiscovery() == false)
        //    {
        //        mAdapter.StartDiscovery();
        //    }
        //}
        
    }

}