using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

using GTERP.Controllers.Device.Model;

namespace StandaloneSDKDemo
{
    public class SDKHelper
    {
        public zkemkeeper.CZKEMClass axCZKEM1 = new zkemkeeper.CZKEMClass();
        //public static event MessageEvent onMessage;

        public List<Employee> employeeList = new List<Employee>();
        public List<BioTemplate> bioTemplateList = new List<BioTemplate>();

        public List<string> biometricTypes = new List<string>();

        private static bool bIsConnected = false;//the boolean value identifies whether the device is connected
        private static int iMachineNumber = 1;
        private static int idwErrorCode = 0;
        private static int iDeviceTpye = 1;
        bool bAddControl = true;        //Get all user's ID

        #region UserBioTypeClass

        private string _biometricType = string.Empty;
        private string _biometricVersion = string.Empty;

        private SupportBiometricType _supportBiometricType = new SupportBiometricType();

        public const string PersBioTableName = "Pers_Biotemplate";

        public const string PersBioTableFields = "*";

        public SupportBiometricType supportBiometricType
        {
            get { return _supportBiometricType; }
        }

        public string biometricType
        {
            get { return _biometricType; }
        }

        public class Employee
        {
            public string pin { get; set; }
            public string name { get; set; }
            public string password { get; set; }
            public int privilege { get; set; }
            public string cardNumber { get; set; }
        }

        public class SupportBiometricType
        {
            public bool fp_available { get; set; }
            public bool face_available { get; set; }
            public bool fingerVein_available { get; set; }
            public bool palm_available { get; set; }
        }

        public class BioTemplate
        {
            /// <summary>
            /// is valid,0:invalid,1:valid,default=1
            /// </summary>
            private int validFlag = 1;
            public virtual int valid_flag
            {
                get { return validFlag; }
                set { validFlag = value; }
            }

            /// <summary>
            /// is duress,0:not duress,1:duress,default=0
            /// </summary>
            public virtual int is_duress { get; set; }

            /// <summary>
            /// Biometric Type
            /// 0： General
            /// 1： Finger Printer
            /// 2： Face
            /// 3： Voiceprint
            /// 4： Iris
            /// 5： Retina
            /// 6： Palm prints
            /// 7： FingerVein
            /// 8： Palm Vein
            /// </summary>
            public virtual int bio_type { get; set; }

            /// <summary>
            /// template version
            /// </summary>
            public virtual string version { get; set; }

            /// <summary>
            /// data format
            /// ZK\ISO\ANSI 
            /// 0： ZK
            /// 1： ISO
            /// 2： ANSI
            /// </summary>
            public virtual int data_format { get; set; }

            /// <summary>
            /// template no
            /// </summary>
            public virtual int template_no { get; set; }

            /// <summary>
            /// template index
            /// </summary>
            public virtual int template_no_index { get; set; }

            /// <summary>
            /// template data
            /// </summary>
            public virtual string template_data { get; set; }

            /// <summary>
            /// pin
            /// </summary>
            public virtual string pin { get; set; }
        }

        public class BioType
        {
            public string name { get; set; }
            public int value { get; set; }

            public override string ToString()
            {
                return name;
            }
        }
        #endregion

        #region ConnectDevice

        public bool GetConnectState()
        {
            return bIsConnected;
        }

        public void SetConnectState(bool state)
        {
            bIsConnected = state;
            //connected = state;
        }

        public int GetMachineNumber()
        {
            return iMachineNumber;
        }

        public void SetMachineNumber(int Number)
        {
            iMachineNumber = Number;
        }

        public int sta_ConnectTCP( string ip, string port, string commKey)
        {
            if (ip == "" || port == "" || commKey == "")
            {
                //lblOutputInfo.Items.Add("*Name, IP, Port or Commkey cannot be null !");
                return -1;// ip or port is null
            }

            if (Convert.ToInt32(port) <= 0 || Convert.ToInt32(port) > 65535)
            {
                //lblOutputInfo.Items.Add("*Port illegal!");
                return -1;
            }

            if (Convert.ToInt32(commKey) < 0 || Convert.ToInt32(commKey) > 999999)
            {
                //lblOutputInfo.Items.Add("*CommKey illegal!");
                return -1;
            }

            int idwErrorCode = 0;

            axCZKEM1.SetCommPassword(Convert.ToInt32(commKey));

            //if (bIsConnected == true)
            //{
            //    axCZKEM1.Disconnect();
            //    sta_UnRegRealTime();
            //    SetConnectState(false);
            //    //lblOutputInfo.Items.Add("Disconnect with device !");
            //    //connected = false;
            //    return -2; //disconnect
            //}

            if (axCZKEM1.Connect_Net(ip, Convert.ToInt32(port)) == true)
            {
                SetConnectState(true);
                //sta_RegRealTime();
                //lblOutputInfo.Items.Add("Connect with device !");

                //get Biotype
                //sta_getBiometricType();

                return 1;
            }
            else
            {
                axCZKEM1.GetLastError(ref idwErrorCode);
                //lblOutputInfo.Items.Add("*Unable to connect the device,ErrorCode=" + idwErrorCode.ToString());
                return idwErrorCode;
            }
        }
        
        //public int sta_ConnectRS(ListBox lblOutputInfo, string deviceid, string port, string baudrate, string commkey)
        //{
        //    if (deviceid == "" || port == "" || baudrate == "" || commkey == "")
        //    {
        //        lblOutputInfo.Items.Add("*Device ID, Port, Baudrate, Comm Key cannot be null !");
        //        return -1;
        //    }

        //    if (Convert.ToInt32(deviceid) < 0 || Convert.ToInt32(deviceid) > 256)
        //    {
        //        lblOutputInfo.Items.Add("*Device illegal!");
        //        return -1;
        //    }

        //    if (Convert.ToInt32(commkey) < 0 || Convert.ToInt32(commkey) > 999999)
        //    {
        //        lblOutputInfo.Items.Add("*CommKey illegal!");
        //        return -1;
        //    }

        //    int idwErrorCode = 0;

