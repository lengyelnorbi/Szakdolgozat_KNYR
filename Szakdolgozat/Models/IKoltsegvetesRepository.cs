using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdolgozat.Models
{
    public interface IKoltsegvetesRepository
    {
        ObservableCollection<BevetelKiadas> GetKoltsegvetesek();
        bool ModifyKoltsegvetes(BevetelKiadas bevetelKiadas);
        bool DeleteKoltsegvetes(int id);
        bool AddKoltsegvetes(BevetelKiadas bevetelKiadas);
    }
}
