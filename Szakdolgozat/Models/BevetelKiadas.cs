﻿using System;

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
        public int? GazdalkodasiSzervID { get; set; }
        public int? MaganszemelyID { get; set; }
        public bool IsSelected { get; set; }

        public BevetelKiadas(int id, int osszeg, Penznem penznem, BeKiKod beKiKod, DateTime teljesitesiDatum, int kotelKovetID, int gazdalkodasiSzervID, int maganSzemelyID, bool isSelected = false)
        {
            ID = id;
            Osszeg = osszeg;
            Penznem = penznem;
            BeKiKod = beKiKod;
            TeljesitesiDatum = teljesitesiDatum;
            KotelKovetID = kotelKovetID;
            GazdalkodasiSzervID = gazdalkodasiSzervID;
            MaganszemelyID = maganSzemelyID;
            IsSelected = isSelected;
        }
        public BevetelKiadas(int id, int osszeg, Penznem penznem, BeKiKod beKiKod, DateTime teljesitesiDatum, int? kotelKovetID = null, int? gazdalkodasiSzervID = null, int? maganSzemelyID = null, bool isSelected = false)
        {
            ID = id;
            Osszeg = osszeg;
            Penznem = penznem;
            BeKiKod = beKiKod;
            TeljesitesiDatum = teljesitesiDatum;
            if (kotelKovetID == 0)
                KotelKovetID = 0;
            else
                KotelKovetID = kotelKovetID;
            if (maganSzemelyID == 0)
                MaganszemelyID = 0;
            else
                MaganszemelyID = maganSzemelyID;
            if (gazdalkodasiSzervID == 0)
                GazdalkodasiSzervID = 0;
            else
                GazdalkodasiSzervID = gazdalkodasiSzervID;
            IsSelected = isSelected;
        }

        public BevetelKiadas(int osszeg, Penznem penznem, BeKiKod beKiKod, DateTime teljesitesiDatum, int kotelKovetID, int gazdalkodasiSzervID, int maganSzemelyID, bool isSelected = false)
        {
            Osszeg = osszeg;
            Penznem = penznem;
            BeKiKod = beKiKod;
            TeljesitesiDatum = teljesitesiDatum;
            KotelKovetID = kotelKovetID;
            GazdalkodasiSzervID = gazdalkodasiSzervID;
            MaganszemelyID = maganSzemelyID;
            IsSelected = isSelected;
        }
        public BevetelKiadas(int osszeg, Penznem penznem, BeKiKod beKiKod, DateTime teljesitesiDatum, int? kotelKovetID = null, int? gazdalkodasiSzervID = null, int? maganSzemelyID = null, bool isSelected = false)
        {
            Osszeg = osszeg;
            Penznem = penznem;
            BeKiKod = beKiKod;
            TeljesitesiDatum = teljesitesiDatum;
            if( kotelKovetID == 0)
                KotelKovetID = null;
            else
                KotelKovetID = kotelKovetID;
            if (maganSzemelyID == 0)
                MaganszemelyID = null;
            else
                MaganszemelyID = maganSzemelyID;
            if (gazdalkodasiSzervID == 0)
                GazdalkodasiSzervID = null;
            else
                GazdalkodasiSzervID = gazdalkodasiSzervID;
            IsSelected = isSelected;
        }
    }
}
