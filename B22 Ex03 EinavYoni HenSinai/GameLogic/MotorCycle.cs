using System.Collections.Generic;
using System;

namespace Ex03.GarageLogic
{
    public enum eLicenseType
    {
        A = 1,
        A1,
        B1,
        BB
    }

    public class Motorcycle : Vehicle
    {
        private const int k_AmountOfWheels = 2;
        private const int k_MaxTirePressure = 31;
        private const float k_MaxAmountOfEnergyElectric = 2.5f;
        private const float k_MaxAmountOfEnergyFuel = 6.2f;
        private const float k_MaxEngineVolumeInCC = 50f;
        private const eFuelType k_FuelType = eFuelType.Octan98;
        private eLicenseType m_LicenseType;
        private int m_EngineVolumeInCC;

        public Motorcycle(
            string i_ModelName,
            string i_LicenseNumber,
            string i_WheelManufacturName,
            eEnergySource i_EnergySource)
            : base(
                  i_ModelName,
                  i_LicenseNumber,
                  k_AmountOfWheels,
                  i_WheelManufacturName,
                  k_MaxTirePressure,
                  i_EnergySource,
                  i_EnergySource == eEnergySource.Fuel ? k_MaxAmountOfEnergyFuel : k_MaxAmountOfEnergyElectric)
        {
            setDetailsMsg();
            setFuelTypeIfNeeded(k_FuelType);
        }

        public eLicenseType LicenseType
        {
            get
            {
                return m_LicenseType;
            }

            set
            {
                m_LicenseType = value;
            }
        }

        public int EngineVolumeInCC
        {
            get
            {
                return m_EngineVolumeInCC;
            }

            set
            {
                m_EngineVolumeInCC = value;
            }
        }

        public override string ToString()
        {
            string details, baseDetails;

            baseDetails = base.ToString();
            details = string.Format(
@"{0}
License type: {1}
Engine volume in CC: {2}",
baseDetails, m_LicenseType, m_EngineVolumeInCC);

            return details;
        }

        protected override void setDetailsMsg()
        {
            string licenseTypeMsg;

            licenseTypeMsg = string.Format(
@"Please enter the license type:
{0}-{1}
{2}-{3} 
{4}-{5} 
{6}-{7} ",
(int)eLicenseType.A, eLicenseType.A,
(int)eLicenseType.A1, eLicenseType.A1,
(int)eLicenseType.B1, eLicenseType.B1,
(int)eLicenseType.BB, eLicenseType.BB);
            Details.Add(licenseTypeMsg);
            Details.Add("Please enter the engine volume in cc");
        }

        public override bool isDetailsValid(List<string> i_Details)
        {
            const int k_LicenseTypeIndex = 0;
            const int k_EngineVolumeInCCIndex = 1;
            bool validDetails;
            System.Array enumOption;


            validDetails = int.TryParse(i_Details[k_EngineVolumeInCCIndex], out m_EngineVolumeInCC)
                && Enum.TryParse<eLicenseType>(i_Details[k_LicenseTypeIndex], out m_LicenseType);
            enumOption = Enum.GetValues(typeof(eLicenseType));
            if (!validDetails)
            {
                throw new FormatException("Data is not valid");
            }

            validDetails = Enum.IsDefined(typeof(eLicenseType), m_LicenseType);
            if (!validDetails)
            {
                throw new ValueOutOfRangeException((int)enumOption.GetValue(0), (int)enumOption.GetValue(enumOption.Length - 1));
            }

            validDetails = m_EngineVolumeInCC > 0 && m_EngineVolumeInCC <= k_MaxEngineVolumeInCC;
            if (!validDetails)
            {
                throw new ValueOutOfRangeException(0, k_MaxEngineVolumeInCC);
            }

            return validDetails;
        }
    }
}
