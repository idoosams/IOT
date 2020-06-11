using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Map = Xamarin.Forms.Maps.Map;

namespace EssentialUIKit.Views.Forms
{
    /// <summary>
    /// Page to login with user name and password
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapPage" /> class.
        /// </summary>
        public MapPage()
        {
            InitializeComponent();
        }
    }
}