namespace GTERP.ViewModels
{
    public class CommercialVM
    {
        class req
        {
            public int Id { get; set; }
            public string OrderId { get; set; }
            public string Password { get; set; }
        }

        public class AspnetUserList
        {
            public string UserId { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
        }

        public class PIDailyReceivingModel
        {
            public string FileNo { get; set; }
            public string Factory { get; set; }
            public string SupplierName { get; set; }
            public string PINo { get; set; }
            public string OrderNo { get; set; }
            public decimal ImportQty { get; set; }
            public string UnitName { get; set; } ///TotalValue	ReceivingDate	PIStatus
            public decimal TotalValue { get; set; }
            public string ReceivingDate { get; set; }
            public string PIStatus { get; set; }
            public string BBLCNo { get; set; }
            public string StatusColor { get; set; }
            public string LcOpeningDate { get; set; }
        }
    }
}
