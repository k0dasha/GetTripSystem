using System;
using System.Collections.Generic;
using System.Text;

namespace GetTripSystem.Entities
{
    public class Registration
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public int TripID { get; set; }
        public string UserStatus { get; set; }
    }
}
