using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keplercraft_V_Orbital_Station_Console
{
        public abstract class StationModule
        {
            //events and delegates
            public delegate void ModuleStatusHandler(string message);
            public delegate void CriticalConditionHandler(string message);

            public event ModuleStatusHandler ModuleStatusChanged;
            public event CriticalConditionHandler CriticalConditionReached;


            // properties without validation
            public string ModuleID { get; private set; }  
            public string ModuleName { get; set; }
            public bool IsOperational { get; protected set; } = true;  // protected set so derived classes only can change it

            // property with validation!!: ensures power consumption is non-negative
            private double _powerConsumptionKw;  // "_" because this eleminates the need to use "this." in the setter and getter

            public double PowerConsumptionKw
            {
                get => _powerConsumptionKw;
                set => _powerConsumptionKw = value >= 0 ? value : throw new ValueBelowZero("Power consuption Cannot be below zero.");  // if value >= 0, set it to value and if value <= 0 throw a exception
                                                                                                                                     

            }

            // constructor
            protected StationModule(string id, string name, double powerDraw)  // protected cuz this cant be instantiated directly, only through derived classes beacause this is an abstract class
            {
                ModuleID = id;
                ModuleName = name;
                PowerConsumptionKw = powerDraw;
            }
            // allows child classes to raise the critical condition event
            protected void RaiseCriticalCondition(string message)
            {
                CriticalConditionReached?.Invoke(message);
            }
 
            public virtual void UpdateDetails(string name = null, double? power = null, bool? isOperational = null)
            {
                if (name != null)
                {
                    if (string.IsNullOrEmpty(name)) { throw new ArgumentException("Name cannot be empty.", nameof(name)); }
                    // Exception : checks if the parameter is empty or not. If it's empty it will return the exception message as well as the parameter name.
                    ModuleName = name;
                }

                if (power.HasValue)
                {
                    if (power.Value < 0) { throw new ArgumentOutOfRangeException("power cannot be negative", nameof(power)); }
                    // Exception : checks if power is below zero. If it's below zero it will return a message as well as the parameter name.
                    PowerConsumptionKw = power.Value;
                }

              
                if (isOperational.HasValue)
                {
                    bool oldStatus = IsOperational;
                    IsOperational = isOperational.Value;

                    if (oldStatus != IsOperational)
                    {
                        ModuleStatusChanged?.Invoke($"{ModuleName} status changed to " + $"{(IsOperational ? "Operational" : "Not Operational")}");
                    }
                }                
            }
        }
}