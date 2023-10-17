using System.ComponentModel;
using System.Reflection;

//namespace Core.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        FieldInfo field = value.GetType().GetField(value.ToString());

        if (field != null)
        {
            DescriptionAttribute attribute =
                Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            if (attribute != null)
            {
                return attribute.Description;
            }
        }

        // Eğer Description attributu yoksa, enum değerini döndür
        return value.ToString();
    }
}
