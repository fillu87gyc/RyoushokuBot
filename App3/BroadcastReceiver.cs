﻿using System;
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
using Java.Util;

namespace App3
{
	[BroadcastReceiver]
	public class MyBroadcastReceiver : BroadcastReceiver
	{
		string local = @"/storage/emulated/0/Ryoshoku/加工済み/";
		public override void OnReceive(Context context, Intent intent)
		{
			//インテントをキャッチ
			var keys = intent.GetStringArrayListExtra("Keys");
			var token = CoreTweet.Tokens.Create(keys[0], keys[1], keys[2], keys[3]);
			var cal = Calendar.GetInstance(Locale.Default);
			string picName = (cal.Get(CalendarField.DayOfWeek) - 2).ToString();
			if (picName == "-1") picName = "6";
			var tweet = "本日 " + cal.Get(CalendarField.Year) + "年" + (cal.Get(CalendarField.Month) + 1).ToString() + 
				"月" + cal.Get(CalendarField.DayOfMonth) + "日の";
			int hour = cal.Get(CalendarField.HourOfDay);
			if (hour== 7)
			{
				picName += 0;
				tweet += "朝食はこちらです";
			}
			else if (hour == 11)
			{
				picName += 1;
				tweet += "昼食はこちらです";
			}
			else //if(cal.hour == 17)
			{
				picName += 2;
				tweet += "夕食はこちらです";
			}
			//for (int i = 0; i < cal.dayofweek; i++)
			//{
			//	tweet += " ";
			//}
			//tweet += ".";
			picName += ".gif";
			CoreTweet.MediaUploadResult first = null;
			try
			{
				first = token.Media.Upload(media: new System.IO.FileInfo(local + picName));
				token.Statuses.Update(
					status: tweet,
					media_ids: new long[] { first.MediaId }
				);
			}
			catch (Exception)
			{
				tweet = "画像検索中にエラーが発生しました\n管理者に連絡してください\n";
				token.Statuses.Update(status: tweet);
			}


		}
		//void debug(Context context, Intent intent)
		//{
		//	Toast.MakeText(context, "ツイートしました これは" + intent.GetStringExtra("num"), ToastLength.Long).Show();
		//	var time = new Android.Text.Format.Time("Asia/Tokyo");
		//	time.SetToNow();
		//	string date = time.Year + "-" + (time.Month + 1) + "-" + time.MonthDay + "-" + time.Hour + "-" + time.Minute + "-" + time.Second + "";
		//	Android.Util.Log.Debug("hogehoge", date + "\t" + intent.GetStringExtra("num"));
		//}
	}
}