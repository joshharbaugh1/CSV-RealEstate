﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_RealEstate
{
    class Program
    {
        static void Main(string[] args)
        {
            List<RealEstateData> realEstateDataList = new List<RealEstateData>();
            //read in the realestatedata.csv file.  As you process each row, you'll add a new 
            // RealEstateData object to the list for each row of the document, excluding the first.
                using (StreamReader reader = new StreamReader("realestatedata.csv"))
            {                    
                // Get and don't use the first line
                string firstline = reader.ReadLine();
                // Loop through the rest of the lines
                while(!reader.EndOfStream) {
                    realEstateDataList.Add(new RealEstateData(reader.ReadLine()));
                }               
            }
         

            //Display the average square footage of a Condo sold in the city of Sacramento, 
            // round to 2 decimal points
            Console.WriteLine(Math.Round(realEstateDataList.Average(x=>x.SQ_FT),2));
            //Display the total sales of all residential homes in Elk Grove, display in dollars
            Console.WriteLine(realEstateDataList.Where(x=>x.City.ToLower()=="elk grove").Sum(y=>y.Price).ToString("C0"));
            //Display the total number of residential homes sold in the following  
            // zip codes: 95842, 95825, 95815
            Console.WriteLine(realEstateDataList.Where(x=>x.Type==RealEstateType.Residential).Where(y=>y.Zip==95842||y.Zip==95825||y.Zip==95815).Count());
            //Display the average sale price of a lot in Sacramento, display in dollars
            Console.WriteLine(realEstateDataList.Where(x=>x.City.ToLower()=="sacramento").Average(y=>y.Price).ToString("C2"));
            //Display the average price per square foot for a condo in Sacramento, display in dollars
            Console.WriteLine((realEstateDataList.Where(x => x.City.ToLower() == "sacramento").Average(y => y.Price) / realEstateDataList.Where(x => x.City.ToLower() == "sacramento").Average(y => y.SQ_FT)).ToString("C2"));
            //Display the number of all sales that were completed on a Wednesday
            Console.WriteLine(realEstateDataList.Where(x=>x.SaleDate.DayOfWeek==System.DayOfWeek.Wednesday).Count());
            //Display the average number of bedrooms for a residential home in Sacramento when the 
            // price is greater than 300000, round to 2 decimal points
            Console.WriteLine(Math.Round(realEstateDataList.Where(x => x.City.ToLower() == "sacramento").Where(y => y.Price > 300000).Average(z=>z.Beds),2));
            //Extra Credit:
            //Display top 5 cities and the number of homes sold (using the GroupBy extension)
            var topCityList = realEstateDataList.GroupBy(x => x.City).Select(x => new { CityName = x.Key, CityCount = x.Count() }).OrderByDescending(x => x.CityCount).Take(5);
            foreach (var dataItem in topCityList)
            {
                Console.WriteLine("{0} : {1}", dataItem.CityName,dataItem.CityCount);
            }

            // keep the console open
            Console.ReadLine();
        }
    }

    public enum RealEstateType
    {
        //fill in with enum types: Residential, MultiFamily, Condo, Lot
        Residential,
        MultiFamily,
        Condo,
        Lot
    }

    public class RealEstateData
    {
        //Create properties, using the correct data types (not all are strings) for all columns of the CSV
        public string Street { get; set; }
        public string City { get; set; }
        public int Zip { get; set; }
        public string State { get; set; }
        public int Beds { get; set; }
        public int Baths { get; set; }
        private int _sqft;

        public int SQ_FT
        {
            get { return _sqft; }
            set { if (value == 0) { this.Type = RealEstateType.Lot; } _sqft = value; }
        }
        
        public RealEstateType Type { get; set; }
        public DateTime SaleDate { get; set; }
        public int Price { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        //The constructor will take a single string arguement.  This string will be one line of the real estate data.
        // Inside the constructor, you will seperate the values into their corrosponding properties, and do the necessary conversions
        public RealEstateData(string lineInput)
        {
            string[] realEstateLine = lineInput.Split(',');
            this.Street = realEstateLine[0];
            this.City = realEstateLine[1];
            this.Zip = int.Parse(realEstateLine[2]);
            this.State = realEstateLine[3];
            this.Beds = int.Parse(realEstateLine[4]);
            this.Baths = int.Parse(realEstateLine[5]);
            this.SQ_FT = int.Parse(realEstateLine[6]);
            switch (realEstateLine[7])
            {
                case "Residential": this.Type = RealEstateType.Residential; break;
                case "Condo": this.Type = RealEstateType.Condo; break;
                case "Multi-Family": this.Type = RealEstateType.MultiFamily; break;
                default: this.Type = RealEstateType.Lot; break;
            }
            this.SaleDate = DateTime.Parse(realEstateLine[8]);
            this.Price = int.Parse(realEstateLine[9]);
            this.Latitude = double.Parse(realEstateLine[10]);
            this.Longitude = double.Parse(realEstateLine[11]);
        }
        //When computing the RealEstateType, if the square footage is 0, then it is of the Lot type, otherwise, use the string
        // value of the "Type" column to determine its corresponding enumeration type.
    }
}