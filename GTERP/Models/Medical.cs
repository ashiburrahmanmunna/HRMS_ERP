using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{
    public partial class Medical_Master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MedicalMasterId { get; set; }

        [Display(Name = "Employee")]
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual HR_Emp_Info HR_Emp_Info { get; set; }

        //[Display(Name = "Plant")]
        //public int PrdUnitId { get; set; }
        //[ForeignKey("PrdUnitId")]
        //public virtual PrdUnit PrdUnit { get; set; }

        //[Display(Name = "Designation")]
        //public int DesigId { get; set; }
        //[ForeignKey("DesigId")]
        //public virtual Cat_Designation Cat_Designation { get; set; }

        //[Display(Name = "Section")]
        //public int SectId { get; set; }
        //[ForeignKey("SectId")]
        //public virtual Cat_Section Cat_Section { get; set; }

        //[Display(Name = "Unit")]
        //public int UnitId { get; set; }
        //[ForeignKey("UnitId")]
        //public virtual Cat_Unit Cat_Unit { get; set; }



        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtInput { get; set; }



        [Display(Name = "Doctor")]
        public int DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public virtual HR_Emp_Info Doctor { get; set; }

        [StringLength(50)]
        public string Patient { get; set; }

        public float Weight { get; set; }
        public float Pulse { get; set; }
        [StringLength(10)]
        public string BP { get; set; }

        [StringLength(300)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Note")]
        public string Advice { get; set; }

        [StringLength(80)]
        public string ComId { get; set; }

        [StringLength(80)]
        public string UserId { get; set; }

        public DateTime? DateAdded { get; set; }

        [StringLength(80)]
        public string UpdateByUserId { get; set; }

        public DateTime? DateUpdated { get; set; }

        public virtual List<Medical_Details> Medical_Detailses { get; set; }
    }

    public partial class Medical_Details
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MedicalDetaisId { get; set; }

        public int MedicalMasterId { get; set; }
        [ForeignKey("MedicalMasterId")]
        public virtual Medical_Master Medical_Master { get; set; }

        [Display(Name = "Medicine Name")]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [Display(Name = "Medicine Name")]
        [StringLength(30)]
        public string MedicineName { get; set; }

        [Display(Name = "Patient")]
        [StringLength(30)]
        public string Patient { get; set; }


        [StringLength(10)]
        public string UOM { get; set; }

        public float Quantity { get; set; }

        [StringLength(100)]
        public string Remarks { get; set; }

        [StringLength(80)]
        public string ComId { get; set; }

        [StringLength(80)]
        public string UserId { get; set; }

        public DateTime? DateAdded { get; set; }

        [StringLength(80)]
        public string UpdateByUserId { get; set; }

        public DateTime? DateUpdated { get; set; }
    }

    public partial class ReorderLevel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReorderLevelId { get; set; }

        public int WarehouseId { get; set; }
        [ForeignKey("WarehouseId")]
        public virtual Warehouse Warehouse { get; set; }

        [Display(Name = "Product Name")]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public float Quantity { get; set; }

        [NotMapped]
        public float StockQty { get; set; }

        [StringLength(100)]
        public string Remarks { get; set; }

        public bool IsActive { get; set; }

        [StringLength(80)]
        public string ComId { get; set; }

        [StringLength(80)]
        public string UserId { get; set; }

        public DateTime? DateAdded { get; set; }

        [StringLength(80)]
        public string UpdateByUserId { get; set; }

        public DateTime? DateUpdated { get; set; }
    }

    public partial class ReorderLevelViewModel
    {

        public int ReorderLevelId { get; set; }

        public int WarehouseId { get; set; }
        [Display(Name = "Warehouse")]
        public string WHName { get; set; }


        [Display(Name = "Product")]
        public string ProductName { get; set; }

        public int ProductId { get; set; }

        public float Quantity { get; set; }

        public decimal StockQty { get; set; }

        public string Remarks { get; set; }

        public bool IsActive { get; set; }

    }



}
