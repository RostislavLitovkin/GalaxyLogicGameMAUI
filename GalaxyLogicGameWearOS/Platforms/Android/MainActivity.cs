﻿using Android.App;
using Android.Content.PM;
using Android.OS;

namespace GalaxyLogicGameWearOS
{
    [Activity(Theme = "@style/GalaxyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
    }


}