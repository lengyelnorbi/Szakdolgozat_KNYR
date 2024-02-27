using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdolgozat.Models
{
    public interface IGazdalkodoSzervezetRepository
    {
        ObservableCollection<GazdalkodoSzervezet> GetGazdalkodoSzervezetek();
        bool ModifyGazdalkodoSzervezet(GazdalkodoSzervezet gazdalkodoSzervezet);
        bool DeleteGazdalkodoSzervezet(int id);
        bool AddGazdalkodoSzervezet(GazdalkodoSzervezet gazdalkodoSzervezet);
    }
}
