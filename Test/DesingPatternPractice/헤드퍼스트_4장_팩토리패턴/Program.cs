namespace 헤드퍼스트_4장_팩토리패턴
{
    #region 팩토리 패턴(관용구버전)
    public abstract class PizzaStore
    {
        public Pizza OrderPizza(string type)
        {
            Pizza pizza = CreatePizza(type);


            pizza.Prepare();
            pizza.Bake();
            pizza.Cut();
            pizza.Box();

            return pizza;
        }

        protected abstract Pizza CreatePizza(string type);
    }

    public class NewyorkPizzaStore : PizzaStore
    {
        protected override Pizza CreatePizza(string type)
        {
            Pizza pizza = null;
            PizzaIngredientFactory ingredientFactory = new NyPizzaIngredientFactory();

            if (type == "치즈")
                return new NYCheesePizza(ingredientFactory);
            else if (type == "페페로니")
                return new NyPeperonyPizza();
            else return null;
        }
    }

    public class KrPizzaStore : PizzaStore
    {
        protected override Pizza CreatePizza(string type)
        {
            if (type == "치즈")
                return new KrCheesePizza();
            else if (type == "페페로니")
                return new KrPeperonyPizza();
            else return null;
        }
    }



    public abstract class Pizza
    {
        protected string 도우 = "";
        protected string 이름 = "";
        protected string 소스 = "";
        protected List<string> 토핑 = new();


        protected Dough 돌우 = new();
        protected Source 솔스 = new();
        protected Cheese 치즈 = new();
        protected Veggies[] 톨핑스;
        public abstract void Prepare();

        public virtual void Cut()
        {
            Console.WriteLine(이름 + "잘랐다!");
        }

        public void Bake()
        {
            Console.WriteLine(이름 + "만들었다!");
        }

        public void Box()
        {
            Console.WriteLine(이름 + "포장했다!");
        }

        public string GetName()
        {
            return 이름;
        }
    }

    public class NYCheesePizza : Pizza
    {
        PizzaIngredientFactory IngredientFactory;

        public NYCheesePizza(PizzaIngredientFactory ingredientFactory)
        {
            this.IngredientFactory = ingredientFactory;


        }

        public override void Prepare()
        {
            Console.WriteLine("준비중");
            돌우 = IngredientFactory.CreateDough();
            솔스 = IngredientFactory.CreateSource();
            톨핑스 = IngredientFactory.CreateVeggies();
            치즈 = IngredientFactory.CreateCheese();
        }
    }
    public class KrCheesePizza : Pizza
    {
        public KrCheesePizza()
        {
            이름 = "코리아스탈치즈피자";
            도우 = "씬 도우";
            소스 = "마리나라 소스";
            토핑.Add("모짜렐라");
            토핑.Add("올리브");
        }
    }
    public class NyPeperonyPizza : Pizza
    {
        public NyPeperonyPizza()
        {
            이름 = "뉴욕스탈페페로니피자";
            도우 = "크러스트도우";
            소스 = "토마토 소스";
            토핑.Add("모짜렐라");
            토핑.Add("페페로니");
        }

        public override void Cut()
        {
            Console.WriteLine("이건 다르게 자른다");
        }
    }
    public class KrPeperonyPizza : Pizza
    {
        public KrPeperonyPizza()
        {
            이름 = "코리아스탈페페로니피자";
            도우 = "크러스트도우";
            소스 = "토마토 소스";
            토핑.Add("모짜렐라");
            토핑.Add("페페로니");
        }

        public override void Cut()
        {
            Console.WriteLine("이건 다르게 자른다");
        }
    }
    #endregion


    public class Program
    {
        static void Main(string[] args)
        {
            PizzaStore nystore = new NewyorkPizzaStore();
            PizzaStore krstore = new KrPizzaStore();

            var pizza = nystore.OrderPizza("치즈");
            Console.WriteLine($"1 {pizza.GetName()} 받음");
            pizza = nystore.OrderPizza("페페로니");
            Console.WriteLine($"2 {pizza.GetName()} 받음");

            pizza = krstore.OrderPizza("치즈");
            Console.WriteLine($"3 {pizza.GetName()} 받음");
            pizza = krstore.OrderPizza("페페로니");
            Console.WriteLine($"4 {pizza.GetName()} 받음");

        }
    }
}
