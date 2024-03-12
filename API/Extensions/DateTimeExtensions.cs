namespace API.Extensions
{
    public static class DateTimeExtensions
    {
       public static int CalculateAge(this DateOnly dob)
       {
           var today = DateOnly.FromDateTime(DateTime.UtcNow);
           var age = today.Year - dob.Year;
           if(dob > today.AddYears(-age)) age--; //user henuz yeni yasina girmemise yasini 1 azalt.
           return age;
       }
    }
}