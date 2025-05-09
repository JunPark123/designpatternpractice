namespace Singleton
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }

    public class SingletonSample
    {
        private static readonly SingletonSample _instance = new SingletonSample();

        public static SingletonSample Instance => _instance;
       private SingletonSample() { } // 외부에서 생성자 호출 불가

    }

}
