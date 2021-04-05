using System;

namespace DeathStar.App.Domain.Models
{
    public class EnvironmentModel
    {
        public string Name { get; private set; }
        public string Connection { get; private set; }
        public bool ShowWarning { get; private set; }

        public EnvironmentModel(string name, string connection, bool showWarning = true)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("name can't be null or empty");
            if (string.IsNullOrEmpty(connection))
                throw new ArgumentException("connection can't be null or empty");

            Name = name;
            Connection = connection;
            ShowWarning = showWarning;
        }
    }
}