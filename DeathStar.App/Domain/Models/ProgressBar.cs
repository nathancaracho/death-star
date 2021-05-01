using System;

namespace DeathStar.App.Domain.Models
{
    public struct ProgressBar
    {
        private const char _space = '■';
        private const char _block = ' ';
        private readonly string _value;
        private ProgressBar((int, int) progress)
        {
            var (done, Total) = progress;

            if (Total < 0)
                throw new ArgumentException("the total value can't be lower than 0");
            if (done < 0)
                throw new ArgumentException("the done value can't be lower than 0");


            var bar = "";
            var percent = Math.Floor((decimal)done * 10 / Total);
            for (byte i = 1; i <= 10; i++)
            {

                if (percent <= i)
                    bar += _block;
                else
                    bar += _space;
            }
            _value = $"[ {bar}] - {done}≃{percent * 10}%";

        }

        public static implicit operator string(ProgressBar progress) => progress._value;
        public override string ToString() => _value;
        public static implicit operator ProgressBar((int, int) progress) => new ProgressBar(progress);
    }
}