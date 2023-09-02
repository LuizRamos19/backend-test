
using System;

namespace teste.ApiCore31.Helpers
{
    /// <summary>
    /// Class for entries treatment
    /// </summary>
    public static class TreatmentHelper
    {
        /// <summary>
        /// Understand format date from text
        /// </summary>
        /// <param name="value">Date value used in the text</param>
        /// <returns>DateTime</returns>
        public static DateTime TreatDateFromText(string value)
        {
            var splitedValue = value.Split('/');
            var day = Convert.ToInt16(splitedValue[1]);
            var month = Convert.ToInt16(splitedValue[0]);
            var year = Convert.ToInt16(splitedValue[2].Split(' ')[0]);
            var hour = Convert.ToInt16(splitedValue[2].Split(' ')[1].Split(':')[0]);
            var minute = Convert.ToInt16(splitedValue[2].Split(' ')[1].Split(':')[1]);
            return new DateTime(year, month, day, hour, minute, 0);
        }
    }
}