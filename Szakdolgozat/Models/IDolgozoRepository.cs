using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdolgozat.Models
{
    public interface IDolgozoRepository
    {
        ObservableCollection<Dolgozo> GetDolgozok();
        bool ModifyDolgozo(Dolgozo dolgozo);
        bool DeleteDolgozo(int id);
        bool AddDolgozo(Dolgozo dolgozo);
        bool CheckForRelatedRecords(int id, out string relatedInfo);
        bool DeleteDolgozo(int id, bool confirmCascade = true);
    }
}
