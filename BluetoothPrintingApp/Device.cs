using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BluetoothPrintingApp
{
    public class Device
    {
        string _name;
        string _address;

        public string Name { get { return _name; } set { _name = value; } }
        public string Address { get { return _address; } set { _address = value; } }

        public Device(string name, string address)
        {
            Name = name;
            Address = address;
        }
    }
}