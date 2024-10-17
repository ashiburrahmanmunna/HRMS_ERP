using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{

    public partial class Temp_COM_MasterLC_Master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int SL { get; set; }
        public string Company { get; set; }

        public string Contract { get; set; }
        public string Buyer { get; set; }
        public Nullable<DateTime> LCIssueDate { get; set; }

        public string OpeningBank { get; set; }
        public string BankAccountNo { get; set; } ///ned to add excel file for upload

        public string ConsigneeBank { get; set; }

        public string Tolerance { get; set; }
        public string ShippingAgent { get; set; }
        public string Shipmode { get; set; }
        public string Incoterm { get; set; }
        public string PortOfLoading { get; set; }
        public string PortOfDischarge { get; set; }

        public string PortOfDestination { get; set; }
        public string LCQty { get; set; }
        public string LCValue { get; set; }
        public string Curency { get; set; }
        public string Insurance { get; set; }
        public Nullable<DateTime> LastShipDate { get; set; }
        public string PaymentTerm { get; set; }
        public string AccountNo { get; set; }
        public string Remarks1 { get; set; }
        public string Remarks2 { get; set; }
        public string Remarks3 { get; set; }
        public string Remarks4 { get; set; }
        public string Remarks5 { get; set; }

        public string userid { get; set; }


    }


    public partial class Temp_COM_MasterLC_Detail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int SL { get; set; }
        public string PONo { get; set; }
        public string Style { get; set; }
        public string ItemName { get; set; }
        public string HSCode { get; set; }
        public string Fabrication { get; set; }
        public float OrderQty { get; set; }
        public string OrderUOM { get; set; }
        public float Factor { get; set; }
        public int QuantityPcs { get; set; }
        public string UOM { get; set; }
        public float UnitPrice { get; set; }
        public float TotalValue { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        //public Nullable<DateTime> ShipDate { get; set; }
        public string ShipDate { get; set; }

        public string Destination { get; set; }
        public string userid { get; set; }


        public string ContractNo { get; set; }
        public string OrderType { get; set; }
        public string CatNo { get; set; }
        public string DeliveryNo { get; set; }

        public string DestinationPO { get; set; }
        public string Kimball { get; set; }
        public string ColorCode { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        //public Nullable<DateTime> ShipDate { get; set; }
        public string ContractDate { get; set; }

    }


    public partial class Temp_COM_ProformaInvoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int SL { get; set; }

        public string PINo { get; set; }


        public Nullable<DateTime> PIDate { get; set; }

        public string Company { get; set; }
        public string Currency { get; set; }
        public string Supplier { get; set; }
        public string ImportPONo { get; set; }
        public string FileNo { get; set; }

        public string LCAF { get; set; }
        public string ItemGroupName { get; set; }
        public string ItemDescription { get; set; }
        public string ImportQty { get; set; }
        public string Unit { get; set; }

        public string ImportRate { get; set; }
        public string TotalValue { get; set; }


        public string Remarks1 { get; set; }
        public string Remarks2 { get; set; }
        public string Remarks3 { get; set; }


        public string userid { get; set; }


    }
}