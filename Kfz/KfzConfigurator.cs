using Kfz.Database;
using System.Text.RegularExpressions;

namespace Kfz
{
    public class KfzConfigurator
    {
        private readonly IKfzService kfzService;
        private readonly IConsoleHelper consoleHelper;

        public KfzConfigurator(IKfzService kfzService, IConsoleHelper consoleHelper)
        {
            this.kfzService = kfzService;
            this.consoleHelper = consoleHelper;
        }

        public void Run()
        {
            while (!Menu())
            {

            }
        }

        private bool Menu()
        {
            Console.WriteLine("Was möchten Sie tun?");
            int option = this.consoleHelper.PickOption(["Neue Bestellung", "Bestellung ändern", "Bestellung stornieren"]);

            switch (option)
            {
                case 0: NewOrder(); return false;
                case 1: EditOrder(); return false;
                case 2: CancelOrder(); return false;
                default: return true;
            }
        }

        private void NewOrder()
        {
            // 1) Name
            Console.WriteLine("Bitte geben Sie den Namen des Kunden ein.");
            string? firstname = this.consoleHelper.InputString("Vorname:");
            if (firstname == null)
            {
                return;
            }

            string? lastname = this.consoleHelper.InputString("Nachname:");
            if (lastname == null)
            {
                return;
            }

            // 2) Geburtsdatum

            DateOnly? birthdate = this.consoleHelper.InputDate("Bitte geben Sie das Geburtsdatum des Kunden im Format TT.MM.JJJJ ein.");
            if (birthdate == null)
            {
                return;
            }

            // 3) Hersteller
            Console.WriteLine("Bitte wählen Sie den Hersteller aus.");

            IList<Manufacturer> manufacturers = this.kfzService.GetManufacturers();

            Manufacturer? chosenManufacturer = this.consoleHelper.PickSingleItemFromList(manufacturers, m => $"{m.DisplayName} - Grundpreis {m.BasePrice} \u20AC");
            if (chosenManufacturer == null)
            {
                return;
            }

            // 4) Treibstoff
            Console.WriteLine("Bitte wählen Sie den Treibstoff aus.");

            IList<Fuel> fuels = this.kfzService.GetFuels();

            Fuel? chosenFuel = this.consoleHelper.PickSingleItemFromList(fuels, f => f.DisplayName);
            if (chosenFuel == null)
            {
                return;
            }

            // 5) Motor
            Console.WriteLine("Bitte wählen Sie den Motor aus.");

            IList<Motor> motors = this.kfzService.GetMotorsForFuel(chosenFuel.Id);

            Motor? chosenMotor = this.consoleHelper.PickSingleItemFromList(motors, m => $"{m.DisplayName} - {m.Price} \u20AC");
            if (chosenMotor == null)
            {
                return;
            }

            // 6) Farbe
            string? color = this.consoleHelper.InputString("Farbe (Format: #rrggbb):", s => Regex.IsMatch(s, @"[#][0-9A-Fa-f]{6}\b"));
            if (color == null)
            {
                return;
            }

            // 7) Optionen
            Console.WriteLine("Bitte wählen Sie nacheinander die Optionen aus, die hinzugefügt werden sollen.");

            IList<Option> options = this.kfzService.GetOptions();

            IList<Option>? chosenOptions = this.consoleHelper.PickMultipleItemsFromList(options, o => $"{o.DisplayName} - {o.Price} \u20AC");
            if (chosenOptions == null)
            {
                return;
            }

            Order order = new()
            {
                FirstName = firstname,
                LastName = lastname,
                BirthDate = (DateOnly)birthdate,
                ManufacturerId = chosenManufacturer.Id,
                Manufacturer = chosenManufacturer,
                MotorId = chosenMotor.Id,
                Motor = chosenMotor,
                Color = color
            };

            foreach (Option o in chosenOptions)
            {
                order.Options.Add(o);
            }

            PrintOrderSummary(order);

            if (this.consoleHelper.Confirm("Möchten Sie die Bestellung speichern? (Ja/q)"))
            {
                order = this.kfzService.CreateOrder(order);
                PrintOrderSummary(order);
            }
        }

