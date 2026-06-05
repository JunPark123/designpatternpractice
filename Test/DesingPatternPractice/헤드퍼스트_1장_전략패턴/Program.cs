

namespace 헤드퍼스트_1장_전략패턴
{
    #region Interface
    public interface FlyBehavior
    {
        void fly();
    }
    public interface QuackBehavior
    {
        void quack();
    }
    #endregion

    #region 구현 Class
    public class FlyWithWings : FlyBehavior
    {
        public void fly()
        {
            Console.WriteLine("날아요");
        }
    }
    public class FlyNoWay : FlyBehavior
    {
        public void fly()
        {
            Console.WriteLine("날지 않아요");
        }
    }
    public class Quack : QuackBehavior
    {
        public void quack()
        {
            Console.WriteLine("꽥꽥");
        }
    }
    public class MuteQuack : QuackBehavior
    {
        public void quack()
        {
            Console.WriteLine("꽥꽥대지 않아요");
        }
    }
    #endregion

    public abstract class Duck
    {
        public FlyBehavior flyBehavior { get; set; }
        public QuackBehavior quackBehavior{ get; set; }

        public Duck() { }

        public abstract void display();

        public void performFly()
        {
            flyBehavior.fly();
        }

        public void performQuack()
        {
            quackBehavior.quack();
        }

        public void swim()
        {
            Console.WriteLine("오리가 수영한다");
        }
    }
    public class MallardDuck : Duck
    {
        public MallardDuck()
        {
            flyBehavior = new FlyWithWings();
            quackBehavior = new MuteQuack();
        }

        public override void display()
        {
            Console.WriteLine("오리가 나타났다!");
        }
    }
    public class Program
    {
        static void Main(string[] args)
        {
            MallardDuck mallardDuck = new MallardDuck();
            mallardDuck.performFly();
            mallardDuck.performQuack();

            mallardDuck.flyBehavior = new FlyNoWay();
            mallardDuck.quackBehavior = new Quack();

            mallardDuck.performFly();
            mallardDuck.performQuack();

            mallardDuck.swim();
            mallardDuck.display();
        }
    }
}