        //    int iDeviceID = Convert.ToInt32(deviceid);
        //    int iPort = 0;
        //    int iBaudrate = Convert.ToInt32(baudrate);
        //    int iCommkey = Convert.ToInt32(commkey);

        //    for (iPort = 1; iPort < 10; iPort++)
        //    {
        //        if (port.IndexOf(iPort.ToString()) > -1)
        //        {
        //            break;
        //        }
        //    }

        //    axCZKEM1.SetCommPassword(iCommkey);

        //    if (bIsConnected == true)
        //    {
        //        axCZKEM1.Disconnect();
        //        sta_UnRegRealTime();
        //        SetConnectState(false);
        //        lblOutputInfo.Items.Add("Disconnect with device !");
        //        return -2; //disconnect
        //    }

        //    if (axCZKEM1.Connect_Com(iPort, iDeviceID, iBaudrate) == true)
        //    {
        //        SetConnectState(true);
        //        sta_RegRealTime(lblOutputInfo);

        //        //get Biotype
        //        sta_getBiometricType();
        //        lblOutputInfo.Items.Add("Connect with device !");
        //        return 1;
        //    }
        //    else
        //    {
        //        axCZKEM1.GetLastError(ref idwErrorCode);
        //        lblOutputInfo.Items.Add("*Unable to connect the device,ErrorCode=" + idwErrorCode.ToString());
        //        return idwErrorCode;
        //    }
        //}

        //public int sta_ConnectUSB(ListBox lblOutputInfo, string deviceid, string commkey)
        //{
        //    if (deviceid == "" || commkey == "")
        //    {
        //        lblOutputInfo.Items.Add("*deviceid, commkey cannot be null !");
        //        return -1;
        //    }

        //    if (Convert.ToInt32(deviceid) < 0 || Convert.ToInt32(deviceid) > 256)
        //    {
        //        lblOutputInfo.Items.Add("*Device illegal!");
        //        return -1;
        //    }

        //    if (Convert.ToInt32(commkey) < 0 || Convert.ToInt32(commkey) > 999999)
        //    {
        //        lblOutputInfo.Items.Add("*CommKey illegal!");
        //        return -1;
        //    }

        //    int idwErrorCode = 0;
        //    int iPort = 0;
        //    int iBaudrate = 115200;
        //    int iDeviceID = Convert.ToInt32(deviceid);
        //    int iCommkey = Convert.ToInt32(commkey);
        //    string sCom = "";

        //    if (iDeviceID == 0 || iDeviceID > 255)
        //    {
        //        lblOutputInfo.Items.Add("*The Device ID is invalid !");
        //        return -1;
        //    }

        //    SearchforUSBCom usbcom = new SearchforUSBCom();
        //    bool bSearch = usbcom.SearchforCom(ref sCom);//modify by Darcy on Nov.26 2009
        //    if (bSearch == false)
        //    {
        //        lblOutputInfo.Items.Add("*Can not find the virtual serial port that can be used !");
        //        return -1;
        //    }

        //    for (iPort = 1; iPort < 10; iPort++)
        //    {
        //        if (sCom.IndexOf(iPort.ToString()) > -1)
        //        {
        //            break;
        //        }
        //    }

        //    axCZKEM1.SetCommPassword(iCommkey);

        //    if (bIsConnected == true)
        //    {
        //        axCZKEM1.Disconnect();
        //        sta_UnRegRealTime();
        //        SetConnectState(false);
        //        lblOutputInfo.Items.Add("Disconnect with device !");
        //        return -2; //disconnect
        //    }

        //    if (axCZKEM1.Connect_Com(iPort, iDeviceID, iBaudrate) == true)
        //    {
        //        SetConnectState(true);
        //        sta_RegRealTime(lblOutputInfo);

        //        //Get BioType
        //        sta_getBiometricType();

        //        lblOutputInfo.Items.Add("Connect with device !");
        //        return 1;
        //    }
        //    else
        //    {
        //        axCZKEM1.GetLastError(ref idwErrorCode);
        //        lblOutputInfo.Items.Add("*Unable to connect the device,ErrorCode=" + idwErrorCode.ToString());
        //        return idwErrorCode;
        //    }
        //}

        //public int sta_GetDeviceInfo(ListBox lblOutputInfo, out string sFirmver, out string sMac, out string sPlatform, out string sSN, out string sProductTime, out string sDeviceName, out int iFPAlg, out int iFaceAlg, out string sProducter)
        //{
        //    int iRet = 0;

        //    sFirmver = "";
        //    sMac = "";
        //    sPlatform = "";
        //    sSN = "";
        //    sProducter = "";
        //    sDeviceName = "";
        //    iFPAlg = 0;
        //    iFaceAlg = 0;
        //    sProductTime = "";
        //    string strTemp = "";

        //    if (GetConnectState() == false)
        //    {
        //        lblOutputInfo.Items.Add("*Please connect first!");
        //        return -1024;
        //    }

        //    axCZKEM1.EnableDevice(GetMachineNumber(), false);//disable the device

        //    axCZKEM1.GetSysOption(GetMachineNumber(), "~ZKFPVersion", out strTemp);
        //    iFPAlg = Convert.ToInt32(strTemp);

        //    axCZKEM1.GetSysOption(GetMachineNumber(), "ZKFaceVersion", out strTemp);
        //    iFaceAlg = Convert.ToInt32(strTemp);

        //    /*
        //    axCZKEM1.GetDeviceInfo(GetMachineNumber(), 72, ref iFPAlg);
        //    axCZKEM1.GetDeviceInfo(GetMachineNumber(), 73, ref iFaceAlg);
        //    */

        //    axCZKEM1.GetVendor(ref sProducter);
        //    axCZKEM1.GetProductCode(GetMachineNumber(), out sDeviceName);
        //    axCZKEM1.GetDeviceMAC(GetMachineNumber(), ref sMac);
        //    axCZKEM1.GetFirmwareVersion(GetMachineNumber(), ref sFirmver);

        //    /*
        //    if (sta_GetDeviceType() == 1)
        //    {
        //        axCZKEM1.GetDeviceFirmwareVersion(GetMachineNumber(), ref sFirmver);
        //    }
        //     */
        //    //lblOutputInfo.Items.Add("[func GetDeviceFirmwareVersion]Temporarily unsupported");

