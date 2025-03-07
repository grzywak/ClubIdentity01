namespace Club.Models
{
    public class Equipment
    {
        public int EquipmentID { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public DateTime PurchaseDate { get; set; }

        public ICollection<SessionEquipment> SessionEquipments { get; set; } = new List<SessionEquipment>();
        public ICollection<Maintenance> Maintenances { get; set; } = new List<Maintenance>();
    }
}
