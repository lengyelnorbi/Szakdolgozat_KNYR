using System.Collections.ObjectModel;

namespace Szakdolgozat.Models
{
    public interface IDolgozoRepository
    {
        ObservableCollection<Dolgozo> GetDolgozok();
        bool ModifyDolgozo(Dolgozo dolgozo);
        bool DeleteDolgozo(int id);
        (bool,int) AddDolgozo(Dolgozo dolgozo);
        bool CheckForRelatedRecords(int id, out string relatedInfo);
        bool DeleteDolgozo(int id, bool confirmCascade = true);
    }
}
