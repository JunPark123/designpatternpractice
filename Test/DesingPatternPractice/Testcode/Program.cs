using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace FaSequenceDemo
{
    // 상태 결과
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

    // 명령 인터페이스
    public interface IDeviceCommand
    {
        Task<CommandResult> ExecuteAsync();
    }

    // 장치 제어 명령들
    public class MotorStartCommand : IDeviceCommand
    {
        public async Task<CommandResult> ExecuteAsync()
        {
            Console.WriteLine("모터 시작...");
            await Task.Delay(300); // 시뮬레이션
            return new CommandResult { Status = CommandResultStatus.Success, Message = "모터가 정상적으로 시작됨" };
        }
    }

    public class WaitSensorCommand : IDeviceCommand
    {
        public async Task<CommandResult> ExecuteAsync()
        {
            if (!CheckInterlock())
            {
                return new CommandResult
                {
                    Status = CommandResultStatus.InterlockFailed,
                    Message = "인터록 조건 미충족"
                };
            }

            int retry = 0;
            int maxRetry = 3;

            while (retry < maxRetry)
            {
                Console.WriteLine($"센서 확인 시도 {retry + 1}");
                if (CheckSensor())
                {
                    return new CommandResult { Status = CommandResultStatus.Success, Message = "센서 감지됨" };
                }

                retry++;
                await Task.Delay(500);
            }

            return new CommandResult { Status = CommandResultStatus.Timeout, Message = "센서 감지 실패 (타임아웃)" };
        }

        private bool CheckInterlock() => true;
        private bool CheckSensor() => false; // 실패하도록 설정
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

    // Executor
    public class DeviceExecutor
    {
        public async Task<bool> ExecuteAsync(IDeviceCommand command)
        {
            var result = await command.ExecuteAsync();
            Console.WriteLine($"[결과] {result.Status} - {result.Message}");

            return result.Status == CommandResultStatus.Success;
        }
    }

    // 상태 인터페이스
    public interface ISequenceState
    {
        Task Handle(MachineContext context);
    }

    // 상태 컨텍스트
    public class MachineContext
    {
        public ISequenceState CurrentState { get; private set; }
        public DeviceExecutor DeviceExecutor { get; } = new();

        public void SetState(ISequenceState state)
        {
            CurrentState = state;
        }

        public async Task Execute()
        {
            count++;

            if (CurrentState is not null)
            {
                await CurrentState.Handle(this);
            }
            Debug.WriteLine($"{count} 번 실행");
        }
        public static int count = 0;
    }


    // 상태들
    public class RunState : ISequenceState
    {
        public async Task Handle(MachineContext context)
        {
            Console.WriteLine("[Run] 상태 진입");

            var executor = context.DeviceExecutor;

            bool ok = await executor.ExecuteAsync(new MotorStartCommand());
            if (!ok) { context.SetState(new FailState("모터 시작 실패")); return; }

            ok = await executor.ExecuteAsync(new WaitSensorCommand());
            if (!ok) { context.SetState(new FailState("센서 감지 실패")); return; }

            ok = await executor.ExecuteAsync(new StopMotorCommand());
            if (!ok) { context.SetState(new FailState("모터 정지 실패")); return; }

            context.SetState(new FinishState());
        }
    }

    public class FinishState : ISequenceState
    {
        public async Task Handle(MachineContext context)
        {
            Console.WriteLine("[Finish] 시퀀스 완료");
        }
    }

    public class FailState : ISequenceState
    {
        private readonly string _reason;

        public FailState(string reason)
        {
            _reason = reason;
        }

        public async Task Handle(MachineContext context)
        {
            Console.WriteLine($"[Fail] 시퀀스 실패: {_reason}");
        }
    }

    // Program 진입점
    class Program
    {
        static async Task Main(string[] args)
        {
            var context = new MachineContext();
            context.SetState(new RunState());

            while (context.CurrentState is not FinishState && context.CurrentState is not FailState)
            {
                await context.Execute();
                await Task.Delay(200); // 상태 처리 간 대기
            }

            Console.WriteLine("종료됨.");
        }
    }
}
