using System;
using System.Collections.Generic;
using System.Text;

namespace GetTripSystem
{
    public class Trip
    {
        public int Id { get; set; }
        public string TripName { get; set; }
        public string Location { get; set; }
        public int CurMembs_amount { get; set; }
        public int MaxMembs_amount { get; set; }
        public int CreatorID { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string CreatorContact { get; set; }
       
    }
}
