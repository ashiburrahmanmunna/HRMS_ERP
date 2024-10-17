using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Xml;


namespace GTERP.Models.Common
{

    public class clsProcedure
    {
        #region Server Configuration File
        public Boolean fncConfiguredFileExist(string filePath, string msg)
        {
            if (File.Exists(filePath) != true)
            {
                //MessageBox.Show(msg);
                return false;
            }
            return true;
        }
        #endregion

        public string GTRSingleQuoteAvoid(string str)
        {
            if (str == null)
            {
                return "";
            }
            str = str.Replace("'", "''");
            return str;
        }

        //Move Like Tab When User Press Enter
        public void GTRTabMove(Int16 KeyCode)
        {
            //if (KeyCode == 13)
            //{
            //    SendKeys.Send("{TAB}");
            //}
        }

        //To Avoid Single Quote Entry
        public Boolean GTRSingleQuote(Int16 KeyCode)
        {
            Boolean bln = false;
            if (KeyCode == 39)
            {
                bln = true;
            }
            return bln;
        }

        //Only Allow Currency Related Character
        public Boolean GTRCurrency(string KeyCode)
        {
            Boolean bln = true;

            string strValue;
            strValue = "0123456789.-";
            if (((strValue.IndexOf(KeyCode)) >= 0) || (KeyCode == "\b"))
            {
                bln = false;
            }
            return bln;
        }

        //Only Allow Number Related Character
        public Boolean GTRNumber(string KeyCode)
        {
            Boolean bln = true;

            string strValue;
            strValue = "0123456789";
            if (((strValue.IndexOf(KeyCode)) >= 0) || (KeyCode == "\b"))
            {
                bln = false;
            }
            return bln;
        }

        //Formatting Number
        public string GTRFormatNumber(String str)
        {
            double d;
            if (str.Length == 0)
            {
                str = "0";
            }
            d = double.Parse(str);
            return d.ToString("##,##0.0000");
        }

        //Make Upper Case to passing passing parameter
        public string GTRUpperCase(string str)
        {
            return str.ToUpper().Trim();
        }


        //Make Lower Case to passing passing parameter
        public string GTRLowerCase(string str)
        {
            return str.ToLower().Trim();
        }


        //Get Computer Name
        public string GTRGetComputerName()
        {
            return Environment.MachineName;
        }


        //Get Computer Name
        public string strComputerName()
        {
            return Environment.MachineName;
        }


        //Get Web Client Computer Name
        public string GTRGetComputerNameWeb(String clientIP)
        {
            //Request.UserHostAddress as param from client
            var HostEntry = System.Net.Dns.GetHostEntry(clientIP);
            return HostEntry.HostName;
        }
        //Get Mac Address
        public string GTRGetMacAddress()
        {
            foreach (NetworkInterface NIC in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (NIC.NetworkInterfaceType == NetworkInterfaceType.Ethernet && NIC.OperationalStatus == OperationalStatus.Up)
                {
                    return NIC.GetPhysicalAddress().ToString();
                }

                if (NIC.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && NIC.OperationalStatus == OperationalStatus.Up)
                {
                    return NIC.GetPhysicalAddress().ToString();
                }
            }
            return "";
        }


