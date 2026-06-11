using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 헤드퍼스트_5장_싱글턴패턴
{
    public class ChocolateBoiler
    {
        private bool empty;
        private bool boiled;

        private ChocolateBoiler()
        {
            empty = true;
            boiled = false;
        }

        public void fill()
        {
            if (IsEmpty())
            {
                empty = false;
                boiled = false;
            }
        }

        public void drain()
        {
            if (!IsEmpty() && IsBoiled())
            {
                empty = true;
            }
        }

        public void boil()
        {
            if (!IsEmpty() && !IsBoiled())
            {
                boiled = true;
            }
        }

        public bool IsEmpty() => empty;
        public bool IsBoiled() => boiled;
    }
}
