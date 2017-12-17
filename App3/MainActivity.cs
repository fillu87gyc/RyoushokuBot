using Android.App;
using Android.Widget;
using Android.OS;
using CoreTweet;
using Android.Net;
using Android.Content;
using Android.Views;
using Java.Util;

namespace App3
{

	[Activity(Label = "App3", MainLauncher = true)]
	public class MainActivity : Activity
	{
		CoreTweet.OAuth.OAuthSession session;
		CoreTweet.Tokens tokens;
		ISharedPreferencesEditor editor;
		ISharedPreferences pref;
		const string ApiKey = "4mNukwD2LbEYn36U8zZxf44bi";
		const string ApiSecret = "FTHB63Vv7A4LizZbGDJTzLW86Oau0M6lQ0JmIvOV2fPVf82Vps";
		public MainActivity()
		{

		}
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			var button = FindViewById<Button>(Resource.Id.button1);
			button.Click += Button_Click;
			var pinButton = FindViewById<Button>(Resource.Id.button2);
			pinButton.Click += PinButton_Click;
			var txtTitle = FindViewById<EditText>(Resource.Id.editText1);

			pinButton.Enabled = false;

			var FireTimerButton = FindViewById<Button>(Resource.Id.TimerFireButtou);
			FireTimerButton.Click += FireTimerButton_Click;

			var PickButton = FindViewById<Button>(Resource.Id.PickButton);
			PickButton.Click += PickButton_Click;


			editor = GetPreferences(FileCreationMode.Private).Edit();
			pref = GetPreferences(FileCreationMode.Private);
			if (pref.GetBoolean("IsEmpty", true) == false)
			{
				tokens = Tokens.Create(ApiKey, ApiSecret, pref.GetString("Token", ""), pref.GetString("Secret", ""));
				FindViewById<TextView>(Resource.Id.textView1).Text = "認証に成功しました!";
			}

			var cal = Calendar.GetInstance(Locale.Default);
			string imgName =  (cal.Get(CalendarField.DayOfWeek) - 2).ToString();
			if (imgName== "-1") imgName = "6";
			int hour = cal.Get(CalendarField.HourOfDay);
			if (hour <= 7) imgName += 0;
			else if (hour <= 11) imgName += 1;
			else imgName += 2;
			try
			{
				var bitmap = Android.Graphics.BitmapFactory.DecodeFile(@"/storage/emulated/0/Ryoshoku/加工済み/" + imgName+".gif");
				FindViewById<ImageView>(Resource.Id.imageView1).SetImageBitmap(bitmap);
			}
			catch (System.Exception)
			{
				//ポケモンゲットだぜ！
				Toast.MakeText(this, "画像が見つからない!!", ToastLength.Long).Show();
			}
			
		}

		private void PickButton_Click(object sender, System.EventArgs e)
		{
			tokens.Statuses.Update(status: "認証確認ツイートです");
		}

