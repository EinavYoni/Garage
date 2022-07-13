using System;

namespace Ex03.GarageLogic
{
    public enum eVehicleStatus
    {
        FixInProgress = 1,
        FixCompleted,
        Paid
    }

    public class OwnerInfo
    {
        private readonly string r_Name;
        private readonly string r_PhoneNumber;
        private eVehicleStatus m_VehicleStatus = eVehicleStatus.FixInProgress;

        public OwnerInfo(string i_Name, string i_PhoneNumber)
        {
            r_Name = i_Name;
            r_PhoneNumber = i_PhoneNumber;
        }

        public string Name
        {
            get
            {
                return r_Name;
            }
        }

        public string PhoneNumber
        {
            get
            {
                return r_PhoneNumber;
            }
        }

        public eVehicleStatus CarStatus
        {
            get
            {
                return m_VehicleStatus;
            }

            set
            {
                m_VehicleStatus = value;
            }
        }

        public override string ToString()
        {
            string details;

            details = string.Format(
@"Owner name: {0}
Owner phone number: {1}
Car status: {2}",
r_Name, r_PhoneNumber, m_VehicleStatus);

            return details;
        }

        public static bool ValidateStatus(string i_UserInput, out eVehicleStatus i_VehicleStatus)
        {
            System.Array enumOption = Enum.GetValues(typeof(eVehicleStatus));
            bool validStatus = Enum.TryParse<eVehicleStatus>(i_UserInput, out i_VehicleStatus);  
            
            if (!validStatus)
            {
                throw new FormatException("Vehicle Status is not exist");
            }

            validStatus = Enum.IsDefined(typeof(eVehicleStatus), i_VehicleStatus);
            if (!validStatus)
            {
                throw new ValueOutOfRangeException((int)enumOption.GetValue(0), (int)enumOption.GetValue(enumOption.Length-1));
            }

            return validStatus;
        }

        public static bool ValidatePhoneNumber(string i_PhoneNumber)
        {
            const int k_ValidLenNumber = 10;
            bool validInput = isFirstDigitIsZero(i_PhoneNumber) && i_PhoneNumber.Length == k_ValidLenNumber;

            if (!validInput)
            {
                throw new ArgumentException("Phone number legnth need to be 10 chars and start with 0");
            }

            validInput = int.TryParse(i_PhoneNumber, out int res);
            if (!validInput)
            {
                throw new FormatException("invalid format");
            }

            return validInput;
        }

        private static bool isFirstDigitIsZero(string i_PhoneNumber)
        {
            const int k_EmptyPhoneNumber = 0;
            const char k_Zero = '0';

            return i_PhoneNumber.Length != k_EmptyPhoneNumber && i_PhoneNumber[0] == k_Zero;
        }
    }
}
