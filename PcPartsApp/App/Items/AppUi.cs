using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Enums;

namespace App.Items
{
    public class AppUi
    {
        private int answer;
        private string typeAnswer;
        private int min;
        private int max;
        private double totalCost;
        private List<Item> Cart;
        private List<Part> outputParts { get; set; }
        private List<Module> outputModules { get; set; }
        private List<Configuration> outputConfigs { get; set; }

        public AppUi()
        {
            Cart = new List<Item>();
        }

        #region Methods
        private void PrintParts(IEnumerable<Part> parts)
        {
            outputParts = parts.ToList();
            for (int i = 0; i < outputParts.Count; i++)
            {
                Console.WriteLine($"{i + 1}P) ID: {outputParts[i].Id}, Name: {outputParts[i].Name}, Type: {outputParts[i].Type}, Company: {outputParts[i].Company}, Quantity: {outputParts[i].Quantity}, Price: {outputParts[i].Price}$, Warranty: {outputParts[i].Warranty}");
            }
        }

        private void PrintModules(IEnumerable<Module> modules)
        {
            outputModules = modules.ToList();
            for (int i = 0; i < outputModules.Count; i++)
            {
                Console.WriteLine($"{i + 1}M) ID: {outputModules[i].Id}, Type: {outputModules[i].Type}, Price: {outputModules[i].Price}$");
                Console.WriteLine("Parts:");
                PrintParts(outputModules[i].Parts);
                Console.WriteLine("-----------");
            }
        }


        private void printConfigurations(IEnumerable<Configuration> configs)
        {
            outputConfigs = configs.ToList();
            for (int i = 0; i < outputConfigs.Count; i++)
            {
                Console.WriteLine($"{i + 1}C) ID: {outputConfigs[i].Id}, Title: {outputConfigs[i].Title}, Type: {outputConfigs[i].Type}, Price: {outputConfigs[i].Price}$, Modules:");
                PrintModules(outputConfigs[i].Modules);
                Console.WriteLine("-----------");
            }
        }

