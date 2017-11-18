using Android.App;
using Android.Widget;
using Android.OS;
using CoreTweet;
using Android.Net;
using Android.Content;
using Android.Views;

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
			var timelineButton = FindViewById<Button>(Resource.Id.timeLimeUpdate);
			timelineButton.Click += TimelineButton_Click;
			var txtTitle = FindViewById<EditText>(Resource.Id.editText1);

			timelineButton.Enabled = false;
			pinButton.Enabled = false;

			var FireTimerButton = FindViewById<Button>(Resource.Id.TimerFireButtou);
			FireTimerButton.Click += FireTimerButton_Click;

			editor = GetPreferences(FileCreationMode.Private).Edit();
			pref = GetPreferences(FileCreationMode.Private);
			if (pref.GetBoolean("IsEmpty", true) == false)
			{
				tokens = Tokens.Create(ApiKey, ApiSecret, pref.GetString("Token", ""), pref.GetString("Secret", ""));
				FindViewById<TextView>(Resource.Id.textView1).Text = "認証に成功しました!";
				timelineButton.Enabled = true;
			}
			this.Window.AddFlags(WindowManagerFlags.TurnScreenOn);
		}

		private void FireTimerButton_Click(object sender, System.EventArgs e)
		{
			var alarmIntent = new Intent(this, typeof(MyBroadcastReceiver)); //アラームレシーバはブロードキャストを継承
			var alarmIntent2 = new Intent(this, typeof(MyBroadcastReceiver));
			////alarmIntent.PutExtra("title", cnt.ToString() + "回目の通知です");      //タイトルって名前でキーを指定する
																						 ////alarmIntent.PutExtra("message", "World!");
			var keyList = new System.Collections.Generic.List<string>(4);
			keyList.Add(ApiKey);
			keyList.Add(ApiSecret);
			keyList.Add(tokens.AccessToken);
			keyList.Add(tokens.AccessTokenSecret);

			alarmIntent.PutStringArrayListExtra("Keys", keyList);
			alarmIntent2.PutStringArrayListExtra("Keys", keyList);
			var alarmManager = (AlarmManager)GetSystemService(AlarmService);
			var alarmManager2 = (AlarmManager)GetSystemService(AlarmService);
			Java.Util.Calendar cal = Java.Util.Calendar.GetInstance(Java.Util.TimeZone.Default);

			//cal.Set(Java.Util.CalendarField.Year,		cal.Get(Java.Util.CalendarField.Year));
			//cal.Set(Java.Util.CalendarField.Month,		cal.Get(Java.Util.CalendarField.Month));
			//cal.Set(Java.Util.CalendarField.DayOfMonth,	cal.Get(Java.Util.CalendarField.DayOfMonth));
			//cal.Set(Java.Util.CalendarField.HourOfDay,	cal.Get(Java.Util.CalendarField.HourOfDay));
			//cal.Set(Java.Util.CalendarField.Minute,		cal.Get(Java.Util.CalendarField.Minute));
			//cal.Set(Java.Util.CalendarField.Second,		cal.Get(Java.Util.CalendarField.Second));
			alarmIntent.PutExtra("num", "This is 1st");
			var pending = PendingIntent.GetBroadcast(this, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);

			alarmIntent2.PutExtra("num", "this is 2th event!");
			var pending2 = PendingIntent.GetBroadcast(this, 1, alarmIntent2, PendingIntentFlags.UpdateCurrent);


			//cal.Set(Java.Util.CalendarField.Second, 35);
			//alarmManager.SetRepeating(AlarmType.RtcWakeup, cal.TimeInMillis, AlarmManager.IntervalHalfHour / 30, pending);

			var time = new Android.Text.Format.Time("Asia/Tokyo");
			time.SetToNow();
			string date = time.Year + "-" + (time.Month + 1) + "-" + time.MonthDay + "-"+time.Hour + "-" + time.Minute + "-" + time.Second;

			Android.Util.Log.Debug("hogehoge",date+"\tLaunch time ");
			cal.Add(Java.Util.CalendarField.Minute, 1);
			alarmManager.SetRepeating(AlarmType.RtcWakeup, cal.TimeInMillis, AlarmManager.IntervalHour / 12, pending);

			cal.Add(Java.Util.CalendarField.Minute, 2);
			alarmManager2.SetRepeating(AlarmType.RtcWakeup, cal.TimeInMillis, AlarmManager.IntervalHour / 12, pending2);

		}

		private void TimelineButton_Click(object sender, System.EventArgs e)
		{
			var TimeLine = FindViewById<TextView>(Resource.Id.TimeLine);
			TimeLine.Text = "";
			foreach (var item in tokens.Statuses.HomeTimeline(count => 10))
			{
				TimeLine.Text += "-----\n";
				TimeLine.Text += item.User.Name + "  (@" + item.User.ScreenName + ")" + "\n";
				TimeLine.Text += item.Text + "\n";
				TimeLine.Text += "RT : " + item.RetweetCount + "   Fav :" + item.FavoriteCount + "\n";
			}
		}

		private void PinButton_Click(object sender, System.EventArgs e)
		{
			tokens = CoreTweet.OAuth.GetTokens(session, FindViewById<EditText>(Resource.Id.editText1).Text);
			if (!(tokens == null))
			{
				//認証成功時
				FindViewById<TextView>(Resource.Id.textView1).Text = "認証に成功しました!";
				FindViewById<Button>(Resource.Id.timeLimeUpdate).Enabled = true;
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
		public void saveToFile(Android.Graphics.Bitmap bitmap,string imgname)
		{
			//保存先のパスとか      
			ContextWrapper cw = new ContextWrapper(this.ApplicationContext);
			Java.IO.File file = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);
			Java.IO.File myfile = new Java.IO.File(file, imgname);

			//保存
			using (var os = new System.IO.FileStream(myfile.AbsolutePath, System.IO.FileMode.Create))
			{
				bitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Png, 100, os);
			}
		}

	}
	enum Meal
	{
		breakfast = 0,
		lunch = 1,
		dinner = 2,
	}
}

