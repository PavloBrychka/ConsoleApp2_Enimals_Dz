// See https://aka.ms/new-console-template for more information
using System.IO;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

Console.WriteLine("Hello, World!");
Menu();

void Menu()
{
    var filepath = "temp.txt";
    var service = new EnimalService(filepath);
    var enimalcontroler = new EnimalControler(service);


    while(true)
    {
        Console.WriteLine("1 - AddEnimal");
        Console.WriteLine("2 - Print Enimals");
        Console.WriteLine("0 - Exit");


        int menu = Convert.ToInt32(Console.ReadLine());
    if(menu == 0)
        {
            break;
        }
        switch(menu)
        {
           //case 0: break;

            case 1: enimalcontroler.AddEnimal();
                break;
            case 2: enimalcontroler.Print();
                break;
            case 0:
                return;
                default: Console.WriteLine("Error enter");
                break;

        }
    }
}




public interface IEnimalDataWriter
{
    void write(Enimal enimal);
}
public class EnimalConsoleWriter : IEnimalDataWriter
{
    public void write(Enimal enimal)
    {
        Console.WriteLine("Name: " + enimal.Name);
        Console.WriteLine("Sound: " + enimal.Sound);
    }
}

public class EnimalFileWrite : IEnimalDataWriter
{
    private readonly string _file;
    public EnimalFileWrite(string path)
    {
        _file = path;
    }
    public void write(Enimal enimal)
    {
        using (var file = new StreamWriter(_file, true))
        {
            file.WriteLine("Name: " + enimal.Name + ",");
            file.WriteLine("Sound: " + enimal.Sound);
            //file.WriteLine("*");
            file.Close();
        }

      
    }
}

public interface IEnimalService
{
    void SaveEnimal(Enimal enimal);
    Enimal[] LoadEnimal();

}

public class EnimalService : IEnimalService
{
    private readonly string _path;

    public EnimalService(string path)
    {
        _path = path;
    }

    public Enimal[] LoadEnimal()
    {
       Enimal[] enimals = new Enimal[0];

        if(!File.Exists(_path))
            {
            return enimals;

        }
        using var reader = new StreamReader(_path);
        while(!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            var parts = line.Split(',');
            if(parts.Length != 2)
            {
                continue;
            }

            var enimal = new Enimal
            {
                Name = parts[0],
                Sound = parts[1]
            };

            Array.Resize(ref enimals, enimals.Length + 1);
            enimals[^1] = enimal;
        }
        return enimals;
    }

    public void SaveEnimal(Enimal enimal)
    {
        using (var file = new StreamWriter(_path, true))
        {
            file.WriteLine("Name: " + enimal.Name);
            file.WriteLine("Sound: " + enimal.Sound);
            file.WriteLine("*");
            file.Close();
        }
    }
}

public class EnimalControler
{
    private readonly IEnimalService _enimalservice;

    public EnimalControler(IEnimalService enimalservice)
    {
        _enimalservice = enimalservice;
    }

    public void AddEnimal()
    {
        Console.Write("Enter Name Enimal_ ");
        string temp1 = Console.ReadLine();
        Console.Write("Enter Sound Enimal_ ");
        string temp2 = Console.ReadLine();
        var enimal = new Enimal
        {
            Name = temp1,
            Sound = temp2
        };
      
        _enimalservice.SaveEnimal(enimal);
        Console.WriteLine("Save");

    }

    public void Print()
    {
        var list =_enimalservice.LoadEnimal();
        Console.Clear();
        foreach (var item in list)
        {
            Console.Write(item.Name);
            Console.WriteLine(item.Sound);

           
        }
        Console.ReadKey();
        Console.Clear();

    }
}

public class Enimal
{

    /////private readonly IEnimalDataWriter _writer;
    public string Name { get; set; }
    public string Sound { get; set; }


    ////////////public Enimal(IEnimalDataWriter writer) {
    ////////////    _writer = writer;
    ////////////}


    //////public void Print()
    //////{
    //////    _writer.write(this);
    //////}

    //  public Enimal(string name, string sound)
    //{
    //  Name = name;
    //Sound = sound;
    // }

}


