using Microsoft.AspNetCore.Mvc;
using DML01Controllers.Models;
using System.Text;


namespace DML01Controllers.Controllers;



public class DictionaryController : Controller
{
    private static List<MyDictionaryEntry>_entries =
     new List<MyDictionaryEntry>();

     [HttpGet]

     public IActionResult Load()
     {
        return View();
     }
     
     [HttpGet]
       public IActionResult AddWord() //Display the form to add a new word
    {
        
        return View();
    }
 
   [HttpPost]
    public IActionResult AddWord(MyDictionaryEntry entry)
    {
        if (!string.IsNullOrEmpty(entry.Word) &&
         !string.IsNullOrEmpty(entry.Meaning))
        {
            _entries.Add(entry);  // Add the new word and its meaning to the dictionary
            return RedirectToAction("Index");
        }

        return View(entry); // If the input is invalid, redisplay the form with the entered data    
    }

    public IActionResult Index()
    {
        return View(_entries); // Display the entries in the Index view
    }

    public IActionResult Save()
    {
        var csv = new StringBuilder();

        foreach (var entry in _entries)
        {
            csv.AppendLine($"{entry.Word},{entry.Meaning}");
        }

        byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
        var stream = new MemoryStream(bytes);

        return File(stream, "text/csv", "dictionary.csv"); // Return the CSV file for download  

    }
     

    [HttpPost]
     public async Task<IActionResult> DoUpload(IList<IFormFile> files)
    {
        IFormFile? fileToUpload =
           files.FirstOrDefault(file => file.Length > 0);
        
        if (fileToUpload == null)
        {
            return RedirectToAction("Index");
        }

        var newEntries = new List<MyDictionaryEntry>();

        using var reader = new StreamReader(
            fileToUpload.OpenReadStream(),
            Encoding.UTF8);
        
        string? line;

        while ((line = await reader.ReadLineAsync()) != null)
        {
            var fields = line.Split(',',2);

            if (fields.Length == 2)
            {
                newEntries.Add(new MyDictionaryEntry
                {
                    Word = fields[0],
                    Meaning = fields[1]
                });
            }    
    
        }

        _entries.Clear();
        _entries.AddRange(newEntries);

        return RedirectToAction("Index");

        
    }
} 