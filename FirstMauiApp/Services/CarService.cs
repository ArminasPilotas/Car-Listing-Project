using FirstMauiApp.Models;
using SQLite;

namespace FirstMauiApp.Services
{
    public class CarService
    {
        SQLiteConnection conn;
        string _dbPath;
        public string StatusMessage;


        public CarService(string dbPath)
        {
            _dbPath = dbPath;
        }

        private void Init()
        {
            if (conn is not null) return;

            conn = new SQLiteConnection(_dbPath);
            conn.CreateTable<Car>();
        }

        public Car GetCar(int id)
        {
            try
            {
                Init();
                return conn.Table<Car>().FirstOrDefault(x => x.Id == id);
            }
            catch (Exception)
            {
                StatusMessage = "Failed to retrieve data.";
            }

            return null;
        }

        public List<Car> GetCars()
        {
            try
            {
                Init();
                return conn.Table<Car>().ToList();
            }
            catch (Exception)
            {
                StatusMessage = "Failed to retrieve data.";
            }

            return new List<Car>();
        }
        
        public void AddCar(Car car)
        {
            try
            {
                Init();

                if (car is null)
                {
                    throw new Exception("Invalid Car Record.");
                }

                var result = conn.Insert(car);
                StatusMessage = result == 0 ? "Insert Failed" : "Insert Successful";
            }
            catch (Exception)
            {
                StatusMessage = "Failed to Insert data.";
            }
        }

        public int DeleteCar(int id)
        {
            try
            {
                Init();
                return conn.Table<Car>().Delete(x => x.Id == id);
            }
            catch (Exception)
            {
                StatusMessage = "Failed to delete data.";
            }

            return default;
        }

        public void UpdateCar(Car car)
        {
            try
            {
                Init();

                if (car is null)
                {
                    throw new Exception("Invalid Car Record");
                }

                var result = conn.Update(car);
                StatusMessage = result == 0 ? "Update Failed" : "Update Successful";
            }
            catch (Exception)
            {
                StatusMessage = "Failed to Update data.";
            }
        }
    }
}
