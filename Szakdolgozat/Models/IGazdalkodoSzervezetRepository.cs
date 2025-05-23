using System.Collections.ObjectModel;

namespace Szakdolgozat.Models
{
    public interface IGazdalkodoSzervezetRepository
    {
        ObservableCollection<GazdalkodoSzervezet> GetGazdalkodoSzervezetek();
        bool ModifyGazdalkodoSzervezet(GazdalkodoSzervezet gazdalkodoSzervezet);
        bool DeleteGazdalkodoSzervezet(int id);
        bool AddGazdalkodoSzervezet(GazdalkodoSzervezet gazdalkodoSzervezet);
        bool CheckForRelatedRecords(int id, out string relatedInfo);
        bool DeleteGazdalkodoSzervezet(int id, bool confirmCascade = true);
    }
}
