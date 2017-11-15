using Android.App;
using Android.Widget;
using Android.OS;
//using CoreTweet;
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
				tokens = CoreTweet.Tokens.Create(ApiKey, ApiSecret, pref.GetString("Token", ""), pref.GetString("Secret", ""));
				FindViewById<TextView>(Resource.Id.textView1).Text = "認証に成功しました!";
				timelineButton.Enabled = true;
			}

		}
		private void FireTimerButton_Click(object sender, System.EventArgs e)
		{
			var keyList = new System.Collections.Generic.List<string>(4)
			{
				ApiKey,
				ApiSecret,
				tokens.AccessToken,
				tokens.AccessTokenSecret
			};
			var alarmIntent = new Intent[3];
			for (int i = 0; i < alarmIntent.Length; i++)
			{
				alarmIntent[i] = new Intent(this, typeof(MyBroadcastReceiver)); //アラームレシーバはブロードキャストを継承
				alarmIntent[i].PutStringArrayListExtra("Keys", keyList);
				alarmIntent[i].PutExtra("Meal", i);
			}

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

				/* cal[i].Set(Java.Util.CalendarField.Year, cal[i].Get(Java.Util.CalendarField.Year));
				 * cal[i].Set(Java.Util.CalendarField.Month, cal[i].Get(Java.Util.CalendarField.Month)); 
				 * cal[i].Set(Java.Util.CalendarField.DayOfMonth, cal[i].Get(Java.Util.CalendarField.DayOfMonth)); 
				 * cal[i].Set(Java.Util.CalendarField.HourOfDay, cal[i].Get(Java.Util.CalendarField.HourOfDay)); 
				 */ 
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
				alarmManager.SetRepeating(AlarmType.RtcWakeup, cal[i].TimeInMillis,AlarmManager.IntervalDay, pending[i]);
			}
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
			session = CoreTweet.OAuth.Authorize(ApiKey, ApiSecret);
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

