using Microsoft.Extensions.DependencyInjection;

namespace StrategyAndDI
{
    internal class Program
    {
        //컨셉 
        //고객마다 커스텀 옵션을 적용해야 할 때 전략 패턴과 의존성 주입을 사용하여 해결 가능 (현재 구현됨)
   
        //Microsoft.Extensions.DependencyInjection; 패키지 추가

        //대형 프로젝트시에는 팩토리 패턴과 DI 를 조합해서 사용 한다고 함.

        static void Main(string[] args)
        {
            // DI 컨테이너 설정
            var services = new ServiceCollection();

            // 고객별 전략 등록
            services.AddTransient<ICustomerStrategy, DefaultStrategy>(); // 기본 전략
            services.AddTransient<CustomerAStrategy>();
            services.AddTransient<CustomerBStrategy>();
            services.AddTransient<CustomerCStrategy>();
            services.AddTransient<DefaultStrategy>();

            services.AddTransient<CustomerStrategySelector>(); // 전략 선택기 등록

            var serviceProvider = services.BuildServiceProvider();

            // 고객 ID 입력
            Console.Write("Enter Customer ID: ");
            string customerID = Console.ReadLine() ?? "";

            // DI 컨테이너에서 전략 선택기 가져오기
            var selector = serviceProvider.GetService<CustomerStrategySelector>();
            if (selector == null)
            {
                PrintError("Error: CustomerStrategySelector could not be resolved from DI container.");
                return;
            }

            // 고객 ID에 따라 적절한 전략 선택 및 실행
            var strategy = selector.GetStrategy(customerID);

            if (strategy is null)
            {
                PrintError("Error: Strategy could not be resolved.");
                return;
            }

            strategy.ApplySettings();

            Console.WriteLine("Program finished.");
        }
        static void PrintError(string message)
        {
            Console.BackgroundColor = ConsoleColor.Red;   
            Console.ForegroundColor = ConsoleColor.White; 
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }

    //고객옵션이 다른 경우 
    public interface ICustomerStrategy
    {
        void ApplySettings();
    }

    public class DefaultStrategy : ICustomerStrategy
    {
        public void ApplySettings()
        {
            Console.WriteLine("기본 세팅이 적용되었습니다.");
        }
    }

    public class CustomerAStrategy : ICustomerStrategy
    {
        public void ApplySettings()
        {
            Console.WriteLine("고객A 옵션이 적용되었습니다.");
        }
    }
    public class CustomerBStrategy : ICustomerStrategy
    {
        public void ApplySettings()
        {
            Console.WriteLine("고객B 옵션이 적용되었습니다.");
        }
    }
    public class CustomerCStrategy : ICustomerStrategy
    {
        public void ApplySettings()
        {
            Console.WriteLine("고객C 옵션이 적용되었습니다.");
        }
    }

    public class CustomerStrategySelector
    {
        private readonly IServiceProvider _serviceProvider;

        public CustomerStrategySelector(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ICustomerStrategy? GetStrategy(string customer)
        {
            return customer.ToLower() switch
            {
                "customera" => _serviceProvider.GetService<CustomerAStrategy>(),
                "customerb" => _serviceProvider.GetService<CustomerBStrategy>(),
                "customerc" => _serviceProvider.GetService<CustomerCStrategy>(),
                _ => _serviceProvider.GetService<DefaultStrategy>()
            };
        }
    }
}
