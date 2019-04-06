using System.Windows.Controls;

namespace Lab05_Pyvovar.Tools.Navigation
{
    internal interface IContentOwner
    {
        ContentControl ContentControl { get; }
    }
}