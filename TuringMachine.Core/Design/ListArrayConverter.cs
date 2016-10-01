using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace TuringMachine.Core.Design
{
    public class ListArrayConverter : CollectionConverter
    {
        bool _OnlyCount = false;
        bool _ReadOnly = false;

        public ListArrayConverter() { }
        public ListArrayConverter(bool readOnly, bool onlyCount) { _ReadOnly = readOnly; _OnlyCount = onlyCount; }
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
        {
            if (destType == typeof(string))
            {
                if (_OnlyCount)
                {
                    switch (((IList)value).Count)
                    {
                        case 0: return "Empty";
                        case 1: return ((IList)value)[0].ToString();
                        default: return "Count: " + ((IList)value).Count.ToString();
                    }
                }
                return string.Join(",", ((IList)value).Cast<object>().Select(c => c.ToString()).ToArray());
            }

            return "None";
        }
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string)) return !_ReadOnly;
            return base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            IList list = (IList)Activator.CreateInstance(context.PropertyDescriptor.PropertyType);

            if (value != null)
            {
                TypeConverter conv = TypeDescriptor.GetConverter(context.PropertyDescriptor.PropertyType.GenericTypeArguments[0]);
                foreach (string s in value.ToString().Split(new char[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (s.Length == 1 && !char.IsNumber(s[0]))
                    {
                        list.Add(conv.ConvertFromInvariantString(((byte)(s[0])).ToString()));
                        continue;
                    }

                    list.Add(conv.ConvertFromInvariantString(s));
                }
            }

            return list;
        }
    }
}