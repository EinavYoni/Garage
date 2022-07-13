using System;

namespace Ex03.GarageLogic
{
    public enum eFuelType
    {
        Octan96 = 1,
        Octan95,
        Octan98,
        Soler
    }

    public class Fuel : EnergySource
    {
        private eFuelType m_FuelType;

        public Fuel(float i_MaxAmountOfEnergy) : base(i_MaxAmountOfEnergy) { }

        public eFuelType FuelType
        {
            get
            {
                return m_FuelType;
            }
            set
            {
                m_FuelType = value;
            }
        }

        public override string ToString()
        {
            string details, baseDetails;

            baseDetails = base.ToString();
            details = string.Format(
@"{0}-{1}
{2}",
eEnergySource.Fuel, m_FuelType, baseDetails);

            return details;
        }

        public bool IsFuelMatchVehicle(eFuelType i_FuelType)
        {
            bool isMatch = i_FuelType == m_FuelType;

            if (!isMatch)
            {
                throw new ArgumentException("Vehicle fuel type is different");
            }

            return isMatch;
        }

        public static bool ValidteType(string userInput, out eFuelType fuelType)
        {
            bool validType;
            System.Array enumOption;

            enumOption = Enum.GetValues(typeof(eFuelType));
            validType = Enum.TryParse<eFuelType>(userInput, out fuelType);
            if (!validType)
            {
                throw new FormatException("Type is not valid");

            }

            validType = Enum.IsDefined(typeof(eFuelType), fuelType);
            if (!validType)
            {
                throw new ValueOutOfRangeException((int)enumOption.GetValue(0), (int)enumOption.GetValue(enumOption.Length - 1));
            }

            return validType;
        }
    }
}
