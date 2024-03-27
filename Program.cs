// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json; //json.NET aussi fonctionne si tu veux des NuGets


Console.Write("Vous etes dans le repertoire :  ");
Console.WriteLine(Directory.GetCurrentDirectory());


var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");
var salesFiles = chercheFich(storesDirectory);

var salesTotal = CalculateSalesTotal(salesFiles);

//Creer le dir des totaux
var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir);


Console.WriteLine("Fichiers se trouvant dans les repertoires :");
foreach (var file in salesFiles)
{
    Console.WriteLine(file);
}

File.WriteAllText(Path.Combine(salesTotalDir, "totals.txt"), String.Empty);

IEnumerable<string> chercheFich(string folderName)
{
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        // The file name will contain the full path, so only check the end of it
        var extension = Path.GetExtension(file);
        if (extension == ".json")
        {
            salesFiles.Add(file);
        }
        /* ou ceci --> if (file.EndsWith("sales.json"))
        {
            salesFiles.Add(file);
        }*/
    }

    return salesFiles;
}

double CalculateSalesTotal(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;
    
    // Loop over each file path in salesFiles
    foreach (var file in salesFiles)
    {      
        // Read the contents of the file
        string salesJson = File.ReadAllText(file);
    
        // Parse the contents as JSON
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);
    
        // Add the amount found in the Total field to the salesTotal variable
        salesTotal += data?.Total ?? 0;
    }
    
    return salesTotal;
}

record SalesData (double Total);