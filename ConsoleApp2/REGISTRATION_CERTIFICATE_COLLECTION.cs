using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsoleApp2;

public class REGISTRATION_CERTIFICATE_COLLECTION
{
    private List<REGISTRATION_CERTIFICATE> _list;

    public void  serilizeToFile()
    {
        var json = JsonSerializer.Serialize(_list);
        using StreamWriter file = new("../../../data.json");
        file.WriteLineAsync(json);
    }

    public void deserealizeFromFile()
    {
        List<REGISTRATION_CERTIFICATE> result = new List<REGISTRATION_CERTIFICATE>();
        string json = new StreamReader("../../../data.json").ReadToEnd();
        _list = JsonSerializer.Deserialize<List<REGISTRATION_CERTIFICATE>>(json);
        var indexToDelete = new List<int>();
        for (int i =0; i<_list.Count-1;++i)
        {
            try
            {
                _list[i].validateWholeClass();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                indexToDelete.Add(i);
            }
        }

        foreach (var index in indexToDelete)
        {
            _list.RemoveAt(index);
        }
    }

    private REGISTRATION_CERTIFICATE certificateByID(int id)
    {
        foreach (var certificate in _list)
        {
            if (certificate._id == id)
                return certificate;
        }

        return null;
    }

    public REGISTRATION_CERTIFICATE_COLLECTION()
    {
        _list = new List<REGISTRATION_CERTIFICATE>();
    }


    public List<REGISTRATION_CERTIFICATE> searchByValue(string value)
    {
        var result = new List<REGISTRATION_CERTIFICATE>();
        foreach (var item in _list)
        {
            foreach (var field in item.GetType().GetProperties())
            {
                if (field.GetValue(item).ToString().ToLower().Contains(value.ToLower()))
                {
                    result.Add(item);
                    break;
                }
            }
        }
        return result;
    }

    public override string ToString()
    {
        string resultStr = new string("");
        foreach (var certificate in _list)
        {
            resultStr += certificate.ToString() + '\n';
        }

        return resultStr;
    }

    public void sort(string field_to_sort_by="_id")
    {
        if(_list.Count == 0)
            return;

        var type = _list[0].GetType();
        if (type.GetProperty(field_to_sort_by).CanRead)
        {
            _list.Sort((x, y) =>
                type.GetProperty(field_to_sort_by).GetValue(x).ToString().ToLower()
                    .CompareTo(type.GetProperty(field_to_sort_by).GetValue(y).ToString().ToLower()));
        }
    }

    public bool checkIfIdIsUnique(int id)
    {
        foreach (var certificate in _list)
        {
            if (certificate._id == id)
                return false;
        }

        return true;
    }
    public void Add(REGISTRATION_CERTIFICATE certificate)
    {
        if (!checkIfIdIsUnique(certificate._id))
        {
            Console.WriteLine("You have registration certificate with the same ID");
            return;
        }
        _list.Add(certificate);
        serilizeToFile();
    }

    public void Remove(int index)
    {
        _list.Remove(certificateByID(index));
        serilizeToFile();
    }

    public void Edit(int index)
    {
        REGISTRATION_CERTIFICATE foundCertificate = null;
        foreach (var _certificate in _list)
        {
            if (_certificate._id == index)
            {
                foundCertificate = _certificate;
                break;
            }
        }
        if (foundCertificate == null)
        {
            throw new Exception("Invalid id");
        }

        Console.WriteLine("Please enter field which you want to edit");
        var field = Console.ReadLine();
        Console.WriteLine("Please enter new value");
        var newval = Console.ReadLine();
        var funname = "Set" + field;

        MethodInfo method = foundCertificate.GetType().GetMethod(funname);
        method.Invoke(foundCertificate, new object[] {newval});

        
        serilizeToFile();
    }
    
    
}