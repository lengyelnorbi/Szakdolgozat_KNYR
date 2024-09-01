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
        public bool IsSelected { get; set; }

        public BevetelKiadas(int id, int osszeg, Penznem penznem, BeKiKod beKiKod, DateTime teljesitesiDatum, int kotelKovetID, int partnerID, bool isSelected = false)
        {
            ID = id;
            Osszeg = osszeg;
            Penznem = penznem;
            BeKiKod = beKiKod;
            TeljesitesiDatum = teljesitesiDatum;
            KotelKovetID = kotelKovetID;
            PartnerID = partnerID;
            IsSelected = isSelected;
        }
        public BevetelKiadas(int id, int osszeg, Penznem penznem, BeKiKod beKiKod, DateTime teljesitesiDatum, int? kotelKovetID = null, int? partnerID = null, bool isSelected = false)
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
            IsSelected = isSelected;
        }

        public BevetelKiadas(int osszeg, Penznem penznem, BeKiKod beKiKod, DateTime teljesitesiDatum, int kotelKovetID, int partnerID, bool isSelected = false)
        {
            Osszeg = osszeg;
            Penznem = penznem;
            BeKiKod = beKiKod;
            TeljesitesiDatum = teljesitesiDatum;
            KotelKovetID = kotelKovetID;
            PartnerID = partnerID;
            IsSelected = isSelected;
        }
        public BevetelKiadas(int osszeg, Penznem penznem, BeKiKod beKiKod, DateTime teljesitesiDatum, int? kotelKovetID = null, int? partnerID = null, bool isSelected = false)
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
            IsSelected = isSelected;
        }
    }
}
