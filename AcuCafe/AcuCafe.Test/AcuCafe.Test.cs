using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AcuCafe.Test
{
    [TestClass]
    public class AcuCafeTest
    {
        [TestMethod]
        public void IsDrinkCorrect()
        {//TODO => Add a test for each Drink type.
            var drinks = new string[]
            {
                "Expresso",
                "HotTea",
                "IceTea"
            };
            for(var i = 0; i < 3;i++)
            {
                var hasMilk = i % 2 == 1;
                var hasSugar = i % 2 == 0;
                var result = AcuCafe.OrderDrink(drinks[i], hasMilk, hasSugar);
                Assert.AreEqual(drinks[i], result.GetType().Name);
                Assert.AreEqual(hasMilk, result.Toppings.Any(t => t.Type == ToppingType.Milk));
                Assert.AreEqual(hasSugar, result.Toppings.Any(t => t.Type == ToppingType.Sugar));
                Assert.IsTrue(result.HasBeenPrepared);
            }
        }

        [TestMethod]
        public void EnsureExpressoCanHaveChocolate()
        {
            var result = AcuCafe.OrderDrink("Expresso", false, false, true);
            Assert.IsTrue(result.Toppings.Any(t => t.Type == ToppingType.Chocolate));
        }

        [TestMethod]
        public void EnsureIceTeaCannotHaveMilk()
        {
            var result = AcuCafe.OrderDrink("IceTea", true, false);
            Assert.IsFalse(result.HasBeenPrepared);
        }
    }
}