        //    axCZKEM1.GetPlatform(GetMachineNumber(), ref sPlatform);
        //    axCZKEM1.GetSerialNumber(GetMachineNumber(), out sSN);
        //    axCZKEM1.GetDeviceStrInfo(GetMachineNumber(), 1, out sProductTime);

        //    axCZKEM1.EnableDevice(GetMachineNumber(), true);//enable the device

        //    lblOutputInfo.Items.Add("Get the device info successfully");
        //    iRet = 1;
        //    return iRet;
        //}

        //public int sta_GetCapacityInfo(ListBox lblOutputInfo, out int adminCnt, out int userCount, out int fpCnt, out int recordCnt, out int pwdCnt, out int oplogCnt, out int faceCnt)
        //{
        //    int ret = 0;

        //    adminCnt = 0;
        //    userCount = 0;
        //    fpCnt = 0;
        //    recordCnt = 0;
        //    pwdCnt = 0;
        //    oplogCnt = 0;
        //    faceCnt = 0;

        //    if (GetConnectState() == false)
        //    {
        //        lblOutputInfo.Items.Add("*Please connect first!");
        //        return -1024;
        //    }

        //    axCZKEM1.EnableDevice(GetMachineNumber(), false);//disable the device

        //    axCZKEM1.GetDeviceStatus(GetMachineNumber(), 2, ref userCount);
        //    axCZKEM1.GetDeviceStatus(GetMachineNumber(), 1, ref adminCnt);
        //    axCZKEM1.GetDeviceStatus(GetMachineNumber(), 3, ref fpCnt);
        //    axCZKEM1.GetDeviceStatus(GetMachineNumber(), 4, ref pwdCnt);
        //    axCZKEM1.GetDeviceStatus(GetMachineNumber(), 5, ref oplogCnt);
        //    axCZKEM1.GetDeviceStatus(GetMachineNumber(), 6, ref recordCnt);
        //    axCZKEM1.GetDeviceStatus(GetMachineNumber(), 21, ref faceCnt);

        //    axCZKEM1.EnableDevice(GetMachineNumber(), true);//enable the device

        //    lblOutputInfo.Items.Add("Get the device capacity successfully");

        //    ret = 1;
        //    return ret;
        //}

        public int sta_GetDeviceInfo( out string sFirmver, out string sMac, out string sPlatform, out string sSN, out string sProductTime, out string sDeviceName, out int iFPAlg, out int iFaceAlg, out string sProducter)
        {
            int iRet = 0;

            sFirmver = "";
            sMac = "";
            sPlatform = "";
            sSN = "";
            sProducter = "";
            sDeviceName = "";
            iFPAlg = 0;
            iFaceAlg = 0;
            sProductTime = "";
            string strTemp = "";

            if (GetConnectState() == false)
            {
                
                return -1024;
            }

            axCZKEM1.EnableDevice(GetMachineNumber(), false);//disable the device

            axCZKEM1.GetSysOption(GetMachineNumber(), "~ZKFPVersion", out strTemp);
            iFPAlg = Convert.ToInt32(strTemp);

            axCZKEM1.GetSysOption(GetMachineNumber(), "ZKFaceVersion", out strTemp);
            iFaceAlg = Convert.ToInt32(strTemp);

            /*
            axCZKEM1.GetDeviceInfo(GetMachineNumber(), 72, ref iFPAlg);
            axCZKEM1.GetDeviceInfo(GetMachineNumber(), 73, ref iFaceAlg);
            */

            axCZKEM1.GetVendor(ref sProducter);
            axCZKEM1.GetProductCode(GetMachineNumber(), out sDeviceName);
            axCZKEM1.GetDeviceMAC(GetMachineNumber(), ref sMac);
            axCZKEM1.GetFirmwareVersion(GetMachineNumber(), ref sFirmver);

            /*
            if (sta_GetDeviceType() == 1)
            {
                axCZKEM1.GetDeviceFirmwareVersion(GetMachineNumber(), ref sFirmver);
            }
             */
            //lblOutputInfo.Items.Add("[func GetDeviceFirmwareVersion]Temporarily unsupported");

            axCZKEM1.GetPlatform(GetMachineNumber(), ref sPlatform);
            axCZKEM1.GetSerialNumber(GetMachineNumber(), out sSN);
            axCZKEM1.GetDeviceStrInfo(GetMachineNumber(), 1, out sProductTime);

            axCZKEM1.EnableDevice(GetMachineNumber(), true);//enable the device

        
            iRet = 1;
            return iRet;
        }

        public int sta_GetCapacityInfo( out int adminCnt, out int userCount, out int fpCnt, out int recordCnt, out int pwdCnt, out int oplogCnt, out int faceCnt)
        {
            int ret = 0;

            adminCnt = 0;
            userCount = 0;
            fpCnt = 0;
            recordCnt = 0;
            pwdCnt = 0;
            oplogCnt = 0;
            faceCnt = 0;

            if (GetConnectState() == false)
            {
             
                return -1024;
            }

           var a= axCZKEM1.EnableDevice(GetMachineNumber(), false);//disable the device

            axCZKEM1.GetDeviceStatus(GetMachineNumber(), 2, ref userCount);
            axCZKEM1.GetDeviceStatus(GetMachineNumber(), 1, ref adminCnt);
            axCZKEM1.GetDeviceStatus(GetMachineNumber(), 3, ref fpCnt);
            axCZKEM1.GetDeviceStatus(GetMachineNumber(), 4, ref pwdCnt);
            axCZKEM1.GetDeviceStatus(GetMachineNumber(), 5, ref oplogCnt);
            axCZKEM1.GetDeviceStatus(GetMachineNumber(), 6, ref recordCnt);
            axCZKEM1.GetDeviceStatus(GetMachineNumber(), 21, ref faceCnt);

           var b= axCZKEM1.EnableDevice(GetMachineNumber(), true);//enable the device

           

            ret = 1;
            return ret;
        }

