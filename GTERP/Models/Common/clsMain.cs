using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace GTERP.Models.Common
{
    internal class clsMain
    {
        #region Variable
        public static DataSet dsConfigure;
        public static DataSet dsConfigure1;
        //public static strFormat = "PDF";


        public static string AppPath = "";      //Store Application path

        public static Int16 intCurrency = 1;   //Default Currecny

        public static Int32 intUserId = 0;        //Store User Id
        public static string strUser = "";          //Store User Name
        public static string strUserCode = "";          //Store User Code

        public static Int32 intSGroupId = 0;      // Store User Sub Group Id
        public static string strSGroupName = "";  // Store User Sub Group Name

        public static Int32 intGroupId = 0;       //Store User Group Id
        public static string strGroupName = "";   //Store User Group Name

        public static string strComputerName = "";  //Computer Name
        public static string strMacAddress = "";    //Mac Address
        public static string strIPAddress = "";     //IP Address

        public static string strTranDate = "";      //Transaction Date

        public static string strRelationalId = "0"; // For Supplier, Customer, Product List Will Connect With Transaction Form
        public static string IsApprubed = "0"; // For Trnsfer Location Form
        public static bool boolDirectConvertQ2WO = false;

        public static Int16 intComId = 0;           // Store Sister Concer / Company Id
        public static string strComName = "";       // Store Sister Concer / Company Name

        public static string DefaultDB = "";       // Database Inventory
        public static string dbPACC = "";       // Database Inventory
        public static string dbACC = "";       // Database Inventory
        public static string dbLAB = "";       // Database Inventory
        public static string dbCOP = "";       // Database Inventory
        public static string dbSYS = "";       // Database Inventory
        public static string dbPOS = "";       // Database HRIS
        public static string dbTLOG = "";       // Database Tran
        public static string dbTLOGMasterUser = "";       // Database Tran
        public static int isMaster = 0;       // Database Tran
        public static int isClientMaster = 0;       // Database Tran

        public static int isBarcode = 0;       // Database Tran
        public static int isProduct = 0;       // Database Tran
        public static int isCorporate = 0;       // Database Tran
        public static int isPOSPrint = 0;       // Database Tran
        public static int IsService = 0;       // Database Tran
        public static int IsOldDue = 0;       // Database Tran
        public static int isShortcutSale = 0;       // Database Tran
        public static int isRestaurantSale = 0;       // Database Tran
        public static int isTouch = 0;       // Database Tran
        public static int isShoeSale = 0;       // Database Tran
        public static int isDashboard = 1;       // Database Tran




        public static int isIMEISale = 0;       // IMEI Tran
        public static int isMultipleWH = 0;       // MultipleWh Tran
        public static int isFixedSalesValue = 1;       // MultipleWh Tran



        public static int BaseCountryID = 0;       // Database Tran
        public static int BaseComId = 0;       // Database Tran
        public static int DecimalField = 0;       // Database Tran






        public static string saveid = "0";
        #region Picture Path variable

        //        public static string appPath = Path.GetDirectoryName(Application.ExecutablePath);

        //public static string strPicPathCategory = appPath + @"\Com\Pics\Inventory\Category";
        //public static string strPicPathProdut = appPath + @"\Com\Pics\Inventory\Product";
        //public static string strPicPathTable = appPath + @"\Com\Pics\Inventory\Table";



        //public static string strPicPathCmps = appPath + @"\Com\Pics\Cmps";
        //public static string strPicPathIcon = appPath + @"\Com\Pics\Icon";


        public static string strDocumentsLocation = "Z:" + @"\Com\Documents";

        public static string strReportName = "";
        public static string strFormat = "";
        public static string strExtension = "";




        #endregion

        #region Report Related Variable

        public static string strReportPathMain = "";
        public static string strDSNMain = "";
        public static string strQueryMain = "";

        public static Int16 intHasSubReport = 0;
        public static string strRelationalField = "";
        public static string strDSNSub = "";
        public static string strQuerySub = "";

        #endregion

        //To store the list of opened form
        public static ArrayList openForm = new ArrayList();

        //To store the list of Menu
        public static ArrayList alMnuFrmName = new ArrayList();
        public static ArrayList alMnuImgName = new ArrayList();

        #region Barcode related variable
        public static string[] strBarcode = new string[2];
        #endregion

        #endregion Variable

        #region Constant

        public const string cnstGTRDateFormat = "dd MMM yyyy";  // Date Format

        #endregion Constant        

        #region XML Reader

        public string fncReadXML(string strFlag)
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

        #endregion XML Reader

        #region Reflection
        //public object GTRMakeFormNameAsObject(string strLocation, string strForm, ref Infragistics.Win.UltraWinTabControl.UltraTabControl utab, Common.FormEntry.frmMaster fm)
        //{
        //    object obj = null;
        //    Type oType = Assembly.GetExecutingAssembly().GetType(strLocation + strForm);
        //    Type[] parameterTypes = new Type[] { typeof(Infragistics.Win.UltraWinTabControl.UltraTabControl).MakeByRefType(), typeof(Common.FormEntry.frmMaster) };
        //    ConstructorInfo constructor = oType.GetConstructor(parameterTypes);
        //    if (oType != null)
        //    {
        //        obj = constructor.Invoke(new Object[] { utab, fm });
        //    }
        //    return obj;
        //}

        //public Form GTRMakeFormNameAsForm(string strFormName)
        //{
        //    Form frm = null;
        //    Type oType = Assembly.GetExecutingAssembly().GetType(strFormName);
        //    if (oType != null)
        //    {
        //        frm = ((Form)(Activator.CreateInstance(oType)));
        //    }
        //    return frm;
        //}
        //#endregion Reflection

        //#region User Login Log
        //public void prcLogin()
        //{
        //    clsConnection clsCon = new clsConnection();
        //    try
        //    {
        //        int Result = 0;
        //        string sqlQuery = "";
        //        sqlQuery = "Insert Into tblLogin_Activity_Log (LUserId, LoginDate, LoginStartTime, LoginPCName, LoginPCIP, LoginPCMac) "
        //            + " Select " + Common.Classes.clsMain.intUserId + ", convert(varchar,GETDATE(),107), GETDATE(), '" + Common.Classes.clsMain.strComputerName + "', '" + Common.Classes.clsMain.strIPAddress + "', '" + Common.Classes.clsMain.strRelationalId + "'";
        //        Result = clsCon.GTRSaveDataWithSQLCommand(sqlQuery);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //    finally
        //    {
        //        clsCon = null;
        //    }
        //}

        //public void prcLogout()
        //{
        //    clsConnection clsCon = new clsConnection();
        //    try
        //    {
        //        int Result = 0;
        //        string sqlQuery = "";
        //        sqlQuery = "Update tblLogin_Activity_Log Set LoginEndTime = GETDATE() Where SLNo = "
        //            + "(Select MAX(SLNo) From tblLogin_Activity_Log Where LUserId = " + Common.Classes.clsMain.intUserId + ")";
        //        Result = clsCon.GTRSaveDataWithSQLCommand(sqlQuery);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //    finally
        //    {
        //        clsCon = null;
        //    }
        //}
        //#endregion

        //#region Form Open Related Transaction (frmMaster Form Tab Control)
        //public static Boolean fncExistOpenForm(Form frm)
        //{
        //    //fncExistOpenForm call to add list to arraylist byDefault
        //    return fncExistOpenForm(frm, "Add");
        //}

        ////Static function to access direct from caller function
        //public static Boolean fncExistOpenForm(Form frm, string flag)
        //{
        //    //creating object of this class
        //    clsMain com = new clsMain();

        //    //Find the index no of searching form
        //    int index = fncFindOpenFormIndex(frm);

        //    Boolean bln = false;
        //    if (index >= 0)
        //    {
        //        if (flag != "Add")
        //        {
        //            //flag : Remove
        //            com.prcRemoveOpenForm(index);
        //        }
        //        bln = true;
        //    }
        //    else
        //    {
        //        if (flag == "Add")
        //        {
        //            //flag : Add
        //            com.prcAddOpenForm(frm);
        //        }
        //    }
        //    return bln;
        //}

        //private void prcAddOpenForm(Form frm)
        //{
        //    //add object to arraylist
        //    openForm.Add(frm);
        //}

        //private void prcRemoveOpenForm(int index)
        //{
        //    //remove object from arraylist
        //    openForm.RemoveAt(index);
        //}

        ////searching & return index no of provided form object
        //public static int fncFindOpenFormIndex(Form frm)
        //{
        //    string strFindFormName = frm.Name.ToString();
        //    int index = -1;
        //    for (int i = 0; i < openForm.Count; i++)
        //    {
        //        string value = ((Form)openForm[i]).Name.ToString();
        //        if (value == strFindFormName)
        //        {
        //            index = i;
        //            break;
        //        }
        //    }
        //    return index;
        //}
        //#endregion Form Open Related Transaction (frmMaster Form Tab Control)

        //#region configuration
        //public static void SetConfiguration()


        //{
        //    dsConfigure = new DataSet();
        //    Common.Classes.clsConnection clscon = new Common.Classes.clsConnection();
        //    try
        //    {
        //        string SQLQuery = "Select moduleId, flagName, flagValue from tblConfiguration";
        //        clscon.GTRFillDatasetWithSQLCommand(ref dsConfigure, SQLQuery);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //    finally
        //    {
        //        clscon = null;
        //    }

        //}
        //public static string GetConfiguration(Int32 moduleId, string flagName)
        //{
        //    string returnValue = "";
        //    DataRow[] dr = dsConfigure.Tables[0].Select("moduleId =" + moduleId + " and flagName = '" + flagName + "'");
        //    foreach (DataRow dr2 in dr)
        //    {
        //        returnValue = dr2["flagValue"].ToString();
        //    }
        //    return returnValue;
        //}
        //#endregion


        //#region configuration1
        //public static void SetConfiguration1()
        //{
        //    dsConfigure1 = new DataSet();
        //    Common.Classes.clsConnection clscon = new Common.Classes.clsConnection(Common.Classes.clsMain.dbPACC.ToString());
        //    try
        //    {
        //        string SQLQuery = "Select moduleId, flagName, flagValue from tblConfiguration";
        //        clscon.GTRFillDatasetWithSQLCommand(ref dsConfigure1, SQLQuery);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //    finally
        //    {
        //        clscon = null;
        //    }

        //}
        //public static string GetConfiguration1(Int32 moduleId, string flagName)
        //{
        //    string returnValue = "";
        //    DataRow[] dr = dsConfigure1.Tables[0].Select("moduleId =" + moduleId + " and flagName = '" + flagName + "'");
        //    foreach (DataRow dr2 in dr)
        //    {
        //        returnValue = dr2["flagValue"].ToString();
        //    }
        //    return returnValue;
        //}
        //#endregion

        //public static string Encrypt(string toEncrypt, bool useHashing)
        //{
        //    byte[] keyArray;
        //    byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

        //    System.Configuration.AppSettingsReader settingsReader =
        //                                        new AppSettingsReader();
        //    //Get the key from config file

        //    string key = (string)settingsReader.GetValue("SecurityKey",
        //                                                     typeof(String));
        //    System.Windows.Forms.MessageBox.Show(key);
        //    //If hashing use get hashcode regards to your key
        //    if (useHashing)
        //    {
        //        MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
        //        keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
        //        //Always release the resources and flush data
        //        // of the Cryptographic service provide. Best Practice

        //        hashmd5.Clear();
        //    }
        //    else
        //        keyArray = UTF8Encoding.UTF8.GetBytes(key);

        //    TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
        //    //set the secret key for the tripleDES algorithm
        //    tdes.Key = keyArray;
        //    //mode of operation. there are other 4 modes.
        //    //We choose ECB(Electronic code Book)
        //    tdes.Mode = CipherMode.ECB;
        //    //padding mode(if any extra byte added)

        //    tdes.Padding = PaddingMode.PKCS7;

        //    ICryptoTransform cTransform = tdes.CreateEncryptor();
        //    //transform the specified region of bytes array to resultArray
        //    byte[] resultArray =
        //      cTransform.TransformFinalBlock(toEncryptArray, 0,
        //      toEncryptArray.Length);
        //    //Release resources held by TripleDes Encryptor
        //    tdes.Clear();
        //    //Return the encrypted data into unreadable string format
        //    return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        //}

        //public static string Decrypt(string cipherString, bool useHashing)
        //{
        //    byte[] keyArray;
        //    //get the byte code of the string

        //    byte[] toEncryptArray = Convert.FromBase64String(cipherString);

        //    System.Configuration.AppSettingsReader settingsReader =
        //                                        new AppSettingsReader();
        //    //Get your key from config file to open the lock!
        //    string key = (string)settingsReader.GetValue("SecurityKey",
        //                                                 typeof(String));

        //    if (useHashing)
        //    {
        //        //if hashing was used get the hash code with regards to your key
        //        MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
        //        keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
        //        //release any resource held by the MD5CryptoServiceProvider

        //        hashmd5.Clear();
        //    }
        //    else
        //    {
        //        //if hashing was not implemented get the byte code of the key
        //        keyArray = UTF8Encoding.UTF8.GetBytes(key);
        //    }

        //    TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
        //    //set the secret key for the tripleDES algorithm
        //    tdes.Key = keyArray;
        //    //mode of operation. there are other 4 modes. 
        //    //We choose ECB(Electronic code Book)

        //    tdes.Mode = CipherMode.ECB;
        //    //padding mode(if any extra byte added)
        //    tdes.Padding = PaddingMode.PKCS7;

        //    ICryptoTransform cTransform = tdes.CreateDecryptor();
        //    byte[] resultArray = cTransform.TransformFinalBlock(
        //                         toEncryptArray, 0, toEncryptArray.Length);
        //    //Release resources held by TripleDes Encryptor                
        //    tdes.Clear();
        //    //return the Clear decrypted TEXT
        //    return UTF8Encoding.UTF8.GetString(resultArray);
        //}

        //public static string GetMACAddress()
        //{
        //    ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
        //    ManagementObjectCollection moc = mc.GetInstances();
        //    string MACAddress = String.Empty;

        //    foreach (ManagementObject mo in moc)
        //    {
        //        if (MACAddress == String.Empty) // only return MAC Address from first card   
        //        {
        //            if ((bool)mo["IPEnabled"] == true) MACAddress = mo["MacAddress"].ToString();
        //        }
        //        mo.Dispose();
        //    }

        //    MACAddress = MACAddress.Replace(":", "");
        //    return MACAddress;
        //}


        //public Bitmap _currentBitmap;
        //#region image resize
        //public void Resize(int newWidth, int newHeight)
        //{
        //    if (newWidth != 0 && newHeight != 0)
        //    {
        //        Bitmap temp = (Bitmap)_currentBitmap;
        //        Bitmap bmap = new Bitmap(newWidth, newHeight, temp.PixelFormat);

        //        double nWidthFactor = (double)temp.Width / (double)newWidth;
        //        double nHeightFactor = (double)temp.Height / (double)newHeight;

        //        double fx, fy, nx, ny;
        //        int cx, cy, fr_x, fr_y;
        //        Color color1 = new Color();
        //        Color color2 = new Color();
        //        Color color3 = new Color();
        //        Color color4 = new Color();
        //        byte nRed, nGreen, nBlue;

        //        byte bp1, bp2;

        //        for (int x = 0; x < bmap.Width; ++x)
        //        {
        //            for (int y = 0; y < bmap.Height; ++y)
        //            {

        //                fr_x = (int)Math.Floor(x * nWidthFactor);
        //                fr_y = (int)Math.Floor(y * nHeightFactor);
        //                cx = fr_x + 1;
        //                if (cx >= temp.Width) cx = fr_x;
        //                cy = fr_y + 1;
        //                if (cy >= temp.Height) cy = fr_y;
        //                fx = x * nWidthFactor - fr_x;
        //                fy = y * nHeightFactor - fr_y;
        //                nx = 1.0 - fx;
        //                ny = 1.0 - fy;

        //                color1 = temp.GetPixel(fr_x, fr_y);
        //                color2 = temp.GetPixel(cx, fr_y);
        //                color3 = temp.GetPixel(fr_x, cy);
        //                color4 = temp.GetPixel(cx, cy);

        //                // Blue
        //                bp1 = (byte)(nx * color1.B + fx * color2.B);

        //                bp2 = (byte)(nx * color3.B + fx * color4.B);

        //                nBlue = (byte)(ny * (double)(bp1) + fy * (double)(bp2));

        //                // Green
        //                bp1 = (byte)(nx * color1.G + fx * color2.G);

        //                bp2 = (byte)(nx * color3.G + fx * color4.G);

        //                nGreen = (byte)(ny * (double)(bp1) + fy * (double)(bp2));

        //                // Red
        //                bp1 = (byte)(nx * color1.R + fx * color2.R);

        //                bp2 = (byte)(nx * color3.R + fx * color4.R);

        //                nRed = (byte)(ny * (double)(bp1) + fy * (double)(bp2));

        //                bmap.SetPixel(x, y, System.Drawing.Color.FromArgb(255, nRed, nGreen, nBlue));
        //            }
        //        }
        //        _currentBitmap = (Bitmap)bmap.Clone();
        //    }
        //}
        //#endregion image resize

        //public enum Dimensions
        //{
        //    Width,
        //    Height
        //}
        //public static Image ConstrainProportions(Image imgPhoto, int Size, Dimensions Dimension)
        //{
        //    int sourceWidth = imgPhoto.Width;
        //    int sourceHeight = imgPhoto.Height;
        //    int sourceX = 0;
        //    int sourceY = 0;
        //    int destX = 0;
        //    int destY = 0;
        //    float nPercent = 0;

        //    switch (Dimension)
        //    {
        //        case Dimensions.Width:
        //            nPercent = ((float)Size / (float)sourceWidth);
        //            break;
        //        default:
        //            nPercent = ((float)Size / (float)sourceHeight);
        //            break;
        //    }

        //    int destWidth = (int)(sourceWidth * nPercent);
        //    int destHeight = (int)(sourceHeight * nPercent);

        //    Bitmap bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
        //    bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

        //    Graphics grPhoto = Graphics.FromImage(bmPhoto);
        //    grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

        //    grPhoto.DrawImage(imgPhoto,
        //    new Rectangle(destX, destY, destWidth, destHeight),
        //    new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
        //    GraphicsUnit.Pixel);

        //    grPhoto.Dispose();
        //    return bmPhoto;
        //}


        //#region Menu Image
        public static void prcAddMenuName(string strMenuFrmName, string strMenuImageName)
        {
            //add object to arraylist
            alMnuFrmName.Add(strMenuFrmName);
            alMnuImgName.Add(strMenuImageName);
        }

        public static string CheckIsPhoneNumber(string str)
        {
            str = Regex.Replace(str, @"\s+", "");
            if (string.IsNullOrWhiteSpace(str)) return "NAN";
            var numberArray = str.Split(',');

            var num = new String(numberArray[0].Where(Char.IsDigit).ToArray());

            var len = num.Length;
            if (len < 10 || len > 13 || len == 12) return "NAN";
            if (len == 13)
            {
                string firstTow = num.Substring(0, 2);
                if (firstTow == "88")
                {
                    string firstFive = num.Substring(0, 5);
                    if (firstFive == "88016" || firstFive == "88015" || firstFive == "88017" || firstFive == "88018" ||
                        firstFive == "88019" || firstFive == "88011" || firstFive == "88013" || firstFive == "88014")
                    {
                        return num.Substring(2, 11);
                    }
                    return "NAN";
                }
                return "NAN";

            }
            if (len == 10)
            {
                var fristTwo = num.Substring(0, 2);
                if (fristTwo == "16" || fristTwo == "15" || fristTwo == "17" || fristTwo == "18" ||
                    fristTwo == "19" || fristTwo == "11" || fristTwo == "13" || fristTwo == "14")
                {
                    return "0" + num;
                }
                return "NAN";
            }
            if (len == 11)
            {
                var firstThree = num.Substring(0, 3);
                if (firstThree == "016" || firstThree == "015" || firstThree == "017" || firstThree == "018" ||
                    firstThree == "019" || firstThree == "011" || firstThree == "013" || firstThree == "014")
                {
                    return num;
                }
                return "NAN";
            }
            return "NAN";
        }

        //searching & return index no of opening menu
        //public static int fncFindMenuImageIndex(string strMenuFrmName)
        //{
        //    string strFindFormName = strMenuFrmName;
        //    int index = -1;
        //    for (int i = 0; i < openForm.Count; i++)
        //    {
        //        string value = ((Form)openForm[i]).Name.ToString();
        //        if (value == strFindFormName)
        //        {
        //            index = i;
        //            break;
        //        }
        //    }
        //    return index;
        //}
        #endregion Menu Image
    }
}