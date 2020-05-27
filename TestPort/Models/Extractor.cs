using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace TestPort.Models
{
    [Serializable]
    public class Extractor
    {
        public DateTime NowDateTime = DateTime.Now;
        public DateTime[] timing { get; }   //Расписание выдачи сижек
        public int _sigarCount { get; protected set; } = 0;
        private int index = 0;
        public int skipSubj { get; protected set; } = 0;
        public Extractor(int countSigarettePerDay = 0)
        {
            NowDateTime = NowDateTime.AddDays(-1);
            NowDateTime = NowDateTime.AddHours(-9);

            for (int i = countSigarettePerDay; i > 0; i--)
                _sigarCount += i;

            timing = new DateTime[_sigarCount];
            for (int i = countSigarettePerDay; i > 0; i--)
                CreateDay(i);

            NowDateTime = DateTime.Now;
            index = 0;
        }

        public int NextSiggaret()
        {
            while ((int)timing[index].Subtract(DateTime.Now).TotalSeconds < 0)
            {
                skipSubj++;
                index++;
                if (index >= timing.Length)
                    return -1;
            }

            return (int)timing[index].Subtract(DateTime.Now).TotalSeconds;

        }
        public int GetSubj()
        {
            //Типа запустил моторчик и выдал контент 
            skipSubj--;
            return 1;
        }

        private bool CreateDay(int countSigarette)   //Добавляем расписание на 1 день
        {
            NowDateTime = NowDateTime.AddDays(1);
            NowDateTime = NowDateTime.AddHours(9);    //Начинаем в 9 утра следующего дня

            int seconds = (12 / countSigarette) * 60 * 60;  //Распределение сигарет на 12 часовой день

            for (int i = 0; i < countSigarette; i++)
            {
                timing[index] = NowDateTime = NowDateTime.AddSeconds(seconds);
                index++;
            }

            NowDateTime.AddHours(1);    //Доводим время до след дня

            return true;
        }
    }
}
