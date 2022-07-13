using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex03.GarageLogic
{
    public enum eVehicleType
    {
        Car = 1,
        Electric_Car,
        Motorcycle,
        Electric_Motorcycle,
        Truck,
    }

    public static class Factory
    {
        public static Vehicle CreateVehicle(
            eVehicleType i_Type,
            string i_ModelName,
            string i_LicenseNumber,
            string i_ManufacturName)
        {
            Vehicle result = null;

            switch (i_Type)
            {
                case eVehicleType.Motorcycle:
                    result = new Motorcycle(i_ModelName, i_LicenseNumber, i_ManufacturName, eEnergySource.Fuel);
                    break;

                case eVehicleType.Electric_Motorcycle:
                    result = new Motorcycle(i_ModelName, i_LicenseNumber, i_ManufacturName, eEnergySource.Battery);
                    break;

                case eVehicleType.Car:
                    result = new Car(i_ModelName, i_LicenseNumber, i_ManufacturName, eEnergySource.Fuel);
                    break;

                case eVehicleType.Electric_Car:
                    result = new Car(i_ModelName, i_LicenseNumber, i_ManufacturName, eEnergySource.Battery);
                    break;

                case eVehicleType.Truck:
                    result = new Truck(i_ModelName, i_LicenseNumber, i_ManufacturName, eEnergySource.Fuel);
                    break;
            }

            return result;
        }

        public static bool ValidateType(string i_UserInput, out eVehicleType i_VehicleStatus)
        {
            System.Array enumOption;
            bool validStatus = Enum.TryParse(i_UserInput, out i_VehicleStatus);

            enumOption = Enum.GetValues(typeof(eVehicleType));
            if (!validStatus)
            {
                throw new FormatException("Vehicle Type is not exist");
            }

            validStatus = Enum.IsDefined(typeof(eVehicleType), i_VehicleStatus);
            if (!validStatus)
            {
                throw new ValueOutOfRangeException((int)enumOption.GetValue(0), (int)enumOption.GetValue(enumOption.Length - 1));
            }

            return validStatus;
        }
    }
}