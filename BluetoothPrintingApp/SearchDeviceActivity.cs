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
using Android.Bluetooth;

using Java.IO;
using Java.Util;
using System.Collections.ObjectModel;

namespace BluetoothPrintingApp
{
    [Activity(Label = "SearchDeviceActivity")]
    public class SearchDeviceActivity : Activity
    {
        public static ObservableCollection<Device> devices;
        public static ArrayAdapter arrayAdapter;
        public static ListView deviceList;
        public static CustomAdapter aAdapter;

        BluetoothAdapter mAdapter;

        private static SearchDeviceActivity _instance = null;
        public static SearchDeviceActivity Instance { get { return _instance; } }


        protected override void OnCreate(Bundle savedInstanceState)
        {

            _instance = this;
            devices = new ObservableCollection<Device>();
            //paired = new ObservableCollection<Device>();
            mAdapter = BluetoothAdapter.DefaultAdapter;
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.DeviceList);

            
            Button ScanBtn = FindViewById<Button>(Resource.Id.btdt_SearchButton);
            ScanBtn.Click += ScanBtn_Click;
            

            RegisterReceiver(new BTReceiver(), new IntentFilter(BluetoothDevice.ActionFound));

            
            deviceList = FindViewById<ListView>(Resource.Id.btdt_listView);

            deviceList.ItemSelected += AvailableList_ItemSelected; ;
            Toast toast = Toast.MakeText(this, "Testing output", ToastLength.Short);
            toast.Show();
            //ListView pairedDevice = FindViewById<ListView>(Resource.Id.pairedDevicesList);
            //pairedDevice.Adapter = new ArrayAdapter<BluetoothDevice>(this, Android.Resource.Layout.SimpleListItem1, mAdapter.BondedDevices.ToList());

            //deviceList = FindViewById<ListView>(Resource.Id.newDevicesList);
            //deviceList.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, devices);

        }

        

        private void AvailableList_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Toast toast = Toast.MakeText(this, e.ToString(), ToastLength.Long);
            toast.Show();
        }

        private void  ScanBtn_Click(object sender, EventArgs e)
        {
            Toast toast = Toast.MakeText(this, "Searching for Device now", ToastLength.Short);
            toast.Show();
            if (!mAdapter.IsDiscovering)
            {                
                mAdapter.StartDiscovery();
            } 
        }

        //public List<Device> GetAvailableDevices()
        //{
        //    dlist = new List<Device>();

        //    // Register the recieve intent filter
        //    Application.Context.RegisterReceiver(new BTReceiver(), new IntentFilter(BluetoothDevice.ActionFound));

        //    Java.Lang.Thread.Sleep(5000);
        //    return dlist;
        //}
        public class BTReceiver : BroadcastReceiver
        {
            public override void OnReceive(Context context, Intent intent)
            {
                String action = intent.Action;
                if (BluetoothDevice.ActionFound == action)
                {
                    BluetoothDevice device = intent.GetParcelableExtra(BluetoothDevice.ExtraDevice) as BluetoothDevice;

                    devices.Add(new Device(device.Name, device.Address));

                    //arrayAdapter = new ArrayAdapter(Android.App.Application.Context, Resource.Layout.DeviceList, devices);
                    //aAdapter = new CustomAdapter(_instance, devices);
                   
                    Device item = devices.First(d => d.Name == device.Name);
                    if (item == null)
                    { devices.Add(new Device(device.Name, device.Address)); }
                    aAdapter = new CustomAdapter(_instance, devices);
                    deviceList.Adapter = aAdapter;
                    //deviceList.Adapter = arrayAdapter;
                    aAdapter.NotifyDataSetChanged();

                    Toast toast = Toast.MakeText(Android.App.Application.Context, device.Name + " " + device.Address, ToastLength.Long);
                    toast.Show();
                }
            }
        }

        //public void BTstartDiscovery()
        //{
        //    if(!mAdapter.IsDiscovering)
        //    {
        //        mAdapter.StartDiscovery();
        //        Toast toast = Toast.MakeText(this, "Please wait for 3-5 seconds for it to scan for available devices", ToastLength.Long);
        //        toast.Show();
        //    }
        //    else
        //    {

        //    }
        //}



}
}