        public void sta_DisConnect()
        {
            if (GetConnectState() == true)
            {
                axCZKEM1.Disconnect();
                //sta_UnRegRealTime();
            }
        }

        #region RealTimeEvent

        public delegate List<string> GetRealEventListBoxHandler();
        private GetRealEventListBoxHandler gRealEventListBoxHandler;
        private List<string> gRealEventListBox;

        public void sta_SetRTLogListBox(GetRealEventListBoxHandler gvHandler)
        {
            gRealEventListBoxHandler = gvHandler;
            gRealEventListBox = gRealEventListBoxHandler();
        }

        public void sta_UnRegRealTime()
        {
            this.axCZKEM1.OnFinger -= new zkemkeeper._IZKEMEvents_OnFingerEventHandler(axCZKEM1_OnFinger);
            this.axCZKEM1.OnVerify -= new zkemkeeper._IZKEMEvents_OnVerifyEventHandler(axCZKEM1_OnVerify);
            this.axCZKEM1.OnAttTransactionEx -= new zkemkeeper._IZKEMEvents_OnAttTransactionExEventHandler(axCZKEM1_OnAttTransactionEx);
            this.axCZKEM1.OnFingerFeature -= new zkemkeeper._IZKEMEvents_OnFingerFeatureEventHandler(axCZKEM1_OnFingerFeature);
            this.axCZKEM1.OnDeleteTemplate -= new zkemkeeper._IZKEMEvents_OnDeleteTemplateEventHandler(axCZKEM1_OnDeleteTemplate);
            this.axCZKEM1.OnNewUser -= new zkemkeeper._IZKEMEvents_OnNewUserEventHandler(axCZKEM1_OnNewUser);
            this.axCZKEM1.OnHIDNum -= new zkemkeeper._IZKEMEvents_OnHIDNumEventHandler(axCZKEM1_OnHIDNum);
            this.axCZKEM1.OnAlarm -= new zkemkeeper._IZKEMEvents_OnAlarmEventHandler(axCZKEM1_OnAlarm);
            this.axCZKEM1.OnDoor -= new zkemkeeper._IZKEMEvents_OnDoorEventHandler(axCZKEM1_OnDoor);
            this.axCZKEM1.OnEnrollFingerEx -= new zkemkeeper._IZKEMEvents_OnEnrollFingerExEventHandler(axCZKEM1_OnEnrollFingerEx);
            this.axCZKEM1.OnWriteCard += new zkemkeeper._IZKEMEvents_OnWriteCardEventHandler(axCZKEM1_OnWriteCard);
            this.axCZKEM1.OnEmptyCard += new zkemkeeper._IZKEMEvents_OnEmptyCardEventHandler(axCZKEM1_OnEmptyCard);
            this.axCZKEM1.OnHIDNum += new zkemkeeper._IZKEMEvents_OnHIDNumEventHandler(axCZKEM1_OnHIDNum);
            this.axCZKEM1.OnAttTransaction -= new zkemkeeper._IZKEMEvents_OnAttTransactionEventHandler(axCZKEM1_OnAttTransaction);
            this.axCZKEM1.OnKeyPress += new zkemkeeper._IZKEMEvents_OnKeyPressEventHandler(axCZKEM1_OnKeyPress);
            this.axCZKEM1.OnEnrollFinger += new zkemkeeper._IZKEMEvents_OnEnrollFingerEventHandler(axCZKEM1_OnEnrollFinger);

        }

        public int sta_RegRealTime()
        {
            List<string> lblOutputInfo=new List<string>();
            if (GetConnectState() == false)
            {
                lblOutputInfo.Add("*Please connect first!");
                return -1024;
            }

            int ret = 0;

            if (axCZKEM1.RegEvent(GetMachineNumber(), 65535))//Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
            {
                //common interface
                this.axCZKEM1.OnFinger += new zkemkeeper._IZKEMEvents_OnFingerEventHandler(axCZKEM1_OnFinger);
                this.axCZKEM1.OnVerify += new zkemkeeper._IZKEMEvents_OnVerifyEventHandler(axCZKEM1_OnVerify);
                this.axCZKEM1.OnFingerFeature += new zkemkeeper._IZKEMEvents_OnFingerFeatureEventHandler(axCZKEM1_OnFingerFeature);
                this.axCZKEM1.OnDeleteTemplate += new zkemkeeper._IZKEMEvents_OnDeleteTemplateEventHandler(axCZKEM1_OnDeleteTemplate);
                this.axCZKEM1.OnNewUser += new zkemkeeper._IZKEMEvents_OnNewUserEventHandler(axCZKEM1_OnNewUser);
                this.axCZKEM1.OnHIDNum += new zkemkeeper._IZKEMEvents_OnHIDNumEventHandler(axCZKEM1_OnHIDNum);
                this.axCZKEM1.OnAlarm += new zkemkeeper._IZKEMEvents_OnAlarmEventHandler(axCZKEM1_OnAlarm);
                this.axCZKEM1.OnDoor += new zkemkeeper._IZKEMEvents_OnDoorEventHandler(axCZKEM1_OnDoor);

                //only for color device
                this.axCZKEM1.OnAttTransactionEx += new zkemkeeper._IZKEMEvents_OnAttTransactionExEventHandler(axCZKEM1_OnAttTransactionEx);
                this.axCZKEM1.OnEnrollFingerEx += new zkemkeeper._IZKEMEvents_OnEnrollFingerExEventHandler(axCZKEM1_OnEnrollFingerEx);

                //only for black&white device
                this.axCZKEM1.OnAttTransaction -= new zkemkeeper._IZKEMEvents_OnAttTransactionEventHandler(axCZKEM1_OnAttTransaction);
                this.axCZKEM1.OnWriteCard += new zkemkeeper._IZKEMEvents_OnWriteCardEventHandler(axCZKEM1_OnWriteCard);
                this.axCZKEM1.OnEmptyCard += new zkemkeeper._IZKEMEvents_OnEmptyCardEventHandler(axCZKEM1_OnEmptyCard);
                this.axCZKEM1.OnKeyPress += new zkemkeeper._IZKEMEvents_OnKeyPressEventHandler(axCZKEM1_OnKeyPress);
                this.axCZKEM1.OnEnrollFinger += new zkemkeeper._IZKEMEvents_OnEnrollFingerEventHandler(axCZKEM1_OnEnrollFinger);


                ret = 1;
            }
            else
            {
                axCZKEM1.GetLastError(ref idwErrorCode);
                ret = idwErrorCode;

                if (idwErrorCode != 0)
                {
                    lblOutputInfo.Add("*RegEvent failed,ErrorCode: " + idwErrorCode.ToString());
                }
                else
                {
                    lblOutputInfo.Add("*No data from terminal returns!");
                }
            }
            return ret;
        }

