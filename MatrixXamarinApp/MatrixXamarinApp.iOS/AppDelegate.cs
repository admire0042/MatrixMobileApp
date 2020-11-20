﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Foundation;
using Matcha.BackgroundService.iOS;
using UIKit;

namespace MatrixXamarinApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            string fileName = "inbox_db.db3";
            string folderPath =Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "..", "Library");
            string completePath = Path.Combine(folderPath, fileName);

            BackgroundAggregator.Init(this);

            LoadApplication(new App(completePath));

            return base.FinishedLaunching(app, options);
        }
    }
}