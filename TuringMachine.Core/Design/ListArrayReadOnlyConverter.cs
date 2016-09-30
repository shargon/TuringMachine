namespace TuringMachine.Core.Design
{
    public class ListArrayReadOnlyConverter : ListArrayConverter
    {
        public ListArrayReadOnlyConverter() : base(true, true) { }
    }
}