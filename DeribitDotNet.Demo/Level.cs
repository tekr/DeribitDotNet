namespace DeribitDotNet.Demo
{
    public readonly struct Level
    {
        public readonly double Price;
        public readonly int Quantity;

        public Level(double price, int quantity)
        {
            Price = price;
            Quantity = quantity;
        }

        public override string ToString() => $"{Quantity}@{Price}";

        public override bool Equals(object obj) =>
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            obj is Level other && Price == other.Price && Quantity == other.Quantity;

        public override int GetHashCode() => (Price.GetHashCode() * 397) ^ Quantity.GetHashCode();
    }
}