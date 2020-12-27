using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using LiveSplit.Web;

namespace LiveSplit.UI.Components
{
    public class RE3RSRT
    {
        private const string HOST = "http://localhost";

        private const int PORT = 7190;
        
        private enum Mode
        {
            Sync,
            Async
        }

        private static Mode CurrentMode = Mode.Async;

        public static bool UpdateValues(Dictionary<string, string> Values)
        {
            if (CurrentMode == Mode.Async)
            {
                ReconnectAsync();
                Values.Clear();
                return false;
            }
            UpdateValuesSync(Values);
            return true;
        }

        private static void UpdateValuesSync(Dictionary<string, string> Values)
        {
            try
            {
                using (var wc = new WebClient())
                {
                    var json = wc.DownloadString(new Uri(string.Format("{0}:{1}", HOST, PORT)));
                    
                    var data = JSON.FromString(json);
                    
                    var da = 0;
                    var hp = 0;
                    var inventory = "";
                    var enemy = "";
                
                    if (data != null)
                    {
                        da = (int) Math.Round((float) data.RankScore);
                        hp = data.PlayerCurrentHealth;

                        var firstEnemy = ((IEnumerable)data.EnemyHealth).Cast<dynamic>()
                            .Where(e => e["IsAlive"])
                            .OrderBy(e => (float) e["Percentage"])
                            .ThenByDescending(e => e["CurrentHP"])
                            .FirstOrDefault();

                        var occupiedSlots = ((IEnumerable)data.PlayerInventory).Cast<dynamic>()
                            .Count(i => !i.IsEmptySlot);

                        inventory = string.Format("{0}/{1}", occupiedSlots, data.PlayerInventoryCount);

                        enemy = string.Format(
                            "{0}HP {1:P1}",
                            firstEnemy != null ? firstEnemy.CurrentHP : 0, 
                            firstEnemy != null ? firstEnemy.Percentage : 0);
                        
                    }
                    Values["DA"] = da.ToString();
                    Values["HP"] = hp.ToString();
                    Values["Enemy"] = enemy;
                    Values["Inventory"] = inventory;
                    
                }
            }
            catch
            {
                CurrentMode = Mode.Async;
            }
        }


        private static void ReconnectAsync()
        {
            using (var wc = new WebClient())
            {
                wc.DownloadStringCompleted += OnDownloadStringCompleted;
                wc.DownloadStringAsync(new Uri(string.Format("{0}:{1}", HOST, PORT)));
            }
        }
        
        private static void OnDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs args)
        {
            try
            {
                var json = args.Result;
                CurrentMode = Mode.Sync;
            }
            catch
            {
            }
        }
    }

}