using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keplercraft_V_Orbital_Station_Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ModuleManager manager = new ModuleManager();

            LifeSupportModule lifeSupport =
                new LifeSupportModule(
                    "LS-1",
                    "Primary Life Support",
                    45.0,
                    80.0,
                    5.0
                );

            manager.AddModule(lifeSupport);
            ModuleThreads.StartOxygenDrain(lifeSupport);

            PowerCoreModule core =
                new PowerCoreModule(
                    "PC-1",
                    "Main Reactor",
                    120,
                    50,
                    1.5
                );

            manager.AddModule(core);
            ModuleThreads.StartReactorHeating(core);

            ResearchLabModule lab =
                new ResearchLabModule(
                    "RL-1",
                    "Bio Lab",
                    30,
                    2,
                    1.0
                );
            manager.AddModule(lab);
            ModuleThreads.StartDataCollection(lab);


            Console.WriteLine("Testing Module Status Event:");

            lifeSupport.UpdateDetails(
                isOperational: false
            );


            Console.WriteLine();

            Console.WriteLine("Testing Critical Condition Event:");

            lifeSupport.UpdateDetails(
                oxygenLevel: 20
            );

            lifeSupport.CheckCriticalCondition();


            Console.ReadLine();
        }
    }
}
