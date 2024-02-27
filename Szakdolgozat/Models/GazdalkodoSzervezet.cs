using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdolgozat.Models
{
    public class GazdalkodoSzervezet
    {
        public int ID { get; set; }
        public string Nev { get; set; }
        public string Kapcsolattarto { get; set; }
        public string Email { get; set; }
        public string Telefonszam { get; set; }

        public GazdalkodoSzervezet(int id, string nev, string kapcsolattarto, string email, string telefonszam)
        {
            ID = id;
            Nev = nev;
            Kapcsolattarto = kapcsolattarto;
            Email = email;
            Telefonszam = telefonszam;
        }
    }
}
