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
	class MyCalendar
	{
		public int year;
		public int month;
		public int dayofweek;
		public int day;
		public int hour;
		public int minute;
		public int second;
		public long ms;
		Calendar calendar;
		public MyCalendar()
		{
			calendar = Calendar.GetInstance(Locale.Japan);

			year = calendar.Get(CalendarField.Year);
			month = calendar.Get(CalendarField.Month);
			dayofweek = calendar.Get(CalendarField.WeekOfMonth);
			day = calendar.Get(CalendarField.DayOfMonth);
			hour = calendar.Get(CalendarField.HourOfDay);
			minute = calendar.Get(CalendarField.Minute);
			second = calendar.Get(CalendarField.Second);
			ms = calendar.Get(CalendarField.Millisecond);
		}
		public void Refresh()
		{
			year = calendar.Get(CalendarField.Year);
			month = calendar.Get(CalendarField.Month);
			dayofweek = calendar.Get(CalendarField.DayOfWeek);
			day = calendar.Get(CalendarField.DayOfMonth);
			hour = calendar.Get(CalendarField.HourOfDay);
			minute = calendar.Get(CalendarField.Minute);
			second = calendar.Get(CalendarField.Second);
			ms = calendar.Get(CalendarField.Millisecond);
		}
		public void Add(CalendarField field, int ad)
		{
			calendar.Add(field, ad);
		}
		public void Set(CalendarField field, int ad)
		{
			calendar.Set(field, ad);
		}
		public long TimeInMillis => calendar.TimeInMillis;
		//public override void Add([GeneratedEnum] CalendarField field, int amount)
		//{
		//	base.Add(field, amount);
		//}
		//public override int GetMinimum([GeneratedEnum] CalendarField field)
		//{
		//	return GetMinimum(field);
		//}

		//protected override void ComputeFields()
		//{
		//	ComputeFields();
		//}

		//protected override void ComputeTime()
		//{
		//	ComputeTime();
		//}

		//public override int GetGreatestMinimum([GeneratedEnum] CalendarField field)
		//{
		//	return GetGreatestMinimum(field);
		//}

		//public override int GetLeastMaximum([GeneratedEnum] CalendarField field)
		//{
		//	return GetLeastMaximum(field);
		//}

		//public override int GetMaximum([GeneratedEnum] CalendarField field)
		//{
		//	return GetMaximum(field);
		//}

		//public override void Roll([GeneratedEnum] CalendarField field, bool up)
		//{
		//	Roll(field, up);
		//}

	}
}