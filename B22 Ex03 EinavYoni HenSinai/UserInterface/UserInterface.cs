using System;
using System.Collections.Generic;
using Ex03.GarageLogic;
using System.Text;

namespace Ex03.ConsoleUI
{
   public class UserInterface
    {
        private GarageManager garageManager = new GarageManager();
        
        public void Start()
        {
            string msgMenu, userInput;
            bool quit = false;
            int userInNum;

            msgMenu =
                      string.Format(
@"***** Hello Welcome to our garage!  *************
* Please select one of the following options:   *
* 1.Add a new vehicle to the garage             *
* 2.View the list of vehicles in the garage     *
* 3.Change vehicle condition                    *
* 4.Maximum air volume in the wheels.           *
* 5.inflate Wheels                              *
* 6.Charge energy source                        *
* 7.View vehicle data                           *
* 0.Quit                                        *
*************************************************");

            while (!quit)
            {
                Console.Clear();
                Console.WriteLine(msgMenu);
                userInput = Console.ReadLine();

                try
                {
                    LogicUI.ValidateMenu(userInput, out userInNum);

                    switch (userInNum)
                    {
                        case 0:
                            quit = !quit;
                            break;

                        case 1:
                            addNewCustomerToGarage();
                            break;

                        case 2:
                            showAllLicenseNumberInGarageByFilter();
                            break;

                        case 3:
                            changeCarStatus();
                            break;

                        case 4:
                            inflateWheelsToMax();
                            break;

                        case 5:
                            inflateWheelsByValue();
                            break;

                        case 6:
                            LoadVehicle();
                            break;

                        case 7:
                            printVehicle();
                            System.Threading.Thread.Sleep(5000);
                            break;

                        default:
                            Console.WriteLine("invalid input");
                            break;
                    }
                }
                catch (FormatException formatException)
                {
                    Console.WriteLine(formatException.Message);
                }
                catch (ValueOutOfRangeException valueOutOfRangeException)
                {
                    Console.WriteLine(valueOutOfRangeException.Message);
                }
            }
        }

        private void addNewCustomerToGarage()
        {
            CustomerData newCustomerData = new CustomerData();
            Vehicle vehicle = createNewVehicle();
            if (garageManager.IsVehicleExist(vehicle.LicenseNumber))
            {
                Console.WriteLine("Customer data is already exist");
                garageManager.ResetCarStatus(vehicle.LicenseNumber);
            }
            else
            {
                newCustomerData.m_OwnerInfo = createOwner();
                newCustomerData.m_Vehicle = vehicle;
                garageManager.CustomersData.Add(vehicle.LicenseNumber, newCustomerData);
            }
        }

        private Vehicle createNewVehicle()
        {
            string model, licenseNum, manufacturerName, msgEnergy;
            eVehicleType type;
            List<string> detailsMsg;
            List<string> userInputDetails = new List<string>();
            Vehicle vehicle;
            bool isVehicleCreated, isExtraDetailsVaild;

            vehicle = null;
            isVehicleCreated = false;
            isExtraDetailsVaild = false;
            do
            {
                type = readVehicleType();
                model = readString(createModelNameMsg());
                licenseNum = readString(createLicenseNumberMsg());
                if (!garageManager.IsVehicleExist(licenseNum))
                {
                    manufacturerName = readString(createWheelsManufacturerNameMsg());
                    vehicle = Ex03.GarageLogic.Factory.CreateVehicle(type, model, licenseNum, manufacturerName);
                    detailsMsg = vehicle.Details;
                    vehicle.UpdateTirePressureByValue(readWheeAirPressure(vehicle));
                    msgEnergy = string.Format(@"Please enter how much energy is in vehicle (0-{0}):", vehicle.EnergySource.MaxAmountOfEnergy);
                    vehicle.EnergySource.CurrentAmountOfEnergy = readEnergy(vehicle, msgEnergy);
                    vehicle.CulcPerecentageOfEnergyLeft();
                    do
                    {
                        try
                        {
                            userInputDetails.Clear();
                            foreach (string message in detailsMsg)
                            {
                                Console.WriteLine(message);
                                userInputDetails.Add(Console.ReadLine());
                            }

                            isExtraDetailsVaild = vehicle.isDetailsValid(userInputDetails);
                        }
                        catch (FormatException formatException)
                        {
                            Console.WriteLine(formatException.Message);
                        }
                        catch (ValueOutOfRangeException valueOutOfRangeException)
                        {
                            Console.WriteLine(valueOutOfRangeException.Message);
                        }
                    }
                    while (!isExtraDetailsVaild);

                    isVehicleCreated = true;
                }
                else
                {
                    Console.WriteLine("License number already exist");
                }
            }
            while (!isVehicleCreated);

            return vehicle;
        }

        private OwnerInfo createOwner()
        {
            OwnerInfo newOwner = new OwnerInfo(readString(createOwnerNameMsg()), readPhoneNumber());

            return newOwner;
        }

        private void changeCarStatus()
        {
            string licenseNumber;
            eVehicleStatus newStatus;

            licenseNumber = null;

            try
            {
                garageManager.IsGarageEmpty();
                licenseNumber = getExistedLicenseNumber();
            }
            catch (ArgumentException argumentException)
            {
                Console.WriteLine(argumentException.Message);
            }

            newStatus = readInputStatus();
            garageManager.ChangeCarStatus(licenseNumber, newStatus);
        }

        private string getExistedLicenseNumber()
        {
            string licenseNumber = null;
            bool isValidLicenseNumber = false;
            do
            {
                try
                {
                    licenseNumber = readString(createLicenseNumberMsg());
                    if (garageManager.IsVehicleExist(licenseNumber))
                    {
                        isValidLicenseNumber = true;
                    }
                    else
                    {
                        Console.WriteLine("Vehicle does not exist");
                    }
                }
                catch (ArgumentException argumentException)
                {
                    Console.WriteLine(argumentException.Message);
                }
            }
            while (!isValidLicenseNumber);

            return licenseNumber;
        }

        private void showAllLicenseNumberInGarageByFilter() 
        {
            bool serchFilter, isValid;
            List<string> filterVehicle = new List<string>();

            try
            {
                garageManager.IsGarageEmpty();
                do
                {
                    try
                    {
                        eVehicleStatus filterBy = readVehicleStatus(out serchFilter);
                        isValid = !serchFilter;
                        filterVehicle = garageManager.FilterVehicleByStatus(filterBy, serchFilter);
                        filterVehicle.ForEach(Console.WriteLine);

                    }
                    catch (ArgumentException argumentExecption)
                    {
                        Console.WriteLine(argumentExecption.Message);
                    }
                } 
                while (filterVehicle.Count == 0);
            }
            catch (ArgumentException argumentException)
            {
                Console.WriteLine(argumentException.Message);
            }

        }

        private void inflateWheelsToMax()
        {
            Vehicle vehicle;

            try
            {
                garageManager.IsGarageEmpty();
                vehicle = GetVehicle();
                vehicle.UpdateAllTiresPressureToMax();
            }
            catch (ArgumentException argumentException)
            {
                Console.WriteLine(argumentException.Message);
            }
        }

        private void inflateWheelsByValue()
        {
            Vehicle vehicle;
            float addTirePressure;

            try
            {
                garageManager.IsGarageEmpty();
                vehicle = GetVehicle();
                try
                {
                    vehicle.WheelsCollection[0].IsTirePressureAtMax();
                    addTirePressure = readWheeAirPressure(vehicle);
                    vehicle.UpdateTirePressureByValue(addTirePressure);
                }
                catch (ArgumentException argumentException)
                {
                    Console.WriteLine(argumentException.Message);
                }
            }
            catch (ArgumentException argumentException)
            {
                Console.WriteLine(argumentException.Message);
            }
        }

        private Vehicle GetVehicle()
        {
            Vehicle vehicle;
            string licenseNumber;

            Console.WriteLine("Please enter vehicle's license number: ");
            licenseNumber = Console.ReadLine();
            while (!garageManager.IsVehicleExist(licenseNumber))
            {
                Console.WriteLine("Vehicle does not exist in garage");
                Console.WriteLine("Please enter vehicle's license number: ");
                licenseNumber = Console.ReadLine();
            }

            vehicle = garageManager.GetVehicleByLicenseNumber(licenseNumber);

            return vehicle;
        }

        private eVehicleStatus readInputStatus()
        {
            string msg, userInput;
            eVehicleStatus VehicleStatus;
            bool valid;

            msg = string.Format(
@"choose vehicle status: 
{0}-{1}
{2}-{3}
{4}-{5}", (int)eVehicleStatus.FixInProgress, eVehicleStatus.FixInProgress,
          (int)eVehicleStatus.FixCompleted, eVehicleStatus.FixCompleted,
          (int)eVehicleStatus.Paid, eVehicleStatus.Paid);
            do
            {
                Console.WriteLine(msg);
                userInput = Console.ReadLine();
                valid = ValidateStatus(userInput, out VehicleStatus);
            } 
            while (!valid);

            return VehicleStatus;
        }

        private bool ValidateStatus(string userInput, out eVehicleStatus VehicleStatus)
        {
            bool valid = false;
            VehicleStatus = eVehicleStatus.FixInProgress;

            try
            {
                valid = OwnerInfo.ValidateStatus(userInput, out VehicleStatus);
            }
            catch (FormatException formatException)
            {
                Console.WriteLine(formatException.Message);
            }
            catch (ValueOutOfRangeException valueOutOfRangeException)
            {
                Console.WriteLine(valueOutOfRangeException.Message);
            }

            return valid;
        }

        private eVehicleStatus readVehicleStatus(out bool i_IsFilterChosen)
        {
            const string k_AllVehicle = "0";
            string msg, userInput;
            eVehicleStatus VehicleStatus = eVehicleStatus.FixInProgress;
            bool valid;
            
            msg = createStatusMsg();
            do
            {
                Console.WriteLine(msg);
                userInput = Console.ReadLine();
                i_IsFilterChosen = !userInput.Equals(k_AllVehicle);
                valid = !i_IsFilterChosen;

                if (i_IsFilterChosen)
                {
                    valid = ValidateStatus(userInput, out VehicleStatus);
                }
            } 
            while (!valid);

            return VehicleStatus;
        }

        private static string createStatusMsg()
        {
            return string.Format(
@"choose vehicle status:
0-All Vehicle 
{0}-{1}
{2}-{3}
{4}-{5}", (int)eVehicleStatus.FixInProgress, eVehicleStatus.FixInProgress,
          (int)eVehicleStatus.FixCompleted, eVehicleStatus.FixCompleted,
          (int)eVehicleStatus.Paid, eVehicleStatus.Paid);
        }

        private eVehicleType readVehicleType()
        {
            string userInput;
            StringBuilder msgType;
            bool validType;
            eVehicleType vehicleType;

            validType = false;
            vehicleType = eVehicleType.Car;
            msgType = createTypeMsg();
            do
            {
                Console.Write(msgType);
                userInput = Console.ReadLine();

                try
                {
                    validType = Factory.ValidateType(userInput, out vehicleType);
                }
                catch (FormatException formatException)
                {
                    Console.WriteLine(formatException.Message);
                }
                catch (ValueOutOfRangeException valueOutOfRangeException)
                {
                    Console.WriteLine(valueOutOfRangeException.Message);
                }
            } 
            while (!validType);

            return vehicleType;
        }

        private StringBuilder createTypeMsg()
        {
            StringBuilder msgType = new StringBuilder();
            int vehicleIndex;
            string msg;

            vehicleIndex = 1;
            msgType.AppendLine("Please enter vehicle type:");
            foreach (string vehicleType1 in Enum.GetNames(typeof(eVehicleType)))
            {
                msg =
                       string.Format(
                           "{0}-{1}",
                       vehicleIndex, vehicleType1);
                vehicleIndex++;
                msgType.AppendLine(msg);
            }

            return msgType;
        }

        private string readString(string i_Msg)
        {
            string userInput = null;
            bool isStringEmpty = true;
            do
            {
                try
                {
                    Console.WriteLine(i_Msg);
                    userInput = Console.ReadLine();
                    isStringEmpty = LogicUI.IsStringEmpty(userInput);
                }
                catch (ArgumentException argumentException)
                {
                    Console.WriteLine(argumentException.Message);
                }
            }
            while (isStringEmpty);

            return userInput;
        }

        private float readWheeAirPressure(Vehicle i_Vehicle)
        {
            string userInput;
            float airPressure;
            Wheel wheel;
            bool isAirPressureValid;

            airPressure = -1;
            isAirPressureValid = false;
            wheel = i_Vehicle.WheelsCollection[0];
            do
            {
                try
                {
                    Console.WriteLine(@"Please enter wheels air pressure (0-{0}):", (wheel.MaxTirePressure - wheel.CurrentTirePressure));
                    userInput = Console.ReadLine();
                    isAirPressureValid = wheel.ValidateTirePressure(userInput, out airPressure);
                }
                catch (FormatException formatException)
                {
                    Console.WriteLine(formatException.Message);
                }
                catch (ValueOutOfRangeException valueOutOfRangeException)
                {
                    Console.WriteLine(valueOutOfRangeException.Message);
                }
            } 
            while (!isAirPressureValid);

            return airPressure;
        }

        private float readEnergy(Vehicle i_Vehicle, string msg)
        {
            string userInput;
            float amountToLoad;
            bool validEnergy;

            amountToLoad = 0;
            validEnergy = false;
            do
            {
                Console.WriteLine(msg);
                userInput = Console.ReadLine();
                try
                {
                    validEnergy = i_Vehicle.EnergySource.IsValidAmountOfEnergy(userInput, out amountToLoad);
                }
                catch (FormatException formatException)
                {
                    Console.WriteLine(formatException.Message);
                }
                catch (ValueOutOfRangeException valueOutOfRangeException)
                {
                    Console.WriteLine(valueOutOfRangeException.Message);
                }
            }
            while (!validEnergy);

            return amountToLoad; 
        }

        private string readPhoneNumber()
        {
            string userInput;
            bool isNumberValid;

            userInput = null;
            isNumberValid = false;
            do
            {
                try
                {
                    Console.WriteLine("Please enter phone number (First digit must be 0):");
                    userInput = Console.ReadLine();
                    isNumberValid = OwnerInfo.ValidatePhoneNumber(userInput);
                }
                catch (FormatException formatException)
                {
                    Console.WriteLine(formatException.Message);
                }
                catch (ArgumentException argumentException)
                {
                    Console.WriteLine(argumentException.Message);
                }
            }
            while (!isNumberValid);

            return userInput;
        }

        private string createModelNameMsg()
        {
            return "Please enter model name:";
        }

        private string createOwnerNameMsg()
        {
            return "Please enter owner name:";
        }

        private string createLicenseNumberMsg()
        {
            return "Please enter license number:";
        }

        private string createWheelsManufacturerNameMsg()
        {
            return "Please enter wheel manufacturer name:";
        }

        private float getAmountToLoad(Vehicle i_Vehicle)
        {
            string msg;
            float amountToLoad;

            msg = string.Format(@"Please enter the amount Of energy to load 0-{0}",
                i_Vehicle.EnergySource.MaxAmountOfEnergy - i_Vehicle.EnergySource.CurrentAmountOfEnergy);
            amountToLoad = readEnergy(i_Vehicle, msg);

            return amountToLoad;
        }

        private Vehicle getVehicleToLoad(out float i_Amount)
        {
            Vehicle vehicle;
            string licensNum;

            licensNum = getExistLicensNum();
            vehicle = garageManager.GetVehicleByLicenseNumber(licensNum);
            i_Amount = 0;

            try
            {
                vehicle.EnergySource.IsCurrAmountIsMax();
                i_Amount = getAmountToLoad(vehicle);
                Fuel fuelVehicle = vehicle.EnergySource as Fuel;

                if (fuelVehicle != null)
                {
                    readMachFuel(fuelVehicle);
                }
            }
            catch (ArgumentException argumentException)
            {
                Console.WriteLine(argumentException.Message);
            }

            return vehicle;
        }

        private void printVehicle()
        {
            string licensNum;
            CustomerData customer;

            try
            {
                garageManager.IsGarageEmpty();
                licensNum = getExistLicensNum();
                customer = garageManager.GetCustomerDataByLicensNumber(licensNum);
                Console.WriteLine(customer.ToString());
            }
            catch (ArgumentException argumentException)
            {
                Console.WriteLine(argumentException.Message);
            }
        }

        string getExistLicensNum()
        {
            string i_LicensNum = readString(createLicenseNumberMsg());
            while (!garageManager.IsVehicleExist(i_LicensNum))
            {
                Console.WriteLine("Vehicle is not in garage");
                i_LicensNum = readString(createLicenseNumberMsg());
            }

            return i_LicensNum;
        }

        private void LoadVehicle()
        {
            float amount;

            try
            {
                garageManager.IsGarageEmpty();
                Vehicle vehicle = getVehicleToLoad(out amount);
                vehicle.EnergySource.LoadEnergySource(amount);
                vehicle.CulcPerecentageOfEnergyLeft();
            }
            catch (ArgumentException argumentException)
            {
                Console.WriteLine(argumentException.Message);
            }
        }

        eFuelType readMachFuel(Fuel fuelVehicle)
        {
            bool isMatch;
            eFuelType fuelType;

            isMatch = false;
            do 
            {
                fuelType = readFuleType();
                try
                {
                    isMatch = fuelVehicle.IsFuelMatchVehicle(fuelType);
                }
                catch (ArgumentException argumentExecption)
                {
                    Console.WriteLine(argumentExecption.Message);
                }
            }
            while (!isMatch);

            return fuelType;
        }

        private eFuelType readFuleType()
        {
            string userInput;
            eFuelType fuelType;
            StringBuilder msgType;
            bool validType;

            validType = false;
            fuelType = eFuelType.Soler;
            msgType = createFuelMsg();
            do
            {
                Console.Write(msgType);
                userInput = Console.ReadLine();
                try
                {
                    validType = Fuel.ValidteType(userInput, out fuelType);
                }
                catch (FormatException formatException)
                {
                    Console.WriteLine(formatException.Message);
                }
                catch (ValueOutOfRangeException valueOutOfRangeException)
                {
                    Console.WriteLine(valueOutOfRangeException.Message);
                }
            }
            while (!validType);

            return fuelType; 
        }

        private StringBuilder createFuelMsg()
        {
            string msg;
            int vehicleIndex;
            StringBuilder msgType;

            vehicleIndex = 1;
            msgType = new StringBuilder();
            msgType.AppendLine("Please enter Fuel type:");
            foreach (string vehicleType1 in Enum.GetNames(typeof(eFuelType)))
            {
                msg =
                       string.Format(
                           @"{0}-{1}",
                       vehicleIndex, vehicleType1);
                vehicleIndex++;
                msgType.AppendLine(msg);
            }

            return msgType;
        }
    }
}
