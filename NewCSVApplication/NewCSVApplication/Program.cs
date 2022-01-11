using System;
using System.Collections.Generic;

namespace CSVApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Dictionary<string, string> keys = new Dictionary<string, string>();

            //creating dictionary as column name and values present in CSV
            keys.Add("id", "2");
            keys.Add("name", "a");
            keys.Add("address", "b");
            keys.Add("status", "400");

            Linq2CSVInput linq2CSVInput = new Linq2CSVInput();

            //reads the CSV and returned the matched rows from CSV
            List<dynamic> dynamicData = linq2CSVInput.ReadFileAsObject(@"C:\Desktop\Projects\CSVApplication\NewCSVApplication\CSV\stub.csv", keys);

            //dynamicData contains values from CSV
            foreach (var item in dynamicData)
            {
                Console.WriteLine(item.id);
                Console.WriteLine(item.name);
                Console.WriteLine(item.address);
                Console.WriteLine(item.status);
                Console.WriteLine(item.code);
                Console.WriteLine(item.error);
            }
            Console.ReadLine();
        }
    }
}
