using Keplercraft_V_Orbital_Station_Console.Keplercraft_V_Orbital_Station_Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keplercraft_V_Orbital_Station_Console
{
    public class PowerCoreModule : StationModule, Imaintainable, IStatusReport
    {
        // property with validation!!: ensures reactor temperature and temperature rise per hour is non-negative
        private double _reactorTemperature;  // measured in °C
        private double _tempRisePerHour;  // measured in °C/h

        public double ReactorTemperature
        {
            get => _reactorTemperature;
            set => _reactorTemperature = value >= 0.0 ? value : throw new ValueBelowZero("Reactor Temperature cannot be below zero.");  // validation 
        }

        public double TempRisePerHour
        {
            get => _tempRisePerHour;
            set => _tempRisePerHour = value >= 0.0 ? value : throw new ValueBelowZero("Tempature rise per hour cannot be below zero.");  // validation 
        }

        // derived constructor we can call in Program to create a PowerCoreModule object
        public PowerCoreModule(string id, string name, double powerDraw, double initialTemp, double tempRisePerHour)
            : base(id, name, powerDraw)
        {
            ReactorTemperature = initialTemp;
            TempRisePerHour = tempRisePerHour;
        }

        // Interface

        public double RiskLevel()
        {
            return (ReactorTemperature / 200) * 100;
        }
        public void CaculateMaintenance() //Determines the level of risk of failure based on the reactor temperature and outputs a maintenance report to the console
        {


            Console.WriteLine("Maitenance Report:");
            Console.WriteLine();
            Console.WriteLine($"ReactorTemperature: {ReactorTemperature}\nRisk of Failure : {RiskLevel()}");
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

        //Interface to get status report
        public string StatusReport()
        {
            return $"Core Temp: {ReactorTemperature:F1}°C | Temp Rise Per Hour: {TempRisePerHour:F1}°C/h";
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
        // checks for a critical reactor condition
        public void CheckCriticalCondition()
        {
            if (RiskLevel() >= 90)
            {
                RaiseCriticalCondition(
                    $"CRITICAL: {ModuleName} reactor temperature is " +
                    $"{ReactorTemperature:F1}°C."
                );
            }
        }
    }
}
