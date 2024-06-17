using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ClassLibrary
{
    public enum ClientType
    {
        ManHaircut,
        ManColoring,
        WomanColoring,
        WomanHaircut,
        WomanStyling,
        ChildHaircut,
        ChildColoring
    }

    public class Hairdresser : ICloneable, IComparable<Hairdresser>
    {
        private string firstName;
        private string lastName;

        public Hairdresser(string firstName, string lastName)
        {
            this.firstName = firstName;
            this.lastName = lastName;
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public object Clone()
        {
            return new Hairdresser(firstName, lastName);
        }

        public int CompareTo(Hairdresser other)
        {
            return string.Compare(lastName, other.lastName, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Hairdresser other = (Hairdresser)obj;
            return string.Equals(firstName, other.firstName, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(lastName, other.lastName, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            unchecked 
            {
                int hash = 17;
                hash = hash * 31 + (firstName?.ToLowerInvariant().GetHashCode() ?? 0);
                hash = hash * 31 + (lastName?.ToLowerInvariant().GetHashCode() ?? 0);
                return hash;
            }
        }

        public override string ToString()
        {
            return $"{firstName} {lastName}";
        }
    }

    public class HairdresserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class Hairdo : ICloneable, IComparable<Hairdo>
    {
        private string description;
        private ClientType clientType;
        private Hairdresser hairdresser;
        private int costOfServices;
        private bool isNeedAdditionalServices;

        public Hairdo(string description, ClientType clientType, Hairdresser hairdresser, int costOfServices, bool isNeedAdditionalServices)
        {
            this.description = description;
            this.clientType = clientType;
            this.hairdresser = hairdresser;
            this.costOfServices = costOfServices;
            this.isNeedAdditionalServices = isNeedAdditionalServices;
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public ClientType ClientType
        {
            get { return clientType; }
            set { clientType = value; }
        }

        public Hairdresser Hairdresser
        {
            get { return hairdresser; }
            set { hairdresser = value; }
        }

        public int CostOfServices
        {
            get { return costOfServices; }
            set { costOfServices = value; }
        }

        public bool IsNeedAdditionalServices
        {
            get { return isNeedAdditionalServices; }
            set { isNeedAdditionalServices = value; }
        }

        public int TotalCost
        {
            get
            {
                if (isNeedAdditionalServices)
                {
                    return costOfServices + (int)Barbershop.AdditionalServiceCost;
                }
                else
                {
                    return costOfServices;
                }
            }
        }

        public object Clone()
        {
            return new Hairdo(description, clientType, hairdresser, costOfServices, isNeedAdditionalServices);
        }

        public int CompareTo(Hairdo other)
        {
            return TotalCost.CompareTo(other.TotalCost);
        }

        public override bool Equals(object obj)
        {
            return obj is Hairdo hairdo &&
                   description == hairdo.description &&
                   clientType == hairdo.clientType &&
                   EqualityComparer<Hairdresser>.Default.Equals(hairdresser, hairdo.hairdresser) &&
                   costOfServices == hairdo.costOfServices &&
                   isNeedAdditionalServices == hairdo.isNeedAdditionalServices;
        }

        public override int GetHashCode()
        {
            int hashCode = 17;
            hashCode = hashCode * 31 + EqualityComparer<string>.Default.GetHashCode(description);
            hashCode = hashCode * 31 + clientType.GetHashCode();
            hashCode = hashCode * 31 + EqualityComparer<Hairdresser>.Default.GetHashCode(hairdresser);
            hashCode = hashCode * 31 + costOfServices.GetHashCode();
            hashCode = hashCode * 31 + isNeedAdditionalServices.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            string additionalServices = isNeedAdditionalServices ? "Yes" : "No";
            return $"{description}, Type: {clientType}, Hairdresser: {hairdresser}, Cost: {TotalCost:C}, Additional Services: {additionalServices}";
        }
    }

    public class HairdoDTO
    {
        public string Description { get; set; }
        public ClientType ClientType { get; set; }
        public HairdresserDTO Hairdresser { get; set; }
        public int CostOfServices { get; set; }
        public bool IsNeedAdditionalServices { get; set; }
    }

    public class Barbershop : ICloneable, IComparable<Barbershop>
    {
        private int barbershopNumber;
        private DateTime currentDate;

        public static decimal AdditionalServiceCost = 50.0m;

        private List<Hairdo> completedHairstyles;

        public int BarbershopNumber
        {
            get { return barbershopNumber; }
            set { barbershopNumber = value; }
        }

        public DateTime CurrentDate
        {
            get { return currentDate; }
            set { currentDate = value; }
        }

        public List<Hairdo> CompletedHairstyles
        {
            get { return completedHairstyles; }
            set { completedHairstyles = value; }
        }

        public Barbershop(int number, DateTime currentDate)
        {
            this.barbershopNumber = number;
            this.currentDate = currentDate;
            this.completedHairstyles = new List<Hairdo>();
        }

        public void AddHairdo(Hairdo hairdo)
        {
            completedHairstyles.Add(hairdo);
        }

        public object Clone()
        {
            Barbershop clonedOrder = new Barbershop(barbershopNumber, currentDate);
            foreach (Hairdo dish in completedHairstyles)
            {
                clonedOrder.AddHairdo((Hairdo)dish.Clone());
            }
            return clonedOrder;
        }

        public int CompareTo(Barbershop other)
        {
            return currentDate.CompareTo(other.currentDate);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Barbershop other = (Barbershop)obj;
            return string.Equals(barbershopNumber, other.barbershopNumber) &&
                   currentDate.Equals(other.currentDate) &&
                   completedHairstyles.Count == other.completedHairstyles.Count &&
                   completedHairstyles.TrueForAll(d => other.completedHairstyles.Contains(d));
        }

        public override int GetHashCode()
        {
            int hashCode = 17;
            hashCode = hashCode * 31 + barbershopNumber.GetHashCode();
            hashCode = hashCode * 31 + currentDate.GetHashCode();
            foreach (var hairdo in completedHairstyles)
            {
                hashCode = hashCode * 31 + hairdo.GetHashCode();
            }
            return hashCode;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Barbershop Number: {barbershopNumber}");
            sb.AppendLine($"Current Date: {currentDate.ToShortDateString()}");
            sb.AppendLine($"Completed Hairstyles:");
            foreach (var hairdo in completedHairstyles)
            {
                sb.AppendLine(hairdo.ToString());
            }
            return sb.ToString();
        }

        public string ToShortString()
        {
            decimal totalCost = 0.0m;
            foreach (var hairdo in completedHairstyles)
            {
                totalCost += hairdo.TotalCost;
            }

            return $"Barbershop Number: {barbershopNumber}, Date: {currentDate.ToShortDateString()}, Total Cost: {totalCost:C}";
        }
    }

    public class BarbershopDTO
    {
        public int BarbershopNumber { get; set; }
        public DateTime CurrentDate { get; set; }
        public List<HairdoDTO> CompletedHairstyles { get; set; }
    }
}
