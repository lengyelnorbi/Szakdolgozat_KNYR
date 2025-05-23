using System;
using System.Windows.Markup;

namespace Szakdolgozat.Specials
{
    public class EnumBindingSourceExtension : MarkupExtension
    {
        public Type EnumType { get; private set; } 
        public EnumBindingSourceExtension(Type enumType) 
        {
            if (enumType is null || !enumType.IsEnum)
                throw new Exception("Enum must not be null and it should be of enum type!");
            EnumType = enumType;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(EnumType);
        }
    }
}
