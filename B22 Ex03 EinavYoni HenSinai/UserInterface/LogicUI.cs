using System;
using Ex03.GarageLogic;

namespace Ex03.ConsoleUI
{
    class LogicUI
    {
        public static bool IsStringEmpty(string i_InputVehicleType)
        {
            bool isStringEmpty = i_InputVehicleType.Length == 0;

            if (isStringEmpty)
            {
                throw new ArgumentException("Empty string is invalid");
            }

            return isStringEmpty;
        }

        public static bool ValidateMenu(string i_UserInput, out int i_NumInput)
        {
            const int k_MaxMenuOption = 8;
            bool validInput = int.TryParse(i_UserInput, out i_NumInput);

            if (!validInput)
            {
                throw new FormatException("invalid format");
            }

            validInput = i_NumInput >= 0 && i_NumInput < k_MaxMenuOption;
            if (!validInput)
            {
                throw new ValueOutOfRangeException(0, k_MaxMenuOption);
            }

            return validInput;
        }
    }
}
