using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using fit;
using System.Text.RegularExpressions;

namespace QaTip.Fitnesse.Demo.DoFixture
{
    public class genericvalidationfixture : ColumnFixture
    {
        private string _CurrentValue;
        private string _NumberOfIncrement;
        private string _inputString;
        /// <summary>
        /// String1
        /// </summary>
        /// 


        public string String1
        { get; set; }
        public StringBuilder buildstring = new StringBuilder();
        public List<string> returnval { get; set;}
        public string stringoutput { get; set; }
        public List<string> listofstrings = new List<string>();
        public string separators { get; set; }

        public string GenExp()
        {
            throw new NotImplementedException("Not yet implemented by me!");
        }

        public string[] stringstoconcatinate { get; set; }

        public string String3
        { get; set; }

        public string String4
        { get; set; }
        /// <summary>
        /// 
        public string String5
        { get; set; }

        public string String6
        { get; set; }

        public string String7
        { get; set; }

        public string String8
        { get; set; }

        public string String9
        { get; set; }
        /// String2
        /// </summary>
        public string String2
        { get; set; }

        /// <summary>
        /// Integer1
        /// This is a string so it can interact with other string variables that can be cast to integers
        /// </summary>
        public string Integer1
        { get; set; }

        /// <summary>
        /// Integer1 - int version
        /// This is a int so it can be evaluated as a true number
        /// </summary>
        public int TrueInteger1
        { get; set; }

        /// <summary>
        /// Integer2
        /// This is a string so it can interact with other string variables that can be cast to integers
        /// </summary>
        public string Integer2
        { get; set; }

        public string Double1
        { get; set; }

        /// <summary>
        /// DateTime1
        /// </summary>
        public string DateTime1
        { get; set; }

        /// <summary>
        /// DateTime2
        /// </summary>
        public string DateTime2
        { get; set; }

        /// <summary>
        /// DateTimeFormat
        /// </summary>
        public string DateTimeFormat
        { get; set; }

        /// <summary>
        /// AdjustDateByDays
        /// </summary>
        public int AdjustDateByDays
        { get; set; }

        /// <summary>
        /// AdjustTimeByMinutes
        /// </summary>
        public int AdjustTimeByMinutes
        { get; set; }

        /// <summary>
        /// AdjustTimeBySeconds
        /// </summary>
        public int AdjustTimeBySeconds
        { get; set; }

        /// <summary>
        /// AdjustTimeByHours
        /// </summary>
        public int AdjustTimeByHours
        { get; set; }

        /// <summary>
        /// TotalWidth for padding
        /// </summary>
        public int TotalWidth
        { get; set; }

        /// <summary>
        /// StartChar
        /// </summary>
        public string StartChar
        { get; set; }

        /// <summary>
        /// FieldLength
        /// </summary>
        public string FieldLength
        { get; set; }

        public string CurrentValue
        {
            get { return _CurrentValue; }
            set { _CurrentValue = value; }
        }

        public string NumberOfIncrement
        {
            get { return _NumberOfIncrement; }
            set { _NumberOfIncrement = value; }
        }
        public string InputString
        {
            get { return _inputString; }
            set { _inputString = value; }
        }

        public string InsertSpace()
        {
            _inputString = _inputString + " ";
            return _inputString;
        }

        /// <summary>
        /// Concatenates String1 and String2
        /// </summary>

        public string Increment()
        {
            int CurrentVal = Convert.ToInt32(_CurrentValue);
            int NumbIncrement = Convert.ToInt32(NumberOfIncrement);
            int NewValue = CurrentVal + NumbIncrement;
            return NewValue.ToString();
        }

       
        private void getListofStrings()
        { 
            foreach (string s in stringstoconcatinate)
            {
                listofstrings.Add(s);
                stringoutput += s + " ";
            }

        }
        //build string with url 

        private void buildurlwithprovidedstrings()
        {
            foreach (string s in stringstoconcatinate)
            {
                listofstrings.Add(s);
                stringoutput += s + "/";
            }

        }

        public void concatinate_strings()
        {
            genericstringbuilder(separators);
        }

        
      
        // GENERIC STRING BUILDER 
        private void genericstringbuilder(string separator)
        {
            // 1.this would add a new list to an already existing string list
            listofstrings.AddRange(stringstoconcatinate.ToList());

            // 2. or if you only want this as list:
            listofstrings = stringstoconcatinate.ToList();

            // this will create a string separated by the type that you want
            stringoutput = String.Join(separator, stringoutput, String.Join(separator, stringstoconcatinate));

        }





        // build string without

        private void buildstringwithoutspace()
        {
            foreach (string s in stringstoconcatinate)
            {
                StringBuilder strg = new StringBuilder();
                listofstrings.Add(s);
                stringoutput += s;
            }

        }

        public void joinstrings()
        {
            getListofStrings();
            
        }

        public void combinestringstobuilddirectorypath()
        {
            genericstringbuilder("\\");
        }

