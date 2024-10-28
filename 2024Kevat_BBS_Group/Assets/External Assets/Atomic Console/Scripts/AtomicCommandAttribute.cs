namespace AtomicConsole
{
    using System;

    [AttributeUsage(AttributeTargets.Method)]
    public class AtomicCommandAttribute : Attribute
    {
        public string Group { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool PasswordProtected { get; private set; }

        public AtomicCommandAttribute(string group = "", string name = "", string description = "", bool isPasswordProtected = false)
        {
            Group = group;
            Name = name;
            Description = description;
            PasswordProtected = isPasswordProtected;
        }
    }
}