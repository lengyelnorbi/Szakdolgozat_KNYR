using System.Collections.ObjectModel;

namespace Szakdolgozat.Models
{
    public interface IKotelezettsegKovetelesRepository
    {
        ObservableCollection<KotelezettsegKoveteles> GetKotelezettsegekKovetelesek();
        bool ModifyKotelezettsegKoveteles(KotelezettsegKoveteles kotelezettsegKoveteles);
        bool DeleteKotelezettsegKoveteles(int id);
        bool AddKotelezettsegKoveteles(KotelezettsegKoveteles kotelezettsegKoveteles);
        bool CheckForRelatedRecords(int id, out string relatedInfo);
        bool DeleteKotelezettsegKoveteles(int id, bool confirmCascade = true);
    }
}
