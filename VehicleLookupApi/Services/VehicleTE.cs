using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class VehicleTE : TableEntity
    {
        private string _manufacturer;
        private string _model;

        public VehicleTE()
        {

        }
         public VehicleTE(int id, int year, string manufacturer, string model)
        {
            this.Id = id;
            this.Year = year;
            this.Manufacturer = manufacturer;
            this.Model = model;

        }

        public int Id { get; set; }
        public int Year { get; set; }
        public String Manufacturer { get { return _manufacturer; } set { _manufacturer = value; PartitionKey = value; } }
        public String Model { get { return _model; } set { _model = value; RowKey = value; } }
    }
}
