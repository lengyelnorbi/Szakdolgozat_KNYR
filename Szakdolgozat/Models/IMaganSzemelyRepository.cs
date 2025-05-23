using System.Collections.ObjectModel;

namespace Szakdolgozat.Models
{
    public interface IMaganSzemelyRepository
    {
        ObservableCollection<MaganSzemely> GetMaganSzemelyek();
        bool ModifyMaganSzemely(MaganSzemely maganSzemely);
        bool DeleteMaganSzemely(int id);
        bool AddMaganSzemely(MaganSzemely maganSzemely);
        bool CheckForRelatedRecords(int id, out string relatedInfo);
        bool DeleteMaganSzemely(int id, bool confirmCascade = true);
    }
}
