using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keplercraft_V_Orbital_Station_Console
{
    public class LifeSupportModule : StationModule
    {
        // property with validation!!: ensures oxygen level is between 0 and 100 and oxygen drain per hour is non-negative
        private double _oxygenLevel;  //measured in %
        private double _oxygenDrainPerHour;  // measured in %/h

        public double OxygenLevel
        {
            get => _oxygenLevel;
            set  // validation / exeption handeling?
            {
                if (value < 0.0)
                {
                    _oxygenLevel = 0.0;
                }
                else if (value > 100.0)
                {
                    _oxygenLevel = 100.0;
                }
                else
                {
                    _oxygenLevel = value;
                }
            }
        }

        public double OxygenDrainPerHour
        {
            get => _oxygenDrainPerHour;
            set => _oxygenDrainPerHour = value >= 0.0 ? value : 0.0;  // validation / exeption handeling??
        }

        // derived constructor we can call in Program to create a LifeSupportModule object
        public LifeSupportModule(string id, string name, double powerDraw, double initialOxygen, double oxygenDrainPerHour)
            : base(id, name, powerDraw)
        {
            OxygenLevel = initialOxygen;
            OxygenDrainPerHour = oxygenDrainPerHour;
        }

        // overriding abstract method from StationModule
        public override void ExecuteRoutine()
        {
            OxygenLevel -= OxygenDrainPerHour;  // simulates oxygen decrease by the specified amount per hour
            if (OxygenLevel < 20.0)  // safety cut off at 20% oxygen level
            {
                IsOperational = false;
            }
        }   // replace if with exception handling, can maybe add an event to idk, alert the crew? mybe same for all simmilar?
            //////// add exception handling and events here!! ////////

        // overriding GetStatusReport to include oxygen level and oxygen drain per hour in the report
        public override string GetStatusReport()
        {
            return $"{base.GetStatusReport()} | O2 Level: {OxygenLevel:F1}% | O2 Drain Per Hour: {OxygenDrainPerHour:F1}%/h";
        }

        // overriding UpdateDetails to include oxygen level and oxygen drain per hour in the update
        public void UpdateDetails(string name = null, double? power = null, bool? isOperational = null, double? oxygenLevel = null, double? oxygenDrainPerHour = null)
        {
            base.UpdateDetails(name, power, isOperational);

            if (oxygenLevel.HasValue)  // .HasValue checks if var has a value
            {
                OxygenLevel = oxygenLevel.Value;  // if true, assign it to the OxygenLevel
            }

            if (oxygenDrainPerHour.HasValue)
            {
                OxygenDrainPerHour = oxygenDrainPerHour.Value;
            }
        }
    }
}
