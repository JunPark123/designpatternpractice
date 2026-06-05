namespace 헤드퍼스트_2장_옵저버패턴
{
    public interface Subject
    {
        void RegisterObserver(Observer ob);
        void RemoveObserver(Observer ob);
        void NotifyObservers();
    }

    public interface Observer
    {
        void Update(float temp, float humidity, float pressure); //push방식
        void Update();//pull방식
    }

    public interface DisplayElement
    {
        void Display();
    }

    public class WeatherData : Subject
    {
        private List<Observer> _observers;
        public float _temperature { get; private set; }
        public float _pressure { get; private set; }
        public float _humidity { get; private set; }
        public WeatherData()
        {
            _observers = new List<Observer>();
        }


        public void RegisterObserver(Observer ob)
        {
            _observers.Add(ob);
        }

        public void RemoveObserver(Observer ob)
        {
            _observers.Remove(ob);
        }

        public void NotifyObservers()
        {
            foreach (Observer ob in _observers)
            {
                //ob.Update(_temperature, _pressure, _humidity);
                ob.Update();
            }
        }
        public void MeasurementsChanged()
        {
            NotifyObservers();
        }

        public void SetMeasurement(float temperature, float pressure, float humidity)
        {
            _temperature = temperature;
            _pressure = pressure;
            _humidity = humidity;
            MeasurementsChanged();
        }
    }


    public class CurrentContionDisplay : Observer, DisplayElement
    {
        private float _temperature;
        private float _humidity;
        private WeatherData _data;
        public CurrentContionDisplay(WeatherData weather)
        {
            this._data = weather;
            _data.RegisterObserver(this);
        }
        public void Update(float temp, float humidity, float pressure)
        {
            this._temperature = temp;
            this._humidity = humidity;
            Display();
        }
        public void Update()
        {
            this._temperature = _data._temperature;
            this._humidity = _data._humidity;
            Display();
        }

        public void Display()
        {
            Console.WriteLine(
                $"현재 상태: 온도 {_temperature}" +
                $"F,습도 : {_humidity}%");
        }

    }
    public class ForecastDisplay : Observer, DisplayElement
    {
        private float currentPressure = 29.92f;
        private float lastPressure;
        private WeatherData weatherData;

        public ForecastDisplay(WeatherData weather)
        {
            this.weatherData = weather;
            weatherData.RegisterObserver(this);
        }
        public void Update(float temp, float humidity, float pressure)
        {
            lastPressure = currentPressure;
            currentPressure = pressure;

            Display();
        }
        public void Update()
        {
            lastPressure = currentPressure;
            currentPressure = weatherData._pressure;
            Display();
        }

        public void Display()
        {
            Console.WriteLine("Forecast: ");
            if (currentPressure > lastPressure)
            {
                Console.WriteLine("Improving weather on the way!");
            }
            else if (currentPressure == lastPressure)
            {
                Console.WriteLine("More of the same");
            }
            else if (currentPressure < lastPressure)
            {
                Console.WriteLine("Watch out for cooler, rainy weather");
            }
        }
    }
    public class StatisticsDisplay : Observer, DisplayElement
    {
        private float maxTemp = 0.0f;
        private float minTemp = 200;
        private float tempSum = 0.0f;
        private int numReadings;
        private WeatherData weatherData;

        public StatisticsDisplay(WeatherData weatherData)
        {
            this.weatherData = weatherData;
            weatherData.RegisterObserver(this);
        }

        public void Update(float temp, float humidity, float pressure)
        {
            tempSum += temp;
            numReadings++;

            if (temp > maxTemp)
            {
                maxTemp = temp;
            }

            if (temp < minTemp)
            {
                minTemp = temp;
            }

            Display();
        }
        public void Update()
        {
            tempSum += weatherData._temperature;
            numReadings++;
            float temp = weatherData._temperature;

            if (temp > maxTemp)
            {
                maxTemp = temp;
            }

            if (temp < minTemp)
            {
                minTemp = temp;
            }

            Display();
        }
        public void Display()
        {
            Console.WriteLine(
                "Avg/Max/Min temperature = " + (tempSum / numReadings) + "/" + maxTemp + "/" + minTemp)
                ;
        }
    }
    public class Program
    {
        static void Main(string[] args)
        {
            WeatherData weatherData = new WeatherData();

            CurrentContionDisplay currentContionDisplay = new CurrentContionDisplay(weatherData);
            StatisticsDisplay statisticsDisplay = new StatisticsDisplay(weatherData);
            ForecastDisplay forecastDisplay = new ForecastDisplay(weatherData);

            weatherData.SetMeasurement(80, 64, 30.4f);
            weatherData.SetMeasurement(82, 60, 35.4f);
            weatherData.SetMeasurement(70, 64, 31.4f);


        }
    }
}
