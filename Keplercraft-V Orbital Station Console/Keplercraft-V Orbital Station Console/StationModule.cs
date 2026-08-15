using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keplercraft_V_Orbital_Station_Console
{
    public abstract class StationModule
    {
        // properties without validation
        public string ModuleID { get; private set; }  // private set so only the constructor can set it and cant be changed later (encapsulation)
        ////// we should make the moduleID start with either "LS" for LifeSupport, "PC" for PowerCore, or "RL" for ResearchLab (we can make a static method that generates the next available ID based on the type of module being created as extra marks maybe?? what we think??)
        public string ModuleName { get; set; }
        public bool IsOperational { get; protected set; } = true;  // protected set so derived classes only can change it

        // property with validation!!: ensures power consumption is non-negative
        private double _powerConsumptionKw;  // "_" because this eleminates the need to use "this." in the setter and getter

        public double PowerConsumptionKw
        {
            get => _powerConsumptionKw;
            set => _powerConsumptionKw = value >= 0 ? value : throw new ValueBelowZero("Power consuption Cannot be below zero.");  // if value >= 0, set it to value and if value <= 0 throw a exception
            //////// add exception handling and events here!! ////////
            
        }

        // constructor
        protected StationModule(string id, string name, double powerDraw)  // protected cuz this cant be instantiated directly, only through derived classes beacause this is an abstract class
        {
            ModuleID = id;
            ModuleName = name;
            PowerConsumptionKw = powerDraw;
        }

        // abstract method: lets derived classes define specialized execution routines for polymorphism and threading?
        
        // make background thread that periodically loops through a list of all modules (make list of LifeSupportModule, PowerCoreModule, ResearchLabModule) every hour (but gonna make second for demonstartion purposes)

        // virtual method: allows derived classes to update details (name, power consumption, operational status) + unique parameters per module type
        // this serves as a building block for the sub classes to override and mutate, we will only call UpdateModule in Program 
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
                if (power.Value<0) { throw new ArgumentOutOfRangeException("power cannot be negative", nameof(power)); }
                // Exception : checks if power is below zero. If it's below zero it will return a message as well as the parameter name.
                PowerConsumptionKw = power.Value;
            }

            if (isOperational.HasValue)
            {
                IsOperational = isOperational.Value;
            }
            //////// add exception handling and events here!! ////////

        }
    }
}
