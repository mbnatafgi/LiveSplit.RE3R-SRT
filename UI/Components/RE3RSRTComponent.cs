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
            Settings = new RE3RSRTSettings();
            Cache = new GraphicsCache();
            Grid = new Dictionary<string, Tuple<SimpleLabel, SimpleLabel>>();
            Values = new Dictionary<string, string>();
            Grid["HP"] = new Tuple<SimpleLabel, SimpleLabel>(new SimpleLabel(), new SimpleLabel());
            Grid["DA"] = new Tuple<SimpleLabel, SimpleLabel>(new SimpleLabel(), new SimpleLabel());
            Grid["Enemy"] = new Tuple<SimpleLabel, SimpleLabel>(new SimpleLabel(), new SimpleLabel());
            Grid["Inventory"] = new Tuple<SimpleLabel, SimpleLabel>(new SimpleLabel(), new SimpleLabel());
        }

        public string ComponentName => "RE3R SRT";

        public float HorizontalWidth { get; set; }

        public float VerticalHeight { get; set; }
        
        public float MinimumWidth { get; set; }
        
        public float MinimumHeight { get; set; }

        public float PaddingTop => 5;

        public float PaddingBottom => 5;

        public float PaddingLeft => 5;

        public float PaddingRight => 5;

        public IDictionary<string, Action> ContextMenuControls => null;

        public void Dispose()
        {
        }

        private void DrawRow(Graphics g, LiveSplitState state, float width, float height, KeyValuePair<string, Tuple<SimpleLabel, SimpleLabel>> pair, float yOffset)
        {
            var nameLabel = pair.Value.Item1;
            var valueLabel = pair.Value.Item2;

            var name = pair.Key;
            var value = Values.TryGetValue(pair.Key, out var val) ? val : "";
            
            var font = state.LayoutSettings.TextFont;
            var textColor = state.LayoutSettings.TextColor;
            var textOutlineColor = state.LayoutSettings.TextOutlineColor;

            valueLabel.HorizontalAlignment = StringAlignment.Near;
            nameLabel.Text = name + ":";
            nameLabel.Font = font;
            nameLabel.X = PaddingLeft;
            nameLabel.Y = yOffset;
            nameLabel.Width = width/2;
            nameLabel.Height = g.MeasureString(name, font).Height;
            nameLabel.Brush = new SolidBrush(textColor);
            nameLabel.OutlineColor = textOutlineColor;
            
            nameLabel.Draw(g);
            
            valueLabel.HorizontalAlignment = StringAlignment.Far;
            valueLabel.Text = value;
            valueLabel.Font = font;
            valueLabel.X = width/2;
            valueLabel.Y = yOffset;
            valueLabel.Width = width/2 - PaddingRight;
            valueLabel.Height = g.MeasureString(value, font).Height;
            valueLabel.Brush = new SolidBrush(textColor);
            valueLabel.OutlineColor = textOutlineColor;
            
            valueLabel.Draw(g);

        }

        public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion)
        {
        }

        public void DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion)
        {
            float yOffset = PaddingTop;
            foreach (var pair in Grid)
            {
                DrawRow(g, state, width, VerticalHeight, pair, yOffset);
                yOffset = Math.Max(
                    pair.Value.Item1.Height + pair.Value.Item1.Y, 
                    pair.Value.Item2.Height + pair.Value.Item2.Y);
            }
            VerticalHeight = yOffset + PaddingBottom;
            
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
            var now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            
            if (now - Timestamp <= 100) return;
            
            if (RE3RSRT.UpdateValues(Values))
            {
                Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            }
            else
            {
                Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds() + 2000;
            }
            invalidator.Invalidate(0, 0, width, height);
        }
        
        private Dictionary<string, Tuple<SimpleLabel, SimpleLabel>> Grid { get; set; }

        public RE3RSRTSettings Settings { get; set; }
        
        public GraphicsCache Cache { get; set; }
        
        public Dictionary<string, string> Values { get; set; }

        private long Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        
    }
    
}
