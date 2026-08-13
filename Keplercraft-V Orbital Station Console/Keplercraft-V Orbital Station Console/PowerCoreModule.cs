using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keplercraft_V_Orbital_Station_Console
{
    public class PowerCoreModule : StationModule
    {
        // property with validation!!: ensures reactor temperature and temperature rise per hour is non-negative
        private double _reactorTemperature;  // measured in °C
        private double _tempRisePerHour;  // measured in °C/h

        public double ReactorTemperature
        {
            get => _reactorTemperature;
            set => _reactorTemperature = value >= 0.0 ? value : 0.0;  // validation / exeption handeling??
        }

        public double TempRisePerHour
        {
            get => _tempRisePerHour;
            set => _tempRisePerHour = value >= 0.0 ? value : 0.0;  // validation / exeption handeling??
        }

        // derived constructor we can call in Program to create a PowerCoreModule object
        public PowerCoreModule(string id, string name, double powerDraw, double initialTemp, double tempRisePerHour)
            : base(id, name, powerDraw)
        {
            ReactorTemperature = initialTemp;
            TempRisePerHour = tempRisePerHour;
        }

        // overriding abstract method from StationModule
        public override void ExecuteRoutine()
        {
            ReactorTemperature += TempRisePerHour;  // simulates reactor temperature increase by entered amount per hour
            if (ReactorTemperature >= 200.0)  // safety cut off at 200°C
            {
                IsOperational = false;
            }
            //////// add exception handling and events here!! //////// 

        }

        // overriding GetStatusReport to include reactor temperature and temperature rise per hour in the report
        public override string GetStatusReport()
        {
            return $"{base.GetStatusReport()} | Core Temp: {ReactorTemperature:F1}°C | Temp Rise Per Hour: {TempRisePerHour:F1}°C/h";
        }

        // overriding UpdateDetails to include reactor temperature and temperature rise per hour in the update
        public void UpdateDetails(string name = null, double? power = null, bool? isOperational = null, double? reactorTemperature = null, double? tempRisePerHour = null)
        {
            base.UpdateDetails(name, power, isOperational);

            if (reactorTemperature.HasValue)
            {
                ReactorTemperature = reactorTemperature.Value;
            }

            if (tempRisePerHour.HasValue)
            {
                TempRisePerHour = tempRisePerHour.Value;
            }
        }
    }
}
