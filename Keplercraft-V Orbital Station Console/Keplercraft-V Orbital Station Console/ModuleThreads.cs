using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Keplercraft_V_Orbital_Station_Console;

namespace Keplercraft_V_Orbital_Station_Console
{
    public static class ModuleThreads
    {
        // LifeSupport threading
        public static void StartOxygenDrain(LifeSupportModule module)
        {
            Task.Run(() =>
            {
                while (module.IsOperational)
                {
                    Task.Delay(2000).Wait(); // simulate 1 hour as 2 seconds
                    module.OxygenLevel -= module.OxygenDrainPerHour;
                    if (module.OxygenLevel <= 20) module.CheckCriticalCondition();
                }
            });
        }

        // PowerCore threading
        public static void StartReactorHeating(PowerCoreModule module)
        {
            Task.Run(() =>
            {
                while (module.IsOperational)
                {
                    Task.Delay(2000).Wait();
                    module.ReactorTemperature += module.TempRisePerHour;
                    module.CheckCriticalCondition();
                }
            });
        }

        // ResearchLab threading
        public static void StartDataCollection(ResearchLabModule module)
        {
            Task.Run(() =>
            {
                while (module.IsOperational)
                {
                    Task.Delay(2000).Wait();
                    module.DataCollectedGb += module.DataPerHour;
                    module.CheckCriticalCondition();
                }
            });
        }
    }
}
