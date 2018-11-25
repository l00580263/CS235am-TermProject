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
    public static class BallisticTools
    {





        public static float GetLaunchAngle(float speed, float x, float y)
        {
            // define gravity
            float g = 9.81f;

            // get angle in radians
            float angle = (float)Math.Atan((Math.Pow(speed, 2) - Math.Sqrt(Math.Pow(speed, 4) - g * (g * Math.Pow(x, 2) + 2 * y * Math.Pow(speed, 2)))) / (g * x));

            return angle;
        }



        public static double GetHold(float angle, float speed, float x, float zero)
        {
            // get angle from sights to barrel
            float sightsAngle = GetLaunchAngle(speed, zero, 0);

            // get real angle
            float realAngle = angle - sightsAngle;

            // get hold over
            return Math.Tan(realAngle) * x;
        }



        public static float GetFlightTime(float angle, float speed, float x)
        {
            // angle is in radians

            // distance 
            float xSpeed = (speed * (float)Math.Cos(angle));
            // get time
            return x / xSpeed;
        }
    }
}