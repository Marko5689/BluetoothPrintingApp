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

            SetContentView(Resource.Layout.BT);

            
            Button ScanBtn = FindViewById<Button>(Resource.Id.btdt_SearchButton);
            ScanBtn.Click += ScanBtn_Click;
            

            RegisterReceiver(new BTReceiver(), new IntentFilter(BluetoothDevice.ActionFound));

            
            deviceList = FindViewById<ListView>(Resource.Id.btdt_listView);

            deviceList.ItemClick += DeviceList_ItemClick;
            Toast toast = Toast.MakeText(this, "Testing output", ToastLength.Short);
            toast.Show();

        }

        private void DeviceList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var item = devices[e.Position];
            Intent intent = new Intent(this, typeof(MainActivity));
            intent.PutExtra("Address",item.Address);

            this.StartActivity(intent);
            this.Finish();
            //Toast toast = Toast.MakeText(this, "", ToastLength.Long);
            //toast.Show();
        }

        private void  ScanBtn_Click(object sender, EventArgs e)
        {
            devices.Clear();
            Toast toast = Toast.MakeText(this, "Searching for Device now", ToastLength.Short);
            toast.Show();
            if (!mAdapter.IsDiscovering)
            {                
                mAdapter.StartDiscovery();
            } 
        }

        public class BTReceiver : BroadcastReceiver
        {
            public override void OnReceive(Context context, Intent intent)
            {
                String action = intent.Action;
                if (BluetoothDevice.ActionFound == action)
                {
                    BluetoothDevice device = intent.GetParcelableExtra(BluetoothDevice.ExtraDevice) as BluetoothDevice;

                    devices.Add(new Device(device.Name, device.Address));

                   
                    Device item = devices.First(d => d.Name == device.Name);
                    if (item == null)
                    { devices.Add(new Device(device.Name, device.Address)); }
                    aAdapter = new CustomAdapter(_instance, devices);
                    deviceList.Adapter = aAdapter;
                    aAdapter.NotifyDataSetChanged();

                    //Toast toast = Toast.MakeText(Android.App.Application.Context, device.Name + " " + device.Address, ToastLength.Long);
                    //toast.Show();
                }
            }
        }


}
}