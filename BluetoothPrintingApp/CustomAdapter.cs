using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BluetoothPrintingApp
{
    public class CustomAdapter : BaseAdapter<Device>
    {
        private ObservableCollection<Device> _devices;
        private Activity _context;

        public CustomAdapter(Activity context, ObservableCollection<Device> devices) : base()
        {
            _context = context;
            _devices = devices;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var device = _devices[position];

            View view = convertView;
            if(view == null)
            {
                view = ((Activity)_context).LayoutInflater.Inflate(Resource.Layout.DeviceList, null);
            }
            view.FindViewById<TextView>(Resource.Id.textName).Text = device.Name;
            view.FindViewById<TextView>(Resource.Id.textAddress).Text = device.Address;

            return view;

        }

        public override int Count
        {
            get { return _devices.Count; }
        }

        public override Device this[int position]
        {
            get { return _devices[position]; }
        }
    }
}