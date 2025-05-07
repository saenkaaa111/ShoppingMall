namespace ShoppingMall.Data
{
    public class Visitor
    {
        public Guid Id { get; set; }
        public DateTime EntryTime { get; set; }
        public DateTime? ExitTime { get; set; }
        public Gateway EntryGate { get; set; }
        public Gateway? ExitGate { get; set; }
    }
}
