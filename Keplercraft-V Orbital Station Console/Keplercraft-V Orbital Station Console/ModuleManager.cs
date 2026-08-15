using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Keplercraft_V_Orbital_Station_Console
{
    public class ModuleManager
    {
        // creating the list as private (encapsulation) / use IReadOnlyList bellow for display
        private readonly List<StationModule> _modules = new List<StationModule>();  // readonly prevents reassignment, but we can still add/remove items

        // display all modules in the list
        public void DisplayAllModules()
        {

            foreach (var module in _modules)
                {
                    IStatusReport statusreport = (IStatusReport)module;
                    Console.WriteLine(statusreport.StatusReport());  //Displays all the modules in the list and their risk level
            }
        }

        // method for adding a module to the list /////THIS SHOULD RUN EVERY TIME A NEW MODULE IS MADE /////
        public void AddModule(StationModule module)
        {
            _modules.Add(module);
        }

        // method for retrieving a module by ID  // this is only used for the UpdateModulePower method, but could be used for other things maybe idk??
        public StationModule GetModuleById(string moduleId)
        {
            foreach (var module in _modules)
            {
                if (module.ModuleID.Equals(moduleId, StringComparison.OrdinalIgnoreCase))
                {
                    return module;
                }
            }
            return null;
        }

        // method for updating a module's details by ID
        public string UpdateModule(
            string moduleId,  // not to be changed, used to find the module in the list
            string name = null,
            double? power = null,
            bool? isOperational = null,
            // LifeSupportModule specific parameters
            double? oxygenLevel = null,
            double? oxygenDrainPerHour = null,
            // PowerCoreModule specific parameters
            double? reactorTemp = null,
            double? tempRisePerHour = null,
            // ResearchLabModule specific parameters
            int? activeExperiments = null,
            double? dataPerHour = null)
        {
            StationModule module = GetModuleById(moduleId);
            if (module == null) return $"Module {moduleId} not found.";

            // updates the base properties
            module.UpdateDetails(name, power, isOperational);

            // specific subclass updates
            if (module is LifeSupportModule ls)  // this takes the module and checks if it is a LifeSupportModule, if so its assigned to the variable ls
            {
                ls.UpdateDetails(oxygenLevel: oxygenLevel, oxygenDrainPerHour: oxygenDrainPerHour);
            }
            else if (module is PowerCoreModule pwr)
            {
                pwr.UpdateDetails(reactorTemperature: reactorTemp, tempRisePerHour: tempRisePerHour);
            }
            else if (module is ResearchLabModule lab)
            {
                lab.UpdateDetails(activeExperiments: activeExperiments, dataPerHour: dataPerHour);
            }

            return $"Module {moduleId} updated successfully.";
        }

        // method for removing a module by ID
        public string RemoveModule(string moduleId)
        {
            StationModule module = GetModuleById(moduleId);
            if (module != null)
            {
                _modules.Remove(module);
                return $"Module {moduleId} removed successfully.";
            }
            else
            {
                return $"Module {moduleId} not found.";
            }
        }

        // method for running all risk ///// THIS SHOULD BE USED FOR THE THREADING!! /////
        public void RunAllRisk()
        {
            foreach (var module in _modules)
            {
                Imaintainable maintainable = (Imaintainable)module;
                Console.WriteLine(maintainable.RiskLevel());
            }
        }

        // generates randomized starting data but dont know if this works yet ///// TEST WHEN INTERFACE IS DONE /////
        public void SeedRandomModules()
        {
            Random rng = new Random();

            AddModule(new LifeSupportModule("LS-1", "Primary Habitat Oxygen Scrubber", 45.0, 100.0, 0.5));
            AddModule(new PowerCoreModule("PC-1", "Main Fusion Reactor Core", 120.0, 50, 1.5));
            AddModule(new ResearchLabModule("RL-1", "Zero-G Biological Lab", 30.0, 2, 1.0));
        }
    }
}
