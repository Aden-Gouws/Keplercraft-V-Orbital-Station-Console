using System;
using System.Collections.Generic;
using System.Linq;

namespace Keplercraft_V_Orbital_Station_Console
{
    internal class Program
    {
        private enum MenuOption
        {
            Seed = 0,
            ListAllModules = 1,
            SingleModuleDetails = 2,
            AddModule = 3,
            UpdateModule = 4,
            DeleteModule = 5,
            RunRiskCheck = 6,
            RunMaintenance = 7,
            ToggleOperationalState = 8,
            Exit = 9,
        }

        static void Main(string[] args)
        {
            var manager = new ModuleManager();
            var known = new List<string>();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=====================================");
                Console.WriteLine("=== Keplercraft-V Station Console ===");
                Console.WriteLine("=====================================");
                Console.WriteLine();
                Console.WriteLine("1) List All Modules");
                Console.WriteLine("2) Show Module Details");
                Console.WriteLine("3) Add Module");
                Console.WriteLine("4) Update Module");
                Console.WriteLine("5) Delete Module");
                Console.WriteLine("6) Run Risk Check For All Modules");
                Console.WriteLine("7) Run Maintenance (Module or All)");
                Console.WriteLine("8) Toggle Module Operational State");
                Console.WriteLine("9) Exit");
                Console.WriteLine("0) Seed sample modules");  ///// for presentation purposes only /////
                Console.WriteLine();
                Console.Write("Choose an option (1 - 9): ");

                try
                {
                    var choice = int.Parse(Console.ReadLine());
                    MenuOption option = (MenuOption)choice;
                    switch (option)
                    {
                        /////////////////////////// presentation purposes only /////////////////////////
                        case MenuOption.Seed:
                            manager.SeedRandomModules();

                            foreach (var item in new[] { "LS-1", "PC-1", "RL-1" })
                            {
                                if (!known.Contains(item)) known.Add(item);
                                var m = manager.GetModuleById(item);

                                if (m is LifeSupportModule ls) ModuleThreads.StartOxygenDrain(ls);
                                if (m is PowerCoreModule pc) ModuleThreads.StartReactorHeating(pc);
                                if (m is ResearchLabModule rl) ModuleThreads.StartDataCollection(rl);
                            }

                            Console.WriteLine("Seeded sample modules.");
                            Console.WriteLine();
                            Console.WriteLine("Press any button to continue..");
                            Console.ReadKey();
                            break;
                        /////////////////////////////////////////////////////////////////////////////////

                        case MenuOption.ListAllModules:
                            Console.Clear();
                            Console.WriteLine("--- Registered Modules ---");
                            if (known.Count() == 0)
                            {
                                Console.WriteLine("(no modules registered)");
                            }

                            foreach (var mid in known)
                            {
                                var mod = manager.GetModuleById(mid);

                                if (mod == null)
                                {
                                    continue;
                                }

                                var status = mod as IStatusReport;
                                Console.WriteLine($"{mod.ModuleID} | {mod.ModuleName} | Power: {mod.PowerConsumptionKw} kW | Operational: {mod.IsOperational} | {status?.StatusReport()}");
                            }

                            Console.WriteLine();
                            Console.WriteLine("Press any button to continue..");
                            Console.ReadKey();
                            break;

                        case MenuOption.SingleModuleDetails:
                            Console.Clear();
                            Console.Write("Module ID: ");
                            var showId = Console.ReadLine();
                            var show = manager.GetModuleById(showId);

                            if (show == null)
                            {
                                Console.WriteLine("Not found.");
                            }
                            else
                            {
                                Console.WriteLine($"ID: {show.ModuleID}");
                                Console.WriteLine($"Name: {show.ModuleName}");
                                Console.WriteLine($"Power: {show.PowerConsumptionKw} kW");
                                Console.WriteLine($"Operational: {show.IsOperational}");

                                if (show is IStatusReport sr)
                                {
                                    Console.WriteLine("Status: " + sr.StatusReport());
                                }

                                if (show is Imaintainable m)
                                {
                                    Console.WriteLine("Risk: " + m.RiskLevel());
                                }
                            }

                            Console.WriteLine();
                            Console.WriteLine("Press any button to continue..");
                            Console.ReadKey();
                            break;

                        case MenuOption.AddModule:
                            Console.Clear();
                            Console.Write("Module ID: ");
                            var id = Console.ReadLine();
                            Console.Write("Name: ");
                            var name = Console.ReadLine();
                            Console.Write("Power consumption (kW): ");
                            var powerTxt = Console.ReadLine();
                            double.TryParse(powerTxt, out var power);

                            Console.WriteLine("Types: 1) LifeSupport 2) PowerCore 3) ResearchLab");
                            Console.Write("Type: ");
                            var type = Console.ReadLine();

                            StationModule created = null;
                            if (type == "1")
                            {
                                Console.Write("Initial O2 level (%): ");
                                double.TryParse(Console.ReadLine(), out var o2);
                                Console.Write("O2 drain per hour (%/h): ");
                                double.TryParse(Console.ReadLine(), out var drain);
                                created = new LifeSupportModule(id, name, power, o2 == 0 ? 100.0 : o2, drain == 0 ? 0.5 : drain);
                            }
                            else if (type == "2")
                            {
                                Console.Write("Initial reactor temp (°C): ");
                                double.TryParse(Console.ReadLine(), out var temp);
                                Console.Write("Temp rise per hour (°C/h): ");
                                double.TryParse(Console.ReadLine(), out var rise);
                                created = new PowerCoreModule(id, name, power, temp == 0 ? 20.0 : temp, rise == 0 ? 0.5 : rise);
                            }
                            else if (type == "3")
                            {
                                Console.Write("Active experiments (int): ");
                                int.TryParse(Console.ReadLine(), out var amt);
                                Console.Write("Data per hour (GB/h): ");
                                double.TryParse(Console.ReadLine(), out var dph);
                                created = new ResearchLabModule(id, name, power, amt, dph);
                            }
                            else
                            {
                                Console.WriteLine("Please enter between 1 and 3.");
                            }

                            if (created != null)
                            {
                                manager.AddModule(created);

                                if (!known.Contains(created.ModuleID))
                                {
                                    known.Add(created.ModuleID);
                                }

                                if (created is LifeSupportModule ls) ModuleThreads.StartOxygenDrain(ls);
                                if (created is PowerCoreModule pc) ModuleThreads.StartReactorHeating(pc);
                                if (created is ResearchLabModule rl) ModuleThreads.StartDataCollection(rl);

                                Console.WriteLine($"Added module {created.ModuleID} ({created.ModuleName}).");
                            }

                            Console.WriteLine();
                            Console.WriteLine("Press any button to continue..");
                            Console.ReadKey();
                            break;

                        case MenuOption.UpdateModule:
                            Console.Clear();
                            Console.Write("Module ID to update: ");
                            var updId = Console.ReadLine();
                            Console.Write("New name (leave empty to skip): ");
                            var newName = Console.ReadLine();
                            Console.Write("New power (leave empty to skip): ");
                            var newPowerTxt = Console.ReadLine();
                            double? newPower = null;

                            if (double.TryParse(newPowerTxt, out var np))
                            {
                                newPower = np;
                            }

                            var target = manager.GetModuleById(updId);

                            if (target == null)
                            {
                                Console.WriteLine("Module not found.");
                                Console.WriteLine();
                                Console.WriteLine("Press any button to continue..");
                                Console.ReadKey();
                            }
                            else
                            {
                                double? o2lvl = null, o2dr = null, reactorTemp = null, tempRise = null, dataPerHour = null;
                                int? activeExp = null;

                                if (target is LifeSupportModule)
                                {
                                    Console.Write("O2 level (leave empty to skip): ");
                                    var t = Console.ReadLine();

                                    if (double.TryParse(t, out var v1))
                                    {
                                        o2lvl = v1;
                                    }

                                    Console.Write("O2 drain per hour (leave empty to skip): ");
                                    t = Console.ReadLine();

                                    if (double.TryParse(t, out var v2))
                                    {
                                        o2dr = v2;
                                    }
                                }
                                else if (target is PowerCoreModule)
                                {
                                    Console.Write("Reactor temp (leave empty to skip): ");
                                    var t = Console.ReadLine();

                                    if (double.TryParse(t, out var v1))
                                    {
                                        reactorTemp = v1;
                                    }

                                    Console.Write("Temp rise per hour (leave empty to skip): ");
                                    t = Console.ReadLine();

                                    if (double.TryParse(t, out var v2))
                                    {
                                        tempRise = v2;
                                    }
                                }
                                else if (target is ResearchLabModule)
                                {
                                    Console.Write("Active experiments (leave empty to skip): ");
                                    var t = Console.ReadLine();

                                    if (int.TryParse(t, out var iv))
                                    {
                                        activeExp = iv;
                                    }

                                    Console.Write("Data per hour (leave empty to skip): ");
                                    t = Console.ReadLine();

                                    if (double.TryParse(t, out var dv))
                                    {
                                        dataPerHour = dv;
                                    }
                                }

                                var result = manager.UpdateModule(
                                    updId,
                                    name: string.IsNullOrWhiteSpace(newName) ? null : newName,
                                    power: newPower,
                                    oxygenLevel: o2lvl,
                                    oxygenDrainPerHour: o2dr,
                                    reactorTemp: reactorTemp,
                                    tempRisePerHour: tempRise,
                                    activeExperiments: activeExp,
                                    dataPerHour: dataPerHour
                                );

                                Console.WriteLine(result);
                                Console.WriteLine();
                                Console.WriteLine("Press any button to continue..");
                                Console.ReadKey();
                            }
                            break;

                        case MenuOption.DeleteModule:
                            Console.Clear();
                            Console.Write("Module ID to remove: ");
                            var remID = Console.ReadLine();
                            var remRes = manager.RemoveModule(remID);

                            if (known.Contains(remID))
                            {
                                known.Remove(remID);
                            }

                            Console.WriteLine(remRes);
                            Console.WriteLine();
                            Console.WriteLine("Press any button to continue..");
                            Console.ReadKey();
                            break;

                        case MenuOption.RunRiskCheck:
                            Console.Clear();
                            manager.RunAllRisk();
                            Console.WriteLine();
                            Console.WriteLine("Press any button to continue..");
                            Console.ReadKey();
                            break;

                        case MenuOption.RunMaintenance:
                            Console.Clear();
                            Console.Write("Run maintenance for (id) or 'all': ");
                            var targetId = Console.ReadLine();

                            if (string.Equals(targetId, "all", StringComparison.OrdinalIgnoreCase))  // check if text matches regardless of case
                            {
                                foreach (var mid in known)
                                {
                                    var mm = manager.GetModuleById(mid);
                                    if (mm is Imaintainable im)
                                    {
                                        Console.WriteLine("Maitenance Report:");
                                        Console.WriteLine();
                                        im.CaculateMaintenance();
                                    }
                                }
                            }
                            else
                            {
                                var mm = manager.GetModuleById(targetId);

                                if (mm is Imaintainable im)
                                {
                                    Console.WriteLine("Maitenance Report:");
                                    Console.WriteLine();
                                    im.CaculateMaintenance();
                                }
                                else
                                {
                                    Console.WriteLine("Module not found or not maintainable.");
                                }
                            }

                            Console.WriteLine();
                            Console.WriteLine("Press any button to continue..");
                            Console.ReadKey();
                            break;

                        case MenuOption.ToggleOperationalState:
                            Console.Clear();
                            Console.Write("Module ID: ");
                            var togID = Console.ReadLine();
                            var modtog = manager.GetModuleById(togID);

                            if (modtog == null)
                            {
                                Console.WriteLine("Not found.");
                                Console.WriteLine();
                                Console.WriteLine("Press any button to continue..");
                                Console.ReadKey();
                            }
                            else
                            {
                                Console.Write("Set operational? (y/n): ");
                                var yn = Console.ReadLine();
                                bool setOp = yn?.ToLower().StartsWith("y") == true;  // exepts anyting starting in y
                                manager.UpdateModule(togID, isOperational: setOp);

                                if (setOp)
                                {
                                    if (modtog is LifeSupportModule ls) ModuleThreads.StartOxygenDrain(ls);
                                    if (modtog is PowerCoreModule pc) ModuleThreads.StartReactorHeating(pc);
                                    if (modtog is ResearchLabModule rl) ModuleThreads.StartDataCollection(rl);
                                }

                                Console.WriteLine("Updated operational state.");
                                Console.WriteLine();
                                Console.WriteLine("Press any button to continue..");
                                Console.ReadKey();
                            }

                            break;

                        case MenuOption.Exit:
                            Environment.Exit(0);
                            break;

                        default:
                            Console.WriteLine("Please choose between 1 and 9");
                            Console.WriteLine();
                            Console.WriteLine("Press any button to continue..");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Please only enter Numbers between 1 and 9");
                    Console.WriteLine();
                    Console.WriteLine("Press any button to continue..");
                    Console.ReadKey();
                }
            }
        }
    }
}