		private void FireTimerButton_Click(object sender, System.EventArgs e)
		{
			var alarmIntent = new Intent(this, typeof(MyBroadcastReceiver));
			var alarmIntent2 = new Intent(this, typeof(MyBroadcastReceiver));
			var alarmIntent3 = new Intent(this, typeof(MyBroadcastReceiver));
			var keyList = new System.Collections.Generic.List<string>(4)
			{
				ApiKey,
				ApiSecret,
				tokens.AccessToken,
				tokens.AccessTokenSecret
			};

			alarmIntent.PutStringArrayListExtra("Keys", keyList);
			alarmIntent2.PutStringArrayListExtra("Keys", keyList);
			alarmIntent3.PutStringArrayListExtra("Keys", keyList);
			var alarmManager = (AlarmManager)GetSystemService(AlarmService);
			var alarmManager2 = (AlarmManager)GetSystemService(AlarmService);
			var alarmManager3 = (AlarmManager)GetSystemService(AlarmService);

			var pending = PendingIntent.GetBroadcast(this, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);
			var pending2 = PendingIntent.GetBroadcast(this, 1, alarmIntent2, PendingIntentFlags.UpdateCurrent);
			var pending3 = PendingIntent.GetBroadcast(this, 2, alarmIntent3, PendingIntentFlags.UpdateCurrent);
			var cal = Calendar.GetInstance(Locale.Japan);
			//cal.Add(Java.Util.CalendarField.DayOfMonth, 1);a
			cal.Set(Java.Util.CalendarField.Millisecond, 0);
			cal.Set(Java.Util.CalendarField.Second, 0);
			long temp = cal.TimeInMillis;
			cal.Set(CalendarField.Minute, 40);
			cal.Set(CalendarField.HourOfDay, 17);
			if (cal.TimeInMillis < temp)
			{
				//過去の時間を指定してしまった
				cal.Add(Java.Util.CalendarField.DayOfMonth, 1);
				alarmManager3.SetRepeating(AlarmType.RtcWakeup, cal.TimeInMillis, AlarmManager.IntervalDay, pending3);
				cal.Add(Java.Util.CalendarField.DayOfMonth, -1);
			}
			else
			{
				alarmManager3.SetRepeating(AlarmType.RtcWakeup, cal.TimeInMillis, AlarmManager.IntervalDay, pending3);
			}
			cal.Set(CalendarField.Minute, 20);
			cal.Set(CalendarField.HourOfDay, 11);
			if (cal.TimeInMillis < temp)
			{
				//過去の時間を指定してしまった
				cal.Add(Java.Util.CalendarField.DayOfMonth, 1);
				alarmManager2.SetRepeating(AlarmType.RtcWakeup, cal.TimeInMillis, AlarmManager.IntervalDay, pending2);
				cal.Add(Java.Util.CalendarField.DayOfMonth, -1);
			}
			else
			{
				alarmManager2.SetRepeating(AlarmType.RtcWakeup, cal.TimeInMillis, AlarmManager.IntervalDay, pending2);
			}
			cal.Set(Java.Util.CalendarField.Minute, 10);
			cal.Set(Java.Util.CalendarField.HourOfDay, 7);
			if (cal.TimeInMillis < temp)
			{
				cal.Add(Java.Util.CalendarField.DayOfMonth, 1);
				alarmManager.SetRepeating(AlarmType.RtcWakeup, cal.TimeInMillis, AlarmManager.IntervalDay, pending);
				cal.Add(Java.Util.CalendarField.DayOfMonth, -1);
			}
			else
			{
				alarmManager.SetRepeating(AlarmType.RtcWakeup, cal.TimeInMillis, AlarmManager.IntervalDay, pending);
			}
			Toast.MakeText(this, "タイマーセット!!", ToastLength.Long).Show();
		}

		private void PinButton_Click(object sender, System.EventArgs e)
		{
			tokens = CoreTweet.OAuth.GetTokens(session, FindViewById<EditText>(Resource.Id.editText1).Text);
			if (!(tokens == null))
			{
				//認証成功時
				FindViewById<TextView>(Resource.Id.textView1).Text = "認証に成功しました!";
				editor.PutString("Token", tokens.AccessToken);
				editor.PutString("Secret", tokens.AccessTokenSecret);
				editor.PutBoolean("IsEmpty", false);
				editor.Commit();
			}
			else
			{
				FindViewById<TextView>(Resource.Id.textView1).Text = "認証に失敗しました...";
			}
		}

		private void Button_Click(object sender, System.EventArgs e)
		{
			var txtTitle = FindViewById<EditText>(Resource.Id.editText1);
			txtTitle.Focusable = true;
			txtTitle.FocusableInTouchMode = true;
			FindViewById<Button>(Resource.Id.button2).Enabled = true;
			session = OAuth.Authorize(ApiKey, ApiSecret);
			Uri uri = Uri.Parse(session.AuthorizeUri.ToString());
			Intent i = new Intent(Intent.ActionView, uri);
			StartActivity(i);
		}

	}
	enum Meal
	{
		breakfast = 0,
		lunch = 1,
		dinner = 2,
	}
}

