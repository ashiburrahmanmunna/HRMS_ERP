using Microsoft.VisualBasic;
using System;


namespace GTERP.Models.Common
{
    internal class clsInWords
    {
        #region InWords Bangladeshi Format

        public string GTRInwordsBDFromat(string M, string PreFix, string PostFix)
        {
            M = double.Parse(M).ToString("#00.00");

            string functionReturnValue = null;
            string sDlr = null;
            int iCent = 0;
            int icentPos = 0;
            string SDlrWord = null;
            string SCentWord = null;
            icentPos = Strings.InStr(M, ".");
            sDlr = Strings.Mid(M, 1, icentPos - 1);
            iCent = int.Parse(Strings.Mid(M, icentPos + 1, Strings.Len(M)));
            SDlrWord = GTRNumberToWords(Convert.ToInt64(sDlr));

            if (iCent > 0)
            {
                SDlrWord = GTRNumberToWords(Convert.ToInt64(sDlr));
                SCentWord = "and " + Strings.Replace(GTRNumberToWords(Convert.ToInt64(iCent)), "", "");
                functionReturnValue = PreFix + " " + SDlrWord + " " + SCentWord + " Paisa" + PostFix;
            }
            else
            {
                functionReturnValue = PreFix + " " + SDlrWord + " " + PostFix;
            }
            return functionReturnValue;
        }

        public string GTRNumberToWords(long OrigNum)
        {
            string functionReturnValue = null;
            //This function converts numbers to words. For example 101 -> One hundred and one
            //It uses standard english notation and will only accept positive long numbers
            long billionpart = 0;
            long millionpart = 0;

            billionpart = Conversion.Int(OrigNum / 10000000);
            millionpart = OrigNum % 10000000;
            functionReturnValue = GTRHundredsToWords(billionpart) + (billionpart != 0 ? " Crore" : "");

            if (millionpart > 99)
            {
                functionReturnValue = functionReturnValue + (millionpart != 0 & billionpart != 0 ? " " : "") +
                                      GTRmillionstowords(millionpart);
            }
            else
            {
                functionReturnValue = functionReturnValue + (millionpart != 0 & billionpart != 0 ? "  " : "") +
                                      GTRmillionstowords(millionpart);
            }
            return functionReturnValue;
        }

        public object GTRmillionstowords(long millionnumber)
        {
            object functionReturnValue = null;
            long millionpart = 0;
            long thousandpart = 0;

            millionpart = Conversion.Int(millionnumber / 100000);
            thousandpart = millionnumber % 100000;
            functionReturnValue = GTRHundredsToWords(millionpart) + (millionpart != 0 ? " Lac" : "");
            if (thousandpart > 99)
            {
                functionReturnValue = functionReturnValue + (thousandpart != 0 & millionpart != 0 ? " " : "") +
                                      GTRthousandstowords(thousandpart);
            }
            else
            {
                functionReturnValue = functionReturnValue + (thousandpart != 0 & millionpart != 0 ? "  " : "") +
                                      GTRthousandstowords(thousandpart);
            }
            return functionReturnValue;
        }

        public string GTRthousandstowords(long thousandnumber)
        {
            string functionReturnValue = null;
            long thousandpart = 0;
            long HundredPart = 0;

            HundredPart = thousandnumber % 1000;
            thousandpart = Conversion.Int(thousandnumber / 1000);
            functionReturnValue = GTRHundredsToWords(thousandpart) + (thousandpart != 0 ? " Thousand" : "");
            if (HundredPart > 99)
            {
                functionReturnValue = functionReturnValue + (HundredPart != 0 & thousandpart != 0 ? " " : "") +
                                      GTRHundredsToWords(HundredPart);
            }
            else
            {
                functionReturnValue = functionReturnValue + (HundredPart != 0 & thousandpart != 0 ? "  " : "") +
                                      GTRHundredsToWords(HundredPart);
            }
            return functionReturnValue;
        }

        public string GTRHundredsToWords(long HundredNumber)
        {
            string functionReturnValue = null;
            //This function converts a three digit long to the hundred wording
            long TensPart = 0;
            long HundredPart = 0;

            TensPart = HundredNumber % 100;
            HundredPart = Conversion.Int(HundredNumber / 100);
            switch (HundredPart)
            {
                case 0:
                    functionReturnValue = GTRTensToWords(TensPart);
                    break;
                default:
                    functionReturnValue = GTRSingleToWord(HundredPart) + " Hundred" + (TensPart != 0 ? " " : "") +
                                          GTRTensToWords(TensPart);
                    break;
            }
            return functionReturnValue;
        }