        private void EditOrder()
        {
            Order? order = ChooseOrder();
            if (order == null)
            {
                return;
            }

            PrintOrderSummary(order);
            // edit Order

            int option = -1;
            bool orderEdited = false;
            while (option != 0)
            {
                Console.WriteLine("Welchen Wert möchten Sie bearbeiten?");

                Func<int, string> optionToString = i =>
                {
                    switch (i)
                    {
                        case 1: return "Hersteller";
                        case 2: return "Treibstoff";
                        case 3: return "Motor";
                        case 4: return "Farbe";
                        case 5: return "Optionen";
                        case 0: return "-- Bearbeitung beenden --";
                        default: return "";
                    }
                };

                option = this.consoleHelper.PickSingleItemFromList(
                    [1, 2, 3, 4, 5, 0],
                    optionToString
                );

                switch (option)
                {
                    case 1:
                        Console.WriteLine("Bitte wählen Sie den Hersteller aus.");

                        IList<Manufacturer> manufacturers = this.kfzService.GetManufacturers();

                        Manufacturer? chosenManufacturer = this.consoleHelper.PickSingleItemFromList(manufacturers, m => $"{m.DisplayName} - Grundpreis {m.BasePrice} \u20AC");
                        if (chosenManufacturer != null && order.ManufacturerId != chosenManufacturer.Id)
                        {
                            order.Manufacturer = chosenManufacturer;
                            order.ManufacturerId = chosenManufacturer.Id;
                            orderEdited = true;
                        }
                        break;
                    case 2:
                        Console.WriteLine("Bitte wählen Sie den Treibstoff aus.");

                        IList<Fuel> fuels = this.kfzService.GetFuels();

                        Fuel? chosenFuel = this.consoleHelper.PickSingleItemFromList(fuels, f => f.DisplayName);
                        if (chosenFuel != null && order.Motor?.FuelId != chosenFuel.Id)
                        {
                            // choose Motor for changed Fuel
                            Console.WriteLine("Bitte wählen Sie den Motor aus.");

                            IList<Motor> motors = this.kfzService.GetMotorsForFuel(chosenFuel.Id);

                            Motor? chosenMotor = this.consoleHelper.PickSingleItemFromList(motors, m => $"{m.DisplayName} - {m.Price} \u20AC");
                            if (chosenMotor != null)
                            {
                                order.Motor = chosenMotor;
                                order.MotorId = chosenMotor.Id;
                                orderEdited = true;
                            }
                        }
                        break;
                    case 3:
                        Console.WriteLine("Bitte wählen Sie den Motor aus.");

                        if (order.Motor == null)
                        {
                            break;
                        }

                        IList<Motor> motors2 = this.kfzService.GetMotorsForFuel(order.Motor.FuelId);

                        Motor? chosenMotor2 = this.consoleHelper.PickSingleItemFromList(motors2, m => $"{m.DisplayName} - {m.Price} \u20AC");
                        if (chosenMotor2 != null)
                        {
                            order.Motor = chosenMotor2;
                            order.MotorId = chosenMotor2.Id;
                            orderEdited = true;
                        }
                        break;
                    case 4:
                        string? color = this.consoleHelper.InputString("Farbe (Format: #rrggbb):", s => Regex.IsMatch(s, @"[#][0-9A-Fa-f]{6}\b"));
                        if (color != null && !color.Equals(order.Color))
                        {
                            order.Color = color;
                            orderEdited = true;
                        }
                        break;
                    case 5:
                        Console.WriteLine("Bitte wählen Sie nacheinander die Optionen aus, die hinzugefügt werden sollen.");

                        IList<Option> options = this.kfzService.GetOptions();

                        IList<Option>? chosenOptions = this.consoleHelper.PickMultipleItemsFromList(options, o => $"{o.DisplayName} - {o.Price} \u20AC");
                        if (chosenOptions != null
                            && !(
                                    order.Options.Count == chosenOptions.Count
                                    && order.Options.Intersect(chosenOptions).Count() == chosenOptions.Count
                                )
                            )
                        {
                            order.Options.Clear();
                            foreach (Option opt in chosenOptions)
                            {
                                order.Options.Add(opt);
                            }
                            orderEdited = true;
                        }
                        break;
                }
            }

            if (orderEdited)
            {
                PrintOrderSummary(order);

                if (this.consoleHelper.Confirm("Möchten Sie die Bestellung abspeichern? (Ja/q)"))
                {
                    this.kfzService.UpdateOrder(order);
                }
            }
        }

