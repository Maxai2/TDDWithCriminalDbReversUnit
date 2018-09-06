using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDD
{
    public class Criminal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Code { get; set; }
        public string Crime { get; set; }
        public bool Jailed { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int DaysTerm { get; set; }
    }
}
