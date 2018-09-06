using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDD
{
    public class CriminalDb
    {
        List<Criminal> criminals = new List<Criminal>();

        public void Imprison(Criminal criminal, int term)
        {
            if (criminal is null)
            {
                throw new ArgumentNullException(nameof(criminal));
            }

            if (DateTime.Now.Subtract(criminal.DateOfBirth).TotalDays/365 <= 8)
            {
                throw new ArgumentOutOfRangeException($"{nameof(criminal.DateOfBirth)}", "criminal should be older than 8 y/0");
            }

            if (term <= 0 || term > 5000)
            {
                var msg = $"{nameof(term)} should be in [1; 5000]";
                throw new ArgumentOutOfRangeException(nameof(term), msg);
            }

            criminal.DaysTerm = term;
            criminal.Jailed = true;
            criminals.Add(criminal);
        }

        public void Release(Criminal criminal)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Criminal> GetAll()
        {
            throw new NotImplementedException();
        }

        public Criminal Get(int id)
        {
            throw new NotImplementedException();
        }

        public void IncreaseTerm(Criminal cr, int days)
        {
            throw new NotImplementedException();
        }

        public void DecreaseTerm(Criminal cr, int days)
        {
            throw new NotImplementedException();
        }

    }
}
