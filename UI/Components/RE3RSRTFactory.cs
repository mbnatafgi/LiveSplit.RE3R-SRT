using LiveSplit.Model;
using System;

namespace LiveSplit.UI.Components
{
    public class RE3RSRTFactory : IComponentFactory
    {
        public string ComponentName => "RE3R SRT";

        public string Description => "A component to display SRT info for RE3R.";

        public ComponentCategory Category => ComponentCategory.Other;
        
        public IComponent Create(LiveSplitState state) => new RE3RSRTComponent(state);

        public string UpdateName => ComponentName;

        public string XMLURL => "";

        public string UpdateURL => "";

        public Version Version => Version.Parse("0.1");

    }
}
