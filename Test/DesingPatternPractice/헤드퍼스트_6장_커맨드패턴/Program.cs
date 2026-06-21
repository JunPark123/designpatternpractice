namespace 헤드퍼스트_6장_커맨드패턴
{

    internal class Program
    {
        static void Main(string[] args)
        {
            SimpleRemote simpleRemote = new SimpleRemote();
            Command cmd = new LightOnCommand(new Light());


            simpleRemote.SetCommand(cmd);
            simpleRemote.buttonWasPressed();


        }
    }
}
