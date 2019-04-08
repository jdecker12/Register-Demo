using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthDemo.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string NormalizedUsername { get; set; }
        public string PaswordHash { get; set; }
    }
}

