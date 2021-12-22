using System;

namespace API.Extensions
{
    public static class DateTimeExtension
    {
        public static int CalculateAge (this DateTime dob)
        {
            var Today = DateTime.Today;
            var age =  Today.Year - dob.Year;
            if(dob.Date > Today.AddYears(-age)) age--;
            return age;
        }
    }
}