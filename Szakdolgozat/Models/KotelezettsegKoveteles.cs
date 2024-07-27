using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdolgozat.Models
{
    public class KotelezettsegKoveteles
    {
        public int ID { get; set; }
        public string Tipus { get; set; }
        public int Osszeg { get; set; }
        public Penznem Penznem { get; set; }
        public DateTime KifizetesHatarideje { get; set; }
        public Int16 Kifizetett { get; set; }

        public KotelezettsegKoveteles(int id, string tipus, int osszeg, Penznem penznem, DateTime kifizetesHatarideje, Int16 kifizetett)
        {
            ID = id;
            Tipus = tipus;
            Osszeg = osszeg;
            Penznem = penznem;
            KifizetesHatarideje = kifizetesHatarideje;
            Kifizetett = kifizetett;
        }
        public KotelezettsegKoveteles(string tipus, int osszeg, Penznem penznem, DateTime kifizetesHatarideje, Int16 kifizetett)
        {
            Tipus = tipus;
            Osszeg = osszeg;
            Penznem = penznem;
            KifizetesHatarideje = kifizetesHatarideje;
            Kifizetett = kifizetett;
        }
    }
}
