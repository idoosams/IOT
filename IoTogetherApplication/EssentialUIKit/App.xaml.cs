﻿using EssentialUIKit.AppLayout;
using EssentialUIKit.Views.Forms;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Xamarin.Essentials;
#if EnableAppCenterAnalytics
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
#endif
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace EssentialUIKit
{
    /// <summary>
    /// The UITemplate Application
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class App
    {
        #region Constructor
        public static ParticipantTableEntity _user { get; set; }

        public static string _groupRowKey { get; set; }

        public static string _groupId { get; set; }

        public static string _groupName { get; set; }

        public static List<ParticipantTableEntity> _activeUsers { get; set; }

        public static Dictionary<string, UserStatsTableEntity> _userStats { get; set; }

        public static string _adminId { get; set; }

        public static List<double> _adminLocation { get; set; }

        //public static UserStatsTableEntity _stats { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="App" /> class.
        /// </summary>
        public App()
        {
#if EnableAppCenterAnalytics
            // AppCenter.Start(
            //    $"ios={AppSettings.IOSSecretCode};android={AppSettings.AndroidSecretCode};uwp={AppSettings.UWPSecretCode}",
            //    typeof(Analytics),
            //    typeof(Crashes));
#endif

            InitializeComponent();

            // this.MainPage = new AppShell();
            AppSettings.Instance.SelectedPrimaryColor = 1;
            this.MainPage = new NavigationPage(new SimpleLoginPage());            
        }

        #endregion

        #region Properties

        public static string BaseImageUrl { get; } = "https://cdn.syncfusion.com/essential-ui-kit-for-xamarin.forms/common/uikitimages/";

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when your app starts
        /// </summary>
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        /// <summary>
        /// Invoked when your app sleeps
        /// </summary>
        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        /// <summary>
        /// Invoked when your app resumes
        /// </summary>
        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        #endregion
    }
}