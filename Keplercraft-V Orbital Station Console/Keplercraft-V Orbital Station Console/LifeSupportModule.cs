using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keplercraft_V_Orbital_Station_Console
{
    public class LifeSupportModule : StationModule,Imaintainable,IStatusReport
    {
        // property with validation!!: ensures oxygen level is between 0 and 100 and oxygen drain per hour is non-negative
        private double _oxygenLevel;  //measured in %
        private double _oxygenDrainPerHour;  // measured in %/h

        public double OxygenLevel
        {
            get => _oxygenLevel;
            set  // validation 
            {
                if (value < 0.0)
                {
                    throw new ValueBelowZero("Oxygen Level cannot be below zero.");  // validation
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
            set => _oxygenDrainPerHour = value >= 0.0 ? value : throw new ValueBelowZero("Oxygen Drain cannot be below zero.");  // validation 
        }

        // derived constructor we can call in Program to create a LifeSupportModule object
        public LifeSupportModule(string id, string name, double powerDraw, double initialOxygen, double oxygenDrainPerHour)
            : base(id, name, powerDraw)
        {
            OxygenLevel = initialOxygen;
            OxygenDrainPerHour = oxygenDrainPerHour;
        }

        public double RiskLevel()
        {
            return OxygenLevel;
        }
        public void CaculateMaintenance() //Determines the level of risk of failure based on the reactor temperature and outputs a maintenance report to the console
        {
            Console.WriteLine($"Oxygen Level: {OxygenLevel}\nRisk of Failure : {RiskLevel()}");
            if (RiskLevel() < 50)
            {
                Console.WriteLine("Risk of Failure : Low");
            }
            else if (RiskLevel() >= 50)
            {
                Console.WriteLine("Risk of Failure : Medium");
            }
            else if (RiskLevel() >= 75)
                Console.WriteLine("Risk of Failure : High");
            else if (RiskLevel() >= 90)
            {
                Console.WriteLine("Risk of Failure : CRITICAL");
                Console.WriteLine("Maintaince is Required!");
            }
            // Event handler if the system is at critical
            else
            {
                Console.WriteLine("Risk of Failure : Unknown");
            }
        }
        // Interface to get the status report of the LifeSupportModule, including oxygen level and oxygen drain per hour
        public string StatusReport()
        {
            return $"O2 Level: {OxygenLevel:F1}% | O2 Drain Per Hour: {OxygenDrainPerHour:F1}%/h";
        }

        public void CheckCriticalCondition()
        {
            if (RiskLevel() >= 90)
                RaiseCriticalCondition($"CRITICAL: {ModuleName} oxygen level at {OxygenLevel:F1}%.");
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
