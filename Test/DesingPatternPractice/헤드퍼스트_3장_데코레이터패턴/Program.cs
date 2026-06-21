#pragma warning disable

namespace 헤드퍼스트_3장_데코레이터패턴
{
    public abstract class Baverage
    {
        protected string _descreption = "제목 없음";

        public virtual string GetDescription()
        {
            return _descreption;
        }

        public abstract double Cost();
    }

    

    public class Espresso : Baverage
    {
        public Espresso()
        {
            _descreption = "에스프레소";
        }

        public override double Cost()
        {
            return 1.99;
        }
    }

    public class HouseBlend : Baverage
    {
        public HouseBlend()
        {
            _descreption = "하우스 블렌드 커피";
        }

        public override double Cost()
        {
            return .89;
        }
    }
    public abstract class CondimentDecorator : Baverage
    {
        protected Baverage _baverage;
        public abstract override string GetDescription();
    }
    public class Mocha : CondimentDecorator
    {
        public Mocha(Baverage baverage)
        {
            this._baverage = baverage;
        }

        public override double Cost()
        {
            return _baverage.Cost() + .20;
        }

        public override string GetDescription()
        {
            return _baverage.GetDescription() + ", 모카";
        }
    }
    public class Soy : CondimentDecorator
    {
        public Soy(Baverage baverage)
        {
            this._baverage = baverage;
        }

        public override double Cost()
        {
            return _baverage.Cost() + .15;
        }

        public override string GetDescription()
        {
            return _baverage.GetDescription() + ", 두유";
        }
    }
    public class Whip : CondimentDecorator
    {
        public Whip(Baverage baverage)
        {
            this._baverage = baverage;
        }

        public override double Cost()
        {
            return _baverage.Cost() + .10;
        }

        public override string GetDescription()
        {
            return _baverage.GetDescription() + ", 휘핑크림";
        }
    }
    public class Program
    {
        static void Main(string[] args)
        {
            Baverage baverage = new Espresso();
            Console.WriteLine(baverage.GetDescription() + " $" + baverage.Cost());

            Baverage baverage2 = new HouseBlend();
            baverage2 = new Mocha(baverage2);
            baverage2 = new Mocha(baverage2);
            baverage2 = new Whip(baverage2);

            Console.WriteLine(baverage2.GetDescription() + " $" + baverage2.Cost().ToString("F4"));

            Baverage baverage3 = new HouseBlend();
            baverage3 = new Soy(baverage3);
            baverage3 = new Mocha(baverage3);
            baverage3 = new Whip(baverage3);
            Console.WriteLine(baverage3.GetDescription() + " $" + baverage3.Cost().ToString("F4"));

        }
    }
}
