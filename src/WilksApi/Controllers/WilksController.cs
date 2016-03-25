using System;
using System.Linq;
using Microsoft.AspNet.Mvc;

namespace WilksApi.Controllers
{
    public class WilksController : Controller
    {
        [HttpGet]
        public double GetWilks(
            string token,
            string team_id,
            string team_domain,
            string channel_id,
            string channel_name,
            string user_id,
            string user_name,
            string command,
            string text)
        {
            text = text.Trim();
            int atIndex = text.IndexOf('@');
            int weightIndex = text.IndexOf('l');
            if (weightIndex == -1) weightIndex = text.IndexOf('k');
            var total = text.Substring(0, atIndex).Trim().Split('/').Select(double.Parse).Sum();
            var bw = double.Parse(text.Substring(atIndex + 1, weightIndex - atIndex - 1).Trim());
            var weightType = text.Substring(weightIndex, 2);
            var gender = (Gender)Enum.Parse(typeof(Gender), text.Substring(text.Length - 1), true);
            if (weightType.Contains("lb"))
            {
                bw = GetWeightInKg(bw);
                total = GetWeightInKg(total);
            }
            return GetWilks(gender, bw, total);
        }

        private double GetWeightInKg(double weightInLbs)
        {
            return weightInLbs * .45359237;
        }

        private double GetWilks(Gender gender, double bodyweightInKg, double totalInKg)
        {
            var x = GetCoefficients(gender);
            return (totalInKg * 500) / 
                (x.a 
                + x.b * bodyweightInKg 
                + x.c * Math.Pow(bodyweightInKg, 2) 
                + x.d * Math.Pow(bodyweightInKg, 3) 
                + x.e * Math.Pow(bodyweightInKg, 4) 
                + x.f * Math.Pow(bodyweightInKg, 5));
        }

        private Coefficients GetCoefficients(Gender gender)
        {
            switch(gender)
            {
                case Gender.M:
                    return new Coefficients
                    {
                        a = -216.0475144,
                        b = 16.2606339,
                        c = -0.002388645,
                        d = -0.00113732,
                        e = 7.01863E-06,
                        f = -1.291E-08
                    };
                case Gender.F:
                    return new Coefficients
                    {
                        a = 594.31747775582,
                        b = -27.23842536447,
                        c = 0.82112226871,
                        d = -0.00930733913,
                        e = 0.00004731582,
                        f = -0.00000009054
                    };
                default:
                    return null;
            }
        }
    }
}
