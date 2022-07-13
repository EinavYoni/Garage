using System;

namespace Ex03.GarageLogic
{
    public class Wheel
    {
        private readonly string r_ManufacturName;
        private float m_CurrentTirePressure = 0;
        private readonly float r_MaxTirePressure;

        public Wheel(string i_ManufacturName, float i_MaxTirePressure)
        {
            r_ManufacturName = i_ManufacturName;
            r_MaxTirePressure = i_MaxTirePressure;
        }

        public Wheel(Wheel wheel)
        {
            r_ManufacturName = wheel.r_ManufacturName;
            m_CurrentTirePressure = wheel.m_CurrentTirePressure;
            r_MaxTirePressure = wheel.r_MaxTirePressure;
        }

        public string ManufacturName
        {
            get
            {
                return r_ManufacturName;
            }
        }

        public float CurrentTirePressure
        {
            get
            {
                return m_CurrentTirePressure;
            }

            set
            {
                m_CurrentTirePressure = value;
            }
        }

        public float MaxTirePressure
        {
            get
            {
                return r_MaxTirePressure;
            }
        }

        public void UpdateTirePressure(float i_AddTirePressure)
        {
            m_CurrentTirePressure += i_AddTirePressure;
        }

        public bool ValidateTirePressure(string i_UserInput, out float i_AirPressure)
        {
            bool validAirPressure;

            validAirPressure = float.TryParse(i_UserInput, out i_AirPressure);
            if (!validAirPressure)
            {
                throw new FormatException("Invalid format");
            }

            validAirPressure = i_AirPressure >= 0 && m_CurrentTirePressure + i_AirPressure <= r_MaxTirePressure;
            if (!validAirPressure)
            {
                throw new ValueOutOfRangeException(0, r_MaxTirePressure - m_CurrentTirePressure);
            }

            return validAirPressure;
        }

        public override string ToString()
        {
            string details;

            details = string.Format(
@"Manufactur name: {0}
Current tire pressure: {1}
Max tire pressure: {2}",
r_ManufacturName,
m_CurrentTirePressure,
r_MaxTirePressure);

            return details;
        }

        public bool IsTirePressureAtMax()
        {
            bool isMax = m_CurrentTirePressure == r_MaxTirePressure;

            if (isMax)
            {
                throw new ArgumentException("Tire pressure is max can't fill more pressure");
            }

            return isMax;
        }

        public bool TirePressureMoreThanMax(float airPressure)
        {
            bool validAirPressure = m_CurrentTirePressure + airPressure > r_MaxTirePressure;

            if (!validAirPressure)
            {
                throw new ArgumentException("Can't fill more than max Pressure");
            }

            return validAirPressure;
        }
    }
}
