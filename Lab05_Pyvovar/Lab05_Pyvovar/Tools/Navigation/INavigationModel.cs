namespace Lab05_Pyvovar.Tools.Navigation
{
    internal enum ViewType
    {
        Info,
        Detail
    }

    interface INavigationModel
    {
        void Navigate(ViewType viewType);
    }
}