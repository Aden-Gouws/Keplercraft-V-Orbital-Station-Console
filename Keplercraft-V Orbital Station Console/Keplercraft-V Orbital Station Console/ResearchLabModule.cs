using Keplercraft_V_Orbital_Station_Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keplercraft_V_Orbital_Station_Console
{
    public class ResearchLabModule : StationModule, Imaintainable, IStatusReport
    {
        // properties with validation!!: ensures active experiments, data per hour and total data collected are non-negative
        private int _activeExperiments;
        private double _dataPerHour;  // measured in GB/h
        private double _dataCollectedGb;  // measured in GB

        public int ActiveExperiments
        {
            get => _activeExperiments;
            set => _activeExperiments = value >= 0 ? value : throw new ValueBelowZero("Active Experiments cannot be zero.");  // validation 
        }

        public double DataPerHour
        {
            get => _dataPerHour;
            set => _dataPerHour = value >= 0.0 ? value : throw new ValueBelowZero("Data per hour cannot be zero.");  // validation 
        }

        public double DataCollectedGb
        {
            get => _dataCollectedGb;
            set => _dataCollectedGb = value >= 0.0 ? value : throw new ValueBelowZero("Data collection cannot be zero");  // validation 
        }

        // derived constructor we can call in Program to create a ResearchLabModule object
        public ResearchLabModule(string id, string name, double powerDraw, int amountExperiments, double dataPerHour)
            : base(id, name, powerDraw)
        {
            ActiveExperiments = amountExperiments;
            DataPerHour = dataPerHour;
            DataCollectedGb = 0.0;  
        }

        // overriding abstract method from StationModule
        public double RiskLevel()
        {
            return (DataCollectedGb / 1000) * 100;
        }
        public void CaculateMaintenance() //Determines the level of risk of failure based on the reactor temperature and outputs a maintenance report to the console
        {
            Console.WriteLine($"Data Collection: {DataCollectedGb}\nRisk of Failure : {RiskLevel()}");
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

        // Interface to get status report of the ResearchLabModule, including active experiments, total data collected and data per hour
        public string StatusReport()
        {
            return $" Experiments: {ActiveExperiments} | Total Data Collected: {DataCollectedGb:F1} GB | Data Per Hour: {DataPerHour:F1} GB/h";
        }

        // overriding UpdateDetails to include active experiments and data per hour in the update
        public void UpdateDetails(string name = null, double? power = null, bool? isOperational = null, int? activeExperiments = null, double? dataPerHour = null)
        {
            base.UpdateDetails(name, power, isOperational);

            if (activeExperiments.HasValue)
            {
                ActiveExperiments = activeExperiments.Value;
            }

            if (dataPerHour.HasValue)
            {
                DataPerHour = dataPerHour.Value;
            }
        }
        // checks for a critical research lab condition
        public void CheckCriticalCondition()
        {
            if (RiskLevel() >= 90)
            {
                RaiseCriticalCondition(
                    $"CRITICAL: {ModuleName} data storage has reached " +
                    $"{DataCollectedGb:F1} GB."
                );
            }
        }
    }
}

