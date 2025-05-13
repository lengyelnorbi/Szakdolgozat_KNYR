using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