        public string GTRTensToWords(long TensNumber)
        {
            string functionReturnValue = null;
            //This function converts a two digit long to a two digit wording
            long tens = 0;
            long Singles = 0;

            tens = Conversion.Int(TensNumber / 10);
            Singles = TensNumber % 10;

            switch (tens)
            {
                case 0:
                    functionReturnValue = GTRSingleToWord(Singles);
                    break;
                case 1:
                    functionReturnValue = GTRteens(TensNumber);
                    break;
                case 2:
                    functionReturnValue = "Twenty" + (Singles != 0 ? " " : "") + GTRSingleToWord(Singles);
                    break;
                case 3:
                    functionReturnValue = "Thirty" + (Singles != 0 ? " " : "") + GTRSingleToWord(Singles);
                    break;
                case 4:
                    functionReturnValue = "Forty" + (Singles != 0 ? " " : "") + GTRSingleToWord(Singles);
                    break;
                case 5:
                    functionReturnValue = "Fifty" + (Singles != 0 ? " " : "") + GTRSingleToWord(Singles);
                    break;
                case 6:
                    functionReturnValue = "Sixty" + (Singles != 0 ? " " : "") + GTRSingleToWord(Singles);
                    break;
                case 7:
                    functionReturnValue = "Seventy" + (Singles != 0 ? " " : "") + GTRSingleToWord(Singles);
                    break;
                case 8:
                    functionReturnValue = "Eighty" + (Singles != 0 ? " " : "") + GTRSingleToWord(Singles);
                    break;
                case 9:
                    functionReturnValue = "Ninety" + (Singles != 0 ? " " : "") + GTRSingleToWord(Singles);
                    break;
            }
            return functionReturnValue;
        }

        public string GTRSingleToWord(long SingleDigit)
        {
            string functionReturnValue = null;
            switch (SingleDigit)
            {
                case 1:
                    functionReturnValue = "One";
                    break;
                case 2:
                    functionReturnValue = "Two";
                    break;
                case 3:
                    functionReturnValue = "Three";
                    break;
                case 4:
                    functionReturnValue = "Four";
                    break;
                case 5:
                    functionReturnValue = "Five";
                    break;
                case 6:
                    functionReturnValue = "Six";
                    break;
                case 7:
                    functionReturnValue = "Seven";
                    break;
                case 8:
                    functionReturnValue = "Eight";
                    break;
                case 9:
                    functionReturnValue = "Nine";
                    break;
                case 0:
                    functionReturnValue = "";
                    break;
            }
            return functionReturnValue;
        }

        public string GTRteens(long TeenNumber)
        {
            string functionReturnValue = null;
            switch (TeenNumber)
            {
                case 10:
                    functionReturnValue = "Ten";
                    break;
                case 11:
                    functionReturnValue = "Eleven";
                    break;
                case 12:
                    functionReturnValue = "Twelve";
                    break;
                case 13:
                    functionReturnValue = "Thirteen";
                    break;
                case 14:
                    functionReturnValue = "Fourteen";
                    break;
                case 15:
                    functionReturnValue = "Fifteen";
                    break;
                case 16:
                    functionReturnValue = "Sixteen";
                    break;
                case 17:
                    functionReturnValue = "Seventeen";
                    break;
                case 18:
                    functionReturnValue = "Eighteen";
                    break;
                case 19:
                    functionReturnValue = "Nineteen";
                    break;
            }
            return functionReturnValue;
        }

        #endregion InWords Bangladeshi Format

        #region Inwords International Format

