using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdolgozat.Models
{
    public class ModelContainer<T>
    {
        public T Model { get; set; }
        public ModelContainer(T model)
        {
            Model = model;
        }
    }
}
