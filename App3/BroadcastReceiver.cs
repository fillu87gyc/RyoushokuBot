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
using Java.Util.Logging;

namespace App3
{
	[BroadcastReceiver]
	public class MyBroadcastReceiver : BroadcastReceiver
	{
		public override void OnReceive(Context context, Intent intent)
		{
			//インテントをキャッチ
			var keys = intent.GetStringArrayListExtra("Keys");
			var token = CoreTweet.Tokens.Create(keys[0], keys[1], keys[2], keys[3]);
			CoreTweet.MediaUploadResult first = token.Media.Upload(media: new System.IO.FileInfo(@"/storage/emulated/0/temp_photo.jpeg"));
			token.Statuses.Update(
				status:  "でもついーと" + intent.GetStringExtra("num"), 
				media_ids: new long[] { first.MediaId }
			);
			Toast.MakeText(context, "ツイートしました これは" + intent.GetStringExtra("num"), ToastLength.Long).Show();
			var time = new Android.Text.Format.Time("Asia/Tokyo");
			time.SetToNow();
			string date = time.Year + "-" + (time.Month + 1) + "-" + time.MonthDay + "-"+time.Hour + "-" + time.Minute + "-" + time.Second + "";
			Android.Util.Log.Debug("hogehoge", date+"\t"+intent.GetStringExtra("num"));
		}
	}
}