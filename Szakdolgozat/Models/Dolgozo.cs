using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdolgozat.Models
{
    public class Dolgozo
    {
        public int ID { get; set; }
        public string Vezeteknev { get; set; }
        public string Keresztnev { get; set; }
        public string Email { get; set; }
        public string Telefonszam { get; set; }

        public Dolgozo(int id, string vezeteknev, string keresztnev, string email, string telefonszam)
        {
            ID = id;
            Vezeteknev = vezeteknev;
            Keresztnev = keresztnev;
            Email = email;
            Telefonszam = telefonszam;
        }
    }
}
