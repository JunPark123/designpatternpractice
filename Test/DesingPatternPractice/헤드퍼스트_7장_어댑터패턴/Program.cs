namespace 헤드퍼스트_7장_어댑터패턴
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Duck mduck = new MilladDuck();
            Console.WriteLine("오리");
            mduck.Quack();
            mduck.Fly();
            Console.WriteLine();
            Turkey wturkey = new WildTurkey();
            Console.WriteLine("칠면조");
            wturkey.Gobbule();
            wturkey.Fly();
            Console.WriteLine();

            Turkey aturkey = new WildTurkey();
            Console.WriteLine("칠면조 어댑터");
            Duck duckadpater = new DuckAdapter(aturkey);
            duckadpater.Quack();
            duckadpater.Fly();



        }
    }
}
