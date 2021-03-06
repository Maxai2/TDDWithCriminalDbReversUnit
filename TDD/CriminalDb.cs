﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//---------------------------------------------------

namespace TDD
{
    public class CriminalDb
    {
        List<Criminal> criminals = new List<Criminal>();

        //---------------------------------------------------

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

            if (criminal.Jailed == true)
            {
                var msg = $"{nameof(criminal.Jailed)} should be false";
                throw new ArgumentException(nameof(criminal.Jailed), msg);
            }

            foreach (var item in criminals)
            {
                if (item.Code == criminal.Code)
                {
                    criminal.Jailed = false;
                    return;
                    //throw new ArgumentException(nameof(criminal.Code), $"{nameof(criminal.Code)} was in CriminalDb");
                }
            }

            criminal.DaysTerm = term;
            criminal.Jailed = true;

            criminals.Add(criminal);
        }

        //---------------------------------------------------

        public void Release(Criminal criminal)
        {
            if (criminal is null)
            {
                throw new ArgumentNullException(nameof(criminal));
            }

            var cr = criminals.Where(c => c.Id == criminal.Id).FirstOrDefault();

            if (cr is null)
            {
                throw new KeyNotFoundException();
            }

            if (criminal.Jailed == false)
            {
                throw new ArgumentException(nameof(criminal.Jailed));
            }

            criminal.Jailed = false;
        }

        //---------------------------------------------------

        public IEnumerable<Criminal> GetAll()
        {
            return criminals;
        }

        //---------------------------------------------------

        public Criminal Get(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            var criminal = criminals.Where(c => c.Id == id).FirstOrDefault();

            if (criminal is null)
            {
                return null;
            }

            return criminal;
        }

        //---------------------------------------------------

        public void IncreaseTerm(Criminal cr, int days)
        {
            if (cr is null)
            {
                throw new ArgumentNullException(nameof(cr));
            }

            var tempCr = criminals.Where(c => c.Id == cr.Id).FirstOrDefault();

            if (tempCr is null)
            {
                throw new KeyNotFoundException();
            }

            if (days <= 0 || days > 5000)
            {
                throw new ArgumentOutOfRangeException(nameof(days));
            }

            cr.DaysTerm += days;

            if (cr.DaysTerm >= 5000)
            {
                cr.DaysTerm = 5000;
            }
        }

        //---------------------------------------------------

        public void DecreaseTerm(Criminal cr, int days)
        {
            if (cr is null)
            {
                throw new ArgumentNullException(nameof(cr));
            }

            var tempCr = criminals.Where(c => c.Id == cr.Id).FirstOrDefault();

            if (tempCr is null)
            {
                throw new KeyNotFoundException();
            }

            if (days <= 0 || days > 5000)
            {
                throw new ArgumentOutOfRangeException(nameof(days));
            }

            cr.DaysTerm -= days;

            if (cr.DaysTerm < 0)
            {
                cr.DaysTerm = 0;
            }

            if (cr.DaysTerm == 0)
            {
                cr.Jailed = false;
            }
        }

        //---------------------------------------------------
    }
}
