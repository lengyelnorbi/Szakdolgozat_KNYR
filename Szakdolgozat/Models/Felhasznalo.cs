namespace Szakdolgozat.Models
{
    public class Felhasznalo
    {
        public int ID { get; set; }
        public string FelhasznaloNev { get; set; }
        public string Jelszo { get; set; }
        public int DolgozoID { get; set; }
        //public Role Role { get; set; }

        public Felhasznalo(int id, string felhasznaloNev, string jelszo, int dolgozoID)
        {
            ID = id;
            FelhasznaloNev = felhasznaloNev;
            Jelszo = jelszo;
            DolgozoID = dolgozoID;
        }
    }
}
