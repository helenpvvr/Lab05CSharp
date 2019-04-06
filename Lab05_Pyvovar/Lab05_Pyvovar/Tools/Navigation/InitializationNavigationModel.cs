using System;
using Lab05_Pyvovar.View;

namespace Lab05_Pyvovar.Tools.Navigation
{
    internal class InitializationNavigationModel : BaseNavigationModel
    {
        public InitializationNavigationModel(IContentOwner contentOwner) : base(contentOwner)
        {

        }

        protected override void InitializeView(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.Info:
                    ViewsDictionary.Add(viewType, new InfoView());
                    break;
                case ViewType.Detail:
                    ViewsDictionary.Add(viewType, new DetailsView());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null);
            }
        }
    }
}