        //When you are enrolling your finger,this event will be triggered.
        void axCZKEM1_OnEnrollFingerEx(string EnrollNumber, int FingerIndex, int ActionResult, int TemplateLength)
        {
            if (ActionResult == 0)
            {
                gRealEventListBox.Add("Enroll finger succeed. UserID=" + EnrollNumber.ToString() + "...FingerIndex=" + FingerIndex.ToString());
            }
            else
            {
                gRealEventListBox.Add("Enroll finger failed. Result=" + ActionResult.ToString());
            }
            throw new NotImplementedException();
        }

        //Door sensor event
        void axCZKEM1_OnDoor(int EventType)
        {
            gRealEventListBox.Add("Door opened" + "...EventType=" + EventType.ToString());

            throw new NotImplementedException();
        }

        //When the dismantling machine or duress alarm occurs, trigger this event.
        void axCZKEM1_OnAlarm(int AlarmType, int EnrollNumber, int Verified)
        {
            gRealEventListBox.Add("Alarm triggered" + "...AlarmType=" + AlarmType.ToString() + "...EnrollNumber=" + EnrollNumber.ToString() + "...Verified=" + Verified.ToString());

            throw new NotImplementedException();
        }

        //When you swipe a card to the device, this event will be triggered to show you the card number.
        void axCZKEM1_OnHIDNum(int CardNumber)
        {
            gRealEventListBox.Add("Card event" + "...Cardnumber=" + CardNumber.ToString());

            throw new NotImplementedException();
        }

        //When you have enrolled a new user,this event will be triggered.
        void axCZKEM1_OnNewUser(int EnrollNumber)
        {
           
            gRealEventListBox.Add("Enroll a　new user" + "...UserID=" + EnrollNumber.ToString());

            throw new NotImplementedException();
        }

        //When you have deleted one one fingerprint template,this event will be triggered.
        void axCZKEM1_OnDeleteTemplate(int EnrollNumber, int FingerIndex)
        {
            gRealEventListBox.Add("Delete a finger template" + "...UserID=" + EnrollNumber.ToString() + "..FingerIndex=" + FingerIndex.ToString());

            throw new NotImplementedException();
        }

        //When you have enrolled your finger,this event will be triggered and return the quality of the fingerprint you have enrolled
        void axCZKEM1_OnFingerFeature(int Score)
        {
            gRealEventListBox.Add("Press finger score=" + Score.ToString());

            throw new NotImplementedException();
        }

        //If your fingerprint(or your card) passes the verification,this event will be triggered,only for color device
        void axCZKEM1_OnAttTransactionEx(string EnrollNumber, int IsInValid, int AttState, int VerifyMethod, int Year, int Month, int Day, int Hour, int Minute, int Second, int WorkCode)
        {
            string time = Year + "-" + Month + "-" + Day + " " + Hour + ":" + Minute + ":" + Second;

            gRealEventListBox.Add("Verify OK.UserID=" + EnrollNumber + " isInvalid=" + IsInValid.ToString() + " state=" + AttState.ToString() + " verifystyle=" + VerifyMethod.ToString() + " time=" + time);

            throw new NotImplementedException();
        }

        //If your fingerprint(or your card) passes the verification,this event will be triggered,only for black%white device
        private void axCZKEM1_OnAttTransaction(int EnrollNumber, int IsInValid, int AttState, int VerifyMethod, int Year, int Month, int Day, int Hour, int Minute, int Second)
        {
            string time = Year + "-" + Month + "-" + Day + " " + Hour + ":" + Minute + ":" + Second;
            gRealEventListBox.Add("Verify OK.UserID=" + EnrollNumber.ToString() + " isInvalid=" + IsInValid.ToString() + " state=" + AttState.ToString() + " verifystyle=" + VerifyMethod.ToString() + " time=" + time);

            throw new NotImplementedException();
        }

        //After you have placed your finger on the sensor(or swipe your card to the device),this event will be triggered.
        //If you passes the verification,the returned value userid will be the user enrollnumber,or else the value will be -1;
        void axCZKEM1_OnVerify(int UserID)
        {
            if (UserID != -1)
            {
                gRealEventListBox.Add("User fingerprint verified... UserID=" + UserID.ToString());
            }
            else
            {
                gRealEventListBox.Add("Failed to verify... ");
            }

            throw new NotImplementedException();
        }

        //When you place your finger on sensor of the device,this event will be triggered
        void axCZKEM1_OnFinger()
        {
            gRealEventListBox.Add("OnFinger event ");

            throw new NotImplementedException();
        }

        //When you have written into the Mifare card ,this event will be triggered.
        void axCZKEM1_OnWriteCard(int iEnrollNumber, int iActionResult, int iLength)
        {
            if (iActionResult == 0)
            {
                gRealEventListBox.Add("Write Mifare Card OK" + "...EnrollNumber=" + iEnrollNumber.ToString() + "...TmpLength=" + iLength.ToString());
            }
            else
            {
                gRealEventListBox.Add("...Write Failed");
            }
        }

        //When you have emptyed the Mifare card,this event will be triggered.
        void axCZKEM1_OnEmptyCard(int iActionResult)
        {
            if (iActionResult == 0)
            {
                gRealEventListBox.Add("Empty Mifare Card OK...");
            }
            else
            {
                gRealEventListBox.Add("Empty Failed...");
            }
        }

