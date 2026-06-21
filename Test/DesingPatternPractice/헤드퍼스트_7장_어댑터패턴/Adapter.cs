using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 헤드퍼스트_7장_어댑터패턴
{
    //타깃 인터페이스
    public interface Duck
    {
        void Quack();
        void Fly();
    }

    //어댑티
    public interface Turkey
    {
        void Gobbule();//스펠링 모르겠는데 골골
        void Fly();
    }

    //어댑터
    public class DuckAdapter : Duck
    {
        Turkey _turkey;
        public DuckAdapter(Turkey turkey)
        {
            _turkey = turkey;
        }
        public void Fly()
        {
            for (int i = 0; i < 5; i++)
            {
                _turkey.Fly();
            }
        }

        public void Quack()
        {
            for (int i = 0; i < 5; i++)
            {
                _turkey.Gobbule();
            }
        }
    }

    public class WildTurkey : Turkey
    {
        public void Fly()
        {

            Console.WriteLine("칠면조 날개짓");


        }

        public void Gobbule()
        {
            Console.WriteLine("골골");
        }
    }
    public class MilladDuck : Duck
    {

        public void Quack()
        {
            Console.WriteLine("꽥꽥");
        }
        public void Fly()
        {
            Console.WriteLine("오리 날다");
        }


    }
}
