using System;

namespace Ex03.GarageLogic
{
    public enum eEnergySource
    {
        Battery = 1,
        Fuel
    }

    public abstract class EnergySource
    {
        private float m_CurrentAmountOfEnergy = 0;
        private float m_MaxAmountOfEnergy;

        public EnergySource(float i_MaxAmountOfEnergy)
        {
            m_MaxAmountOfEnergy = i_MaxAmountOfEnergy;
        }

        public float CurrentAmountOfEnergy
        {
            get
            {
                return m_CurrentAmountOfEnergy;
            }

            set
            {
                m_CurrentAmountOfEnergy = value;
            }
        }

        public float MaxAmountOfEnergy
        {
            get
            {
                return m_MaxAmountOfEnergy;
            }

            set
            {
                m_MaxAmountOfEnergy = value;
            }
        }

        public override string ToString()
        {
            string details;

            details = string.Format(
@"Current amount of energy: {0}
Max amount of energy: {1}",
m_CurrentAmountOfEnergy,
m_MaxAmountOfEnergy);

            return details;
        }

        public void LoadEnergySource(float i_AmountToLoad)
        {
            m_CurrentAmountOfEnergy += i_AmountToLoad;
        }

        public bool IsValidAmountOfEnergy(string i_UserInputAmount, out float i_AmountToLoad)
        {
            bool validAmount = float.TryParse(i_UserInputAmount, out i_AmountToLoad);

            if (!validAmount)
            {
                throw new FormatException("Invalid format");
            }

            validAmount = i_AmountToLoad >= 0 && i_AmountToLoad + m_CurrentAmountOfEnergy <= m_MaxAmountOfEnergy;
            if (!validAmount)
            {
                throw new ValueOutOfRangeException(0, m_MaxAmountOfEnergy - m_CurrentAmountOfEnergy);
            }

            return validAmount;
        }

        public bool IsCurrAmountIsMax()
        {
            bool validAmount = m_CurrentAmountOfEnergy != m_MaxAmountOfEnergy;

            if (!validAmount)
            {
                throw new ArgumentException("Energy is full");
            }

            return validAmount;
        }
    }
}
