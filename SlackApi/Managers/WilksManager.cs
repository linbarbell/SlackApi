using System;
using System.Linq;
using SlackApi.Models;

namespace SlackApi.Managers
{
    public class WilksManager
    {
        /// <summary>
        /// Gets the wilks message based on what the weakpot typed in slack
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public string GetWilksMessage(string text)
        {
            text = text.Trim();
            int atIndex = text.IndexOf('@');
            int weightIndex = text.IndexOf('l');
            if (weightIndex == -1) weightIndex = text.IndexOf('k');
            var total = text.Substring(0, atIndex).Trim().Split('/').Select(double.Parse).Sum();
            var bw = double.Parse(text.Substring(atIndex + 1, weightIndex - atIndex - 1).Trim());
            var weightType = text.Substring(weightIndex, 2);
            var gender = (Gender)Enum.Parse(typeof(Gender), text.Substring(text.Length - 1), true);
            double wilks = weightType.Contains("lb") ? GetWilksLb(gender, bw, total) : GetWilksKg(gender, bw, total);
            return $"({gender}) {total.ToString("F")} @ {bw.ToString("F")} {weightType}: *{wilks.ToString("F")}* wilks";
        }

        /// <summary>
        /// Gets the wilks in lb.
        /// </summary>
        /// <param name="gender">The gender.</param>
        /// <param name="bodyweightInLb">The bodyweight in lb.</param>
        /// <param name="totalInLb">The total in lb.</param>
        /// <returns></returns>
        public double GetWilksLb(Gender gender, double bodyweightInLb, double totalInLb)
        {
            var bwKg = GetWeightInKg(bodyweightInLb);
            var totalKg = GetWeightInKg(totalInLb);
            return GetWilksKg(gender, bwKg, totalKg);
        }

        /// <summary>
        /// Gets the wilks in kg.
        /// </summary>
        /// <param name="gender">The gender.</param>
        /// <param name="bodyweightInKg">The bodyweight in kg.</param>
        /// <param name="totalInKg">The total in kg.</param>
        /// <returns></returns>
        public double GetWilksKg(Gender gender, double bodyweightInKg, double totalInKg)
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

        /// <summary>
        /// Gets the weight in kg.
        /// </summary>
        /// <param name="weightInLbs">The weight in LBS.</param>
        /// <returns></returns>
        public double GetWeightInKg(double weightInLbs)
        {
            return weightInLbs * .45359237;
        }

        private Coefficients GetCoefficients(Gender gender)
        {
            switch (gender)
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
