using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTERP.Controllers.Device.Model
{
    public class ZkModel
    {
    }
    public class empData
    {
        public string? cardNo { get; set; }
        public string? empName { get; set; }
        public int? userGroup { get; set; }
    }
    public class viewModel
    {
        public string? EmpCode { get; set; }
        public string? EmpName { get; set; }
        public string? CardNo { get; set; }
        public string? ComId { get; set; }
        public string? desigName { get; set; }
        public string? floor { get; set; }
        public string? line { get; set; }
        public string? DeptName { get; set; }
        public string? SectName { get; set; }
        public string? IpAddress { get; set; }
        public byte[]? EmpImage { get; set; }
        public string? emp_image { get; set; }
        public byte[]? FingerData { get; set; }
        public string? finger_Data { get; set; }
        public int? indexNo { get; set; }
        public string sPassword { get; set; }
        public int? iPrivilege { get; set; }
        public bool? bEnabled { get; set; }
        public int? userGroup { get; set; }

    }

    public class EventViewModel
    {
        public string deviceNo { get; set; }
        public string cardNo { get; set; }

        public string date { get; set; }
        public string time { get; set; }
    }


    public class HR_MachineNoZKT
    {
        [Key]
        public Guid Id { get; set; }
        public string? ComId { get; set; }
        public string? Location { get; set; }
        public string? IsActive { get; set; }
        public string? PortNo { get; set; }
        public short? LuserId { get; set; }
        public string? Pcname { get; set; }
        public string? IpAddress { get; set; }
        public string? ZKtUser { get; set; }
        public string? ZKtPassword { get; set; }
        public string? Status { get; set; }
        public string? Inout { get; set; }

    }

    //public class HR_Emp_DeviceInfoHIK
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public Guid id { get; set; }
    //    public string comId { get; set; }
    //    public string EmpName { get; set; }
    //    public string CardNo { get; set; }
    //    public byte[] EmpImage { get; set; }
    //    public byte[] FingerData { get; set; }
    //    public string IpAddress { get; set; }

    //}
    [Table("HR_Emp_DeviceInfoHIK")]
    public class HR_Emp_DeviceInfoHIK
    {
        [Key]
        public Guid id { get; set; }
        public string? comId { get; set; }
        public string? empName { get; set; }
        public string? cardNo { get; set; }
        public byte[]? empImage { get; set; }
        public byte[]? fingerData { get; set; }
        public string? IpAddress { get; set; }
        public string? DeviceName { get; set; }
        public long? userGroup { get; set; }

    }
}
