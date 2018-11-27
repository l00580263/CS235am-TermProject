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

        public const float GRAVITY = 9.81f;



        public static float GetLaunchAngle(float speed, float x, float y)
        {
            // angle to hit self should return zero
            if (x <= 0 && y <= 0)
            {
                return 0;
            }

            // get angle in radians
            float angle = (float)Math.Atan((Math.Pow(speed, 2) - Math.Sqrt(Math.Pow(speed, 4) - GRAVITY * (GRAVITY * Math.Pow(x, 2) + 2 * y * Math.Pow(speed, 2)))) / (GRAVITY * x));

            return angle;
        }



        public static double GetHold(float angle, float speed, float x, float y, float zero)
        {
            // get angle from sights to barrel
            float sightsAngle = GetLaunchAngle(speed, zero, 0);

            // get real angle
            float realAngle = angle - sightsAngle;

            // get height from 0
            var h = Math.Tan(realAngle) * x;

            // get direction from target to hold
            var d = h - y;

            // get hold over
            return d;
        }



        public static float GetFlightTime(float angle, float speed, float x)
        {
            // distance 
            float xSpeed = speed * (float)Math.Cos(angle);
            // get time
            return x / xSpeed;
        }



        public static float GetElevationAtDistance(float speed, float x, float angle)
        {
            // get time in terms of distance x
            double t = x / (speed * Math.Cos(angle));

            // calculate elevation
            return (float) (-GRAVITY * .5f * Math.Pow(t, 2) + speed * Math.Sin(angle) * t);
        }
    }
}