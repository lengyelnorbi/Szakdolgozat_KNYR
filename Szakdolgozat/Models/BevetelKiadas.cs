using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdolgozat.Models
{
    public class BevetelKiadas
    {
        public int ID { get; set; }
        public int Osszeg{ get; set; }
        public Penznem Penznem { get; set; }
        public BeKiKod BeKiKod { get; set; }
        public DateTime TeljesitesiDatum { get; set; }
        public int? KotelKovetID { get; set; }
        public int? PartnerID { get; set; }

        public BevetelKiadas(int id, int osszeg, Penznem penznem, BeKiKod beKiKod, DateTime teljesitesiDatum, int kotelKovetID, int partnerID)
        {
            ID = id;
            Osszeg = osszeg;
            Penznem = penznem;
            BeKiKod = beKiKod;
            TeljesitesiDatum = teljesitesiDatum;
            KotelKovetID = kotelKovetID;
            PartnerID = partnerID;
        }
        public BevetelKiadas(int id, int osszeg, Penznem penznem, BeKiKod beKiKod, DateTime teljesitesiDatum, int? kotelKovetID = null, int? partnerID = null)
        {
            ID = id;
            Osszeg = osszeg;
            Penznem = penznem;
            BeKiKod = beKiKod;
            TeljesitesiDatum = teljesitesiDatum;
            if (kotelKovetID == 0)
                KotelKovetID = null;
            else
                KotelKovetID = kotelKovetID;
            if (partnerID == 0)
                PartnerID = null;
            else
                PartnerID = partnerID;
        }

        public BevetelKiadas(int osszeg, Penznem penznem, BeKiKod beKiKod, DateTime teljesitesiDatum, int kotelKovetID, int partnerID)
        {
            Osszeg = osszeg;
            Penznem = penznem;
            BeKiKod = beKiKod;
            TeljesitesiDatum = teljesitesiDatum;
            KotelKovetID = kotelKovetID;
            PartnerID = partnerID;
        }
        public BevetelKiadas(int osszeg, Penznem penznem, BeKiKod beKiKod, DateTime teljesitesiDatum, int? kotelKovetID = null, int? partnerID = null)
        {
            Osszeg = osszeg;
            Penznem = penznem;
            BeKiKod = beKiKod;
            TeljesitesiDatum = teljesitesiDatum;
            if( kotelKovetID == 0)
                KotelKovetID = null;
            else
                KotelKovetID = kotelKovetID;
            if (partnerID == 0)
                PartnerID = null;
            else
                PartnerID = partnerID;
        }
    }
}
