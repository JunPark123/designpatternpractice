using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 헤드퍼스트_6장_커맨드패턴
{
    public interface Command
    {
        void Execute();
    }

    public class LightOnCommand : Command
    {
        private Light light;
        public LightOnCommand(Light light)
        {
            this.light = light;
        }

        public void Execute()
        {
            light.LightOn();
        }
    }
    public class LightOffCommand : Command
    {
        private Light light;
        public LightOffCommand(Light light)
        {
            this.light = light;
        }

        public void Execute()
        {
            light.LightOff();
        }
    }
    public class Light
    {
        public void LightOn() { Console.WriteLine("조명 켜짐"); }
        public void LightOff() { Console.WriteLine("조명 꺼짐"); }
    }

    public class SimpleRemote
    {
        public Command cmd;

        public void SetCommand(Command cmd)
        {
            this.cmd = cmd;
        }

        public void buttonWasPressed()
        {
            cmd.Execute();
        }

    }
}
