using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Provider;
using Android.Support.V7.App;
using Android.Widget;

using Java.IO;

using Environment = Android.OS.Environment;
using Uri = Android.Net.Uri;

namespace OlinTube
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        private File _dir;
        private File _file;
        private string imageFileName;

        private const int CAMERA_REQUEST_IMAGE = 1889;
        private const int CAMERA_REQUEST_VIDEO = 1888;
        private const int BARCODE_REQUEST = 1984;
        

        private const int ZOOM_ACTION_VIEW = -1;

        private const string EXTRA_RESULT = "com.realwear.barcodereader.intent.extra.RESULT";
        private const string ACTION_BARCODE = "com.realwear.barcodereader.intent.action.SCAN_BARCODE";

        private const string ZOOM_URL = "https://zoom.us/j/3928541242";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            if (IsThereAnAppToTakePictures())
            {
                Button button = FindViewById<Button>(Resource.Id.openCameraButton);
                button.Click += TakeAPicture;

                button = FindViewById<Button>(Resource.Id.inspectEquipmentButton);
                button.Click += BarcodeReader;

                button = FindViewById<Button>(Resource.Id.auditEquipmentButton);
                button.Click += AuditEquipment;

                button = FindViewById<Button>(Resource.Id.remoteHelpButton);
                button.Click += RemoteHelp;

                //_imageView = FindViewById<ImageView>(Resource.Id.imageView1);
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == CAMERA_REQUEST_IMAGE)
            {
                OnActivityResultPicture(resultCode, data);
            }
            else if (requestCode == CAMERA_REQUEST_VIDEO)
            {
                OnActivityResultVideo(resultCode, data);
            }
            else if (requestCode == BARCODE_REQUEST)
            {
                OnActivityBarcode(resultCode, data);
            }
        }

        private void OnActivityBarcode(Result resultCode, Intent data)
        {
            if (resultCode == Result.Ok)
            {
                String result = "[No Barcode]";
                if (data != null)
                {
                    result = data.GetStringExtra(EXTRA_RESULT);

                    Intent i = new Intent(ApplicationContext, typeof(EquipmentSupportActivity));
                    i.PutExtra("barcode", result);

                    StartActivity(i);
                }
            }
        }

        private void OnActivityResultVideo(Result resultCode, Intent data)
        {

        }

        private void OnActivityResultPicture(Result resultCode, Intent data)
        {

        }

        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities = PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        public File GetAlbumDir()
        {
            //string path = Android.OS.Environment.ExternalStorageDirectory.Path + "/Pictures/" + "pump.pdf";

            File storageDir = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures),"OlinTube/");
            // Create directories if needed
            if (!storageDir.Exists())
            {
                storageDir.Mkdirs();
            }

            return storageDir;
        }

        private File CreateImageFile()
        {
            imageFileName = GetAlbumDir().ToString() + Guid.NewGuid().ToString() + ".jpg";
            File image = new File(imageFileName);
            return image;
        }

        private void TakeAPicture(object sender, EventArgs eventArgs)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            _file = CreateImageFile();

            intent.PutExtra(Android.Provider.MediaStore.ExtraOutput, Uri.FromFile(_file));
            StartActivityForResult(intent, CAMERA_REQUEST_IMAGE);
        }

        private void AuditEquipment(object sender, EventArgs eventArgs)
        {
            Intent intent = new Intent(MediaStore.ActionVideoCapture);
            StartActivityForResult(intent, CAMERA_REQUEST_VIDEO);
        }

        private void BarcodeReader(object sender, EventArgs eventArgs)
        {
            Intent intent = new Intent(ACTION_BARCODE);
            StartActivityForResult(intent, BARCODE_REQUEST);
        }

        private void RemoteHelp(object sender, EventArgs eventArgs)
        {
            Intent myIntent = new Intent(Intent.ActionView, Uri.Parse(ZOOM_URL));
            StartActivityForResult(myIntent, ZOOM_ACTION_VIEW);
        }
    }
}

