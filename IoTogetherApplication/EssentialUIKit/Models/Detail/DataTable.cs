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
        /// Gets or sets the FirstName .
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the LastName.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the Phone.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the BatteryPercentageDiagram.
        /// </summary>
        public string[] BatteryPercentageDiagram { get; set; }

        #endregion
    }
}
