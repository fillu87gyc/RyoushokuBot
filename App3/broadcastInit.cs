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

namespace App3
{
	[BroadcastReceiver]
	class BroadcastInit:BroadcastReceiver
	{
		public BroadcastInit()
		{

		}
		public override void OnReceive(Context context, Intent intent)
		{
		}

	}
}