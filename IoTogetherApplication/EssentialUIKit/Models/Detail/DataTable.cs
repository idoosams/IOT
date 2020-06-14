using Xamarin.Forms.Internals;

namespace EssentialUIKit.Models.Detail
{
    /// <summary>
    /// Model for Data table
    /// /// </summary>
    [Preserve(AllMembers = true)]
    public class DataTable
    {

        #region Public Properties

        /// <summary>RowKey
        /// Gets or sets the RowKey.
        /// </summary>
        public string RowKey { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Phone.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the Emergency Phone.
        /// </summary>
        public string EmergencyPhone { get; set; }

        /// <summary>
        /// Gets or sets the Distance.
        /// </summary>
        public int Distance { get; set; }

        /// <summary>
        /// Gets or sets the BatteryPercentageDiagram.
        /// </summary>
        public string[] BatteryPercentageDiagram { get; set; }

        #endregion
    }
}
