﻿namespace Szakdolgozat.Models
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
        public GazdalkodoSzervezet(string nev, string kapcsolattarto, string email, string telefonszam)
        {
            Nev = nev;
            Kapcsolattarto = kapcsolattarto;
            Email = email;
            Telefonszam = telefonszam;
        }
    }
}
