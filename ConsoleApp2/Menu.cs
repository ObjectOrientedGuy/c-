namespace ConsoleApp2;

using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using ConsoleTools;

public class Menu
{
    private REGISTRATION_CERTIFICATE_COLLECTION _collection;
    public Menu()
    {
        _collection = new REGISTRATION_CERTIFICATE_COLLECTION();
        
        _collection.deserealizeFromFile();
        var dictionary = new Dictionary<int, Delegate>()
        {
            { 1, Add},
            { 2, Remove},
            { 3, Find},
            { 4, Sort},
            { 5, Edit},
        };
        
        const string text = "1 - add\n" +
                            "2 - remove\n" +
                            "3 - search\n" +
                            "4 - sort\n" +
                            "5 - edit\n" +
                            "0 - exit";
        
        foreach (var operation in InterfaceGetOperation(text, 0)) {
            try
            {
                dictionary[operation].DynamicInvoke();
            }
            catch (Exception e)
            {
                Console.WriteLine("Incorect index\n" +
                                  "try again");
            }
            
        }

    }
    
    static public IEnumerable<int> InterfaceGetOperation(string text, int exitKey) {
        while (true)
        {
            Console.WriteLine(text);
            int operation = Convert.ToInt32(Console.ReadLine());
                   
            if (operation == exitKey)
            {
                break;
            }
            yield return operation;
        }
    }

    private void Sort()
    {
        Console.WriteLine("Please enter field by which you want sort: ");
        var value = Console.ReadLine();

        try
        {
            _collection.sort(value);
            Console.Write(_collection);
        }
        catch (Exception e)
        {
            Console.WriteLine("There isn't such fields");
        }
    }

    private void Find()
    {
        Console.WriteLine("Please enter value for search: ");
        var value = Console.ReadLine();
        var seachedByValue = _collection.searchByValue(value);
        seachedByValue.ForEach(item => Console.WriteLine(item));
        if(seachedByValue.Count==0)
            Console.WriteLine("There is no such elements");
    }

    private void Add()
    {
        var certificate = REGISTRATION_CERTIFICATE.readFromConsole();
        _collection.Add(certificate);
        Console.Write(_collection);
    }

    private void Remove()
    {
        Console.WriteLine("Please enter id:");
        try
        {
            var index = Convert.ToInt32(Console.ReadLine());
            _collection.Remove(index);
            Console.Write(_collection);
        }
        catch (Exception e)
        {
            Console.WriteLine("Invalid index \n" +
                              "Try again");
        }
       
    }

    private void Edit()
    {
        Console.WriteLine("Please enter id of certificate which you want to edit:");
        try
        {
            var index = Convert.ToInt32(Console.ReadLine());
            _collection.Edit(index);
            Console.Write(_collection);
        }
        catch (Exception e)
        {
            Console.WriteLine("Invalid values");
        }
        
    }
}