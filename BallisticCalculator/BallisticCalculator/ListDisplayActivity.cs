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

namespace BallisticCalculator
{
    [Activity(Label = "ListActivity")]
    public class ListDisplayActivity : Activity
    {

        public const string SPEED_KEY = "speed";
        public const string DISTANCE_KEY = "distance";
        public const string ANGLE_KEY = "angle";

        public const float DISTANCE_PERCENT = .05f;



        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // setup ui
            SetContentView(Resource.Layout.activity_list);

            // get list view
            var view = FindViewById<ListView>(Resource.Id.listView);

            // get data from intent
            float speed = Intent.GetFloatExtra(SPEED_KEY, 0);
            float distance = Intent.GetFloatExtra(DISTANCE_KEY, 0);
            float angle = Intent.GetFloatExtra(ANGLE_KEY, 0);

            // create list of data
            List<string> data = new List<string>();

            // get length that is a percentage of distance
            float increment = distance * DISTANCE_PERCENT;
            float x = 0;
            while (x <= distance)
            {
                // get elevation
                float y = BallisticTools.GetElevationAtDistance(speed, x, angle);
                // create string
                var entry = string.Format("( {0},  {1} )", x.ToString("n0"), y.ToString("n2"));
                // add to list
                data.Add(entry);

                // end
                if (x >= distance)
                {
                    break;
                }

                // next distance mark
                x += increment;

                // cap distance
                if (x > distance)
                {
                    x = distance;
                }
            }



            // plug in data to list view
            view.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, data);
        }
    }
}