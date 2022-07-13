namespace Ex03.GarageLogic
{
    public class Battery : EnergySource
    {
        public Battery(float i_MaxAmountOfEnergy) : base(i_MaxAmountOfEnergy) { }

        public override string ToString()
        {
            string details, baseDetails;

            baseDetails = base.ToString();
            details = string.Format(
@"{0}
{1}",
eEnergySource.Battery, baseDetails);

            return details;
        }
    }
}
