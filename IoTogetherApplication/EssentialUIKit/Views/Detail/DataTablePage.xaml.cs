using EssentialUIKit.Controls;
using EssentialUIKit.Models.Detail;
using EssentialUIKit.ViewModels.Detail;
using GeoCoordinatePortable;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace EssentialUIKit.Views.Detail
{
    /// <summary>
    /// Page showing the data table
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DataTablePage : ContentPage
    {
        private DataTableViewModel dataTableViewModel;

        private bool continueRefresh = true;

        public DataTablePage()
        {
            InitializeComponent();
        }

        public DataTablePage(DataTableViewModel dataTableViewModel) : this()
        {
            this.dataTableViewModel = dataTableViewModel;
            this.BindingContext = this.dataTableViewModel;

            Device.StartTimer(TimeSpan.FromSeconds(3), () =>
            {
                if (continueRefresh)
                {
                    Device.BeginInvokeOnMainThread(() => RefreshView());
                }
                return true;
            });
        }   

        private string[] CreateBatteryColorView(double batteryPercentage)
        {
            if (batteryPercentage <= 15)
                return new string[5] { "#ff4a4a", "#b2b8c2", "#b2b8c2", "#b2b8c2", "#b2b8c2" };
            else if (batteryPercentage <= 30)
                return new string[5] { "#ff4a4a", "#ff4a4a", "#b2b8c2", "#b2b8c2", "#b2b8c2" };
            else if (batteryPercentage <= 50)
                return new string[5] { "#7ed321", "#7ed321", "#7ed321", "#b2b8c2", "#b2b8c2" };
            else if (batteryPercentage <= 80)
                return new string[5] { "#7ed321", "#7ed321", "#7ed321", "#7ed321", "#b2b8c2" };
            else
                return new string[5] { "#7ed321", "#7ed321", "#7ed321", "#7ed321", "#7ed321" };
        }

        private void RefreshView()
        {
            var participantsFromTable = App._activeUsers;
            var tmp = new List<DataTable>();

            UserStatsTableEntity statEntity;

            foreach (var participant in participantsFromTable)
            {
                if (App._userStats != null && App._userStats.TryGetValue(participant?.RowKey, out statEntity))
                {

                    var adminLocation = new GeoCoordinate(App._adminLocation[0], App._adminLocation[1]);
                    var userLocation = new GeoCoordinate(statEntity.Latitude, statEntity.Longtitude);
                    var distanceFromAdmin = (int)adminLocation.GetDistanceTo(userLocation);
                    var chargeLevel = statEntity.BaterryCharge;

                    tmp.Add(new DataTable
                    {
                        Name = $"{participant.FirstName} {participant.LastName}",
                        Phone = participant.Phone,
                        BatteryPercentageDiagram = CreateBatteryColorView(chargeLevel * 100),
                        Distance = distanceFromAdmin,
                    });

                    /*if (chargeLevel <= 0.15)
                    {
                        this.continueRefresh = false;
                        var ack = await Application.Current.MainPage.DisplayAlert("Alert!", $"Seems like {participant.FirstName} {participant.LastName} is loosing connection!", "Call", "Dismiss");
                        if (ack)
                        {
                            await Task.Delay(5000);
                            this.continueRefresh = true;
                        } else
                        {
                            await Task.Delay(5000);
                            this.continueRefresh = true;
                        }                 
                    }

                    if (distanceFromAdmin >= 5000)
                    {
                        this.continueRefresh = false;
                        var ack = await Application.Current.MainPage.DisplayAlert("Alert!", $"Seems like {participant.FirstName} {participant.LastName} is getting far away!", "Call", "Dismiss");
                        if (ack)
                        {
                            await Task.Delay(5000);
                            this.continueRefresh = true;
                        }
                        else
                        {
                            await Task.Delay(5000);
                            this.continueRefresh = true;
                        }
                    }*/

                }
            }
            this.dataTableViewModel.Items = tmp;
        }

        /// <summary>
        /// Invoked when view size is changed.
        /// </summary>x
        /// <param name="width">The Width</param>
        /// <param name="height">The Height</param>
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (width > height)
            {
                if (Search.IsVisible)
                {
                    Search.WidthRequest = width;
                }
            }
        }

        /// <summary>
        /// Invoked when search button is clicked.
        /// </summary>
        /// <param name="sender">The Sender</param>
        /// <param name="e">Event Args</param>
        private void SearchButton_Clicked(object sender, EventArgs e)
        {
            this.SearchButton.IsVisible = false;
            this.Search.IsVisible = true;
            this.Title.IsVisible = false;

            if (this.TitleView != null)
            {
                double opacity;

                // Animating Width of the search box, from 0 to full width when it added to the view.
                var expandAnimation = new Animation(
                    property =>
                    {
                        Search.WidthRequest = property;
                        opacity = property / TitleView.Width;
                        Search.Opacity = opacity;
                    }, 0, TitleView.Width, Easing.Linear);
                expandAnimation.Commit(Search, "Expand", 16, 250, Easing.Linear);
            }

            SearchEntry.Focus();
        }

        /// <summary>
        /// Invoked when back to title button is clicked.
        /// </summary>
        /// <param name="sender">The Sender</param>
        /// <param name="e">Event Args</param>
        private void BackToTitle_Clicked(object sender, EventArgs e)
        {
            this.SearchButton.IsVisible = true;
            if (this.TitleView != null)
            {
                double opacity;

                // Animating Width of the search box, from full width to 0 before it removed from view.
                var shrinkAnimation = new Animation(property =>
                {
                    Search.WidthRequest = property;
                    opacity = property / TitleView.Width;
                    Search.Opacity = opacity;
                },
                TitleView.Width, 0, Easing.Linear);
                shrinkAnimation.Commit(Search, "Shrink", 16, 250, Easing.Linear, (p, q) => this.SearchBoxAnimationCompleted());
            }

            SearchEntry.Text = string.Empty;
        }
        

        /// <summary>
        /// Invokes when search box Animation completed.
        /// </summary>
        private void SearchBoxAnimationCompleted()
        {
            this.Search.IsVisible = false;
            this.Title.IsVisible = true;
        }        
    }
}