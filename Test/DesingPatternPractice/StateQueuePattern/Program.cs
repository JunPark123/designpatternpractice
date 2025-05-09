using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaSequenceQueueDemo
{
    // ====== [명령 실행 결과 정의] ======

    public enum CommandResultStatus
    {
        Success,
        Failed,
        InterlockFailed,
        Timeout
    }

    public class CommandResult
    {
        public CommandResultStatus Status { get; set; }
        public string Message { get; set; } = "";
    }

    // ====== [명령 인터페이스 및 명령 구현] ======

    public interface IDeviceCommand
    {
        Task<CommandResult> ExecuteAsync();
    }

    public class MotorStartCommand : IDeviceCommand
    {
        public async Task<CommandResult> ExecuteAsync()
        {
            Console.WriteLine("모터 시작...");
            await Task.Delay(300);
            return new CommandResult { Status = CommandResultStatus.Success, Message = "모터가 정상적으로 시작됨" };
        }
    }

    public class WaitSensorCommand : IDeviceCommand
    {
        public async Task<CommandResult> ExecuteAsync()
        {
            Console.WriteLine("센서 상태 확인 중...");
            await Task.Delay(500);
            bool success = false; // 실패 테스트용

            return new CommandResult
            {
                Status = success ? CommandResultStatus.Success : CommandResultStatus.Timeout,
                Message = success ? "센서 감지됨" : "센서 감지 실패"
            };
        }
    }

    public class StopMotorCommand : IDeviceCommand
    {
        public async Task<CommandResult> ExecuteAsync()
        {
            Console.WriteLine("모터 정지...");
            await Task.Delay(200);
            return new CommandResult { Status = CommandResultStatus.Success, Message = "모터 정지 완료" };
        }
    }

    // ====== [장치 명령 실행기] ======

    public class DeviceExecutor
    {
        public async Task<bool> ExecuteAsync(IDeviceCommand command)
        {
            var result = await command.ExecuteAsync();
            Console.WriteLine($"[결과] {result.Status} - {result.Message}");

            return result.Status == CommandResultStatus.Success;
        }
    }

    // ====== [상태 인터페이스 및 상태 구현] ======

    public interface ISequenceState
    {
        Task<CommandResultStatus> HandleAsync(MachineContext context);
    }

    public class MotorStartState : ISequenceState
    {
        public async Task<CommandResultStatus> HandleAsync(MachineContext context)
        {
            Console.WriteLine("[상태] 모터 시작");
            bool ok = await context.DeviceExecutor.ExecuteAsync(new MotorStartCommand());
            return ok ? CommandResultStatus.Success : CommandResultStatus.Failed;
        }
    }

    public class WaitSensorState : ISequenceState
    {
        public async Task<CommandResultStatus> HandleAsync(MachineContext context)
        {
            Console.WriteLine("[상태] 센서 대기");
            bool ok = await context.DeviceExecutor.ExecuteAsync(new WaitSensorCommand());
            return ok ? CommandResultStatus.Success : CommandResultStatus.Timeout;
        }
    }

    public class StopMotorState : ISequenceState
    {
        public async Task<CommandResultStatus> HandleAsync(MachineContext context)
        {
            Console.WriteLine("[상태] 모터 정지");
            bool ok = await context.DeviceExecutor.ExecuteAsync(new StopMotorCommand());
            return ok ? CommandResultStatus.Success : CommandResultStatus.Failed;
        }
    }

    // ====== [컨텍스트 및 상태 큐 관리] ======

    public class MachineContext
    {
        private readonly Queue<ISequenceState> _stateQueue = new();
        public DeviceExecutor DeviceExecutor { get; } = new();

        public void EnqueueState(ISequenceState state)
        {
            _stateQueue.Enqueue(state);
        }

        public async Task<bool> ExecuteNextAsync()
        {
            if (_stateQueue.Count == 0)
                return false;

            var state = _stateQueue.Dequeue();
            var result = await state.HandleAsync(this);

            if (result != CommandResultStatus.Success)
            {
                Console.WriteLine($"[중단] {state.GetType().Name} 실패: {result}");
                return false;
            }

            return true;
        }

        public bool HasMoreStates => _stateQueue.Count > 0;
        public int Count => _stateQueue.Count;
    }

    // ====== [Main 프로그램] ======

    class Program
    {
        static async Task Main(string[] args)
        {
            bool isStart = false;
            var context = new MachineContext();

            // 상태 큐 정의
            //context.EnqueueState(new MotorStartState());
            //context.EnqueueState(new WaitSensorState());
            //context.EnqueueState(new StopMotorState());

            while (true)
            {
                Console.WriteLine("상태 큐에 추가할 작업 목록 선택:");
                Console.WriteLine("1.MotorStartState 2.WaitSensorState 3.StopMotorState 4.작업 시작 5. 종료");
                string? input = Console.ReadLine()?.Trim();
                if (input?.ToLower() == "5" || input?.ToLower() == "exit")
                {
                    Console.WriteLine("프로세스 종료");
                    break;
                }

                switch (input)
                {
                    case "1":
                        context.EnqueueState(new MotorStartState());
                        break;
                    case "2":
                        context.EnqueueState(new WaitSensorState());
                        break;
                    case "3":
                        context.EnqueueState(new StopMotorState());
                        break;
                    case "4":
                        isStart = true;
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                };

                Console.WriteLine($"작업 목록 총 갯수는 {context.Count}개 입니다");

                if (isStart)
                {
                    while (context.HasMoreStates)
                    {
                        bool success = await context.ExecuteNextAsync();
                        if (!success)
                        {
                            Console.WriteLine("시퀀스 실패로 중단됨.");
                            return;
                        }

                        await Task.Delay(100); // 상태 간 딜레이
                    }

                    Console.WriteLine(" 시퀀스 정상 완료");
                    isStart = false;
                }
            }
        }
    }
}