        public string GTRInwordsForegnFromat(string M, string PreFix, string PostFix)
        {
            string functionReturnValue = null;
            string sDlr = null;
            int iCent = 0;
            int icentPos = 0;
            string SDlrWord = null;
            string SCentWord = null;
            //icentPos = string.InStr(M, ".");

            if (M.IndexOf(".") > 0)

            {
                icentPos = int.Parse(M.Substring(0, M.IndexOf(".") - 1));

            }


            sDlr = Strings.Mid(M, 1, icentPos - 1);
            iCent = int.Parse(Strings.Mid(M, icentPos + 1, Strings.Len(M)));
            SDlrWord = GTRNumberToWordsForeign(Convert.ToInt64(sDlr));
            if (iCent > 0)
            {
                SDlrWord = GTRNumberToWordsForeign(Convert.ToInt64(sDlr));
                SCentWord = "Point. " + Strings.Replace(GTRNumberToWordsForeign(Convert.ToInt64(iCent)), "and", "");
                functionReturnValue = PreFix + " " + SDlrWord + " " + SCentWord + " " + PostFix;
            }
            else
            {
                functionReturnValue = PreFix + " " + SDlrWord + " " + PostFix;
            }
            return functionReturnValue;
        }

        public string GTRNumberToWordsForeign(long OrigNum)
        {
            string functionReturnValue = null;
            //This function converts numbers to words. For example 101 -> One hundred and one
            //It uses standard english notation and will only accept positive long numbers
            long billionpart = 0;
            long millionpart = 0;
            billionpart = Conversion.Int(OrigNum / 1000000000);
            millionpart = OrigNum % 1000000000;
            functionReturnValue = GTRHundredsToWords(billionpart) + (billionpart != 0 ? " Bil" : "");
            if (millionpart > 99)
            {
                functionReturnValue = functionReturnValue + (millionpart != 0 & billionpart != 0 ? " " : "") +
                                      GTRmillionstowordsForeign(millionpart);
            }
            else
            {
                functionReturnValue = functionReturnValue + (millionpart != 0 & billionpart != 0 ? "  " : "") +
                                      GTRmillionstowordsForeign(millionpart);
            }
            return functionReturnValue;
        }

        public object GTRmillionstowordsForeign(long millionnumber)
        {
            object functionReturnValue = null;
            long millionpart = 0;
            long thousandpart = 0;
            millionpart = Conversion.Int(millionnumber / 1000000);
            thousandpart = millionnumber % 1000000;
            functionReturnValue = GTRHundredsToWords(millionpart) + (millionpart != 0 ? " Mil" : "");
            if (thousandpart > 99)
            {
                functionReturnValue = functionReturnValue + (thousandpart != 0 & millionpart != 0 ? " " : "") +
                                      GTRthousandstowords(thousandpart);
            }
            else
            {
                functionReturnValue = functionReturnValue + (thousandpart != 0 & millionpart != 0 ? "  " : "") +
                                      GTRthousandstowords(thousandpart);
            }
            return functionReturnValue;
        }

        #endregion Inwords International Format

        #region Inwords In Bengali

