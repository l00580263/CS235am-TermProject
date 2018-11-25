using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Text;

namespace BallisticCalculator
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        EditText speedField;
        EditText xField;
        EditText yField;
        EditText zeroField;

        ProgressBar progressBar;
        TextView holdLabel;
        TextView timeLabel;

        Button dataButton;



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

            // set up events
            speedField.TextChanged += NewData;
            xField.TextChanged += NewData;
            yField.TextChanged += NewData;
            zeroField.TextChanged += NewData;
        }



        void NewData(object sender, TextChangedEventArgs e)
        {
            float speed;
            bool tp1 = float.TryParse(speedField.Text, out speed);
            float x;
            bool tp2 = float.TryParse(xField.Text, out x);
            float y;
            bool tp3 = float.TryParse(yField.Text, out y);
            float zero;
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
                float angle = BallisticTools.GetLaunchAngle(speed, x, y);
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
    }
}