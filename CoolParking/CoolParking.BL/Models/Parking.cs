// TODO: implement class Parking.
//       Implementation details are up to you, they just have to meet the requirements 
//       of the home task and be consistent with other classes and tests.
using CoolParking.BL.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CoolParking.BL.Models
{
    public class Parking
    {
        private static Parking _instance;

        public decimal Balance { get; set; }

        public List<Vehicle> Vehicles = new List<Vehicle>();


        public Parking() { }

        public static Parking GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Parking();
                }

                return _instance;
            }
        }
    }
}
