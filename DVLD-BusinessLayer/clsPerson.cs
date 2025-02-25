using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccessLayer;
namespace DVLD_BusinessLayer
{
    public class clsPerson
    {
        private int _ID;
        public int ID { get { return _ID; } }
        public string NationalNumber { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }

        public string FullName
        {
            get { return $"{FirstName} {SecondName} {ThirdName} {LastName}"; }
        }
        public DateTime DateOfBirth { get; set; }
        public enum enGendor { Male = 0 , Female = 1,}
        private enGendor _Gendor;
        public enGendor Gendor 
        { 
            get { return _Gendor; }

            set 
            { 
                _Gendor = value;
            } 
        }

        public string GendorString { get { return (Gendor == enGendor.Male) ? "Male" : "Female"; } }
        public string Address { get; set; }
        public string Phone {  get; set; }
        public string Email { get; set; }
        public int NationalityCountryID { get; set; }
        public string CountryName { get { return clsCountryDataAccessLayer.FindCountry(NationalityCountryID); } }
        public string ImagePath {  get; set; }
        private string _OriginalImagePath;

        enum enMode { Update , AddNew}
        private enMode _Mode;

        private clsPerson(int PersonID , string NationalNumber , string FirstName , string SecondName , string ThirdName , string LastName 
            , DateTime DateOfBirth , enGendor Gendor , string Address , string Phone , string Email , int NationalityCountryID , string ImagePath )
        {
            this._ID = PersonID ;
            this.NationalNumber = NationalNumber ;
            this.FirstName = FirstName ;
            this.LastName = LastName ;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName ;
            this.DateOfBirth = DateOfBirth ;
            this.Gendor = Gendor ;
            this.Address = Address ;
            this.Phone = Phone ;
            this.Email = Email ;
            this.NationalityCountryID = NationalityCountryID ;
            this.ImagePath  = ImagePath ;
            _OriginalImagePath = ImagePath;
            _Mode = enMode.Update;
        }

        private clsPerson() 
        {
            this._ID = -1;
            this.NationalNumber = null;
            this.FirstName = null;
            this.LastName = null;
            this.SecondName = null;
            this.ThirdName = null;
            this.DateOfBirth = new DateTime();
            this.Gendor = enGendor.Male;
            this.Address = null;
            this.Phone = null;
            this.Email = null;
            this.NationalityCountryID = -1;
            this.ImagePath = null;
            _OriginalImagePath = ImagePath;
            _Mode = enMode.AddNew;
        }

        static public clsPerson Find(int PersonID)
        {
            string NationalityNumber = null;
            byte _ShortGendor = 0;
            string FirstName = null;
            string SecondName = null;
            string ThirdName = null;
            string LastName = null;
            DateTime DateOfBirth = new DateTime();
            string Address = null;
            string Phone = null;    
            string Email = null;
            int NationalityCountryID = -1;  
            string ImagePath = null;
            bool IsFound = clsPersonDataAccess.FindPerson(PersonID, out NationalityNumber, out FirstName, out SecondName, out ThirdName, out LastName
                , out DateOfBirth, out _ShortGendor, out Address, out Phone, out Email, out NationalityCountryID, out ImagePath);

            if(IsFound)
            {
                enGendor eGendor = (enGendor)Convert.ToInt32(_ShortGendor);
                return new clsPerson(PersonID, NationalityNumber, FirstName, SecondName, ThirdName, LastName,
                    DateOfBirth, eGendor, Address, Phone, Email, NationalityCountryID, ImagePath);
            }
            else
            {
                return null;
            }


        }

        static public clsPerson Find(string NationalityNumber)
        {
            
            bool IsFound = clsPersonDataAccess.FindPerson(NationalityNumber,out int PersonID , out string FirstName , out string SecondName , out string ThirdName ,
                                                          out string LastName , out DateTime DateOfBirth , out byte Gendor , out string Address , out string Phone ,
                                                           out string Email , out int NationalityCountryID , out string ImagePath);

            if (IsFound)
            {
                enGendor eGendor = (enGendor)Convert.ToInt32(Gendor);
                return new clsPerson(PersonID, NationalityNumber, FirstName, SecondName, ThirdName, LastName,
                    DateOfBirth, eGendor, Address, Phone, Email, NationalityCountryID, ImagePath);
            }
            else
            {
                return null;
            }


        }

        static public clsPerson GetAddNewObject()
        {
            return new clsPerson();
        }

        bool _RemoveImage(string ImagePath)
        {
            if (File.Exists(ImagePath))
            {
                File.Delete(ImagePath);
                return true;
            }
            else
            {
                return false;
            }
        }

        string _SetImage(string ImagePath)
        {


            if (File.Exists(ImagePath))
            {

                Guid ImageName = Guid.NewGuid();
                string DestinationImagePath = "C:\\DVLD-Images\\" + ImageName.ToString() + ".png";
                File.Copy(ImagePath, DestinationImagePath);
                return DestinationImagePath;
            }
            return null;
        }

        bool _AddNew()
        {
            if (_OriginalImagePath != ImagePath) ImagePath = _SetImage(ImagePath);
            _ID = clsPersonDataAccess.AddNewPerson(NationalNumber, FirstName, SecondName, ThirdName, LastName, DateOfBirth, (byte)_Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);
            if(_ID != -1)
            {
                _Mode = enMode.Update;
                if (_OriginalImagePath != ImagePath)
                {
                    _RemoveImage(_OriginalImagePath);
                    _OriginalImagePath = ImagePath;
                }
                return true;
            }
            if (_OriginalImagePath != ImagePath) _RemoveImage(ImagePath);
            return false;
        }

        bool _Update()
        {
            if (_OriginalImagePath != ImagePath) ImagePath = _SetImage(ImagePath);
             if(clsPersonDataAccess.UpdatePerson(_ID, NationalNumber, FirstName, SecondName, ThirdName, LastName, DateOfBirth, (byte)_Gendor, Address, Phone, Email, NationalityCountryID, ImagePath))
            {
                if (_OriginalImagePath != ImagePath)
                {
                    _RemoveImage(_OriginalImagePath);
                    _OriginalImagePath = ImagePath;
                }
                return true;
            }
            if (_OriginalImagePath != ImagePath) _RemoveImage(ImagePath);
            return false;
        }

        public bool Save()
        {
            switch(_Mode)
            {
                case enMode.Update:
                    return _Update();
                case enMode.AddNew:
                    return _AddNew();
                default: return false;
            }
        }

        static public bool Delete(int PersonID)
        {
            return clsPersonDataAccess.DeletePerson(PersonID);
        }

        static public DataTable ListPeople(string Field , string Value)
        {
            return clsPersonDataAccess.ListPeople(Field, Value);
        }
        static public DataTable ListPeople()
        {
            return clsPersonDataAccess.ListPeople();
        }


        static public bool IsPersonExists(int PersonID)
        {
            return clsPersonDataAccess.IsPersonExists(PersonID);
        }

        static public bool IsPersonExists(string NationalNumber)
        {
            return clsPersonDataAccess.IsPersonExists(NationalNumber);
        }



    }
}
