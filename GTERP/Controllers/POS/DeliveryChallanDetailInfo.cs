namespace GTERP.Controllers.POS
{
    internal class DeliveryChallanDetailInfo
    {

        public int Id { get; set; }
        public int DeliveryChallanId { get; set; }
        public int GatePassSubId { get; set; }
        public int FiscalMonthId { get; set; }
        public string CustomerCode { get; set; }
        public int FYId { get; set; }
        public int distid { get; set; }
        public int PStationId { get; set; }
        public int TruckLoadQty { get; set; }
        public string FyName { get; set; }
        public string MonthName { get; set; }
        public int DONo { get; set; }

        public int ChallanNo { get; set; }
        public string CustomerName { get; set; }
        public string DistName { get; set; }
        public string PStationName { get; set; }
        public decimal BalanceQty { get; set; }

        public decimal DeliveryQty { get; set; }
        public decimal SLNo { get; set; }

        public string RepresentativeName { get; set; }



    }
}