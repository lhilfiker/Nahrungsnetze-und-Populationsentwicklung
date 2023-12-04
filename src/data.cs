using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nahrungsnetze_und_Populationsentwicklung
{
    public class data
    {
        public static string Version = "v.0.1.0";
        
        public static List<string> Names = new();
        
        public static List<string> Eats = new();

        public static List<float> Quantity = new();

        public static List<float> EatsHowMany = new();
        
        public static List<float> DeathsPerDay  = new();

        public static List<float> Replication  = new();

        public static List<float> Multiplier  = new();

        public static string ShowSelection = "Anzahl";

        
        public static string path;
    }
}
