using System;
using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public enum eColor
    {
        White = 1,
        Red,
        Green,
        Blue
    }

    public enum eAmountOfDoors
    {
        Two = 2,
        Three,
        Four,
        Five
    }

    public class Car : Vehicle
    {
        private const int k_AmountOfWheels = 4;
        private const int k_MaxTirePressure = 29;
        private const float k_MaxAmountOfEnergyElectric = 3.3f;
        private const float k_MaxAmountOfEnergyFuel = 38f;
        private const eFuelType k_FuelType = eFuelType.Octan95;
        private eColor m_Color;
        private eAmountOfDoors m_AmountOfDoors;

        public Car(
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

        public eColor Color
        {
            get
            {
                return m_Color;
            }

            set
            {
                m_Color = value;
            }
        }

        public eAmountOfDoors AmountOfDoors
        {
            get
            {
                return m_AmountOfDoors;
            }

            set
            {
                m_AmountOfDoors = value;
            }
        }

        public override string ToString()
        {
            string details, baseDetails;

            baseDetails = base.ToString();
            details = string.Format(
@"{0}
Color: {1}
Amount of doors: {2}",
baseDetails, m_Color, m_AmountOfDoors);

            return details;
        }

        protected override void setDetailsMsg()
        {
            string msgColor, doorsMsg;

            msgColor = string.Format(
@"Please enter the car color:
{0}-{1}
{2}-{3} 
{4}-{5} 
{6}-{7} ",
(int)eColor.White, eColor.White,
(int)eColor.Red, eColor.Red,
(int)eColor.Green, eColor.Green,
(int)eColor.Blue, eColor.Blue);
            doorsMsg = string.Format(
@"Please enter the number of doors:
{0}-{1}
{2}-{3} 
{4}-{5}
{6}-{7} ",
(int)eAmountOfDoors.Two, eAmountOfDoors.Two, 
(int)eAmountOfDoors.Three, eAmountOfDoors.Three,
(int)eAmountOfDoors.Four, eAmountOfDoors.Four,
(int)eAmountOfDoors.Five, eAmountOfDoors.Five);

            Details.Add(msgColor);
            Details.Add(doorsMsg);
        }

        public override bool isDetailsValid(List<string> i_Details)
        {
            const int k_ColorIndex = 0;
            const int k_AmountOfDoorsIndex = 1;
            bool validDetails;
            System.Array enumOptionDoors, enumColorOptions;

            validDetails = Enum.TryParse<eColor>(i_Details[k_ColorIndex], out m_Color)
                && Enum.TryParse<eAmountOfDoors>(i_Details[k_AmountOfDoorsIndex], out m_AmountOfDoors);
            if (!validDetails)
            {
                throw new FormatException("Data is not valid");
            }

            validDetails = Enum.IsDefined(typeof(eColor), m_Color);
            enumColorOptions = Enum.GetValues(typeof(eColor));

            if (!validDetails)
            {
                throw new ValueOutOfRangeException((int)enumColorOptions.GetValue(0), (int)enumColorOptions.GetValue(enumColorOptions.Length-1));
            }

            enumOptionDoors = Enum.GetValues(typeof(eAmountOfDoors));
            validDetails = Enum.IsDefined(typeof(eAmountOfDoors), m_AmountOfDoors);
            if (!validDetails)
            {
                throw new ValueOutOfRangeException((int)enumOptionDoors.GetValue(0), (int)enumOptionDoors.GetValue(enumOptionDoors.Length-1));
            }

            return validDetails;
        }
    }
}
