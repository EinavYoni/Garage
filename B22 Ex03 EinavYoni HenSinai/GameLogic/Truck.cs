using System;
using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public class Truck : Vehicle
    {
        private const int k_AmountOfWheels = 16;
        private const int k_MaxTirePressure = 24;
        private const float k_MaxAmountOfEnergyFuel = 120f;
        private const float k_MaxCargoVolume = 100;
        private const eFuelType k_FuelType = eFuelType.Soler;
        private bool m_IsTransferItemsInCoolTemp;
        private float m_CargoVolume;

        public Truck(
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
                  k_MaxAmountOfEnergyFuel)
        {
            setDetailsMsg();
            setFuelTypeIfNeeded(k_FuelType);
        }

        public bool IsTransferItemsInCoolTemp
        {
            get
            {
                return m_IsTransferItemsInCoolTemp;
            }

            set
            {
                m_IsTransferItemsInCoolTemp = value;
            }
        }

        public float CargoVolume
        {
            get
            {
                return m_CargoVolume;
            }

            set
            {
                m_CargoVolume = value;
            }
        }

        public override string ToString()
        {
            string details, baseDetails;

            baseDetails = base.ToString();
            details = string.Format(
@"{0}
Is transfer items in cool temp: {1}
Cargo volume: {2}",
baseDetails, m_IsTransferItemsInCoolTemp, m_CargoVolume);

            return details;
        }

        protected override void setDetailsMsg()
        {
            Details.Add("Does the truck carry refrigerated contents in cool temperture (Enter: True, False)");
            Details.Add("Please enter the cargo volume");
        }

        public override bool isDetailsValid(List<string> i_Details)
        {
            const int k_IsTransferItemsInCoolTempIndex = 0;
            const int k_CargoVolumeIndex = 1;
            bool validDetails;

            validDetails = float.TryParse(i_Details[k_CargoVolumeIndex], out m_CargoVolume)
                && bool.TryParse(i_Details[k_IsTransferItemsInCoolTempIndex], out m_IsTransferItemsInCoolTemp);
            if (!validDetails)
            {
                throw new FormatException("Data is not valid");
            }

            validDetails = m_CargoVolume > 0 && m_CargoVolume <= k_MaxCargoVolume;
            if (!validDetails)
            {
                throw new ValueOutOfRangeException(0, k_MaxCargoVolume);
            }

            return validDetails;
        }
    }
}
