using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UP_11
{
    partial class Agents
    {
        public byte[] LogoTip
        {
            get
            {

                if (File.Exists($"{LogoAgent}"))
                    return File.ReadAllBytes($"{LogoAgent.Trim()}");
                return null;
            }
        }
        public string CountRealization
        {
            get
            {
                int sum = ProductSales.Sum(s => s.Count);

                return sum.ToString() + " продаж за год";
            }
        }
        public string TypeNameAgents
        {
            get
            {
                return $"{TypeAgent.NameTypeAgent} | {NameAgent}";
            }
        }
        public string PriorityZnach
        {
            get
            {
                return $"Приоритетность: {Priority}";

            }
        }
        public string Skidka
        {
            get
            {
                decimal sum = (decimal)ProductSales.Sum(s => s.Count * s.Product.MinPrice);
                if (sum < 10000) { return "Скидки нет"; }
                if (sum >= 10000 && sum < 50000) { return "5%"; }
                if (sum >= 50000 && sum < 150000) { return "10%"; }
                if (sum >= 150000 && sum < 500000) { return "20%"; }
                return "25%";
            }
        }
        public string AType
        {
            get
            {
                return $"{TypeAgent.NameTypeAgent}";
            }
        }
        public int YearSales
        {
            get
            {
                int sum = ProductSales.Sum(s => s.Count);

                return sum;
            }
        }
    }
}
