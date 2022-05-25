using System.Text.RegularExpressions;

namespace ConsoleApp2;

public class Validation
{
    public static int ValidateId(int id)
    {
        if (id < 0)
            throw new Exception("Invalid ID");
        return id;
    }

    public static string ValidateRegistrationNumber(string number)
    {
        Regex re = new Regex("[А-я]{2}\\d{4}[А-я]{2}");
        if (!re.IsMatch(number))
        {
            throw new Exception("Invalid Registration Number");
        }

        return number;
    }

    public static DateTime ValidateDateOfRegistration(string date)
    {
        if(!DateTime.TryParse(date,out var returnDate))
            throw new Exception("Invalid date of registration");

        return returnDate;
    }

    public static string ValidateVINCode(string vinCode)
    {
        Regex re = new Regex("[A-HJ-NPR-Za-hj-npr-z\\d]{8}[\\dX][A-HJ-NPR-Za-hj-npr-z\\d]{2}\\d{6}");
        if (vinCode.Length != 17 || !re.IsMatch(vinCode))
        {
            throw new Exception("Invalid VIN Code");
        }

        return vinCode;
    }
}