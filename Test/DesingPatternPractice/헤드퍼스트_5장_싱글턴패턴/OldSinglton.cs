using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#pragma warning disable

namespace 헤드퍼스트_5장_싱글턴패턴
{
    public class OldSinglton
    {
        private static OldSinglton Instance;

        private OldSinglton() { }

        public static OldSinglton GetInstance()
        {
            if (Instance is null)
            {
                Instance = new OldSinglton();
            }

            return Instance;
        }
    }
}