        //When you press the keypad,this event will be triggered.
        void axCZKEM1_OnKeyPress(int iKey)
        {
            gRealEventListBox.Add("RTEvent OnKeyPress Has been Triggered, Key: " + iKey.ToString());
        }

        //When you are enrolling your finger,this event will be triggered.
        void axCZKEM1_OnEnrollFinger(int EnrollNumber, int FingerIndex, int ActionResult, int TemplateLength)
        {
            if (ActionResult == 0)
            {
                gRealEventListBox.Add("Enroll finger succeed. UserID=" + EnrollNumber + "...FingerIndex=" + FingerIndex.ToString());
            }
            else
            {
                gRealEventListBox.Add("Enroll finger failed. Result=" + ActionResult.ToString());
            }
            throw new NotImplementedException();
        }

        #endregion

        #endregion

        #region DeviceType

        public int sta_GetDeviceType()
        {
            string sPlatform = "";
            int iFaceDevice = 0;

            if (axCZKEM1.IsTFTMachine(GetMachineNumber()))
            {
                axCZKEM1.GetDeviceInfo(GetMachineNumber(), 75, ref iFaceDevice);
                axCZKEM1.GetPlatform(GetMachineNumber(), ref sPlatform);
                if (sPlatform.Contains("ZMM"))
                {
                    return 1;//new firmware device
                }
                else if (iFaceDevice == 1)
                {
                    return 2;//face serial
                }
                else
                {
                    return 3;//color device
                }
            }
            else
            {
                return 4;//black&whith device
            }

        }
           public void sta_getBiometricType()
        {
            string result = string.Empty;
            result = sta_getSysOptions("BiometricType");
            if (!string.IsNullOrEmpty(result))
            {
                _supportBiometricType.fp_available = result[1] == '1';
                _supportBiometricType.face_available = result[2] == '1';
                if (result.Length >= 9)
                {
                    _supportBiometricType.fingerVein_available = result[7] == '1';
                    _supportBiometricType.palm_available = result[8] == '1';
                }
            }
            _biometricType = result;
        }
        private string sta_getSysOptions(string option)
        {
            string value = string.Empty;
            axCZKEM1.GetSysOption(iMachineNumber, option, out value);
            return value;
        }
        #endregion
        #region UesrFP
        public List<viewModel> GetAllUserFPInfo()
        {
            List<viewModel> Vml = new List<viewModel>();
            if (GetConnectState() == false)
            {
               
                return Vml ;
            }

            string sEnrollNumber = "";
            bool bEnabled = false;
            string sName = "";
            string sPassword = "";
            int iPrivilege = 0;
            string sFPTmpData = "";
            string sCardnumber = "";
            int idwFingerIndex = 0;
            int iFlag = 0;
            int iFPTmpLength = 0;
            int iFaceIndex = 50;//the only possible parameter value
            string sTmpData = "";
            int iLength = 0;
            int i = 0;
            int num = 0;
            int iFpCount = 0;
            int index = 0;
            int xx = 1;
            int iUserGrp = 0;



            var a = axCZKEM1.EnableDevice(iMachineNumber, false);
                var b = axCZKEM1.ReadAllUserID(iMachineNumber);//read all the user information to the memory  except fingerprint Templates
                var c = axCZKEM1.ReadAllTemplate(iMachineNumber);//read all the users' fingerprint templates to the memory
                    while (axCZKEM1.SSR_GetAllUserInfo(iMachineNumber, out sEnrollNumber, out sName, out sPassword, out iPrivilege, out bEnabled))//get all the users' information from the memory
                    {
                     var userId = Convert.ToInt32(sEnrollNumber);
                     axCZKEM1.GetStrCardNumber(out sCardnumber);//get the card number from the memory
                     axCZKEM1.GetUserGroup(iMachineNumber, userId,ref iUserGrp);

                    viewModel Vm = new viewModel();
                    Vm.EmpCode = sEnrollNumber == null ? "Null" : sEnrollNumber;
                    Vm.EmpName = sName == null ? "Null" : sName;
                    Vm.CardNo = sCardnumber == null ? "Null" : sCardnumber;
                    Vm.sPassword = sPassword == null ? "Null" : sPassword;
                    Vm.userGroup = iUserGrp;
                      


                    i = 0;
                    xx = 1;
                    for (idwFingerIndex = 0; idwFingerIndex < 10; idwFingerIndex++)
                    {
                        if (axCZKEM1.GetUserTmpExStr(iMachineNumber, sEnrollNumber, idwFingerIndex, out iFlag, out sFPTmpData, out iFPTmpLength))//get the corresponding templates string and length from the memory
                        {
                            if (xx == 1)
                            {
                                Vm.indexNo = (idwFingerIndex);
                                Vm.finger_Data = sFPTmpData == null ? "Null" : sFPTmpData; ;

                            }
                            else
                            {


                                Vm.EmpCode = sEnrollNumber==null?"Null": sEnrollNumber;
                                Vm.EmpName = sName;
                                Vm.CardNo = sCardnumber;
                                Vm.indexNo = (idwFingerIndex);
                                Vm.finger_Data = sFPTmpData == null ? "Null" : sFPTmpData;
                            }


                            xx = 0;
                            iFpCount++;
                        }
                        else
                        {
                            i++;
                        }
                    }
                    if (axCZKEM1.GetUserFaceStr(iMachineNumber, sEnrollNumber, iFaceIndex, ref sTmpData, ref iLength))//get the face templates from the memory
                    {
                        Vm.emp_image = sTmpData == null ? "Null" : sTmpData;
                    }

                    Vml.Add(Vm);
                    num++;

                }

                axCZKEM1.EnableDevice(iMachineNumber, true);
                return Vml;
            
        }

