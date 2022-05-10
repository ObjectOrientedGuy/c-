using System.Reflection;

namespace ConsoleApp2;

public class REGISTRATION_CERTIFICATE: ICollectionItem
{
    public int _id{ get; set;}
    public string _registration_number{ get; set; }
    public DateTime _date_of_registration { get; set;}
    public string _VIN_code{ get; set;}
    public string _car{ get; set;}
    public int  _year_of_manufacture{ get; set;}


    public void validateWholeClass()
    {
        Validation.ValidateRegistrationNumber(_registration_number);
        Validation.ValidateDateOfRegistration(_date_of_registration.ToString());
        Validation.ValidateId(_id);
        Validation.ValidateVINCode(_VIN_code);
    }
    public REGISTRATION_CERTIFICATE(){}
    public override string ToString()
    {
        return $"ID:{_id} \n" +
               $"Registration number:{_registration_number} \n" +
               $"Date of registration:{_date_of_registration} \n" +
               $"VIN code:{_VIN_code} \n" +
               $"Car:{_car} \n" +
               $"Year of manufacture:{_year_of_manufacture} \n";
    }

    public REGISTRATION_CERTIFICATE(string id, string registration_number,string date_of_registration, string vinCode, string nameOfCar, string yearOfCar)
    {
        Set_id(id);
        Set_registration_number(registration_number);
        Set_date_of_registration(date_of_registration);
        Set_VIN_code(vinCode);
        Set_car(nameOfCar);
        Set_year_of_manufacture(yearOfCar);
    }

    
    public void Set_id(string idStr)
    {
        var id = Convert.ToInt32(idStr);
        _id = Validation.ValidateId(id);
    }
    public void Set_registration_number(string number)
    {

            _registration_number = Validation.ValidateRegistrationNumber(number);

    }

    public void Set_date_of_registration(string date)
    {
        _date_of_registration = Validation.ValidateDateOfRegistration(date);
        }

    public void Set_VIN_code(string vinCode)
    {
        _VIN_code = Validation.ValidateVINCode(vinCode);
        }

    public void Set_car(string car)
    {
        _car = car;
    }

    public void Set_year_of_manufacture(string yearStr)
    {
        var year = Convert.ToInt32(yearStr);
        if(_date_of_registration.Year < year)
        {
            var exception = new Exception("Invalid date of registration");
            throw exception;
        }
        
        _year_of_manufacture = year;
    }

    public static REGISTRATION_CERTIFICATE readFromConsole()
    {
        var resultCertificate = new REGISTRATION_CERTIFICATE();

        try
        {
            Console.WriteLine("Please enter ID: ");
            resultCertificate.Set_id(Console.ReadLine());
            Console.WriteLine("Please enter car name: ");
            resultCertificate.Set_car(Console.ReadLine());
            Console.WriteLine("Please enter registration number: ");
            resultCertificate.Set_registration_number(Console.ReadLine());
            Console.WriteLine("Please enter date of registration: ");
            resultCertificate.Set_date_of_registration(Console.ReadLine());
            Console.WriteLine("Please enter VIN code: ");
            resultCertificate.Set_VIN_code(Console.ReadLine());
            Console.WriteLine("Please enter year of car: ");
            resultCertificate.Set_year_of_manufacture(Console.ReadLine());
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message.Trim('\n'));
            return readFromConsole();
        }
        
        return resultCertificate;
    }
}