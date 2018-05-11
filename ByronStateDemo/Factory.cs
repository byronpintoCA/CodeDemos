using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByronStateDemo
{
    public class Factory
    {
        
        private static MainDrinkChoice _mainDrinkChoice;
        private static FlavorChoice _flavor;
        private static SalesTaxChoice _salesTax;

        private static PaymentInfo _payment;
        private static bool _setupDone;

        public static void Setup()
        {
            _payment = new PaymentInfo();
            _mainDrinkChoice = new MainDrinkChoice(_payment ,"Main Selection");
            _flavor = new FlavorChoice(_payment, "Flavor Selection");
            _salesTax = new SalesTaxChoice(_payment, " Sales Tax Selection");


            _mainDrinkChoice.Next = _flavor;
            _mainDrinkChoice.Back = null;
            _mainDrinkChoice.Finish = _salesTax;


            
            _flavor.Next = _salesTax;
            _flavor.Back = _mainDrinkChoice;
            _flavor.Finish = _salesTax;


           
            _salesTax.Next = null;
            _salesTax.Back = _flavor;
            _salesTax.Finish = null;

        }

        public static OrderState GetInitialState()
        {
            if (_setupDone ==false)
            {
                Setup();
                _setupDone = true;
            }

            return _mainDrinkChoice;
        }
        
    }
}
