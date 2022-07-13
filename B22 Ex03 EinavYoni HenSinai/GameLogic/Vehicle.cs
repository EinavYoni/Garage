using System.Collections.Generic;
using System;

namespace Ex03.GarageLogic
{
    public abstract class Vehicle
    {
        private readonly string r_ModelName;
        private readonly string r_LicenseNumber;
        private readonly List<Wheel> r_WheelsCollection;
        protected EnergySource m_EnergySource;
        protected readonly List<string> r_Details = new List<string>();
        private float m_PerecentageOfEnergyLeft;

        public Vehicle(
            string i_ModelName,
            string i_LicenseNumber,
            int i_AmountOfWheels,
            string i_WheelManufacturName,
            float i_MaxTirePressure,
            eEnergySource i_EnergySource,
            float i_MaxAmountOfEnergy)
        {
            Wheel wheel = new Wheel(i_WheelManufacturName, i_MaxTirePressure);
            r_ModelName = i_ModelName;
            r_LicenseNumber = i_LicenseNumber;
            r_WheelsCollection = createWheels(wheel, i_AmountOfWheels);
            setEnergySource(i_EnergySource, i_MaxAmountOfEnergy);
        }

        public string ModelName
        {
            get
            {
                return r_ModelName;
            }
        }

        public string LicenseNumber
        {
            get
            {
                return r_LicenseNumber;
            }
        }

        public List<Wheel> WheelsCollection
        {
            get
            {
                return r_WheelsCollection;
            }
        }

        public EnergySource EnergySource
        {
            get
            {
                return m_EnergySource;
            }
        }

        public List<string> Details
        {
            get
            {
                return r_Details;
            }
        }

        private void setEnergySource(eEnergySource i_EnergySource, float i_MaxAmountOfEnergy)
        {
            if (i_EnergySource == eEnergySource.Fuel)
            {
                m_EnergySource = new Fuel(i_MaxAmountOfEnergy);
            }
            else
            {
                m_EnergySource = new Battery(i_MaxAmountOfEnergy);
            }
        }

        private List<Wheel> createWheels(Wheel i_Wheel, int i_AmountOfWheels)
        {
            List<Wheel> wheels = new List<Wheel>(i_AmountOfWheels);

            for (int i = 0; i < i_AmountOfWheels; i++)
            {
                wheels.Add(new Wheel(i_Wheel));
            }

            return wheels;
        }

        protected void setFuelTypeIfNeeded(eFuelType i_FuelType)
        {
            if (m_EnergySource is Fuel)
            {
                ((Fuel)m_EnergySource).FuelType = i_FuelType;
            }
        }

        protected abstract void setDetailsMsg();

        public abstract bool isDetailsValid(List<string> i_DetailsList);

        public void UpdateAllTiresPressureToMax()
        {
            float addNeededTirePressureToMax, maxTirePressure;

            maxTirePressure = r_WheelsCollection[0].MaxTirePressure;
            foreach (Wheel wheel in r_WheelsCollection)
            {
                addNeededTirePressureToMax = maxTirePressure - wheel.CurrentTirePressure;
                wheel.UpdateTirePressure(addNeededTirePressureToMax);
            }
        }

        public void UpdateTirePressureByValue(float i_AddTirePressure)
        {
            foreach (Wheel wheel in r_WheelsCollection)
            {
                wheel.UpdateTirePressure(i_AddTirePressure);
            }
        }

        public override string ToString()
        {
            string details;

            details = string.Format(
@"Model name: {0}
License number: {1}
Perecentage of energy left: {2}%
Energy source: 
{3}
Wheels collection:
{4}",
r_ModelName,
r_LicenseNumber,
m_PerecentageOfEnergyLeft,
m_EnergySource.ToString(),
r_WheelsCollection[0].ToString());
            
            return details;
        }

        public void CulcPerecentageOfEnergyLeft()
        {
            m_PerecentageOfEnergyLeft = 100 * m_EnergySource.CurrentAmountOfEnergy / m_EnergySource.MaxAmountOfEnergy;
        }
    }
}
