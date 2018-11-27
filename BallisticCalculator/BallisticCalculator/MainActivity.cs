using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Text;
using Android.Content;

namespace BallisticCalculator
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, LaunchMode = Android.Content.PM.LaunchMode.SingleInstance)]
    public class MainActivity : AppCompatActivity
    {

        const string SAVE_HOLD_KEY = "holdValue";
        const string SAVE_FLIGHT_TIME_KEY = "flightTimeValue";

        EditText speedField;
        EditText xField;
        EditText yField;
        EditText zeroField;

        ProgressBar progressBar;
        TextView holdLabel;
        TextView timeLabel;

        Button dataButton;

        float speed;
        float x;
        float y;
        float zero;

        float angle;




        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
           
            // set up UI
            SetContentView(Resource.Layout.activity_main);

            // get UI controls
            speedField = FindViewById<EditText>(Resource.Id.speedField);
            xField = FindViewById<EditText>(Resource.Id.distanceField);
            yField = FindViewById<EditText>(Resource.Id.elevationField);
            zeroField = FindViewById<EditText>(Resource.Id.zeroField);

            progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);
            holdLabel = FindViewById<TextView>(Resource.Id.holdLabel);
            timeLabel = FindViewById<TextView>(Resource.Id.flightLabel);

            dataButton = FindViewById<Button>(Resource.Id.dataButton);

            // load saved bundle
            if (savedInstanceState != null)
            {
                SetUp(savedInstanceState);
            }

            // set up events
            speedField.TextChanged += NewData;
            xField.TextChanged += NewData;
            yField.TextChanged += NewData;
            zeroField.TextChanged += NewData;

            dataButton.Click += GoToDataListView;
        }



        void NewData(object sender, TextChangedEventArgs e)
        {
            // get input
            bool tp1 = float.TryParse(speedField.Text, out speed);
            bool tp2 = float.TryParse(xField.Text, out x);
            bool tp3 = float.TryParse(yField.Text, out y);
            bool tp4 = float.TryParse(zeroField.Text, out zero);

            // put in array
            bool[] dataEntry = {tp1, tp2, tp3, tp4 };

            // calculate progress
            progressBar.SetProgress(0, false);

            foreach (bool b in dataEntry)
            {
                if (b)
                {
                    // update progress bar
                    progressBar.IncrementProgressBy(25);
                }
            }

            // try to calculate
            if (progressBar.Progress == 100)
            {
                // get launch angle
                angle = BallisticTools.GetLaunchAngle(speed, x, y);
                // get hold
                double hold = BallisticTools.GetHold(angle, speed, x, zero);
                // display hold
                holdLabel.Text = string.Format("Hold {0} (m) Over", hold.ToString("n2"));

                // get flight time
                float flightTime = BallisticTools.GetFlightTime(angle, speed, x);
                // display flight time
                timeLabel.Text = string.Format("{0} (s) Flight Time", flightTime.ToString("n2"));
            }
            
        }



        void GoToDataListView(object sender, System.EventArgs e)
        {
            // check for input
            if (progressBar.Progress != 100)
            {
                // not all input exists, abort
                Toast.MakeText(this, "Not All Data Has Been Entered", ToastLength.Short).Show();
                return;
            }

            // new intent
            var intent = new Intent(this, typeof(ListDisplayActivity));

            // add extras
            intent.PutExtra(ListDisplayActivity.SPEED_KEY, speed);
            intent.PutExtra(ListDisplayActivity.DISTANCE_KEY, x);
            intent.PutExtra(ListDisplayActivity.ANGLE_KEY, angle);

            // next activity
            StartActivity(intent);
        }



        protected override void OnSaveInstanceState(Bundle outState)
        {
            // save info
            outState.PutString(SAVE_HOLD_KEY, holdLabel.Text);
            outState.PutString(SAVE_FLIGHT_TIME_KEY, timeLabel.Text);


            // give bundle
            base.OnSaveInstanceState(outState);
        }



        void SetUp(Bundle inState)
        {
            // assign saved text to labels
            holdLabel.Text = inState.GetString(SAVE_HOLD_KEY, "");
            timeLabel.Text = inState.GetString(SAVE_FLIGHT_TIME_KEY, "");
        }

    }
}