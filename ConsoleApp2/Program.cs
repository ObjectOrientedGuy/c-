
namespace ConsoleApp2
{
    public class Registration
    {

        // This is the main entry point for the application.
        public static void Main(string[] args)
        {
            var d = new REGISTRATION_CERTIFICATE("1", "ВС9909АВ","5/1/2008","4Y1SL65848Z411439","test","2007");
            var dq = new REGISTRATION_CERTIFICATE("2", "ВС9908АВ","5/1/2008","4Y1SL65848Z411439","TEst","2008");
            var dw = new REGISTRATION_CERTIFICATE("3", "ВС9907АВ","5/1/2008","4Y1SL65848Z411439","Audi","2002");
            var a = new REGISTRATION_CERTIFICATE_COLLECTION();
            var menu = new Menu();
        }
    }
}