        public string GTRInwordBengali(Double num1)
        {
            string bangla = "";
            string ward = "";
            Double ward1 = 0;
            string tx;
            string a1 = "", a2 = "", a3 = "", a4 = "", a5 = "", a6 = "", a7 = "";

            if (num1 > 999999999999) //12 digit
            {
                bangla = "ïb¨ "; //Zero
            }
            else
            {
                ward1 = num1;
                string[] n = {"", "GK ", "`yB ", "wZb ", "Pvi ", "cuvP ", "Qq ", "mvZ ", "AvU ", "bq ", "`k ", "GMvi ",
                    "evi ", "†Zi ", "†PŠÏ ", "c‡bi ", "†lvj ", "m‡Zi ", "AvVvi ", "Ewbk ", "wek ", "GKzk ", "evBk ",
                    "†ZBk ", "PweŸk ", "cwPk ", "QvweŸk ", "mvZvk ", "AvVvk ", "EbwÎk ", "wÎk ", "GKwÎk ", "ewÎk ",
                    "†ZwÎk ", "†PŠwÎk ", "cqwÎk ", "QwÎk ", "mvBwÎk ",
                    "AvUwÎk ", "EbPwj­k ", "Pwj­k ", "GKPwj­k ", "weqvwj­k ", "†ZZvwj­k ", "Pzqvwj­k ", "cqZvwj­k ",
                    "wQPwj­k ", "mvZPwj­k ", "AvUPwj­k ", "EbcÂvk ", "cÂvk ", "GKvbœ ", "evqvbœ ", "wZàvbœ ", "Pzqvbœ ",
                    "cÂvbœ ", "Qvàvbœ ", "mvZvbœ ", "AvUvbœ ", "EblvU ", "lvU ", "GKlwÆ ", "evlwÆ ", "†ZlwÆ ", "†PŠlwÆ ",
                    "cqlwÆ ", "wQlwÆ ", "mvZlwÆ ", "AvUlwÆ ", "EbmIyi ", "mIzi ", "GKvIyi ",
                    "evnvIyi ", "wZqvIyi ", "PzqvIyi ", "cPvIyi ", "wQqvIyi ", "mvZvIyi ", "AvUvIyi ", "EbAvwk ",
                    "Avwk ", "GKvwk ", "weivwk ", "wZivwk ", "Pzivwk ", "cPvwk ", "wQqvwk ", "mvZvwk ", "AóAvwk ",
                    "EbvbeŸyB ", "beŸyB ", "GKvbeŸB ", "weivbeŸB ", "wZivbeŸB ", "PzivbeŸB ", "cPvbeŸB ", "wQqvbeŸB ",
                    "mvZvbeŸB ", "AvUvbeŸB ", "wbivbeŸB ", " kZ "};

                if (ward1 > 99999999999 && ward1 <= 999999999999)
                {
                    tx = ward1.ToString().Substring(0, 1);//Mid(ward1, 1, 1)
                    ward1 = Double.Parse(ward1.ToString().Substring(ward1.ToString().Length - 11, 11));//Right(ward1, 11);
                    a1 = tx + " nvRvi ";
                }

                if (ward1 > 999999999 && ward1 <= 10000000000)
                {
                    if (ward1 > 9999999999)
                    {
                        tx = ward1.ToString().Substring(0, 2);//Mid(ward1, 1, 2)
                    }
                    else
                    {
                        tx = ward1.ToString().Substring(0, 1);//Mid(ward1, 1, 1)
                    }
                    ward1 = Double.Parse(ward1.ToString().Substring(ward1.ToString().Length - 9, 9));//Right(ward1, 9);
                    a2 = n[Int32.Parse(tx)] + " kZ ";
                }
                if (ward1 > 9999999 && ward1 < 1000000000)
                {
                    if (ward1 > 99999999)
                    {
                        tx = ward1.ToString().Substring(0, 2);//Mid(ward1, 1, 2)
                    }
                    else
                    {
                        tx = ward1.ToString().Substring(0, 1);//Mid(ward1, 1, 1)
                    }
                    ward1 = Double.Parse(ward1.ToString().Substring(ward1.ToString().Length - 7, 7));//Right(ward1, 7);
                    a3 = n[Int32.Parse(tx)] + " ‡KvwU "; //" KzwU ";
                }
                if (ward1 > 99999 && ward1 < 10000000)
                {
                    if (ward1 > 999999)
                    {
                        tx = ward1.ToString().Substring(0, 2);//Mid(ward1, 1, 2)
                    }
                    else
                    {
                        tx = ward1.ToString().Substring(0, 1);//Mid(ward1, 1,1)
                    }
                    ward1 = Double.Parse(ward1.ToString().Substring(ward1.ToString().Length - 5, 5));//Right(ward1, 5);
                    a4 = n[Int32.Parse(tx)] + " jvL ";
                }
                if (ward1 > 999 && ward1 < 100000)
                {
                    if (ward1 > 9999)
                    {
                        tx = ward1.ToString().Substring(0, 2);//Mid(ward1, 1, 2)
                    }
                    else
                    {
                        tx = ward1.ToString().Substring(0, 1);//Mid(ward1, 1,1)
                    }
                    ward1 = Double.Parse(ward1.ToString().Substring(ward1.ToString().Length - 3, 3));//Right(ward1, 3);
                    a5 = n[Int32.Parse(tx)] + " nvRvi ";
                }
                if (ward1 > 99 && ward1 < 1000)
                {
                    tx = ward1.ToString().Substring(0, 1);//Mid(ward1, 1,1)
                    ward1 = Double.Parse(ward1.ToString().Substring(ward1.ToString().Length - 2, 2));//Mid(ward1, 1, 2)
                    a6 = n[Int32.Parse(tx)] + " kZ ";
                }
                if (ward1 > 0 && ward1 < 100)
                {
                    tx = ward1.ToString();
                    a7 = n[Int32.Parse(tx)];
                }
                ward = a1 + a2 + a3 + a4 + a5 + a6 + a7 + " UvKv gvÎ.";
            }
            return ward;
        }

        #endregion
    }
}