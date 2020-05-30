using EssentialUIKit.AppLayout.Views;
using EssentialUIKit.ViewModels.Forms;
using EssentialUIKit.Views.Catalog;
using EssentialUIKit.Views.Detail;
using EssentialUIKit.Views.Forms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace EssentialUIKit.ViewModels
{
    /// <summary>
    /// ViewModel for login page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class ChooseActionViewModel : BaseViewModel
    {
        #region Fields

        private string groupName;

        private string groupId;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="ChooseActionViewModel" /> class.
        /// </summary>
        public ChooseActionViewModel()
        {
            this.CreateCommand = new Command(this.CreateClicked);
            this.JoinCommand = new Command(this.JoinClicked);
        }

        #endregion

        #region property
        public string GroupName
        {
            get
            {
                return this.groupName;
            }

            set
            {
                if (this.groupName == value)
                {
                    return;
                }

                this.groupName = value;
                this.NotifyPropertyChanged();
            }
        }

        public string WelcomeText
        {
            get
            {
                return $"Hello, {App._user.FirstName} {App._user.LastName}";
            }
        }

        public string GroupId
        {
            get
            {
                return this.groupId;
            }

            set
            {
                if (this.groupId == value)
                {
                    return;
                }

                this.groupId = value;
                this.NotifyPropertyChanged();
            }
        }
        #endregion

        #region Command

        public Command CreateCommand { get; set; }

        public Command JoinCommand { get; set; }

        #endregion

        #region methods

        /// <summary>
        /// Invoked when the Log In button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void CreateClicked(object obj)
        {
            string guid = Guid.NewGuid().ToString("N");
            App._groupId = GenerateGroupId();
            var sessionParticipant = new SessionParticipant(guid, App._groupId, this.groupName, true, App._user.RowKey);
            AzureDbClient.AddParticipantToGroup(sessionParticipant).ConfigureAwait(false);
            Application.Current.MainPage.Navigation.PushAsync(new DataTablePage(), true);
        }

        /// <summary>
        /// Invoked when the Sign Up button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void JoinClicked(object obj)
        {
            string guid = Guid.NewGuid().ToString("N");
            App._groupId = this.groupId;
            this.groupName = AzureDbClient.GroupIdToName(App._groupId);
            if (this.groupName == null)
            {
                Application.Current.MainPage.DisplayAlert("Oops", $"We couldn't find group id {this.groupId}", "Ok");
            }
            else
            {
                var sessionParticipant = new SessionParticipant(guid, App._groupId, this.groupName, false, App._user.Id);
                AzureDbClient.AddParticipantToGroup(sessionParticipant).ConfigureAwait(false);
                App._activeUsers = AzureDbClient.GetGroupParticipants(App._groupId);

                Application.Current.MainPage.Navigation.PushAsync(new DataTablePage(), true);
            }
        }

        private string GenerateGroupId()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new String(stringChars);
        }

        #endregion
    }
}