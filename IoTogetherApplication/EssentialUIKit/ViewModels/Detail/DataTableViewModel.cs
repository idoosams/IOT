using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EssentialUIKit.Models.Detail;
using EssentialUIKit.Views.Forms;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace EssentialUIKit.ViewModels.Detail
{
    /// <summary>
    /// View model for data table 
    /// </summary> 
    [Preserve(AllMembers = true)]
    public class DataTableViewModel : BaseViewModel, INotifyPropertyChanged
    {
        #region Fields

        private string groupId = App._groupId;

        private string _groupName = App._groupName;

        private List<DataTable> items;

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="DataTableViewModel" /> class.
        /// </summary>
        public DataTableViewModel()
        {
            this.Items = new List<DataTable>();
            this.MapClicked = new Command(this.OnMapClick);
            this.LeaveClicked = new Command(this.OnLeaveClick);
        }
        #endregion

        #region Public Properties

        public Command MapClicked { get; set; }

        public Command LeaveClicked { get; set; }

        public string GroupName
        {
            get
            {
                return this._groupName;
            }

            set
            {
                if (this._groupName == value)
                {
                    return;
                }

                this._groupName = value;
                this.NotifyPropertyChanged();
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

        /// <summary>
        /// Gets or sets the property that has been bound with a list view, which displays the items.
        /// </summary>
        public List<DataTable> Items
        {
            get
            {
                return this.items;
            }

            set
            {
                if (this.items == value)
                {
                    return;
                }

                this.items = value;
                this.NotifyPropertyChanged();
            }
        }

        #endregion

        #region Private Methods
        public void OnLeaveClick()
        {
            Application.Current.MainPage.Navigation.PushAsync(new ChooseActionPage(), true);
        }

        public async void OnMapClick()
        {
            var map = await MapPageViewModel.GetMap(App._groupId);
            await Application.Current.MainPage.Navigation.PushAsync(map);
        }

        #endregion
    }
}
