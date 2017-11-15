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
using Java.Util;

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
			//token.Statuses.Update(new { status = "でもついーと２" });
			Toast.MakeText(context, "ツイートしました", ToastLength.Long).Show();

			switch (intent.GetIntExtra("Meal", -1))
			{
				case (int)Meal.breakfast:
					token.Statuses.Update(new { status = "朝ごはんです！！" });
					break;

				case (int)Meal.lunch:
					token.Statuses.Update(new { status = "昼ごはんです！！" });
					break;

				case (int)Meal.dinner:
					token.Statuses.Update(new { status = "夜ご飯です！！" });
					break;

				default:
					token.Statuses.Update(new { status = "@fill_u87gyc \n寮食BOTで例外が発生しました" });
					break;
			}
		}
	}
	//public class DailyScheduler
	//{

	//	private Context context;

	//	public DailyScheduler(Context context)
	//	{
	//		this.context = context;
	//	}

	//	/*
	//	 * duration_time(ミリ秒)後 launch_serviceを実行する
	//	 * service_idはどのサービスかを区別する為のID(同じなら上書き)
	//	 * 一回起動するとそのタイミングで毎日1回動き続ける
	//	 */
	//	 void Set(long duration_time, int service_id)
	//	{
	//		Intent intent = new Intent(context, typeof(MyBroadcastReceiver));

	//		PendingIntent action = PendingIntent.GetService(context, service_id, intent,PendingIntentFlags.UpdateCurrent);
	//		AlarmManager alarm = (AlarmManager)context.GetSystemService("alarm");
	//		alarm.SetRepeating(AlarmType.Rtc,duration_time, AlarmManager.IntervalDay, action);
	//	}

	//	/*
	//	 * 起動したい時刻(hour:minuite)を指定するバージョン
	//	 * 指定した時刻で毎日起動する
	//	 */
	//	public void SetByTime(int hour, int minuite, int service_id)
	//	{
	//		//今日の目標時刻のカレンダーインスタンス作成
	//		Java.Util.Calendar cal_target = Java.Util.Calendar.GetInstance(Java.Util.TimeZone.Default);
			
	//		cal_target.Set(CalendarField.HourOfDay, hour);
	//		cal_target.Set(CalendarField.Minute, minuite);
	//		cal_target.Set(CalendarField.Second, 0);

	//		//現在時刻のカレンダーインスタンス作成
	//		Calendar cal_now = Calendar.GetInstance(Java.Util.TimeZone.Default);

	//		//ミリ秒取得
	//		long target_ms = cal_target.TimeInMillis;
	//		long now_ms = cal_now.TimeInMillis;

	//		//今日ならそのまま指定
	//		if (target_ms >= now_ms)
	//		{
	//			Set(target_ms, service_id);
	//			//過ぎていたら明日の同時刻を指定
	//		}
	//		else
	//		{
	//			cal_target.Add(CalendarField.DayOfMonth, 1);
	//			target_ms = cal_target.TimeInMillis;
	//			Set( target_ms, service_id);
	//		}

	//	}

		/*
		 * キャンセル用
		 */
		//public <T> void cancel(Class<T> launch_service, long wake_time, int service_id)
		//{
		//	Intent intent = new Intent(context, launch_service);
		//	PendingIntent action = PendingIntent.getService(context, service_id, intent,
		//			PendingIntent.FLAG_UPDATE_CURRENT);
		//	AlarmManager alarm = (AlarmManager)context
		//			.getSystemService(Context.ALARM_SERVICE);
		//	alarm.cancel(action);
		//}
}