using GTERP.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Models
{
    public class Warehouse : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WarehouseId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "WH Name")]
        public string WhName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "WH Short Name")]
        public string WhShortName { get; set; }

        [StringLength(1, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Ware House Type")]
        public string WhType { get; set; }

        [ForeignKey("Warehouses")]
        public Nullable<int> ParentId { get; set; }
        [Display(Name = "Warehouse Name")]
        public virtual Warehouse Warehouses { get; set; }

        [Display(Name = "Is Main Warehouse")]
        public bool IsMainWarehouse { get; set; }

        [Display(Name = "Is Sub Warehouse")]
        public bool IsSubWarehouse { get; set; }

        [Display(Name = "Is Medical Warehouse")]
        public bool IsMedicalWarehouse { get; set; }

        [Display(Name = "Is Production Warehouse")]
        public bool IsProductionWarehouse { get; set; }

        [Display(Name = "Is Consumable Warehouse")]
        public bool IsConsumableWarehouse { get; set; }


        //[StringLength(128)]
        //public string comid { get; set; }


        //[StringLength(128)]
        //public string userid { get; set; }

        //[StringLength(128)]
        //[Display(Name = "Update By")]

        //public string useridUpdate { get; set; }

        //public Nullable<System.DateTime> DateAdded { get; set; }
        //public Nullable<System.DateTime> DateUpdated { get; set; }

        public virtual ICollection<SalesSub> vWarehouseSalesSubs { get; set; }



    }






}