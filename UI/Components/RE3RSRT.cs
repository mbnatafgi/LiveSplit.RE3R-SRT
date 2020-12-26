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
        public static void UpdateValues(Dictionary<string, string> Values)
        {
            var data = JSON.FromString(GetSRTData());
            if (data != null)
            {
                Values["DA"] = ((int)Math.Round((float)data.RankScore)).ToString();
                Values["HP"] = data.PlayerCurrentHealth.ToString();
                var enemy = ((IEnumerable) data.EnemyHealth).Cast<dynamic>()
                    .Where(e => e.IsAlive)
                    .OrderBy(e => (float)e.Percentage)
                    .ThenByDescending(e => e.CurrentHP)
                    .First();

                var occupiedSlots = ((IEnumerable) data.PlayerInventory).Cast<dynamic>().Count(i => !i.IsEmptySlot);

                Values["Inventory"] = string.Format("{0}/{1}", occupiedSlots, data.PlayerInventoryCount);

                if (enemy != null)
                {
                    Values["Enemy"] = string.Format("{0}HP {1:P1}", enemy.CurrentHP, enemy.Percentage);
                }
                else
                {
                    Values["Enemy"] = "";
                }
            }
            else
            {
                Values["DA"] = "";
                Values["HP"] = "";
                Values["Enemy"] = "";
                Values["Inventory"] = "";
            }
        }
        
        private static string GetSRTData()
        {
            using (var wc = new WebClient())
            {
                try
                {
                    var json = wc.DownloadString(SRTURL);
                    return json;
                }
                catch
                {
                    return "";
                }
            }
        }

        private const string SRTURL = "http://localhost:7190";
    }
}