        public string ConcatenateString()
        {


            string sRet = String1 + String2;
            return sRet;
        }


        public string ConcatenateLongString()
        {
            string sRet = String1 + String2 + String3 + String4 + String5 + String6 + String7;
            return sRet;
        }


        public string ConcatenateUrlString()
        {

            string sRet = String1 + "\\" + String2 + "\\" + String3 + "\\" + String4 + "\\" + String5 + String6 + String7 + String8 + String9;
            return sRet;
        }
        /// <summary>
        /// Concatenates String1 and String2
        /// </summary>
        public string AddSpaceToEndOfString()
        {
            return String1 + " ";
        }





        /// <summary>

        /// Concatenates String1 and end of line char
        /// </summary>
        public string AddEndOfLineToEndOfString()
        {
            return String1 + "\r\n";
        }

        /// Concatenates String1 and end of line char
        /// </summary>
        public string AddLineFeedToEndOfString()
        {
            return String1 + "\n";
        }

        /// <summary>
        /// Pads String1 with necessary spaces to fill TotalWidth number of characters
        /// </summary>
        public string AddPaddingtoEndOfString()
        {
            string sRet = String1;

            if (String1 != null)
            {
                if (String1.Length > TotalWidth)
                    sRet = String1.Substring(0, TotalWidth);
            }
            else
            {
                sRet = new String(' ', TotalWidth);
            }

            return sRet.PadRight(TotalWidth);
        }

        /// <summary>
        /// Pads String1 with necessary zeros to fill TotalWidth number of characters
        /// </summary>
        public string AddNumericPadding()
        {
            string sRet = String1;

            if (String1 != null)
            {
                if (String1.Length > TotalWidth)
                {
                    sRet = String1.Substring(0, TotalWidth);
                }
            }
            else
            {
                sRet = new String('0', TotalWidth);
            }

            return sRet.PadLeft(TotalWidth, '0');
        }

        /// <summary>
        /// Concatenates String1 and String2
        /// </summary>
        public string AddSpaceToStartOfString()
        {
            return " " + String1;
        }

        /// <summary>
        /// Returns the current system time
        /// </summary>
        public string GetCurrentDateTime()
        {
            return DateTime.Now.ToString();

        }

        /// <summary>
        /// Returns the current system time
        /// </summary>
        public string GetDateRelativeToToday()
        {
            return DateTime.Now.Date.AddDays(AdjustDateByDays).ToShortDateString();
        }

        /// <summary>
        /// Returns the current system time relative hours
        /// </summary>
        public string GetDateRelativeToHours()
        {
            return DateTime.Now.AddHours(AdjustTimeByHours).ToString();
        }

        /// <summary>
        /// Returns the current system time adjusted by a number of minutes
        /// </summary>
        public string AdjustMinutesRelativeToNow()
        {

            DateTime1 = DateTime.Now.AddMinutes(AdjustTimeByMinutes).ToShortTimeString();
            return DateTime.Parse(DateTime1).ToString("HH:mm");
        }

        /// <summary>
        /// Returns the current system time adjusted by a number of seconds
        /// </summary>
        public string AdjustSecondsRelativeToNow()
        {
            DateTime1 = DateTime.Now.AddSeconds(AdjustTimeBySeconds).ToLongTimeString();
            return DateTime.Parse(DateTime1).ToString("HH:mm:ss");
        }

        /// <summary>
        /// Returns the current system time
        /// </summary>
        public string GetDateRelativeToDate1()
        {
            DateTime _dateTime1;
            if (!DateTime.TryParse(DateTime1, out _dateTime1))
            {
                throw new ApplicationException(string.Format("Provided DateTime1 value of '{0}' cannot be converted to a valid DateTime value.", DateTime1));
            }
            return _dateTime1.AddDays(AdjustDateByDays).ToShortDateString();
        }

        /// <summary>
        /// Returns the current system time
        /// </summary>
        public string GetDateTimeRelativeToDate1()
        {
            DateTime _dateTime1;
            if (!DateTime.TryParse(DateTime1, out _dateTime1))
            {
                throw new ApplicationException(string.Format("Provided DateTime1 value of '{0}' cannot be converted to a valid DateTime value.", DateTime1));
            }
            return _dateTime1.AddDays(AdjustDateByDays).ToString();
        }

        /// <summary>
        /// Returns Path to FitNesseRoot
        /// </summary>
        public string GetFitNesseRoot()
        {
            return Environment.CurrentDirectory + @"\FitNesseRoot\";
        }

        /// <summary>
        /// Returns the current machine name
        /// </summary>
        public string GetMachineName()
        {
            return Environment.MachineName;
        }

        /// <summary>
        /// Compares two strings and determines if that are equal.
        /// </summary>
        public string String1EqualsString2()
        {
            if (String1 == String2)
            {
                return Constants.YES;
            }

            return Constants.NO;
        }

        /// <summary>
        /// Determines if String1 contains String2.
        /// </summary>
        public string String1ContainsString2()
        {
            if (String1 == String2)
            {
                return Constants.YES;
            }

            if (String1 != null && String1.Contains(String2))
            {
                return Constants.YES;
            }

            return Constants.NO;
        }