        //Get IP Address
        public string GTRGetIPAddress()
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                return "";
            }

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            return host
                .AddressList
                .FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork
                ).ToString();
        }

        #region Amzad dll function

        //Format datetime
        public string GTRDate(string strDate)
        {
            if (strDate.Length == 0)
            {
                return "";
            }
            DateTime dt = DateTime.Parse(strDate);
            return string.Format("{0:dd}", dt) + "-" + string.Format("{0:MMM}", dt) + "-" + string.Format("{0:yyyy}", dt);
        }

        public string GTRTime(string strDate)
        {
            if (strDate.Length == 0)
            {
                return "";
            }
            DateTime dt = DateTime.Parse(strDate);
            return string.Format("{0:hh}", dt) + ":" + string.Format("{0:mm}", dt);
        }

        //Find Last Day Of Month
        public Int16 GTRLastDayOfMonth(Int32 intYear, Int16 intMonth)
        {
            Int16 intDays = (Int16)DateTime.DaysInMonth(intYear, intMonth);
            return intDays;
        }

        //Total Days In Month
        public Int16 GTRTotalDaysInMonth(Int32 intYear, Int16 intMonth)
        {
            Int16 intDays = (Int16)DateTime.DaysInMonth(intYear, intMonth);
            return intDays;
        }

        //Month Name Short Style
        public string GTRMonthShortName(Int16 monthNo)
        {
            string returnValue = "";
            switch (monthNo)
            {
                case 1:
                    returnValue = "Jan";
                    break;
                case 2:
                    returnValue = "Feb";
                    break;
                case 3:
                    returnValue = "Mar";
                    break;
                case 4:
                    returnValue = "Apr";
                    break;
                case 5:
                    returnValue = "May";
                    break;
                case 6:
                    returnValue = "Jun";
                    break;
                case 7:
                    returnValue = "Jul";
                    break;
                case 8:
                    returnValue = "Aug";
                    break;
                case 9:
                    returnValue = "Sep";
                    break;
                case 10:
                    returnValue = "Oct";
                    break;
                case 11:
                    returnValue = "Nov";
                    break;
                case 12:
                    returnValue = "Dec";
                    break;
            }
            return returnValue;
        }

        //Validating String or return blank
        public string GTRValidateString(string value)
        {
            string str = "";
            double dbl;
            try
            {
                if (Double.TryParse(value, out dbl) == true)
                {
                    str = "";
                }
                else
                {
                    str = value;
                }
            }
            catch (Exception)
            {
                str = value;
            }
            return str;
        }

        //Validating double or return 0
        public Double GTRValidateDouble(string value)
        {
            Double dbl;
            try
            {
                dbl = Double.Parse(value);
            }
            catch (Exception)
            {
                dbl = 0;
            }
            return dbl;
        }

        //check even number
        public Boolean GTRIsEvenNumber(Int32 num)
        {
            Boolean bln = false;
            if (num % 2 == 0)
            {
                bln = true;
            }
            return bln;
        }

        //check odd number
        public Boolean GTRIsOddNumber(Int32 num)
        {
            Boolean bln = false;
            if (num % 2 != 0)
            {
                bln = true;
            }
            return bln;
        }

        //Format Currency In USD or International
        public string GTRFormatCurrencyUSD(string num)
        {
            string returnValue = "";
            try
            {
                returnValue = double.Parse(num).ToString("##,###.##");
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return returnValue;
        }

        //Format Currency in BDT
        public string GTRFormatCurrencyBDT(string num)
        {
            string returnValue = "";
            string strNegSymbol = "";
            try
            {
                if ((num.Substring(0, 1).ToString() == "-"))
                    strNegSymbol = "-";

                num = num.Replace(",", "");

                string strFPS = "n" + string.Format("{0}", 2);
                num = string.Format("{0:" + strFPS + "}", Math.Abs(double.Parse(num)));
                string GeneralValue = "0";
                string floatValue = "0";
                int i = num.IndexOf(".");

                GeneralValue = num;

                if (i > 0)
                {
                    GeneralValue = num.Substring(0, num.IndexOf(".")).Replace(",", "");
                    floatValue = num.Substring(num.IndexOf(".") + 1, num.Length - (num.IndexOf(".") + 1));
                }

                string gv = "";
                char[] c = GeneralValue.ToCharArray();
                Array.Reverse(c);
                GeneralValue = new string(c);

                int startDed = 0;
                int dedChar = 3;
                for (int m = 0; m <= GeneralValue.Length - 1; m++)
                {
                    if (startDed + dedChar >= GeneralValue.Length)
                    {
                        gv = gv + GeneralValue.Substring(startDed, GeneralValue.Length - startDed) + ",";
                        break; // TODO: might not be correct. Was : Exit For
                    }
                    else
                    {
                        gv = gv + GeneralValue.Substring(startDed, dedChar) + ",";
                    }

                    startDed = startDed + dedChar;
                    dedChar = 2;
                }

                GeneralValue = gv.Substring(0, gv.Length - 1);
                c = GeneralValue.ToCharArray();
                Array.Reverse(c);
                GeneralValue = new string(c);
                if (floatValue.Length == 0)
                {
                    floatValue = "00";
                }
                else if (floatValue.Length == 1)
                {
                    floatValue += "0";
                }
                returnValue = strNegSymbol + GeneralValue + "." + floatValue;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return returnValue;
        }

        //With floating point
        public string GTRFormatCurrencyBDT(string num, Int16 floatingPointSize)
        {
            string returnValue = "";
            string strNegSymbol = "";
            try
            {
                if ((num.Substring(0, 1).ToString() == "-"))
                    strNegSymbol = "-";

                num = num.Replace(",", "");

                string strFPS = "n" + string.Format("{0}", floatingPointSize.ToString());
                num = string.Format("{0:" + strFPS + "}", Math.Abs(double.Parse(num)));
                string GeneralValue = "0";
                string floatValue = "0";
                int i = num.IndexOf(".");

                GeneralValue = num;

                if (i > 0)
                {
                    GeneralValue = num.Substring(0, num.IndexOf(".")).Replace(",", "");
                    floatValue = num.Substring(num.IndexOf(".") + 1, num.Length - (num.IndexOf(".") + 1));
                }

                string gv = "";
                char[] c = GeneralValue.ToCharArray();
                Array.Reverse(c);
                GeneralValue = new string(c);

                int startDed = 0;
                int dedChar = 3;
                for (int m = 0; m <= GeneralValue.Length - 1; m++)
                {
                    if (startDed + dedChar >= GeneralValue.Length)
                    {
                        gv = gv + GeneralValue.Substring(startDed, GeneralValue.Length - startDed) + ",";
                        break; // TODO: might not be correct. Was : Exit For
                    }
                    else
                    {
                        gv = gv + GeneralValue.Substring(startDed, dedChar) + ",";
                    }

                    startDed = startDed + dedChar;
                    dedChar = 2;
                }

                GeneralValue = gv.Substring(0, gv.Length - 1);
                c = GeneralValue.ToCharArray();
                Array.Reverse(c);
                GeneralValue = new string(c);
                if (floatValue.Length == 0)
                {
                    floatValue = "00";
                }
                else if (floatValue.Length == 1)
                {
                    floatValue += "0";
                }

                returnValue = strNegSymbol + GeneralValue + "." + floatValue;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return returnValue;
        }

        //Convert Currency To Number
        public string GTRConvertCurrToNum(string num)
        {
            return num.Replace(",", "");
        }

        //Encryption Method
        public string GTREncryptWord(string s)
        {
            int crack;
            crack = 13;

            char T1; string T2;
            T2 = "";

            for (int i = 0; i < s.Length; i++)
            {
                T1 = Convert.ToChar(s.Substring(i, 1));
                T2 = T2 + Convert.ToChar(T1 + crack);
            }
            return T2;
        }

        //Decryption Method
        public string GTRDecryptWord(string s)
        {
            int crack;
            crack = 13;

            char T1; string T2;
            T2 = "";

            for (int i = 0; i < s.Length; i++)
            {
                T1 = Convert.ToChar(s.Substring(i, 1));
                T2 = T2 + Convert.ToChar(T1 - crack);
            }
            return T2;
        }

        //Get System Dirtory Pat
        public string GTRSystemDir()
        {
            return Environment.SystemDirectory;
        }

        //Get Only Numeric value from a string
        public string GTRAID(string saID)
        {
            string returnValue = "";
            char[] c = saID.ToCharArray();

            for (Int16 i = 0; i <= saID.Length - 1; i++)
            {
                if (char.IsNumber(c[i]))
                {
                    returnValue += c[i].ToString();
                }
            }
            return returnValue;
        }

        //Get Last Number from a IP Address to find out device no
        public string GTRIpAddressLast(string ipAddress)
        {
            string ReturnValue = "0";
            //check parameter IP Address is blank or not
            if (ipAddress.Length == 0 || ipAddress == string.Empty)
            {
                return ReturnValue;
            }

            char[] c = ipAddress.ToCharArray();
            Array.Reverse(c);
            ipAddress = new string(c);
            c = ipAddress.Substring(0, ipAddress.IndexOf(".")).ToCharArray();
            Array.Reverse(c);
            ReturnValue = new string(c);

            return ReturnValue;
        }

        public string GTRInwordsFormatBD(string M, string PreFix, string PostFix)
        {
            clsInWords clsIW = new clsInWords();
            return clsIW.GTRInwordsBDFromat(M, PreFix, PostFix);
        }

        public string GTRInwordsFormatUS(string M, string PreFix, string PostFix)
        {
            clsInWords clsIW = new clsInWords();
            return clsIW.GTRInwordsForegnFromat(M, PreFix, PostFix);
        }


        public string GTRInwordsBangla(string M, string PreFix, string PostFix)
        {
            clsInWordsBangla clsIW = new clsInWordsBangla();
            return clsIW.GTRInwordsBDFromat(M, PreFix, PostFix);
        }




        #endregion Amzad dll function

        #region Number Conversion

        //Convert Hex To Dec
        public string GTRHexToDec(string proxCardNo)
        {
            string str = int.Parse(proxCardNo, NumberStyles.HexNumber).ToString();
            switch (str.Length)
            {

                case 5:
                    str = "00000" + str;
                    break;
                case 6:
                    str = "0000" + str;
                    break;
                case 7:
                    str = "000" + str;
                    break;
                case 8:
                    str = "00" + str;
                    break;
                case 9:
                    str = "0" + str;
                    break;
            }
            return str;
        }

        //Convert Dec To Hex
        public string GTRDecToHex(string proxCardNo)
        {
            string str = int.Parse(proxCardNo).ToString("X");
            ///string str = number.ToString("X");
            switch (str.Length)
            {
                case 5:
                    str = "00000" + str;
                    break;
                case 6:
                    str = "0000" + str;
                    break;
                case 7:
                    str = "000" + str;
                    break;
                case 8:
                    str = "00" + str;
                    break;
                case 9:
                    str = "0" + str;
                    break;
            }
            return str;
        }

        //Rounding Down
        public Double GTRRoundDown(Double number)
        {
            //Int32 i = 0;
            double returnValue = Math.Floor(number);//number;

            //i = number.ToString().IndexOf(".");
            //if (i > 0)
            //{
            //    returnValue = double.Parse(number.ToString().Substring(0, i));
            //}
            return returnValue;
        }

        //Rounding up
        public Double GTRRoundup(Double number)
        {
            //Int32 i = 0;
            double returnValue = Math.Ceiling(number);//number;

            //i = number.ToString().IndexOf(".");
            //if (i > 0)
            //{
            //    returnValue = double.Parse(number.ToString().Substring(0, i)) + 1;
            //}
            return returnValue;
        }

        #endregion Number Conversion

        #region Database Related

        //Count Column Within a Table
        //public int GTRCountColumn(string tableName)
        //{
        //    clsConnectionNew clsCon = new clsConnectionNew(false);
        //    string sqlQuery = "select count(Name) from syscolumns where id=(select id from sysobjects where name='" + tableName + "')";
        //    return clsCon.GTRCountingData(sqlQuery);
        //}

        ////Generate Serial No
        //public string GTRGenerateSLNo(string sqlQuery)
        //{
        //    clsConnectionNew clsCon = new clsConnectionNew(false);
        //    return (clsCon.GTRCountingData(sqlQuery) + 1).ToString();
        //}

        //Generate Serial No With Format
        //public string GTRGenerateSLNoFormated(string sqlQuery)
        //{
        //    int i = 0;
        //    string str = "";
        //    clsConnectionNew clsCon = new clsConnectionNew(false);

        //    i = clsCon.GTRCountingData(sqlQuery) + 1;
        //    str = GTRFormatNumber(i.ToString());
        //    return str;
        //}

        #endregion Database Related

        #region Inwords

        //public string GTRInwordsFormatBD(string M, string PreFix, string PostFix)
        //{
        //    clsInWords clsIW = new clsInWords();
        //    return clsIW.GTRInwordsBDFromat(M, PreFix, PostFix);
        //}

        //public string GTRInwordsFormatUS(string M, string PreFix, string PostFix)
        //{
        //    //clsInWords clsIW = new clsInWords();
        //    //return clsIW.GTRInwordsForegnFromat(M, PreFix, PostFix);
        //}

        #endregion Inwords

        #region XML

        public string GTRReadXML(string strFlag)
        {
            string str = "", xmlFileName = "", strElement = "";
            XmlTextReader reader = null;

            xmlFileName = "GTRMaster.xml";
            reader = new XmlTextReader(xmlFileName);

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        strElement = reader.Name;
                        break;
                    case XmlNodeType.Text:
                        if (strFlag.Trim().ToUpper() == strElement.Trim().ToUpper())
                        {
                            str = reader.Value;
                        }
                        break;
                }
            }
            return str;
        }

        public string GTRXMLReader(string strFlag)
        {
            string str = "";
            str = GTRReadXML(strFlag);
            return str;
        }

        #endregion XML

        //public void GTRQRCode(string Str)
        //{
        //    QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
        //    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;

        //    try
        //    {
        //        qrCodeEncoder.QRCodeScale = 4;
        //        //int version = Convert.ToInt16(cboVersion.Text);
        //        qrCodeEncoder.QRCodeVersion = 7;
        //    }
        //    catch (Exception ex)
        //    {
        //        //MessageBox.Show("Invalid version !");
        //    }


        //    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
        //    Image image;
        //    String data = Str;
        //    image = qrCodeEncoder.Encode(data);

        //    string path;
        //    if (!Directory.Exists("C:\\QRCode\\"))
        //    {
        //        Directory.CreateDirectory("C:\\QRCode\\");
        //    }
        //    path = "C:\\QRCode\\" + Str + ".gif";
        //    image.Save(path);
        //}

        //public void GTRQRCode(string StrName, string strValue)
        //{
        //    QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
        //    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;

        //    try
        //    {
        //        qrCodeEncoder.QRCodeScale = 4;
        //        //int version = Convert.ToInt16(cboVersion.Text);
        //        qrCodeEncoder.QRCodeVersion = 7;
        //    }
        //    catch (Exception ex)
        //    {
        //        //MessageBox.Show("Invalid version !");
        //    }


        //    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
        //    Image image;
        //    String data = strValue;
        //    image = qrCodeEncoder.Encode(data);

        //    string path;
        //    if (!Directory.Exists("C:\\QRCode\\"))
        //    {
        //        Directory.CreateDirectory("C:\\QRCode\\");
        //    }
        //    path = "C:\\QRCode\\" + StrName + ".gif";
        //    image.Save(path);

        //}
    }

}
