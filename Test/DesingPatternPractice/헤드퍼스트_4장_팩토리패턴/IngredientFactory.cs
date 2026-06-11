using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 헤드퍼스트_4장_팩토리패턴
{
    public interface PizzaIngredientFactory
    {
        public Dough CreateDough();
        public Source CreateSource();
        public Cheese CreateCheese();
        public Veggies[] CreateVeggies();
        public Peperoni CreatePeperoni();
        public Clamis CreateClams();
    }

    public class NyPizzaIngredientFactory : PizzaIngredientFactory
    {
        public Cheese CreateCheese()
        {
            throw new NotImplementedException();
        }

        public Clamis CreateClams()
        {
            throw new NotImplementedException();
        }

        public Dough CreateDough()
        {
            throw new NotImplementedException();
        }

        public Peperoni CreatePeperoni()
        {
            throw new NotImplementedException();
        }

        public Source CreateSource()
        {
            throw new NotImplementedException();
        }

        public Veggies[] CreateVeggies()
        {
            throw new NotImplementedException();
        }
    }
}
