//	Generated using Unvired Modeller - Build R-4.000.0129

using Unvired.Kernel.SQLite;
using Unvired.Kernel.UWP.Model;

namespace Entity
{
    // This class is part of the BE "CONTACT". 
    public partial class CONTACT_HEADER : DataStructure
    {

        // No desc available
        private int? _ContactId;

        // No desc available
        private string _ContactName;

        // No desc available
        private string _Phone;

        // No desc available
        private string _Email;

        internal static bool IsHeader
        {
            get
            {
                return true;
            }
        }

        [Unique]
        public int? ContactId
        {
            get
            {
                return _ContactId;
            }
            set
            {
                _ContactId = value;
                RaisePropertyChanged("ContactId");
            }
        }

        public string ContactName
        {
            get
            {
                return _ContactName;
            }
            set
            {
                _ContactName = value;
                RaisePropertyChanged("ContactName");
            }
        }

        public string Phone
        {
            get
            {
                return _Phone;
            }
            set
            {
                _Phone = value;
                RaisePropertyChanged("Phone");
            }
        }

        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                _Email = value;
                RaisePropertyChanged("Email");
            }
        }
    }
}