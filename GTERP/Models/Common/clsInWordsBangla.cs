using Microsoft.VisualBasic;
using System;


namespace GTERP.Models.Common
{
    internal class clsInWordsBangla
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
                SCentWord = "দশমিক " + Strings.Replace(GTRNumberToWords(Convert.ToInt64(iCent), 1), "", "");
                //functionReturnValue = PreFix + " " + SDlrWord + " " + SCentWord + " পয়সা" + PostFix;
                functionReturnValue = PreFix + " " + SDlrWord + " " + SCentWord + " " + PostFix;

            }
            else
            {
                functionReturnValue = PreFix + " " + SDlrWord + " " + PostFix;
            }
            return functionReturnValue;
        }

        public string GTRNumberToWords(long OrigNum, int isDecimal = 0)
        {
            string functionReturnValue = null;
            //This function converts numbers to words. For example 101 -> One hundred and one
            //It uses standard english notation and will only accept positive long numbers
            long billionpart = 0;
            long millionpart = 0;

            //billionpart = Conversion.Int(OrigNum / 10000000);
            millionpart = OrigNum % 10000000;
            functionReturnValue = GTRHundredsToWords(billionpart) + (billionpart != 0 ? " কোটি" : "");

            if (millionpart > 99)
            {
                functionReturnValue = functionReturnValue + (millionpart != 0 & billionpart != 0 ? " " : "") +
                                      GTRmillionstowords(millionpart);
            }
            else
            {
                functionReturnValue = functionReturnValue + (millionpart != 0 & billionpart != 0 ? "  " : "") +
                                      GTRmillionstowords(millionpart, isDecimal);
            }
            return functionReturnValue;
        }

        public object GTRmillionstowords(long millionnumber, int isDecimal = 0)
        {
            object functionReturnValue = null;
            long millionpart = 0;
            long thousandpart = 0;

            millionpart = Conversion.Int(millionnumber / 100000);
            thousandpart = millionnumber % 100000;
            functionReturnValue = GTRHundredsToWords(millionpart) + (millionpart != 0 ? " লক্ষ" : "");
            if (thousandpart > 99)
            {
                functionReturnValue = functionReturnValue + (thousandpart != 0 & millionpart != 0 ? " " : "") +
                                      GTRthousandstowords(thousandpart);
            }
            else
            {
                functionReturnValue = functionReturnValue + (thousandpart != 0 & millionpart != 0 ? "  " : "") +
                                      GTRthousandstowords(thousandpart, isDecimal);
            }
            return functionReturnValue;
        }

        public string GTRthousandstowords(long thousandnumber, int isDecimal = 0)
        {
            string functionReturnValue = null;
            long thousandpart = 0;
            long HundredPart = 0;

            HundredPart = thousandnumber % 1000;
            thousandpart = Conversion.Int(thousandnumber / 1000);
            functionReturnValue = GTRHundredsToWords(thousandpart) + (thousandpart != 0 ? " হাজার" : "");
            if (HundredPart > 99)
            {
                functionReturnValue = functionReturnValue + (HundredPart != 0 & thousandpart != 0 ? " " : "") +
                                      GTRHundredsToWords(HundredPart);
            }
            else
            {
                functionReturnValue = functionReturnValue + (HundredPart != 0 & thousandpart != 0 ? "  " : "") +
                                      GTRHundredsToWords(HundredPart, isDecimal);
            }
            return functionReturnValue;
        }

        public string GTRHundredsToWords(long HundredNumber, int isDecimal = 0)
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
                    functionReturnValue = GTRTensToWords(TensPart, isDecimal);
                    break;
                default:
                    functionReturnValue = GTRSingleToWord(HundredPart) + " শত" + (TensPart != 0 ? " " : "") +
                                          GTRTensToWords(TensPart, isDecimal);
                    break;
            }
            return functionReturnValue;
        }

        public string GTRTensToWords(long TensNumber, int isDecimal = 0)
        {
            string functionReturnValue = null;
            //This function converts a two digit long to a two digit wording
            long tens = 0;
            long Singles = 0;

            if (isDecimal == 1)
            {
                tens = Conversion.Int(TensNumber / 10);
                Singles = TensNumber % 10;
            }
            else
            {
                if (TensNumber < 10)
                {
                    tens = Conversion.Int(TensNumber / 10);
                    Singles = TensNumber % 10;

                }
                else
                {
                    tens = Conversion.Int(TensNumber);
                    Singles = TensNumber;
                }
            }


            switch (tens)
            {

                case 0:
                    functionReturnValue = GTRSingleToWord(Singles);
                    break;
                case 1:
                    functionReturnValue = GTRteens(TensNumber);
                    break;
                case 2:
                    functionReturnValue = "দুই" + (Singles != 0 ? " " : "") + GTRSingleToWord(Singles, isDecimal);
                    break;
                case 3:
                    functionReturnValue = "তিন" + (Singles != 0 ? " " : "") + GTRSingleToWord(Singles, isDecimal);
                    break;
                case 4:
                    functionReturnValue = "চার" + (Singles != 0 ? " " : "") + GTRSingleToWord(Singles, isDecimal);
                    break;
                case 5:
                    functionReturnValue = "পাঁচ" + (Singles != 0 ? " " : "") + GTRSingleToWord(Singles, isDecimal);
                    break;
                case 6:
                    functionReturnValue = "ছয়" + (Singles != 0 ? " " : "") + GTRSingleToWord(Singles, isDecimal);
                    break;
                case 7:
                    functionReturnValue = "সাত" + (Singles != 0 ? " " : "") + GTRSingleToWord(Singles, isDecimal);
                    break;
                case 8:
                    functionReturnValue = "আট" + (Singles != 0 ? " " : "") + GTRSingleToWord(Singles, isDecimal);
                    break;
                case 9:
                    functionReturnValue = "নয়" + (Singles != 0 ? " " : "") + GTRSingleToWord(Singles, isDecimal);
                    break;


                case 10:
                    functionReturnValue = "দশ";
                    break;
                case 11:
                    functionReturnValue = "এগার";
                    break;
                case 12:
                    functionReturnValue = "বার";
                    break;
                case 13:
                    functionReturnValue = "তের";
                    break;
                case 14:
                    functionReturnValue = "চৌদ্দ";
                    break;
                case 15:
                    functionReturnValue = "পনের";
                    break;
                case 16:
                    functionReturnValue = "ষোল";
                    break;
                case 17:
                    functionReturnValue = "সতের";
                    break;
                case 18:
                    functionReturnValue = "আঠার";
                    break;
                case 19:
                    functionReturnValue = "উনিশ";
                    break;

                case 20:
                    functionReturnValue = "বিশ";
                    break;
                case 21:
                    functionReturnValue = "একুশ";
                    break;
                case 22:
                    functionReturnValue = "বাইশ";
                    break;
                case 23:
                    functionReturnValue = "তেইশ";
                    break;


                case 24:
                    functionReturnValue = "চব্বিশ";
                    break;

                case 25:
                    functionReturnValue = "পঁচিশ";
                    break;

                case 26:
                    functionReturnValue = "ছাব্বিশ";
                    break;

                case 27:
                    functionReturnValue = "সাতাশ";
                    break;

                case 28:
                    functionReturnValue = "আঠাশ";
                    break;

                case 29:
                    functionReturnValue = "ঊনত্রিশ";
                    break;

                case 30:
                    functionReturnValue = "ত্রিশ";
                    break;

                case 31:
                    functionReturnValue = "একত্রিশ";
                    break;

                case 32:
                    functionReturnValue = "বত্রিশ";
                    break;


                case 33:
                    functionReturnValue = "তেত্রিশ";
                    break;

                case 34:
                    functionReturnValue = "চৌত্রিশ";
                    break;


                case 35:
                    functionReturnValue = "পঁয়ত্রিশ";
                    break;


                case 36:
                    functionReturnValue = "ছত্রিশ";
                    break;


                case 37:
                    functionReturnValue = "সাইত্রিশ";
                    break;

                case 38:
                    functionReturnValue = "আটত্রিশ";
                    break;

                case 39:
                    functionReturnValue = "ঊনচল্লিশ";
                    break;

                case 40:
                    functionReturnValue = "চল্লিশ";
                    break;

                case 41:
                    functionReturnValue = "একচল্লিশ";
                    break;

                case 42:
                    functionReturnValue = "বিয়াল্লিশ";
                    break;


                case 43:
                    functionReturnValue = "তেতাল্লিশ";
                    break;

                case 44:
                    functionReturnValue = "চুয়াল্লিশ";
                    break;


                case 45:
                    functionReturnValue = "পঁয়তাল্লিশ";
                    break;

                case 46:
                    functionReturnValue = "ছেচল্লিশ";
                    break;

                case 47:
                    functionReturnValue = "সাতচল্লিশ";
                    break;

                case 48:
                    functionReturnValue = "আটচল্লিশ";
                    break;

                case 49:
                    functionReturnValue = "ঊনপঞ্চাশ";
                    break;

                case 50:
                    functionReturnValue = "পঞ্চাশ";
                    break;


                case 51:
                    functionReturnValue = "একান্ন";
                    break;

                case 52:
                    functionReturnValue = "বায়ান্ন";
                    break;


                case 53:
                    functionReturnValue = "তেপ্পান্ন";
                    break;


                case 54:
                    functionReturnValue = "চুয়ান্ন";
                    break;



                case 55:
                    functionReturnValue = "পঞ্চান্ন";
                    break;



                case 56:
                    functionReturnValue = "ছাপ্পান্ন";
                    break;


                case 57:
                    functionReturnValue = "সাতান্ন";
                    break;


                case 58:
                    functionReturnValue = "আটান্ন";
                    break;


                case 59:
                    functionReturnValue = "ঊনষাট";
                    break;


                case 60:
                    functionReturnValue = "ষাট";
                    break;


                case 61:
                    functionReturnValue = "একষট্টি";
                    break;


                case 62:
                    functionReturnValue = "বাষট্টি";
                    break;


                case 63:
                    functionReturnValue = "তেষট্টি";
                    break;


                case 64:
                    functionReturnValue = "চৌষট্টি";
                    break;

                case 65:
                    functionReturnValue = "পঁয়ষট্টি";
                    break;

                case 66:
                    functionReturnValue = "ছেষট্টি";
                    break;

                case 67:
                    functionReturnValue = "সাতষট্টি";
                    break;

                case 68:
                    functionReturnValue = "আটষট্টি";
                    break;

                case 69:
                    functionReturnValue = "উনসত্তর";
                    break;

                case 70:
                    functionReturnValue = "সত্তর";
                    break;

                case 71:
                    functionReturnValue = "একাত্তর";
                    break;

                case 72:
                    functionReturnValue = "বাহাত্তর";
                    break;

                case 73:
                    functionReturnValue = "তেহাত্তর";
                    break;

                case 74:
                    functionReturnValue = "চুহাত্তর";
                    break;


                case 75:
                    functionReturnValue = "পঁচাত্তর";
                    break;


                case 76:
                    functionReturnValue = "ছিয়াত্তর";
                    break;

                case 77:
                    functionReturnValue = "সাতাত্তর";
                    break;


                case 78:
                    functionReturnValue = "আটাত্তর";
                    break;

                case 79:
                    functionReturnValue = "ঊনআশি";
                    break;

                case 80:
                    functionReturnValue = "আশি";
                    break;


                case 81:
                    functionReturnValue = "একাশি";
                    break;


                case 82:
                    functionReturnValue = "বিরাশি";
                    break;


                case 83:
                    functionReturnValue = "তিরাশি";
                    break;


                case 84:
                    functionReturnValue = "চুরাশি";
                    break;


                case 85:
                    functionReturnValue = "পঁচাশি";
                    break;


                case 86:
                    functionReturnValue = "ছিয়াশি";
                    break;


                case 87:
                    functionReturnValue = "সাতাশি";
                    break;


                case 88:
                    functionReturnValue = "আটাশি";
                    break;


                case 89:
                    functionReturnValue = "ঊনানব্বই";
                    break;


                case 90:
                    functionReturnValue = "নব্বই";
                    break;

                case 91:
                    functionReturnValue = "একানব্বই";
                    break;


                case 92:
                    functionReturnValue = "বিরানব্বই";
                    break;


                case 93:
                    functionReturnValue = "তিরানব্বই";
                    break;

                case 94:
                    functionReturnValue = "চুরানব্বই";
                    break;


                case 95:
                    functionReturnValue = "পঁচানব্বই";
                    break;

                case 96:
                    functionReturnValue = "ছিয়ানব্বই";
                    break;


                case 97:
                    functionReturnValue = "সাতানব্বই";
                    break;

                case 98:
                    functionReturnValue = "আটানব্বই";
                    break;


                case 99:
                    functionReturnValue = "নিরানব্বই";
                    break;

            }


            //switch (tens)
            //{
            //    case 0:
            //        functionReturnValue = GTRSingleToWord(Singles);
            //        break;
            //    case 1:
            //        functionReturnValue = GTRteens(TensNumber);
            //        break;
            //    case 2:
            //        functionReturnValue = "বিশ" + (Singles != 0 ? " " : "") + GTRSingleToWord(Singles);
            //        break;
            //    case 3:
            //        functionReturnValue = "তিরিশ" + (Singles != 0 ? " " : "") + GTRSingleToWord(Singles);
            //        break;
            //    case 4:
            //        functionReturnValue = "চল্লিশ" + (Singles != 0 ? " " : "") + GTRSingleToWord(Singles);
            //        break;
            //    case 5:
            //        functionReturnValue = "পঞ্চাশ" + (Singles != 0 ? " " : "") + GTRSingleToWord(Singles);
            //        break;
            //    case 6:
            //        functionReturnValue = "ষাট" + (Singles != 0 ? " " : "") + GTRSingleToWord(Singles);
            //        break;
            //    case 7:
            //        functionReturnValue = "সত্তর" + (Singles != 0 ? " " : "") + GTRSingleToWord(Singles);
            //        break;
            //    case 8:
            //        functionReturnValue = "আশি" + (Singles != 0 ? " " : "") + GTRSingleToWord(Singles);
            //        break;
            //    case 9:
            //        functionReturnValue = "নব্বই" + (Singles != 0 ? " " : "") + GTRSingleToWord(Singles);
            //        break;
            //}

            if (isDecimal == 1)
            {
                switch (tens)
                {
                    case 0:
                        functionReturnValue = GTRSingleToWord(Singles);
                        break;
                    case 1:
                        functionReturnValue = GTRteens(TensNumber);
                        break;
                    case 2:
                        functionReturnValue = "দুই" + (Singles != 0 ? " " : "") + GTRSingleToWord(Singles, isDecimal);
                        break;
                    case 3:
                        functionReturnValue = "তিন" + (Singles != 0 ? " " : "") + GTRSingleToWord(Singles, isDecimal);
                        break;
                    case 4:
                        functionReturnValue = "চার" + (Singles != 0 ? " " : "") + GTRSingleToWord(Singles, isDecimal);
                        break;
                    case 5:
                        functionReturnValue = "পাঁচ" + (Singles != 0 ? " " : "") + GTRSingleToWord(Singles, isDecimal);
                        break;
                    case 6:
                        functionReturnValue = "ছয়" + (Singles != 0 ? " " : "") + GTRSingleToWord(Singles, isDecimal);
                        break;
                    case 7:
                        functionReturnValue = "সাত" + (Singles != 0 ? " " : "") + GTRSingleToWord(Singles, isDecimal);
                        break;
                    case 8:
                        functionReturnValue = "আট" + (Singles != 0 ? " " : "") + GTRSingleToWord(Singles, isDecimal);
                        break;
                    case 9:
                        functionReturnValue = "নয়" + (Singles != 0 ? " " : "") + GTRSingleToWord(Singles, isDecimal);
                        break;
                }
            }

            return functionReturnValue;

        }

        public string GTRSingleToWord(long SingleDigit, int isdecimalvalue = 0)
        {
            string functionReturnValue = null;
            switch (SingleDigit)
            {
                case 1:
                    functionReturnValue = "এক";
                    break;
                case 2:
                    functionReturnValue = "দুই";
                    break;
                case 3:
                    functionReturnValue = "তিন";
                    break;
                case 4:
                    functionReturnValue = "চার";
                    break;
                case 5:
                    functionReturnValue = "পাঁচ ";
                    break;
                case 6:
                    functionReturnValue = "ছয়";
                    break;
                case 7:
                    functionReturnValue = "সাত";
                    break;
                case 8:
                    functionReturnValue = "আট";
                    break;
                case 9:
                    functionReturnValue = "নয়";
                    break;
                case 0:
                    if (isdecimalvalue == 1)
                    {
                        functionReturnValue = " শূন্য";

                    }
                    else
                    {
                        functionReturnValue = "";
                    }
                    break;//শূন্য
            }
            return functionReturnValue;
        }

        public string GTRteens(long TeenNumber)
        {
            string functionReturnValue = null;
            switch (TeenNumber)
            {
                case 10:
                    functionReturnValue = "দশ";
                    break;
                case 11:
                    functionReturnValue = "এগার";
                    break;
                case 12:
                    functionReturnValue = "বার";
                    break;
                case 13:
                    functionReturnValue = "তের";
                    break;
                case 14:
                    functionReturnValue = "চৌদ্দ";
                    break;
                case 15:
                    functionReturnValue = "পনের";
                    break;
                case 16:
                    functionReturnValue = "ষোল";
                    break;
                case 17:
                    functionReturnValue = "সতের";
                    break;
                case 18:
                    functionReturnValue = "আঠার";
                    break;
                case 19:
                    functionReturnValue = "উনিশ";
                    break;

                case 20:
                    functionReturnValue = "বিশ";
                    break;
                case 21:
                    functionReturnValue = "একুশ";
                    break;
                case 22:
                    functionReturnValue = "বাইশ";
                    break;
                case 23:
                    functionReturnValue = "তেইশ";
                    break;


                case 24:
                    functionReturnValue = "চব্বিশ";
                    break;

                case 25:
                    functionReturnValue = "পঁচিশ";
                    break;

                case 26:
                    functionReturnValue = "ছাব্বিশ";
                    break;

                case 27:
                    functionReturnValue = "সাতাশ";
                    break;

                case 28:
                    functionReturnValue = "আঠাশ";
                    break;

                case 29:
                    functionReturnValue = "ঊনত্রিশ";
                    break;

                case 30:
                    functionReturnValue = "ত্রিশ";
                    break;

                case 31:
                    functionReturnValue = "একত্রিশ";
                    break;

                case 32:
                    functionReturnValue = "বত্রিশ";
                    break;


                case 33:
                    functionReturnValue = "তেত্রিশ";
                    break;

                case 34:
                    functionReturnValue = "চৌত্রিশ";
                    break;


                case 35:
                    functionReturnValue = "পঁয়ত্রিশ";
                    break;


                case 36:
                    functionReturnValue = "ছত্রিশ";
                    break;


                case 37:
                    functionReturnValue = "সাইত্রিশ";
                    break;

                case 38:
                    functionReturnValue = "আটত্রিশ";
                    break;

                case 39:
                    functionReturnValue = "ঊনচল্লিশ";
                    break;

                case 40:
                    functionReturnValue = "চল্লিশ";
                    break;

                case 41:
                    functionReturnValue = "একচল্লিশ";
                    break;

                case 42:
                    functionReturnValue = "বিয়াল্লিশ";
                    break;


                case 43:
                    functionReturnValue = "তেতাল্লিশ";
                    break;

                case 44:
                    functionReturnValue = "চুয়াল্লিশ";
                    break;


                case 45:
                    functionReturnValue = "পঁয়তাল্লিশ";
                    break;

                case 46:
                    functionReturnValue = "ছেচল্লিশ";
                    break;

                case 47:
                    functionReturnValue = "সাতচল্লিশ";
                    break;

                case 48:
                    functionReturnValue = "আটচল্লিশ";
                    break;

                case 49:
                    functionReturnValue = "ঊনপঞ্চাশ";
                    break;

                case 50:
                    functionReturnValue = "পঞ্চাশ";
                    break;


                case 51:
                    functionReturnValue = "একান্ন";
                    break;

                case 52:
                    functionReturnValue = "বায়ান্ন";
                    break;


                case 53:
                    functionReturnValue = "তেপ্পান্ন";
                    break;


                case 54:
                    functionReturnValue = "চুয়ান্ন";
                    break;



                case 55:
                    functionReturnValue = "পঞ্চান্ন";
                    break;



                case 56:
                    functionReturnValue = "ছাপ্পান্ন";
                    break;


                case 57:
                    functionReturnValue = "সাতান্ন";
                    break;


                case 58:
                    functionReturnValue = "আটান্ন";
                    break;


                case 59:
                    functionReturnValue = "ঊনষাট";
                    break;


                case 60:
                    functionReturnValue = "ষাট";
                    break;


                case 61:
                    functionReturnValue = "একষট্টি";
                    break;


                case 62:
                    functionReturnValue = "বাষট্টি";
                    break;


                case 63:
                    functionReturnValue = "তেষট্টি";
                    break;


                case 64:
                    functionReturnValue = "চৌষট্টি";
                    break;

                case 65:
                    functionReturnValue = "পঁয়ষট্টি";
                    break;

                case 66:
                    functionReturnValue = "ছেষট্টি";
                    break;

                case 67:
                    functionReturnValue = "সাতষট্টি";
                    break;

                case 68:
                    functionReturnValue = "আটষট্টি";
                    break;

                case 69:
                    functionReturnValue = "উনসত্তর";
                    break;

                case 70:
                    functionReturnValue = "সত্তর";
                    break;

                case 71:
                    functionReturnValue = "একাত্তর";
                    break;

                case 72:
                    functionReturnValue = "বাহাত্তর";
                    break;

                case 73:
                    functionReturnValue = "তেহাত্তর";
                    break;

                case 74:
                    functionReturnValue = "চুহাত্তর";
                    break;


                case 75:
                    functionReturnValue = "পঁচাত্তর";
                    break;


                case 76:
                    functionReturnValue = "ছিয়াত্তর";
                    break;

                case 77:
                    functionReturnValue = "সাতাত্তর";
                    break;


                case 78:
                    functionReturnValue = "আটাত্তর";
                    break;

                case 79:
                    functionReturnValue = "ঊনআশি";
                    break;

                case 80:
                    functionReturnValue = "আশি";
                    break;


                case 81:
                    functionReturnValue = "একাশি";
                    break;


                case 82:
                    functionReturnValue = "বিরাশি";
                    break;


                case 83:
                    functionReturnValue = "তিরাশি";
                    break;


                case 84:
                    functionReturnValue = "চুরাশি";
                    break;


                case 85:
                    functionReturnValue = "পঁচাশি";
                    break;


                case 86:
                    functionReturnValue = "ছিয়াশি";
                    break;


                case 87:
                    functionReturnValue = "সাতাশি";
                    break;


                case 88:
                    functionReturnValue = "আটাশি";
                    break;


                case 89:
                    functionReturnValue = "ঊনানব্বই";
                    break;


                case 90:
                    functionReturnValue = "নব্বই";
                    break;

                case 91:
                    functionReturnValue = "একানব্বই";
                    break;


                case 92:
                    functionReturnValue = "বিরানব্বই";
                    break;


                case 93:
                    functionReturnValue = "তিরানব্বই";
                    break;

                case 94:
                    functionReturnValue = "চুরানব্বই";
                    break;


                case 95:
                    functionReturnValue = "পঁচানব্বই";
                    break;

                case 96:
                    functionReturnValue = "ছিয়ানব্বই";
                    break;


                case 97:
                    functionReturnValue = "সাতানব্বই";
                    break;

                case 98:
                    functionReturnValue = "আটানব্বই";
                    break;


                case 99:
                    functionReturnValue = "নিরানব্বই";
                    break;




            }
            //switch (TeenNumber)
            //{
            //    case 10:
            //        functionReturnValue = "এক";
            //        break;
            //    case 11:
            //        functionReturnValue = "এক";
            //        break;
            //    case 12:
            //        functionReturnValue = "এক";
            //        break;
            //    case 13:
            //        functionReturnValue = "এক";
            //        break;
            //    case 14:
            //        functionReturnValue = "এক";
            //        break;
            //    case 15:
            //        functionReturnValue = "এক";
            //        break;
            //    case 16:
            //        functionReturnValue = "এক";
            //        break;
            //    case 17:
            //        functionReturnValue = "এক";
            //        break;
            //    case 18:
            //        functionReturnValue = "এক";
            //        break;
            //    case 19:
            //        functionReturnValue = "এক";
            //        break;
            //}
            return functionReturnValue;
        }

        #endregion InWords Bangladeshi Format



    }
}