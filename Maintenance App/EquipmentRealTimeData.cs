using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Java.Util;

namespace OlinTube
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class EquipmentRealTimeData : Activity
    {
        private TextView temperatureValue;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EquipmentRealTimeData);
            SetupComponents();

            // Create your application here
            SetupTemperatureRandomValue();

            StartTimer(TimeSpan.FromSeconds(3), () =>
            {
                SetupTemperatureRandomValue();
                return true; // True = Repeat again, False = Stop the timer
            });
        }

        private void SetupTemperatureRandomValue()
        {
            double temperatureDouble = new System.Random().NextDouble();
            int temperatureInt = new System.Random().Next(80, 82);

            double temperature = temperatureDouble + temperatureInt;
            string temperatureString = temperatureValue.Text;

            temperatureValue.Text = "F° " + temperature.ToString("##.##");
        }

        private void SetupComponents()
        {
            temperatureValue = FindViewById<TextView>(Resource.Id.temperatureValue);
        }

        public void StartTimer(TimeSpan interval, Func<bool> callback)
        {
            var handler = new Handler(Looper.MainLooper);
            handler.PostDelayed(() =>
            {
                if (callback())
                    StartTimer(interval, callback);

                handler.Dispose();
                handler = null;
            }, (long)interval.TotalMilliseconds);
        }
    }
}