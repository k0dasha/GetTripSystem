using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using static GetTripSystem.DAL;

namespace GetTripSystem.Entities
{
    public class Picture
    {
        public int Id { get; set; }
        public int TripID { get; set; }
        public string FilePath { get; set; }
    }
}
