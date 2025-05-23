namespace Szakdolgozat.Models
{
    public class MaganSzemely
    {
        public int ID { get; set; }
        public string Nev { get; set; }
        public string Telefonszam { get; set; }
        public string Email { get; set; }
        public string Lakcim { get; set; }

        public MaganSzemely(int id, string nev, string telefonszam, string email, string lakcim)
        {
            ID = id;
            Nev = nev;
            Telefonszam = telefonszam;
            Email = email;
            Lakcim = lakcim;
        }
        public MaganSzemely(string nev, string telefonszam, string email, string lakcim)
        {
            Nev = nev;
            Telefonszam = telefonszam;
            Email = email;
            Lakcim = lakcim;
        }
    }
}
