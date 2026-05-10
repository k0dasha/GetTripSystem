using System;
using System.Collections.Generic;
using System.Text;

namespace GetTripSystem
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PasswdHash { get; set; }
        public bool Banned { get; set; }

    }
}
