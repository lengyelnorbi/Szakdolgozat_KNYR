using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
