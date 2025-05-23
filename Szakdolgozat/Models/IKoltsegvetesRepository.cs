using System.Collections.ObjectModel;

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
