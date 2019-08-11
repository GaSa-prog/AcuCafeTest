using System;
using System.Collections.Generic;
using System.Linq;

namespace AcuCafe
{
    public class AcuCafe
    {
        public static Drink OrderDrink(string type, 
            bool hasMilk, 
            bool hasSugar,
            bool hasChocolate = false)//Adding hasChocolate as optional parameter to avoid regression
        {
            Drink drink = null;
            type = RemoveSpaceIfAny(type);
            Enum.TryParse(type, out DrinkType drinkType);
            switch (drinkType)
            {
                case DrinkType.Expresso:
                    drink = new Expresso();
                    break;
                case DrinkType.HotTea:
                    drink = new HotTea();
                    break;
                case DrinkType.IceTea:
                    drink = new IceTea();
                    break;
            }
            try
            {
                if (hasMilk)
                    drink.Toppings.Add(new Milk());

                if (hasSugar)
                    drink.Toppings.Add(new Sugar());

                if (hasChocolate)
                    drink.Toppings.Add(new Chocolate());

                drink.Prepare();
            }
            catch (Exception ex)
            {
                Console.WriteLine("We are unable to prepare your drink.");
                //Warning: I Add to change to d:\ as root is readonly on my machine.
                System.IO.File.WriteAllText(@"c:\Error.txt", ex.ToString());
            }

            return drink;
        }
        private static string RemoveSpaceIfAny(string type)
        {
            return type.Replace(" ", string.Empty);
        }
    }

    public abstract class Topping
    {
        public abstract ToppingType Type { get; set; }
        public  abstract double Cost { get; set; }
    }

    public class Chocolate : Topping
    {
        public override double Cost { get; set; } = 1;

        public override ToppingType Type { get; set; } = ToppingType.Chocolate;
    }

    public class Milk : Topping
    {
        public override double Cost { get; set; } = .5;

        public override ToppingType Type { get; set; } = ToppingType.Milk;
    }

    public class Sugar : Topping
    {
        public override double Cost { get; set; } = .5;

        public override ToppingType Type { get; set; } = ToppingType.Sugar;
    }

    /// <summary>
    /// Default implementation of a Drink
    /// </summary>
    public abstract class Drink
    {
        public List<Topping> Toppings { get; set; } = new List<Topping>();
        public abstract string Description { get; }

        public abstract double Cost { get; set; }

        //Added to get test result case Prepare did not finish.
        public bool HasBeenPrepared { get; set; }

        //Get the sum of toppings to add to initial cost 
        //(as virtual in case a product need a specific implementation)
        public virtual double TotalCost
            => Cost + Toppings?.Sum(t => t.Cost) ?? 0;

        public virtual void Prepare()
        {
            string message = "We are preparing the following drink for you: " + Description;
            if (Toppings.Any(t => t.Type == ToppingType.Milk))
                message += "with milk";
            else
                message += "without milk";

            if (Toppings.Any(t => t.Type == ToppingType.Sugar))
                message += "with sugar";
            else
                message += "without sugar";

            HasBeenPrepared = true;
            Console.WriteLine(message);
        }
    }

    public class Expresso : Drink
    {
        public override string Description
        {
            get { return "Expresso"; }
        }

        public override double Cost { get; set; } = 1.8;

        public override void Prepare()
        {
            string message = "We are preparing the following drink for you: " + Description;
            if (Toppings.Any(t => t.Type == ToppingType.Milk))
                message += "with milk";
            else
                message += "without milk";

            if (Toppings.Any(t => t.Type == ToppingType.Sugar))
                message += "with sugar";
            else
                message += "without sugar";

            if (Toppings.Any(t => t.Type == ToppingType.Chocolate))
                message += "with chocolate";
            else
                message += "without chocolate";
            HasBeenPrepared = true;
            Console.WriteLine(message);
        }
    }

    public class HotTea : Drink
    {
        public override string Description
        {
            get { return "Hot tea"; }
        }

        public override double Cost { get; set; } = 1;
    }

    public class IceTea : Drink
    {
        public override string Description
        {
            get { return "Ice tea"; }
        }

        public override double Cost { get; set; } = 1.5;

        public override void Prepare()
        {
            var message = "";
            if (Toppings.Any(t => t.Type == ToppingType.Milk))
            {//Asssuming log file is for barista
                message = "You cannot select Milk with Ice Tea.";
                throw new InvalidOperationException(
                    $"warning: IceTea was not prepared, reason: " +
                    $"Milk was selected as topping. {DateTime.Now.ToString("dd/MM/yyyy h:mm tt")}");
            }

            message  = "We are preparing the following drink for you: " + Description;

            if (Toppings.Any(t => t.Type == ToppingType.Sugar))
                message += "with sugar";
            else
                message += "without sugar";

            if (Toppings.Any(t => t.Type == ToppingType.Chocolate))
                message += "with chocolate";
            else
                message += "without chocolate";
            HasBeenPrepared = true;
            Console.WriteLine(message);
        }
    }
}