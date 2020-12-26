using LiveSplit.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.UI.Components
{
    public class RE3RSRTComponent : IComponent

    {
        public RE3RSRTComponent(LiveSplitState state)
        {
            VerticalHeight = 10;
            this.state = state;
            RE3RSRTNameLabel = new SimpleLabel();
            RE3RSRTValueLabel = new SimpleLabel();
            Settings = new RE3RSRTSettings();
        }

        public string ComponentName => "RE3R SRT";

        public float HorizontalWidth { get; set; }

        public float MinimumHeight { get; set; }

        public float VerticalHeight { get; set; }

        public float MinimumWidth => RE3RSRTNameLabel.X + RE3RSRTValueLabel.ActualWidth;

        public float PaddingTop { get; set; }

        public float PaddingBottom { get; set; } 

        public float PaddingLeft { get { return 7f; } }

        public float PaddingRight { get { return 7f; } }

        public IDictionary<string, Action> ContextMenuControls
        {
            get { return null; }
        }

        public void Dispose()
        {
        }

        public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion)
        {
        }

        public void DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion)
        {
        }

        public XmlNode GetSettings(XmlDocument document)
        {
            return Settings.GetSettings(document);
        }

        public Control GetSettingsControl(LayoutMode mode)
        {
            return Settings;
        }

        public void SetSettings(XmlNode settings)
        {
            Settings.SetSettings(settings);
        }

        public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
            this.state = state;

        }

        private LiveSplitState state;

        protected SimpleLabel RE3RSRTNameLabel = new SimpleLabel();
        
        protected SimpleLabel RE3RSRTValueLabel = new SimpleLabel();

        public RE3RSRTSettings Settings { get; set; }
    }
}
