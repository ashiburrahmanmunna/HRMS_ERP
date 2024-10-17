using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HVFaceManagement.Models
{
    //public class empData
    //{
    //    public string CardNo { get; set; }
    //    public string EmpName { get; set; }
    //}

    //public class viewModel
    //{
    //    public string CardNo { get; set; }
    //    public string EmpCode { get; set; }
    //    public string EmpName { get; set; }
    //    public byte[] EmpImage { get; set; }
    //    public string emp_image { get; set; }
    //    public byte[] FingerData { get; set; }
    //    public string finger_data { get; set; }
    //    public string ComId { get; set; }

    //    public string DesigName { get; set; }

    //    public string DeptName { get; set; }
    //    public string SectName { get; set; }
    //    public string IpAddress { get; set; }

    //}
    public class HR_MachineNoHIK
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string ComId { get; set; }
        public string Location { get; set; }
        public string IsActive { get; set; }
        public string PortNo { get; set; }
        public short? LuserId { get; set; }
        public string Pcname { get; set; }
        public string IpAddress { get; set; }
        public string Hikuser { get; set; }
        public string Hikpassword { get; set; }
        public string Status { get; set; }
        public string InOut { get; set; }

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
    //    public string? DeviceName { get; set; }
    //    public long? userGroup { get; set; }


    //}
    public class ResponseStatus
    {
        public string requestURL { get; set; }
        public string statusCode { get; set; }
        public string statusString { get; set; }
        public string subStatusCode { get; set; }
        public string errorCode { get; set; }
        public string errorMsg { get; set; }
    }

    public class MatchListItem
    {



        public string FPID { get; set; }



        public string faceURL { get; set; }



        public string modelData { get; set; }
    }

    public class Root
    {



        public string requestURL { get; set; }



        public int statusCode { get; set; }



        public string statusString { get; set; }



        public string subStatusCode { get; set; }



        public string responseStatusStrg { get; set; }



        public int numOfMatches { get; set; }



        public int totalMatches { get; set; }



        public List<MatchListItem> MatchList { get; set; }
    }

    public class Valid
    {



        public string enable { get; set; }



        public string beginTime { get; set; }



        public string endTime { get; set; }



        public string timeType { get; set; }
    }
    public class RightPlanItem
    {



        public int doorNo { get; set; }



        public string planTemplateNo { get; set; }
    }
    public class UserInfoItem
    {



        public string employeeNo { get; set; }



        public string name { get; set; }



        public string userType { get; set; }



        public string closeDelayEnabled { get; set; }



        public Valid Valid { get; set; }



        public string belongGroup { get; set; }



        public string password { get; set; }



        public string doorRight { get; set; }



        public List<RightPlanItem> RightPlan { get; set; }



        public int maxOpenDoorTime { get; set; }



        public int openDoorTime { get; set; }



        public string userVerifyMode { get; set; }
    }

    public class UserInfoSearch
    {



        public string searchID { get; set; }



        public string responseStatusStrg { get; set; }



        public int numOfMatches { get; set; }



        public int totalMatches { get; set; }



        public List<UserInfoItem> UserInfo { get; set; }
    }

    public class UserInfoSearchRoot
    {



        public UserInfoSearch UserInfoSearch { get; set; }
    }

    public class FaceLib
    {



        public string requestURL { get; set; }




        public int statusCode { get; set; }




        public string statusString { get; set; }




        public string subStatusCode { get; set; }




        public int errorCode { get; set; }




        public string errorMsg { get; set; }




        public string faceLibType { get; set; }




        public string name { get; set; }




        public string customInfo { get; set; }

    }

    public class StatusListItem
    {



        public int id { get; set; }



        public int cardReaderRecvStatus { get; set; }



        public string errorMsg { get; set; }
    }

    public class FingerPrintStatus
    {



        public List<StatusListItem> StatusList { get; set; }



        public int totalStatus { get; set; }
    }

    public class FPRoot
    {



        public FingerPrintStatus FingerPrintStatus { get; set; }
    }

    public class FingerPrintListItem
    {



        public int cardReaderNo { get; set; }



        public int fingerPrintID { get; set; }



        public string fingerType { get; set; }



        public string fingerData { get; set; }



        public List<string> leaderFP { get; set; }
    }

    public class FingerPrintInfo
    {



        public string searchID { get; set; }



        public string status { get; set; }



        public List<FingerPrintListItem> FingerPrintList { get; set; }
    }

    public class FPInfo
    {



        public FingerPrintInfo FingerPrintInfo { get; set; }
    }


    public class FingerPrintDeleteProcess
    {



        public string status { get; set; }
    }

    public class FPDel
    {



        public FingerPrintDeleteProcess FingerPrintDeleteProcess { get; set; }
    }

    public class InfoListItem
    {
        public int major { get; set; }

        public int minor { get; set; }

        public string time { get; set; }

        public string netUser { get; set; }

        public string remoteHostAddr { get; set; }

        public string cardNo { get; set; }

        public int cardType { get; set; }

        public int whiteListNo { get; set; }

        public int reportChannel { get; set; }

        public int cardReaderKind { get; set; }

        public int cardReaderNo { get; set; }

        public int doorNo { get; set; }

        public int verifyNo { get; set; }

        public int alarmInNo { get; set; }

        public int alarmOutNo { get; set; }

        public int caseSensorNo { get; set; }

        public int RS485No { get; set; }

        public int multiCardGroupNo { get; set; }

        public int accessChannel { get; set; }

        public int deviceNo { get; set; }

        public int distractControlNo { get; set; }

        public string employeeNoString { get; set; }

        public int localControllerID { get; set; }

        public int InternetAccess { get; set; }

        public int type { get; set; }

        public string MACAddr { get; set; }

        public int swipeCardType { get; set; }

        public int serialNo { get; set; }

        public int channelControllerID { get; set; }

        public int channelControllerLampID { get; set; }

        public int channelControllerIRAdaptorID { get; set; }

        public int channelControllerIREmitterID { get; set; }

        public string userType { get; set; }

        public string currentVerifyMode { get; set; }

        public string filename { get; set; }

    }

    public class EventViewModel
    {
        public string deviceNo { get; set; }
        public string cardNo { get; set; }

        public string date { get; set; }
        public string time { get; set; }
    }


    public class AcsEvent
    {
        public string searchID { get; set; }

        public string responseStatusStrg { get; set; }

        public int numOfMatches { get; set; }

        public int totalMatches { get; set; }

        public List<InfoListItem> InfoList { get; set; }

    }

    public class AcsEventObject
    {
        public AcsEvent AcsEvent { get; set; }
    }


}
