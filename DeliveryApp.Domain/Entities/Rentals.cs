namespace DeliveryApp.Domain.Entities
{
    public class Rentals
    {
        public string Identifier { get; }
        public int PlanType { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; private set; }
        public DateTime PrevisionTime { get; }
        public string MotorcycleId { get; }
        public string DriverId { get; }
        public decimal Price { get; set; }
        public decimal? AdditionalFee { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedTime { get; } = DateTime.UtcNow.ToUniversalTime();

        // Navigation properties
        public Motorcycle Motorcycle { get; set; } = null!;
        public Driver Driver { get; set; } = null!;

        public Rentals(
            string identifier,
            int planType,
            DateTime startTime,
            DateTime endTime,
            DateTime previsionTime,
            string motorcycleId,
            string driverId,
            decimal price)
        {
            Identifier = identifier;
            PlanType = planType;
            StartTime = startTime;
            EndTime = endTime;
            PrevisionTime = previsionTime;
            MotorcycleId = motorcycleId;
            DriverId = driverId;
            Price = price;
            IsActive = true;
        }

        public void SetAdditionalFee(decimal additionalFee)
        {
            AdditionalFee = additionalFee;
        }

        public void ReturnRental(DateTime endTime)
        {
            EndTime = endTime;
            IsActive = false;
        }
    }
}
