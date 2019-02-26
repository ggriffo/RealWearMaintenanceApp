using System;
using System.Collections.Generic;
using Java.IO;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using Android.Webkit;

namespace OlinTube
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class EquipmentSupportActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EquipmentSupport);
            // Create your application here

            Button button = FindViewById<Button>(Resource.Id.openPdfInstructionsButton);
            button.Click += OpenPDF;

            button = FindViewById<Button>(Resource.Id.openVideoInstructionsButton);
            button.Click += OpenVideo;

            button = FindViewById<Button>(Resource.Id.realTimeData);
            button.Click += RealTimeData;

            //ImageView imageView = (ImageView)FindViewById(Resource.Id.dashboardView);

            //DisplayMetrics displaymetrics = new DisplayMetrics();
            //WindowManager.DefaultDisplay.GetMetrics(displaymetrics);

            //imageView.LayoutParameters.Height = displaymetrics.HeightPixels;
            //imageView.LayoutParameters.Width = displaymetrics.WidthPixels;

            string barcode = Intent.GetStringExtra("barcode");
            TextView equipmentIDText = FindViewById<TextView>(Resource.Id.equipmentIDText);
            equipmentIDText.Text = barcode;
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {

        }

        private void OpenPDF(object sender, EventArgs eventArgs)
        {
            string path = Android.OS.Environment.ExternalStorageDirectory.Path + "/Documents/" + "pump.pdf";
            File file = new Java.IO.File(path);

            string mimeType = MimeTypeMap.Singleton.GetMimeTypeFromExtension(MimeTypeMap.GetFileExtensionFromUrl(file.ToURI().ToString()));

            if (mimeType != null)
            {
                Intent intent = new Intent(Intent.ActionView);
                intent.AddCategory(Intent.CategoryDefault);
                intent.SetDataAndType(Android.Net.Uri.FromFile(file), mimeType);
                intent.AddFlags(ActivityFlags.NewTask);

                intent.PutExtra("page", "3");
                intent.PutExtra("zoom", "4");
                intent.PutExtra("x", "336");
                intent.PutExtra("y", "176");

                StartActivity(intent);
            }
        }

        private void OpenVideo(object sender, EventArgs eventArgs)
        {
            string path = Android.OS.Environment.ExternalStorageDirectory.Path + "/Movies/" + "movie.mp4";
            File movie = new File(path);

            string mimeType = MimeTypeMap.Singleton.GetMimeTypeFromExtension(
              MimeTypeMap.GetFileExtensionFromUrl(movie.ToURI().ToString()));

            if (mimeType != null)
            {
                Intent intent = new Intent(Intent.ActionView);
                intent.AddCategory(Intent.CategoryDefault);
                intent.SetDataAndType(Android.Net.Uri.FromFile(movie), mimeType);
                intent.AddFlags(ActivityFlags.NewTask);

                StartActivity(intent);
            }   
        }

        private void RealTimeData(object sender, EventArgs eventArgs)
        {
            Intent i = new Intent(ApplicationContext, typeof(EquipmentRealTimeData));
            string barcode = Intent.GetStringExtra("barcode");
            i.PutExtra("barcode", barcode);

            StartActivity(i);
        }
    }
}