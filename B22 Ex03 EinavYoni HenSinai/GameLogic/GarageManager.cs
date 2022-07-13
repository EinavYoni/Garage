using System;
using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public struct CustomerData
    {
        public OwnerInfo m_OwnerInfo;
        public Vehicle m_Vehicle;

        public override string ToString()
        {
            string details;

            details = string.Format(
@"{0}
{1}",
m_Vehicle.ToString(), m_OwnerInfo.ToString());

            return details;
        }
    }

    public class GarageManager
    {
        private readonly Dictionary<string, CustomerData> r_CustomersData =
            new Dictionary<string, CustomerData>();

        public Dictionary<string, CustomerData> CustomersData
        {
            get
            {
                return r_CustomersData;
            }
        }

        public bool IsVehicleExist(string i_LicenseNumber)
        {
            return r_CustomersData.ContainsKey(i_LicenseNumber);
        }

        public void ResetCarStatus(string i_LicenseNumber)
        {
            CustomersData[i_LicenseNumber].m_OwnerInfo.CarStatus = eVehicleStatus.FixInProgress;
        }

        public List<string> FilterVehicleByStatus(eVehicleStatus i_Status, bool i_IsFilterChosen)
        {
            List<string> licenseNumber = new List<string>();

            foreach (CustomerData customerData in r_CustomersData.Values)
            {
                if (!i_IsFilterChosen || customerData.m_OwnerInfo.CarStatus == i_Status)
                {
                    licenseNumber.Add(customerData.m_Vehicle.LicenseNumber);
                }
            }

            if (licenseNumber.Count == 0)
            {
                throw new ArgumentException("L1ist is empty");
            }

            return licenseNumber;
        }

        public void ChangeCarStatus(string i_LicenseNumber, eVehicleStatus i_NewStatus)
        {
            r_CustomersData[i_LicenseNumber].m_OwnerInfo.CarStatus = i_NewStatus;
        }

        public Vehicle GetVehicleByLicenseNumber(string i_LicenseNumber)
        {
           return r_CustomersData[i_LicenseNumber].m_Vehicle;
        }

        public CustomerData GetCustomerDataByLicensNumber(string i_LicenseNumber)
        {
            return r_CustomersData[i_LicenseNumber];
        }

        public bool IsGarageEmpty()
        {
            bool isEmpty = r_CustomersData.Count == 0;

            if (isEmpty)
            {
                throw new ArgumentException("No vehicle in garage");
            }

            return isEmpty;
        }
    }
}
