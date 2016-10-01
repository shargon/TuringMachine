using System;
using System.Reflection;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using TuringMachine.Core.Helpers;
using System.Collections.Generic;

namespace TuringMachine.Core.Design
{
    public class IGetValueEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        IWindowsFormsEditorService _editorService;
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            _editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            // use a list box
            ListBox lb = new ListBox();
            lb.SelectionMode = SelectionMode.One;
            lb.SelectedValueChanged += OnListBoxSelectedValueChanged;

            // use the IBenchmark.Name property for list box display
            lb.DisplayMember = "Name";

            foreach (Type tp in GetValues(value.GetType().GetTypeInfo().GenericTypeArguments[0]))
            {
                if (!ReflectionHelper.HavePublicConstructor(tp)) continue;

                int index = lb.Items.Add(Activator.CreateInstance(tp));
                if (tp == value.GetType())
                {
                    lb.SelectedIndex = index;
                }
            }

            // show this model stuff
            _editorService.DropDownControl(lb);
            if (lb.SelectedItem == null || lb.SelectedItem.GetType() == value.GetType())
                return value;

            return lb.SelectedItem;
        }

        IEnumerable<Type> GetValues(Type item)
        {
            Type[] typeArgs = { item };

            yield return typeof(FixedValue<>).MakeGenericType(typeArgs);
            yield return typeof(FromToValue<>).MakeGenericType(typeArgs);
        }

        void OnListBoxSelectedValueChanged(object sender, EventArgs e)
        {
            // close the drop down as soon as something is clicked
            _editorService.CloseDropDown();
        }
    }
}