        /// <summary>
        /// Determines the character length of a given string.
        /// </summary>
        public int String1Length()
        {
            return String1.Length;
        }

        /// <summary>
        /// Concatenates Integer1 and Integer2
        /// </summary>
        public string Integer1IsGreaterThanInteger2()
        {
            int int1;
            int int2;

            if (!Int32.TryParse(Integer1, out int1))
            {
                throw new ApplicationException(string.Format("Unable to cast {0} to a valid integer", Integer1));
            }

            if (!Int32.TryParse(Integer2, out int2))
            {
                throw new ApplicationException(string.Format("Unable to cast {0} to a valid integer", Integer2));
            }

            if (int1 > int2)
            {
                return Constants.YES;
            }

            return Constants.NO;
        }

        /// <summary>
        /// Compares two DateTime objects and determines if Date2 is more recent than Date1
        /// </summary>
        public string Date2IsMoreRecentThanDate1()
        {
            DateTime _dateTime1;
            if (!DateTime.TryParse(DateTime1, out _dateTime1))
            {
                throw new ApplicationException(string.Format("Provided DateTime1 value of '{0}' cannot be converted to a valid DateTime value.", DateTime1));
            }

            DateTime _dateTime2;
            if (!DateTime.TryParse(DateTime2, out _dateTime2))
            {
                throw new ApplicationException(string.Format("Provided DateTime2 value of '{0}' cannot be converted to a valid DateTime value.", DateTime2));
            }

            if (_dateTime2.CompareTo(_dateTime1) > 0)
            {
                return Constants.YES;
            }

            return Constants.NO;
        }

        /// <summary>
        /// Determines if a DateTime value is the current date
        /// </summary>
        public string DateTime1IsToday()
        {
            DateTime _dateTime1;
            if (!DateTime.TryParse(DateTime1, out _dateTime1))
            {
                throw new ApplicationException(string.Format("Provided DateTime1 value of '{0}' cannot be converted to a valid DateTime value.", DateTime1));
            }

            if (_dateTime1.DayOfYear == DateTime.Now.DayOfYear && _dateTime1.Year == DateTime.Now.Year)
            {
                return Constants.YES;
            }

            return Constants.NO;
        }


        /// <summary>
        /// get current date.
        /// </summary>
        public string GetDateTimeReportFormat()
        {
            return DateTime.Now.ToString("MMM dd, yyyy");

        }

        public string GetDateTimeTimeStampFormat()
        {
            return DateTime.Now.ToString("yyyyMMddHHmm");
        }


        /// <summary>
        /// convert datetime value in String1 to format specified in String2.
        /// </summary>
        public string ConvertDateTimeFormat()
        {
            if (String1 == null)
            {
                return null;
            }
            DateTime dt = Convert.ToDateTime(String1);

            return dt.ToString(String2);

        }

        public string ParseDateTimeFormat()
        {
            DateTime dt = DateTime.ParseExact(String1, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);

            return dt.ToString(String2);
        }

        public string ParseDateTimeToSpecifiedFormat()
        {
            return DateTime.Parse(String1).ToString(DateTimeFormat);
        }

        public string ConvertFromUtcToLocalTimeZone()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.Parse(DateTime1), TimeZoneInfo.Local).ToString();
        }

        /// <summary>
        /// converts the value in String1 to a currency value.
        /// </summary>
        public string ConvertToCurrency()
        {
            double dRet = Convert.ToDouble(String1);

            return dRet.ToString("0.00");
        }

        /// <summary>
        /// returns the length of String1.
        /// </summary>
        public string GetStringLength()
        {
            int nLength = 0;
            if (String1 != null)
                nLength = String1.Length;
            return nLength.ToString();
        }

        private string GetField()
        {
            if (String1 != null)
            {
                if (String1.Length > Convert.ToInt16(StartChar) + Convert.ToInt16(FieldLength))
                {
                    return String1.Substring(Convert.ToInt16(StartChar) - 1, Convert.ToInt16(FieldLength));
                }
            }
            return null;
        }

        private string IsNumeric()
        {
            if (String1 != null)
            {
                for (int nCharIndex = 0; nCharIndex < String1.Length; nCharIndex++)
                {
                    if (!Char.IsNumber(String1[nCharIndex]))
                    {
                        return Constants.NO;
                    }
                }

                Integer1 = Convert.ToInt64(String1).ToString();

                return Constants.YES;
            }

            return Constants.NO;
        }

        private string IsMonetary()
        {
            if (String1 != null)
            {
                if (!Regex.IsMatch(String1, @"^\d*\.\d\d$"))
                {
                    return Constants.NO;
                }

                Double1 = Convert.ToDouble(String1).ToString("0.00");
                return Constants.YES;
            }

            return Constants.NO;
        }

        private string TrimString()
        {
            return String1.Trim();
        }

    }
}