        public int sta_SetAllUserFPInfo(List<viewModel> lvUserInfos)
        {
            if (GetConnectState() == false)
            {
               
                return -999;
            }

            if (lvUserInfos.Count == 0)
            {

                return -999;
            }

            string sEnrollNumber = "";
            string sEnabled = "";
            bool bEnabled = false;

            string sName = "";
            string sPassword = "";
            int iPrivilege = 0;
            string sFPTmpData = "";
            string sCardnumber = "";
            int idwFingerIndex = 0;
            string sdwFingerIndex = "";
            int iFlag = 0;
            string sFlag = "";
            int num = 0;
            int iFaceIndex = 50;//the only possible parameter value
            string sTmpData = "";
            int iLength = 0;
            int userGroup = 0;
            
            int index = 0;


            axCZKEM1.EnableDevice(iMachineNumber, false);
            string Tz = "";

            Tz = "00000000000023590000235900002359000023590000235900002359";
            axCZKEM1.SetTZInfo(iMachineNumber, 1, Tz);


            Tz = "00002359000000000000235900002359000023590000235900002359";
            axCZKEM1.SetTZInfo(iMachineNumber, 2, Tz);


            Tz = "00002359000023590000000000002359000023590000235900002359";
            axCZKEM1.SetTZInfo(iMachineNumber, 3, Tz);


            Tz = "00002359000023590000235900000000000023590000235900002359";
            axCZKEM1.SetTZInfo(iMachineNumber, 4, Tz);


            Tz = "00002359000023590000235900002359000000000000235900002359";
            axCZKEM1.SetTZInfo(iMachineNumber, 5, Tz);


            Tz = "00002359000023590000235900002359000023590000000000002359";
            axCZKEM1.SetTZInfo(iMachineNumber, 6, Tz);



            Tz = "00002359000023590000235900002359235900002359000000000000";
            axCZKEM1.SetTZInfo(iMachineNumber, 7, Tz);



            foreach (var lvUserInfo in lvUserInfos)
            {
                var id = Convert.ToInt32(lvUserInfo.CardNo);
                sEnrollNumber = id.ToString();
                sEnabled = "true";
                if (sEnabled == "true")
                {
                    bEnabled = true;
                }
                else
                {
                    bEnabled = false;
                }

                sName = lvUserInfo.EmpName;
                sCardnumber = "";
                sPassword = lvUserInfo.sPassword==null? "": lvUserInfo.sPassword;
                sdwFingerIndex = lvUserInfo.indexNo.ToString();
                sFlag = "1";
                if (lvUserInfo.finger_Data != null) 
                    {
                        sFPTmpData = lvUserInfo.finger_Data;
                    }
                iPrivilege = 0;
                if (lvUserInfo.emp_image != null)
                {
                    iLength = Convert.ToInt32(lvUserInfo.emp_image.Length);
                    sTmpData = lvUserInfo.emp_image;
                }
            //// weekday

                if (sCardnumber != "" && sCardnumber != "0")
                {
                    axCZKEM1.SetStrCardNumber(sCardnumber);
                }
                if (sEnrollNumber != null)
                {
                    if (axCZKEM1.SSR_SetUserInfo(iMachineNumber, sEnrollNumber, sName, sPassword, iPrivilege, bEnabled))//upload user information to the device
                    {
                        if (sTmpData != "")
                        {
                            if (axCZKEM1.SetUserFaceStr(iMachineNumber, sEnrollNumber, iFaceIndex, sTmpData, iLength))
                            {
                                axCZKEM1.StartEnrollEx(sEnrollNumber, 111, iFlag);
                                axCZKEM1.StartIdentify();//After enrolling templates,you should let the device into the 1:N verification condition
                                axCZKEM1.RefreshData(1);//the data in the device should be refreshed
                                                        //axCZKEM1.SetUserFaceStr(iMachineNumber, sEnrollNumber, iFaceIndex, sTmpData, iLength);
                            }

                            //upload face templates information to the device
                        }
                        if (sFlag != "" && sFPTmpData != "")
                        {
                            idwFingerIndex = sdwFingerIndex == "" || sdwFingerIndex == null ? 6 : Convert.ToInt32(sdwFingerIndex);
                            iFlag = Convert.ToInt32(sFlag);
                            axCZKEM1.SetUserTmpExStr(iMachineNumber, sEnrollNumber, idwFingerIndex, iFlag, sFPTmpData);//upload templates information to the device
                        }
                        if ( lvUserInfo.userGroup != null)
                        {
                            userGroup = (int)lvUserInfo.userGroup;


                            var userId = Convert.ToInt32(sEnrollNumber);


                            if (userGroup == 1)////weekday
                            {
                                axCZKEM1.SetUserTZStr(iMachineNumber, userId, "1:0:0:1");
                                //axCZKEM1.SetUserGroup(iMachineNumber, userId, userGroup);
                                //axCZKEM1.SSR_SetGroupTZ(iMachineNumber, userGroup, userGroup, 0, 0, 1, 0);

                            }
                            else if (userGroup == 2)
                            {
                                axCZKEM1.SetUserTZStr(iMachineNumber, userId, "2:0:0:1");

                            }
                            else if (userGroup == 3)
                            {
                                axCZKEM1.SetUserTZStr(iMachineNumber, userId, "3:0:0:1");

                            }
                            else if (userGroup == 4)
                            {
                                axCZKEM1.SetUserTZStr(iMachineNumber, userId, "4:0:0:1");

                            }
                            else if (userGroup == 5)
                            {
                                axCZKEM1.SetUserTZStr(iMachineNumber, userId, "5:0:0:1");

                            }
                            else if (userGroup == 6)
                            {
                                axCZKEM1.SetUserTZStr(iMachineNumber, userId, "6:0:0:1");

                            }
                            else if (userGroup == 7)
                            {
                                axCZKEM1.SetUserTZStr(iMachineNumber, userId, "7:0:0:1");

                            }
                        }

                        num++;

                    }
                    else
                    {
                        axCZKEM1.GetLastError(ref idwErrorCode);
                        //axCZKEM1.EnableDevice(iMachineNumber, true);
                        //return -1022;
                    }
                }

            }

           

            axCZKEM1.RefreshData(iMachineNumber);//the data in the device should be refreshed
            axCZKEM1.EnableDevice(iMachineNumber, true);
            

            return 1;
        }

        //public int sta_batch_SetAllUserFPInfo(ListBox lblOutputInfo, ProgressBar prgSta, ListView lvUserInfo)
        //{
        //    if (GetConnectState() == false)
        //    {
        //        lblOutputInfo.Items.Add("*Please connect first!");
        //        return -1024;
        //    }

