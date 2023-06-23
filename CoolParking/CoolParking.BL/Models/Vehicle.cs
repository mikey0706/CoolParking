// TODO: implement class Vehicle.
//       Properties: Id (string), VehicleType (VehicleType), Balance (decimal).
//       The format of the identifier is explained in the description of the home task.
//       Id and VehicleType should not be able for changing.
//       The Balance should be able to change only in the CoolParking.BL project.
//       The type of constructor is shown in the tests and the constructor should have a validation, which also is clear from the tests.
//       Static method GenerateRandomRegistrationPlateNumber should return a randomly generated unique identifier.
using Fare;
using System;
using System.Text.RegularExpressions;

namespace CoolParking.BL.Models
{
    public class Vehicle
    {
        public readonly string Id;
        public readonly VehicleType Type;
        public decimal Balance { get; set; }
        private static readonly string _expression = @"[\w]([A-Z1-2])-\d{4}-[\w]([A-Z1-2])";

        public Vehicle(string id, VehicleType type, decimal balance)
        {
            Regex regex = new Regex(_expression);
            if (!regex.IsMatch(id) || balance < 0)
            {
                throw new ArgumentException();
            }
            Id = id;
            Type = type;
            Balance = balance;
        }

        public static string GenerateRandomRegistrationPlateNumber() 
        {
        Xeger xEger = new Xeger(_expression);
        return xEger.Generate();
        }

    }
}