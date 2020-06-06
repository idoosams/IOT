using EssentialUIKit.Views.Forms;
using System;
using System.Security.Cryptography;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace EssentialUIKit.ViewModels.Forms
{
    /// <summary>
    /// ViewModel for sign-up page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class SignUpPageViewModel : LoginViewModel
    {
        #region Fields

        private string firstName;        

        private string lastName;

        private string phone;

        private string emergencyName;

        private string emergencyPhone;

        private string password;

        private string confirmPassword;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="SignUpPageViewModel" /> class.
        /// </summary>
        public SignUpPageViewModel()
        {
            this.LoginCommand = new Command(this.LoginClicked);
            this.SignUpCommand = new Command(this.SignUpClicked);
        }

        #endregion

        #region Property

        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the name from user in the Sign Up page.
        /// </summary>
        public string FirstName
        {
            get
            {
                return this.firstName;
            }

            set
            {
                if (this.firstName == value)
                {
                    return;
                }

                this.firstName = value;
                this.NotifyPropertyChanged();
            }
        }

        public string LastName
        {
            get
            {
                return this.lastName;
            }

            set
            {
                if (this.lastName == value)
                {
                    return;
                }

                this.lastName = value;
                this.NotifyPropertyChanged();
            }
        }

        public string Phone
        {
            get
            {
                return this.phone;
            }

            set
            {
                if (this.phone == value)
                {
                    return;
                }

                this.phone = value;
                this.NotifyPropertyChanged();
            }
        }

        public string EmergencyName
        {
            get
            {
                return this.emergencyName;
            }

            set
            {
                if (this.emergencyName == value)
                {
                    return;
                }

                this.emergencyName = value;
                this.NotifyPropertyChanged();
            }
        }

        public string EmergencyPhone
        {
            get
            {
                return this.emergencyPhone;
            }

            set
            {
                if (this.emergencyPhone == value)
                {
                    return;
                }

                this.emergencyPhone = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the password from users in the Sign Up page.
        /// </summary>
        public string Password
        {
            get
            {
                return this.password;
            }

            set
            {
                if (this.password == value)
                {
                    return;
                }

                this.password = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the password confirmation from users in the Sign Up page.
        /// </summary>
        public string ConfirmPassword
        {
            get
            {
                return this.confirmPassword;
            }

            set
            {
                if (this.confirmPassword == value)
                {
                    return;
                }

                this.confirmPassword = value;
                this.NotifyPropertyChanged();
            }
        }

        #endregion

        #region Command

        /// <summary>
        /// Gets or sets the command that is executed when the Log In button is clicked.
        /// </summary>
        public Command LoginCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the Sign Up button is clicked.
        /// </summary>
        public Command SignUpCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when the Log in button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void LoginClicked(object obj)
        {            
            await Application.Current.MainPage.Navigation.PushAsync(new SimpleLoginPage());
        }

        /// <summary>
        /// Invoked when the Sign Up button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void SignUpClicked(object obj)
        {
            string id = Guid.NewGuid().ToString("N");
            Participant participant = new Participant(id, this.firstName, this.lastName, this.phone, this.emergencyPhone, this.emergencyName, this.Email, PasswordHasher.GetHashString(this.password));
            await AzureDbClient.SaveParticipant(participant);
            await Application.Current.MainPage.DisplayAlert($"Welcome, {this.firstName} {this.lastName}", "Please login with your new credentials", "Ok");
            await Application.Current.MainPage.Navigation.PushAsync(new SimpleLoginPage());
        }

    #endregion
}
}