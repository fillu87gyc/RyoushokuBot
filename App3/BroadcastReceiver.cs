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
			var token = CoreTweet.Tokens.Create(keys[0], keys[1], keys[2], keys[3]);
			//token.Statuses.Update(new { status = "でもついーと２" });
			Toast.MakeText(context, "ツイートしました", ToastLength.Long).Show();

			switch (intent.GetIntExtra("Meal", 4))
			{
				case (int)Meal.breakfast:
					break;
				case (int)Meal.lunch:
					break;
				case (int)Meal.dinner:
					break;

				default:
					token.Statuses.Update(new { status = "@fill_u87gyc \n寮食BOTで例外が発生しました" });
					break;
			}
		}

	}
	public class TimerSetter:Service
	{
		AlarmManager[] AlarmManager;
		public TimerSetter()
		{
			for (int i = 0; i < 3; i++)
			{
				AlarmManager = new AlarmManager[3];
				AlarmManager[i] = (AlarmManager)GetSystemService(Context.AlarmService);
			}

		}
		public override IBinder OnBind(Intent intent)
		{
			return null;
		}
		public void Breakfast(string[] keyList,Meal meal)
		{
			var alarmIntent =  new Intent();
			
				alarmIntent = new Intent(this, typeof(MyBroadcastReceiver)); //アラームレシーバはブロードキャストを継承
				alarmIntent.PutStringArrayListExtra("Keys", keyList);
				alarmIntent.PutExtra("Meal", meal);
			

			var pending = new PendingIntent[3];
			for (int i = 0; i < pending.Length; i++)
			{
				pending[i] = PendingIntent.GetBroadcast(this, i, alarmIntent[i], PendingIntentFlags.UpdateCurrent);
			}

			var alarmManager = (AlarmManager)GetSystemService(AlarmService);
			var cal = new Java.Util.Calendar[3];
			for (int i = 0; i < cal.Length; i++)
			{
				cal[i] = Java.Util.Calendar.GetInstance(Java.Util.TimeZone.Default);

				//明日の年、月、日を取得
				cal[i].Add(Java.Util.CalendarField.DayOfYear, 1);

				cal[i].Set(Java.Util.CalendarField.Year, cal[i].Get(Java.Util.CalendarField.Year));
				cal[i].Set(Java.Util.CalendarField.Month, cal[i].Get(Java.Util.CalendarField.Month));
				cal[i].Set(Java.Util.CalendarField.DayOfMonth, cal[i].Get(Java.Util.CalendarField.DayOfMonth));
				cal[i].Set(Java.Util.CalendarField.HourOfDay, cal[i].Get(Java.Util.CalendarField.HourOfDay));

				cal[i].Set(Java.Util.CalendarField.Minute, 20);
				cal[i].Set(Java.Util.CalendarField.Second, 0);
				cal[i].Set(Java.Util.CalendarField.Millisecond, 0);
			}

			cal[(int)Meal.breakfast].Set(Java.Util.CalendarField.HourOfDay, 7);
			cal[(int)Meal.breakfast].Set(Java.Util.CalendarField.Minute, 15);

			cal[(int)Meal.lunch].Set(Java.Util.CalendarField.HourOfDay, 11);

			cal[(int)Meal.dinner].Set(Java.Util.CalendarField.HourOfDay, 17);

			for (int i = 0; i < cal.Length; i++)
			{
				alarmManager.Set(AlarmType.RtcWakeup, cal[i].TimeInMillis, pending[i]);
			}
		}
		public void Lunnch()
		{

		}
		public	void Dinner()
		{

		}
	}
}