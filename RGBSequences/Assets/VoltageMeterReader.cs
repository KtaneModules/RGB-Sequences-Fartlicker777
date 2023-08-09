using System.Linq;
using UnityEngine;

public class VoltageMeterReader : MonoBehaviour {

   public KMBombInfo Bomb;

   public int VoltageMeterInt () {
      if (Bomb.QueryWidgets("volt", "").Count() != 0) {
         double voltage = double.Parse(Bomb.QueryWidgets("volt", "")[0].Substring(12).Replace("\"}", ""));
         return (int) voltage;
      }
      else {
         return -1;
      }
   }

   public int VoltageMeterDecimal () {
      if (Bomb.QueryWidgets("volt", "").Count() != 0) {
         double voltage = double.Parse(Bomb.QueryWidgets("volt", "")[0].Substring(12).Replace("\"}", ""));
         return (int) (voltage * 10 % 10);
      }
      else {
         return -1;
      }
   }
}
