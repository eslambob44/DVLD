using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_PresentationLayer
{
    public partial class ctrlUserImage : UserControl
    {
        public ctrlUserImage()
        {
            InitializeComponent();
        }

        public event Action<bool> EventUserImageChanged;

        public void UserImageChanged(bool IsPictureExist)
        {
            Action<bool> Handler = EventUserImageChanged;
            if (Handler != null)
            {
                Handler(IsPictureExist);
            }
        }

        private string _ImageLocation;
        public string ImageLocation { get { return _ImageLocation; } }
        
        public void SetImage(string imageLocation , clsPerson.enGendor Gendor)
        {
            pbImage.ImageLocation = imageLocation;
            if(pbImage.ImageLocation != null)
            {
                _ImageLocation = imageLocation;
                UserImageChanged(true);
            }
            else
            {
                pbImage.Image = (Gendor == clsPerson.enGendor.Male) ? Properties.Resources.Male_512 : Properties.Resources.Female_512 ;
                _ImageLocation = null;
                UserImageChanged(false);
            }
        }
    }
}