        //    if (lvUserInfo.Items.Count == 0)
        //    {
        //        lblOutputInfo.Items.Add("*There is no data can upload!");
        //        return -1023;
        //    }

        //    string sEnrollNumber = "";
        //    string sEnabled = "";
        //    bool bEnabled = false;

        //    string sName = "";
        //    string sPassword = "";
        //    int iPrivilege = 0;
        //    string sFPTmpData = "";
        //    string sCardnumber = "";
        //    int idwFingerIndex = 0;
        //    string sdwFingerIndex = "";
        //    int iFlag = 0;
        //    string sFlag = "";
        //    int num = 0;

        //    prgSta.Value = 0;
        //    axCZKEM1.EnableDevice(iMachineNumber, false);
        //    if (axCZKEM1.BeginBatchUpdate(iMachineNumber, 1))//create memory space for batching data
        //    {
        //        for (int i = 0; i < lvUserInfo.Items.Count; i++)
        //        {
        //            sEnrollNumber = lvUserInfo.Items[i].SubItems[0].Text;
        //            sEnabled = lvUserInfo.Items[i].SubItems[1].Text;
        //            if (sEnabled == "true")
        //            {
        //                bEnabled = true;
        //            }
        //            else
        //            {
        //                bEnabled = false;
        //            }
        //            sName = lvUserInfo.Items[i].SubItems[2].Text;
        //            sCardnumber = lvUserInfo.Items[i].SubItems[3].Text;
        //            sPassword = lvUserInfo.Items[i].SubItems[4].Text;
        //            sdwFingerIndex = lvUserInfo.Items[i].SubItems[5].Text;
        //            sFlag = lvUserInfo.Items[i].SubItems[6].Text;
        //            sFPTmpData = lvUserInfo.Items[i].SubItems[7].Text;
        //            iPrivilege = Convert.ToInt32(lvUserInfo.Items[i].SubItems[8].Text);

        //            if (sCardnumber != "" && sCardnumber != "0")
        //            {
        //                axCZKEM1.SetStrCardNumber(sCardnumber);
        //            }
        //            if (axCZKEM1.SSR_SetUserInfo(iMachineNumber, sEnrollNumber, sName, sPassword, iPrivilege, bEnabled))//upload user information to the device
        //            {
        //                if (sdwFingerIndex != "" && sFlag != "" && sFPTmpData != "")
        //                {
        //                    idwFingerIndex = Convert.ToInt32(sdwFingerIndex);
        //                    iFlag = Convert.ToInt32(sFlag);
        //                    axCZKEM1.SetUserTmpExStr(iMachineNumber, sEnrollNumber, idwFingerIndex, iFlag, sFPTmpData);//upload templates information to the device
        //                }
        //                num++;
        //                prgSta.Value = num % 100;
        //            }
        //            else
        //            {
        //                axCZKEM1.GetLastError(ref idwErrorCode);
        //                lblOutputInfo.Items.Add("*Upload user " + sEnrollNumber + " error,ErrorCode=!" + idwErrorCode.ToString());
        //                //axCZKEM1.EnableDevice(iMachineNumber, true);
        //                //return -1022;
        //            }
        //        }
        //    }
        //    prgSta.Value = 100;
        //    axCZKEM1.BatchUpdate(iMachineNumber);//upload all the information in the memory
        //    axCZKEM1.RefreshData(iMachineNumber);//the data in the device should be refreshed
        //    axCZKEM1.EnableDevice(iMachineNumber, true);
        //    lblOutputInfo.Items.Add("Upload user successfully in batch");
        //    return 1;
        //}

        #endregion

        #region Event Log
        public List<EventViewModel> sta_readLogByPeriod(string fromTime, string toTime)
        {
          List<EventViewModel> Evmlist = new List<EventViewModel>();
            int ret = 0;

            axCZKEM1.EnableDevice(GetMachineNumber(), false);//disable the device

            string sdwEnrollNumber = "";
            int idwVerifyMode = 0;
            int idwInOutMode = 0;
            int idwYear = 0;
            int idwMonth = 0;
            int idwDay = 0;
            int idwHour = 0;
            int idwMinute = 0;
            int idwSecond = 0;
            int idwWorkcode = 0;


            if (axCZKEM1.ReadTimeGLogData(GetMachineNumber(), fromTime, toTime))
            {
                while (axCZKEM1.SSR_GetGeneralLogData(GetMachineNumber(), out sdwEnrollNumber, out idwVerifyMode,
                            out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref idwWorkcode))//get records from the memory
                {
                    EventViewModel log = new EventViewModel();
                    log.cardNo = sdwEnrollNumber;
                    log.date = idwYear + "-" + idwMonth + "-" + idwDay;
                    log.time = idwHour + ":" + idwMinute + ":" + idwSecond;
                    Evmlist.Add(log);
                }
                axCZKEM1.EnableDevice(GetMachineNumber(), true);
                return Evmlist;

            }
            else 
            {
                if (axCZKEM1.ReadGeneralLogData(GetMachineNumber()))
                {
                    while (axCZKEM1.SSR_GetGeneralLogData(GetMachineNumber(), out sdwEnrollNumber, out idwVerifyMode,
                                out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref idwWorkcode))//get records from the memory
                    {
                        EventViewModel log = new EventViewModel();
                        log.cardNo = sdwEnrollNumber;
                        log.date = idwYear + "-" + idwMonth + "-" + idwDay;
                        log.time = idwHour + ":" + idwMinute + ":" + idwSecond;
                        Evmlist.Add(log);
                    }
                    axCZKEM1.EnableDevice(GetMachineNumber(), true);
                    return Evmlist;
                }
                else
               {
                    axCZKEM1.GetLastError(ref idwErrorCode);
                    ret = idwErrorCode;
                    return Evmlist;
                }

            }
        


    //lblOutputInfo.Items.Add("[func ReadTimeGLogData]Temporarily unsupported");
    axCZKEM1.EnableDevice(GetMachineNumber(), true);//enable the device

            return Evmlist;
        }
        #endregion

    }
}
