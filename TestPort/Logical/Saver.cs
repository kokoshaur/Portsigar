using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.AspNetCore.Mvc;
using TestPort.Models;
using System.Net;

namespace TestPort.Logical
{
    public static class Saver
    {
        private static Extractor Subj = null;

        public static int Refresh(int subjPerDay)
        {
            FileStream extractor = new FileStream("extract.json", FileMode.OpenOrCreate);
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(extractor, new Extractor(subjPerDay)); 
            extractor.Close();

            extractor = new FileStream("extract.json", FileMode.OpenOrCreate);

            Subj = (Extractor)formatter.Deserialize(extractor);
            extractor.Close();

            return 1;
        }
        public static int Show(int subjPerDay = 5)
        {
            if (Subj == null)
            {
                FileStream extractor = new FileStream("extract.json", FileMode.OpenOrCreate);
                BinaryFormatter formatter = new BinaryFormatter();

                if (extractor.Length == 0)
                {
                    formatter.Serialize(extractor, new Extractor(subjPerDay)); // Если бд пустой, серриализуем в него экстрактор по subjPerDay.
                    extractor.Close();
                    extractor = new FileStream("extract.json", FileMode.OpenOrCreate);
                }

                Subj = (Extractor)formatter.Deserialize(extractor);
                extractor.Close();
            }
            
            return Subj.NextSiggaret();
        }
        [Serializable]
        struct wifiReq
        {
            public string name;
            public string password;
        }
        public static int wiFiAsync()
        {
            FileStream wifi = new FileStream("wifi.json", FileMode.OpenOrCreate);
            BinaryFormatter formatter = new BinaryFormatter();


            if (wifi.Length == 0)
            {
                wifi.Close();
                return 0;
            }
            try
            {
                wifiReq T = (wifiReq) formatter.Deserialize(wifi);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.google.ru");
                request.Credentials = new NetworkCredential(T.name, T.password);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                wifi.Close();
                response.Close();
                return 1;
            }
            catch (Exception e)
            {
                wifi.Close();
                return 0;
            }
        }

        public static int CreateWifi(string Name, string Pas)
        {
            FileStream wifi = new FileStream("wifi.json", FileMode.OpenOrCreate);
            BinaryFormatter formatter = new BinaryFormatter();

            wifiReq T = new wifiReq();
            T.name = Name;
            T.password = Pas;
            formatter.Serialize(wifi, T);
            wifi.Close();

            return wiFiAsync();
        }

        public static int isFirst()
        {
            FileStream extractor = new FileStream("extract.json", FileMode.OpenOrCreate);

            if (extractor.Length == 0)
            {
                extractor.Close();
                return 1;
            }
            extractor.Close();
            return 0;
        }
        public static int isOpen()
        {
            if (Subj != null)
                if (Subj.skipSubj > 0)
                    return 1;
            return 0;
        }
        public static int GetSubj()
        {
            if (isOpen() == 1)
                return Subj.GetSubj();
            return 0;
        }

    }
}