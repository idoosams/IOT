using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EssentialUIKit.Models.Detail;
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

        private List<DataTable> items;

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="DataTableViewModel" /> class.
        /// </summary>
        public DataTableViewModel()
        {
            Task.Factory.StartNew(() => methodRunPeriodically());
        }
        #endregion

        #region Public Properties

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

        private string[] CreateBatteryColorView(int batteryPercentage)
        {
            if (batteryPercentage <= 15)
                return new string[5] { "#ff4a4a", "#b2b8c2", "#b2b8c2", "#b2b8c2", "#b2b8c2" };
            else if (batteryPercentage <= 30)
                return new string[5] { "#ff4a4a", "#ff4a4a", "#b2b8c2", "#b2b8c2", "#b2b8c2" };
            else if (batteryPercentage <= 50)
                return new string[5] { "#7ed321", "#7ed321", "#7ed321", "#b2b8c2", "#b2b8c2" };
            else if (batteryPercentage <= 50)
                return new string[5] { "#7ed321", "#7ed321", "#7ed321", "#7ed321", "#b2b8c2" };
            else
                return new string[5] { "#7ed321", "#7ed321", "#7ed321", "#7ed321", "#7ed321" };
        }

        async Task methodRunPeriodically()
        {
            while (true)
            {
                var participantsFromTable = App._activeUsers;
                this.Items = new List<DataTable>();
                foreach (var participant in participantsFromTable)
                {
                    this.Items.Add(new DataTable
                    {
                        //   RowKey = participant.RowKey,
                        FirstName = participant.FirstName,
                        LastName = participant.LastName,
                        Phone = participant.Phone,
                        //BatteryPercentageDiagram = CreateBatteryColorView(participant.BatteryPercentage)
                        BatteryPercentageDiagram = CreateBatteryColorView(20)
                    });
                }
                await Task.Delay(3000); //Refresh every 3 seconds
            }
        }

        #endregion
    }
}
