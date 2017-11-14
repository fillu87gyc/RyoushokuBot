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
	public class MyBroadcastReceiver : BroadcastReceiver
	{
		public override void OnReceive(Context context, Intent intent)
		{
			//インテントをキャッチ
			var keys = intent.GetStringArrayListExtra("Keys");
			var token =  CoreTweet.Tokens.Create(keys[0], keys[1], keys[2], keys[3]);
			//token.Statuses.Update(new { status = "でもついーと２" });
			Toast.MakeText(context,"ツイートしました",ToastLength.Long).Show();
		}
	}
}