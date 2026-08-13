using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keplercraft_V_Orbital_Station_Console
{
    public class ResearchLabModule : StationModule
    {
        // properties with validation!!: ensures active experiments, data per hour and total data collected are non-negative
        private int _activeExperiments;
        private double _dataPerHour;  // measured in GB/h
        private double _dataCollectedGb;  // measured in GB

        public int ActiveExperiments
        {
            get => _activeExperiments;
            set => _activeExperiments = value >= 0 ? value : 0;  // validation / exeption handeling??
        }

        public double DataPerHour
        {
            get => _dataPerHour;
            set => _dataPerHour = value >= 0.0 ? value : 0.0;  // validation / exeption handeling??
        }

        public double DataCollectedGb
        {
            get => _dataCollectedGb;
            set => _dataCollectedGb = value >= 0.0 ? value : 0.0;  // validation / exeption handeling??
        }

        // derived constructor we can call in Program to create a ResearchLabModule object
        public ResearchLabModule(string id, string name, double powerDraw, int amountExperiments, double dataPerHour)
            : base(id, name, powerDraw)
        {
            ActiveExperiments = amountExperiments;
            DataPerHour = dataPerHour;
            DataCollectedGb = 0.0;  // no data collection at start, will add up with treading with ExecuteRoutine
        }

        // overriding abstract method from StationModule
        public override void ExecuteRoutine()
        {
            DataCollectedGb += ActiveExperiments * DataPerHour;  // simulates data collection of entered GB per active experiment
            if (DataCollectedGb >= 1000.0)  // storage cut of at 1000GB
            {
                IsOperational = false; 
            }
            //////// add exception handling and events here!! ////////
            
        }

        // overriding GetStatusReport to include active experiments, total data collected and data per hour in the report
        public override string GetStatusReport()
        {
            return $"{base.GetStatusReport()} | Experiments: {ActiveExperiments} | Total Data Collected: {DataCollectedGb:F1} GB | Data Per Hour: {DataPerHour:F1} GB/h";
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
    }
}
