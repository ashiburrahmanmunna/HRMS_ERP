using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using GTERP.Models.Base;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace GTERP.Models
{
    public class Tax_ClientInfo : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClientId { get; set; }
        [Column(TypeName = "nvarchar(80)")]
        [Display(Name = "Client Name")]
        public string ClientName { get; set; }
        [Display(Name = "Code")]
        [StringLength(20)]
        public string ClientCode { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        [Display(Name = "Address")]
        public string ClientAddress { get; set; }
        [Display(Name = "Designation")]
        [Column(TypeName = "nvarchar(80)")] 
        public string Desigid { get; set; }
        [Display(Name = "Email")]
        [Column(TypeName = "nvarchar(30)")]
        public string ClientEmail { get; set; }
        [Display(Name = "Phone Number")]
        [StringLength(25)]
        public string? PhoneNumber { get; set; }
        [Display(Name = "Tin Number")]
        [StringLength(30)]
        public string? ClinetTinNo { get; set; }
        [Column(TypeName = "nvarchar(40)")]
        [Display(Name = "Tax Circle")]
        public string? ClinetTaxCircle { get; set; }
        [Display(Name = "Contact Start Date")]
        public DateTime dtContStar { get; set; }
        [Display(Name = "Company Name")]
        public int ClientCompId { get; set; }
        [ForeignKey("ClientCompId")]
        public virtual Tax_ClientCompany ClientComp { get; set; }
        }
        public class Tax_ClientContactPayment : BaseModel
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int ClientPaymentId { get; set; }
            [Display(Name = "Tax Year")]
            [Column(TypeName = "nvarchar(20)")]
            public string FiscalYearId { get; set; }
            [Display(Name = "Contract Payment")]
            public float ContractPayment { get; set; }
            [Display(Name = "Client Name")]
            public int ClientId { get; set; }
            [ForeignKey("ClientId")]
            public virtual Tax_ClientInfo Tax_ClientInfo { get; set; }

            [NotMapped]
            public string ClientName { get; set; }
            [NotMapped]
            public string ClientCode { get; set; }
    }
        public class Tax_ClientCompany : BaseModel
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int ClientComId { get; set; }
            [StringLength(80)]
            [Display(Name = "Company Name")]
            public string ClientComName { get; set; }
            [StringLength(250)]
            [Display(Name = "Address")]
            public string ClientComAddress { get; set; }
            [StringLength(30)]
            [Display(Name = "Email Address")]
            public string? ClientComEmail { get; set; }
            [StringLength(25)]
            [Display(Name = "Phone Number")]
            public string? ClientComPhone { get; set; }
            [StringLength(30)]

            [Display(Name = "Company Type")]
            public string ClientComCompanyType { get; set; }
        }
        public class Tax_ClientTaxInfo : BaseModel
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int ClientTaxInfoId { get; set; }
            [Column(TypeName = "nvarchar(20)")]
            [Display(Name = "Tax Year")]
            public string FiscalYearId { get; set; }
            [StringLength(5)]
            [Display(Name = "Return Submit")]
            public string IsSubmit { get; set; }
            [Display(Name = "Return Sub. Date")]
            public DateTime DtSubmission { get; set; }
            [StringLength(5)]
            [Display(Name = "Acknowledgement Slip Receive")]
            public string IsAckSlipRcv { get; set; }
            [Display(Name = "Acknowledgement Slip Receive Date")]
            public DateTime DtAckSlipReceive { get; set; }
            [StringLength(5)]
            [Display(Name = "Tax Certificate Receive")]
            public string IsTaxCerRcv { get; set; }
            [Display(Name = "Tax Certificate Receive Date")]
            public DateTime DtTaxCerRcv { get; set; }
            [StringLength(5)]
            [Display(Name = "Tax Extension")]
            public string IsTexExtension { get; set; }
            [Display(Name = "Tax Extension Date")]
            public DateTime DtTexExtension { get; set; }
            [Display(Name = "Client Name")]
            public int ClientId { get; set; }
            [ForeignKey("ClientId")]
            public virtual Tax_ClientInfo Tax_ClientInfo { get; set; }
            [NotMapped]
            public string ClientName { get; set; }
            [NotMapped]
            public string ClientCode { get; set; }
    }
        public class Tax_PaymentReceived : BaseModel
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int PaymentReceiveId { get; set; }
            [Display(Name = "Tax Year")]
            [Column(TypeName = "nvarchar(20)")]
            public string FiscalYearId { get; set; }
            [Display(Name = "Payment Receive Date")]
            public DateTime DtPaymentReceive { get; set; }
            [Display(Name = "Payment Amount")]
            public float PaymentAmt { get; set; }
            [Column(TypeName = "nvarchar(250)")]
            [Display(Name = "Remarks")]
            public string PaymentRemarks { get; set; }
            [Display(Name = "Client Name")]
            public int ClientId { get; set; }
            [ForeignKey("ClientId")]
            public virtual Tax_ClientInfo Tax_ClientInfo { get; set; }
    }
        public class Tax_Costing : BaseModel
        {
           [Key]
           [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
           public int CostingID { get; set; }
           [Display(Name = "Input Date")]
            public DateTime DtCostingInput { get; set; }
            [Display(Name = "Cost Amount")]
            public float CostAmount { get; set; }
            [Display(Name = "Remarks")]
            [Column(TypeName = "nvarchar(250)")]
            public string CostRemarks { get; set; }

        }
        public class Tax_DocumentInfo : BaseModel
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int DocumentInfoId { get; set; }
            [Column(TypeName = "nvarchar(20)")]
            [Display(Name = "Tax Year")]
            public string FiscalYearId { get; set; }

            [StringLength(25)]
            [Display(Name = "Document Type")]
            public string VarType { get; set; }

            [StringLength(100)]
            public string Title { get; set; }

            [Display(Name = "File")]
            public String FileName { get; set; }
            
            [Display(Name = "ClientName")]
            public int ClientId { get; set; }
            [ForeignKey("ClientId")]
            public virtual Tax_ClientInfo Tax_ClientInfo { get; set; }
            [NotMapped]
            public List<IFormFile>? FileToUpload { get; set; }
            [NotMapped]
            public List<string>? imageName { get; set; }

            [NotMapped]
            public string ClientName { get; set; }
            [NotMapped]
            public string ClientCode { get; set; }

    }
}