        private void CancelOrder()
        {
            Order? order = ChooseOrder();
            if (order == null)
            {
                return;
            }

            PrintOrderSummary(order);
            // confirm deletion

            if (this.consoleHelper.Confirm("Soll die Bestellung wirklich storniert werden? (Ja/q)"))
            {
                this.kfzService.DeleteOrder(order);
            }
        }

        private Order? ChooseOrder()
        {
            Console.WriteLine("Wie möchten Sie nach der Bestellung suchen?");

            int searchOption = this.consoleHelper.PickSingleItemFromList([1, 2], opt => opt == 1 ? "Nummer" : "Name + Geburtsdatum");

            switch (searchOption)
            {
                case 1:
                    return ChooseOrderByNumber();
                case 2:
                    return ChooseOrderByLastnameBirthDate();
                default:
                    return null;
            }
        }

        private Order? ChooseOrderByNumber()
        {
            Order? order = null;
            while (order == null)
            {
                int orderNumber = 0;
                while (orderNumber == 0)
                {
                    if (!int.TryParse(this.consoleHelper.InputString("Nummer:"), out orderNumber))
                    {
                        this.consoleHelper.InvalidUserInputMessage();
                    }
                }

                order = this.kfzService.GetOrderByNumber(orderNumber);

                if (order == default)
                {
                    Console.WriteLine($"Eine Bestellung mit der Nummer {orderNumber} existiert nicht. Bitte geben Sie eine valide Bestellnummer ein.");
                }
            }
            return order;
        }
        private Order? ChooseOrderByLastnameBirthDate()
        {
            Console.WriteLine("Bitte geben Sie den Nachnamen des Kunden ein.");

            string? lastname = this.consoleHelper.InputString("Nachname:");
            if (lastname == null)
            {
                return null;
            }

            DateOnly? birthdate = this.consoleHelper.InputDate("Bitte geben Sie das Geburtsdatum des Kunden im Format TT.MM.JJJJ ein.");
            if (birthdate == null)
            {
                return null;
            }

            IList<Order> orders = this.kfzService.GetOrdersByLastnameBirthdate(lastname, (DateOnly)birthdate);

            if (orders.Count == 0)
            {
                Console.WriteLine($"Eine Bestellung für den Kunden '{lastname} {birthdate}' existiert nicht. Versuchen Sie es bitte erneut.");
            }

            if (orders.Count == 1)
            {
                return orders[0];
            }
            else
            {
                return this.consoleHelper.PickSingleItemFromList(
                    orders,
                    order => $"{order.Id} - {order.Manufacturer?.DisplayName} {order.Motor?.DisplayName} - {OrderPrice(order)} EUR");
            }
        }

        private void PrintOrderSummary(Order order)
        {
            Console.WriteLine();
            Console.WriteLine("Zusammenfassung der Bestellung");
            if (order.Id != 0)
            {
                Console.WriteLine($"Nummer: {order.Id}");
            }
            Console.WriteLine($"Vorname: {order.FirstName}");
            Console.WriteLine($"Nachname: {order.LastName}");
            Console.WriteLine($"Geburtstag: {order.BirthDate}");
            Console.WriteLine($"Hersteller: {order.Manufacturer?.DisplayName} - {order.Manufacturer?.BasePrice}");
            Console.WriteLine($"Treibstoff: {order.Motor?.Fuel?.DisplayName}");
            Console.WriteLine($"Motor: {order.Motor?.DisplayName} - {order.Motor?.Price} EUR");
            Console.WriteLine($"Farbe: {order.Color}");

            Console.WriteLine("Optionen:");

            int i = 1;
            foreach (Option opt in order.Options)
            {
                Console.WriteLine($"{i}) {opt.DisplayName} - {opt.Price} EUR");
                i++;
            }

            Console.WriteLine($"Gesamtpreis: {OrderPrice(order)} EUR");
            Console.WriteLine();
        }

        private int OrderPrice(Order order)
        {
            int orderPrice;

            if (order.Manufacturer == null || order.Motor == null)
            {
                return -1;
            }

            orderPrice = order.Manufacturer.BasePrice + order.Motor.Price;
            foreach (Option opt in order.Options)
            {
                orderPrice += opt.Price;
            }
            return orderPrice;
        }
    }
}