        private void ShowParts(IEnumerable<Part> parts)
        {
            Console.Clear();
            Console.WriteLine("Select a part:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            PrintParts(parts);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void ShowModules(IEnumerable<Module> modules)
        {
            Console.Clear();
            Console.WriteLine("Select a Module:");
            Console.ForegroundColor = ConsoleColor.Cyan;
            PrintModules(modules);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void ShowConfigurations(IEnumerable<Configuration> configs)
        {
            Console.Clear();
            Console.WriteLine("Select a Configuration:");
            Console.ForegroundColor = ConsoleColor.Red;
            printConfigurations(configs);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void SeeCart()
        {
            foreach(Item cartItem in Cart)
            {
                if(cartItem.Id < 46)
                {
                    foreach(Part dbPart in Program.Db.Parts)
                    {
                        if (cartItem.Id == dbPart.Id) Console.WriteLine($"Part, ID: {dbPart.Id}, Name: {dbPart.Name}, Type: {dbPart.Type}, Company: {dbPart.Company}, Quantity: {dbPart.Quantity}, Price: {dbPart.Price}$, Warranty: {dbPart.Warranty}");
                    }
                }
                else if (cartItem.Id > 49 && cartItem.Id < 61)
                {
                    foreach (Module dbModule in Program.Db.Modules)
                    {
                        if (cartItem.Id == dbModule.Id) Console.WriteLine($"Module, ID: {dbModule.Id}, Type: {dbModule.Type}, Price: {dbModule.Price}$");
                    }
                }
                else if (cartItem.Id > 79 && cartItem.Id < 84)
                {
                    foreach(Configuration dbConfig in Program.Db.Configurations)
                    {
                        if (cartItem.Id == dbConfig.Id) Console.WriteLine($"Configuration, ID: {dbConfig.Id}, Title: {dbConfig.Title}, Type: {dbConfig.Type}, Price: {dbConfig.Price}$");
                    }
                }
                totalCost += cartItem.Price;
            }
            Console.WriteLine($"Total Price: {totalCost}$");
        }

        private void AddToCart(int itemTypeAnswer)
        {
            if (int.TryParse(Console.ReadLine(), out answer))
            {
                switch (itemTypeAnswer)
                {
                    case 1:
                        if (answer > 0 && answer <= outputParts.Count)
                        {
                            if (Cart.Where(x => x.Id > 0 && x.Id < 46).ToList().Count < 10) Cart.Add(outputParts[answer - 1]);
                            else
                            {
                                Console.WriteLine("You have to many Parts in your cart");
                                Console.ReadKey();
                            }
                        }
                        else throw new InvalidInputException();
                        break;
                    case 2:
                        if (answer > 0 && answer <= outputModules.Count)
                        {
                            if (Cart.Where(x => x.Id > 49 && x.Id < 61).ToList().Count < 5) Cart.Add(outputModules[answer - 1]);
                            else
                            {
                                Console.WriteLine("You have to many Modules in your cart");
                                Console.ReadKey();
                            }
                        }
                        else throw new InvalidInputException();
                        break;
                    case 3:
                        if (answer > 0 && answer <= outputConfigs.Count)
                        {
                            if (Cart.Where(x => x.Id > 79 && x.Id < 84).ToList().Count < 1) Cart.Add(outputConfigs[answer - 1]);
                            else
                            {
                                Console.WriteLine("You already have a configuration in your cart");
                                Console.ReadKey();
                            }                                
                        }
                        else throw new InvalidInputException();
                        break;
                }
                Console.Clear();
                Console.WriteLine($"The item: ID: {Cart.Last().Id} was added to your cart");

                Console.WriteLine("Select an Option:");
                Console.WriteLine("1) Continue shopping");
                Console.WriteLine("2) Choose something else");
                Console.WriteLine("3) See cart");
                Console.WriteLine("4) Continue to check out");

                if (int.TryParse(Console.ReadLine(), out answer))
                {
                    switch(answer)
                    {
                        case 1:
                            switch(itemTypeAnswer)
                            {
                                case 1:
                                    ShowParts(outputParts);
                                    AddToCart(1);
                                    break;
                                case 2:
                                    ShowModules(outputModules);
                                    AddToCart(2);
                                    break;
                                case 3:
                                    ShowConfigurations(outputConfigs);
                                    AddToCart(3);
                                    break;
                            }
                            break;
                        case 2:
                            WelcomePage();
                            break;
                        case 3:
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("--------------------------------------------------------YOUR CART-------------------------------------------------------");
                            SeeCart();
                            Console.WriteLine("--------------------------------------------------------YOUR CART-------------------------------------------------------");
                            Console.ResetColor();
                            Console.ReadKey();
                            WelcomePage();
                            break;
                        case 4:
                            Console.Clear();
                            Console.WriteLine("--------------------------------------------------------COMING SOON-----------------------------------------------------");
                            break;
                    }
                }

            }
            else throw new InvalidInputException();
        }
        #endregion

        #region Welcome page
        public void WelcomePage()
        {            
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("                                       ------WELCOME TO OUR STORE------");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("Select an Option:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("1) Parts");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("2) Modules");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("3) Configurations");
            Console.ResetColor();
            if (int.TryParse(Console.ReadLine(), out answer))
            {
                switch (answer)
                {
                    case 1:                                                  // PARTS OPTIONS
                        Console.Clear();
                        Console.WriteLine("Select An Option:");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("1) All Products");
                        Console.WriteLine("2) ByPrice");
                        Console.WriteLine("3) By Type");
                        Console.ForegroundColor = ConsoleColor.White;
                        if (int.TryParse(Console.ReadLine(), out answer))
                        {
                            switch (answer)
                            {
                                case 1:
                                    ShowParts(Program.Db.Parts);
                                    AddToCart(1);
                                    break;
                                case 2:
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Enter minimum price(8$):");
                                    if (int.TryParse(Console.ReadLine(), out min) && min >= 8)
                                    {
                                        Console.WriteLine("Enter maximum price:");
                                        if (int.TryParse(Console.ReadLine(), out max) && max >= min)
                                        {
                                            ShowParts(Program.Db.Parts.Where(x => x.Price >= min && x.Price <= max));
                                            AddToCart(1);
                                        }
                                        else throw new InvalidInputException();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Lowest Price of a part is 8$");
                                        Console.ReadKey();
                                        WelcomePage();
                                    }
                                    break;
                                case 3:
                                    Console.WriteLine("Enter part type(Proccessing,Graphics,Casing,MainBoard,Memory,Other)");
                                    typeAnswer = Console.ReadLine();
                                    switch (typeAnswer.ToLower())
                                    {
                                        case "proccessing":
                                            ShowParts(Program.Db.Parts.Where(x => x.Type == PartType.Cpu || x.Type == PartType.CpuCooler));
                                            break;
                                        case "graphics":
                                            ShowParts(Program.Db.Parts.Where(x => x.Type == PartType.Gpu || x.Type == PartType.GpuCooler));
                                            break;
                                        case "casing":
                                            ShowParts(Program.Db.Parts.Where(x => x.Type == PartType.Case || x.Type == PartType.PowerSuply));
                                            break;
                                        case "mainboard":
                                            ShowParts(Program.Db.Parts.Where(x => x.Type == PartType.MotherBoard || x.Type == PartType.ConnectionCable || x.Type == PartType.PowerCable));
                                            break;
                                        case "memory":
                                            ShowParts(Program.Db.Parts.Where(x => x.Type == PartType.SSD || x.Type == PartType.HDD || x.Type == PartType.RAM));
                                            break;
                                        case "other":
                                            ShowParts(Program.Db.Parts.Where(x => x.Type == PartType.Monitor || x.Type == PartType.Mouse || x.Type == PartType.Keyboard));
                                            break;
                                        default:
                                            throw new InvalidInputException();
                                            break;
                                    }
                                    AddToCart(1);
                                    break;
                                default:
                                    Console.WriteLine("you need to choose a number between 1 and 3!");
                                    Console.ReadKey();
                                    WelcomePage();
                                    break;
                            }
                        }
                        else throw new InvalidInputException();
                        break;
                    case 2:                                                 // MODULES OPTIONS
                        Console.Clear();
                        Console.WriteLine("Select An Option:");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("1) All Products");
                        Console.WriteLine("2) ByPrice");
                        Console.WriteLine("3) By Type");
                        Console.ForegroundColor = ConsoleColor.White;
                        if (int.TryParse(Console.ReadLine(), out answer))
                        {
                            switch (answer)
                            {
                                case 1:
                                    ShowModules(Program.Db.Modules);
                                    AddToCart(2);
                                    break;
                                case 2:
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                    Console.WriteLine("Enter minimum price(100$):");
                                    if (int.TryParse(Console.ReadLine(), out min) && min >= 100)
                                    {
                                        Console.WriteLine("Enter maximum price:");
                                        if (int.TryParse(Console.ReadLine(), out max) && max >= min)
                                        {
                                            ShowModules(Program.Db.Modules.Where(x => x.Price >= min && x.Price <= max));
                                            AddToCart(2);
                                        }
                                        else throw new InvalidInputException();
                                    }
                                    else
                                    {
                                        Console.WriteLine("The Lowest Price of a module is 100$");
                                        Console.ReadKey();
                                        WelcomePage();
                                    }
                                    break;
                                case 3:
                                    Console.WriteLine("Enter Module type(Proccessing,Graphics,Casing,MainBoard,Memory,Other)");
                                    typeAnswer = Console.ReadLine();
                                    switch (typeAnswer.ToLower())
                                    {
                                        case "proccessing":
                                            ShowModules(Program.Db.Modules.Where(x => x.Type == ModuleType.Processing));
                                            break;
                                        case "graphics":
                                            ShowModules(Program.Db.Modules.Where(x => x.Type == ModuleType.Graphics));
                                            break;
                                        case "casing":
                                            ShowModules(Program.Db.Modules.Where(x => x.Type == ModuleType.Casing));
                                            break;
                                        case "mainboard":
                                            ShowModules(Program.Db.Modules.Where(x => x.Type == ModuleType.MainBoard));
                                            break;
                                        case "memory":
                                            ShowModules(Program.Db.Modules.Where(x => x.Type == ModuleType.Memory));
                                            break;
                                        case "other":
                                            ShowModules(Program.Db.Modules.Where(x => x.Type == ModuleType.Other));
                                            break;
                                        default:
                                            throw new InvalidInputException();
                                            break;
                                    }
                                    AddToCart(2);
                                    break;
                                default:
                                    Console.WriteLine("you need to choose a number between 1 and 3!");
                                    Console.ReadKey();
                                    WelcomePage();
                                    break;
                            }
                        }
                        else throw new InvalidInputException();
                        break;
                    case 3:                                                    // CONFIGURATIONS OPTIONS
                        Console.Clear();
                        Console.WriteLine("Select An Option:");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("1) All Products");
                        Console.WriteLine("2) ByPrice");
                        Console.WriteLine("3) By Type");
                        Console.ForegroundColor = ConsoleColor.White;
                        if (int.TryParse(Console.ReadLine(), out answer))
                        {
                            switch (answer)
                            {
                                case 1:
                                    ShowConfigurations(Program.Db.Configurations);
                                    AddToCart(3);
                                    break;
                                case 2:
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Enter minimum price(800$):");
                                    if (int.TryParse(Console.ReadLine(), out min) && min >= 800)
                                    {
                                        Console.WriteLine("Enter maximum price:");
                                        if (int.TryParse(Console.ReadLine(), out max) && max >= min)
                                        {
                                            ShowConfigurations(Program.Db.Configurations.Where(x => x.Price >= min && x.Price <= max));
                                            AddToCart(3);
                                        }
                                        else throw new InvalidInputException();
                                    }
                                    else
                                    {
                                        Console.WriteLine("The Lowest Price of a Configuration is 800$");
                                        Console.ReadKey();
                                        WelcomePage();
                                    }
                                    break;
                                case 3:
                                    Console.WriteLine("Enter Configuration type(Standard,Office,Gaming)");
                                    typeAnswer = Console.ReadLine();
                                    switch (typeAnswer.ToLower())
                                    {
                                        case "standard":
                                            ShowConfigurations(Program.Db.Configurations.Where(x => x.Type == ConfigurationType.Standard));
                                            break;
                                        case "office":
                                            ShowConfigurations(Program.Db.Configurations.Where(x => x.Type == ConfigurationType.Office));
                                            break;
                                        case "gaming":
                                            ShowConfigurations(Program.Db.Configurations.Where(x => x.Type == ConfigurationType.Gaming));
                                            break;                                        
                                        default:
                                            throw new InvalidInputException();
                                            break;
                                    }
                                    AddToCart(3);
                                    break;
                                default:
                                    Console.WriteLine("you need to choose a number between 1 and 3!");
                                    Console.ReadKey();
                                    WelcomePage();
                                    break;
                            }
                        }
                        else throw new InvalidInputException();
                        break;
                    default:
                        Console.WriteLine("you need to choose a number between 1 and 3!");
                        Console.ReadKey();
                        WelcomePage();
                        break;
                }
            }
            else throw new InvalidInputException();

        }
        #endregion
    }
}
