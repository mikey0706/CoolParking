// TODO: implement the ParkingService class from the IParkingService interface.
//       For try to add a vehicle on full parking InvalidOperationException should be thrown.
//       For try to remove vehicle with a negative balance (debt) InvalidOperationException should be thrown.
//       Other validation rules and constructor format went from tests.
//       Other implementation details are up to you, they just have to match the interface requirements
//       and tests, for example, in ParkingServiceTests you can find the necessary constructor format and validation rules.
using CoolParking.BL.Interfaces;
using CoolParking.BL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;

namespace CoolParking.BL.Services
{
    public class ParkingService : IParkingService
    {
        private ILogService _logService;
        private ITimerService _withdrawTimer;
        private ITimerService _loggTimer;
        private int tIndex = 0;
        private Parking _parking = Parking.GetInstance;
        private List<TransactionInfo> _transactions = new List<TransactionInfo>();


        public ParkingService(ITimerService withdrawTimer, ITimerService loggTimer, ILogService logService)
        {
            _withdrawTimer = withdrawTimer;
            _loggTimer = loggTimer;
            _logService = logService;

            _withdrawTimer.Elapsed += new ElapsedEventHandler(OnPaymentEvent);
            _withdrawTimer.Interval = Settings.PaymentPeriod;
            _withdrawTimer.Start();

            _loggTimer.Elapsed += new ElapsedEventHandler(OnTransactionsLog);
            _loggTimer.Interval = Settings.LoggingPeriod;
            _loggTimer.Start();
        }

        public void AddVehicle(Vehicle vehicle)
        {
            var vehicles = _parking.Vehicles;
            if (GetFreePlaces() >= 0)
            {
                if (vehicles.Find(v => v.Id.Equals(vehicle.Id)) == null)
                {
                    _parking.Vehicles.Add(vehicle);

                }
                else
                {
                    throw new ArgumentException();
                }
            }
            else
            {
                throw new InvalidOperationException();
            }

        }

        public void Dispose()
        {
            _withdrawTimer.Stop();
            _loggTimer.Stop();
        }

        public decimal GetBalance()
        {
            return _parking.Balance;
        }

        public int GetCapacity()
        {
            return Settings.ParkingCapacity;
        }

        public int GetFreePlaces()
        {
            int capacity = Settings.ParkingCapacity;
            if (_parking.Vehicles.Count() > 0)
            {
                capacity -= _parking.Vehicles.Count;
            }
            return capacity;
        }

        public TransactionInfo[] GetLastParkingTransactions()
        {
            int cnt = 0;
            TransactionInfo[] output = new TransactionInfo[_transactions.Count()];

            foreach (var item in _transactions)
            {

                output[cnt++] = item;
            }

            return output;
        }

        public ReadOnlyCollection<Vehicle> GetVehicles()
        {
            return new ReadOnlyCollection<Vehicle>(_parking.Vehicles);
        }

        public string ReadFromLog()
        {
            return _logService.Read();
        }

        public void RemoveVehicle(string vehicleId)
        {
            var vehicle = _parking.Vehicles.FirstOrDefault(v => v.Id == vehicleId);
            if (vehicle == null)
            {
                throw new ArgumentException();
            }
            _parking.Vehicles.Remove(vehicle);
        }

        public void TopUpVehicle(string vehicleId, decimal sum)
        {
            var vehicle = _parking.Vehicles.FirstOrDefault(v => v.Id == vehicleId);
            if (sum < 0 || vehicle == null)
            {
                throw new ArgumentException();
            }
            vehicle.Balance = decimal.Add(vehicle.Balance, sum);
        }

        private void OnPaymentEvent(object sender, ElapsedEventArgs e)
        {

            foreach (var car in _parking.Vehicles)
            {
                var tarif = Settings.Tariffs.FirstOrDefault(t => t.Item1.Equals(car.Type)).Item2;

                if (tarif <= car.Balance)
                {
                    car.Balance -= tarif;

                }
                else
                if (car.Balance < tarif && car.Balance > 0)
                {
                    var diff = tarif - car.Balance;
                    tarif = car.Balance + diff * Settings.PenaltyCoefficient;
                    car.Balance -= tarif;
                }
                else
                {
                    car.Balance -= tarif * Settings.PenaltyCoefficient;
                }

                _parking.Balance += tarif;
                _transactions.Add(new TransactionInfo { TarnsactionTime = DateTime.Now, VehicleId = car.Id, Sum = tarif });
            }
        }

        private void OnTransactionsLog(object sender, ElapsedEventArgs e)
        {
            for (; tIndex < _transactions.Count; tIndex++)
            {
                _logService.Write($"{_transactions[tIndex].TarnsactionTime}/{_transactions[tIndex].VehicleId}/{_transactions[tIndex].Sum}|");
            }
        }
    }
}