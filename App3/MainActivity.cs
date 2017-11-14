﻿using Android.App;
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
			cnt = 0;
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

		}

		private void FireTimerButton_Click(object sender, System.EventArgs e)
		{
			var alarmIntent = new Intent(this, typeof(MyBroadcastReceiver)); //アラームレシーバはブロードキャストを継承
											 ////alarmIntent.PutExtra("title", cnt.ToString() + "回目の通知です");      //タイトルって名前でキーを指定する
											 ////alarmIntent.PutExtra("message", "World!");
			var keyList = new System.Collections.Generic.List<string>(4);
			keyList.Add(ApiKey);
			keyList.Add(ApiSecret);
			keyList.Add(tokens.AccessToken);
			keyList.Add(tokens.AccessTokenSecret);

			alarmIntent.PutStringArrayListExtra("Keys", keyList);
			var pending = PendingIntent.GetBroadcast(this, , alarmIntent, PendingIntentFlags.UpdateCurrent);

			var alarmManager = (AlarmManager)GetSystemService(AlarmService);
			Java.Util.Calendar cal = Java.Util.Calendar.GetInstance(Java.Util.TimeZone.Default);
 
			cal.Set(Java.Util.CalendarField.Year,		cal.Get(Java.Util.CalendarField.Year));
			cal.Set(Java.Util.CalendarField.Month,		cal.Get(Java.Util.CalendarField.Month));
			cal.Set(Java.Util.CalendarField.DayOfMonth,	cal.Get(Java.Util.CalendarField.DayOfMonth));
			cal.Set(Java.Util.CalendarField.HourOfDay,	cal.Get(Java.Util.CalendarField.HourOfDay));
			//cal.Set(Java.Util.CalendarField.Minute,		cal.Get(Java.Util.CalendarField.Minute));
			//cal.Set(Java.Util.CalendarField.Second,		cal.Get(Java.Util.CalendarField.Second));
			cal.Set(Java.Util.CalendarField.Minute, 21);
			cal.Set(Java.Util.CalendarField.Second,0);
			cal.Set(Java.Util.CalendarField.Millisecond, 0);


			alarmManager.Set(AlarmType.RtcWakeup, cal.TimeInMillis, pending);
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
	}
	enum Meal
	{
		breakfast = 0,
		lunch = 1,
		dinner = 2,
	}
}
