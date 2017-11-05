using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommSubsystem
{
    public class StateChange
    {
        public delegate void Handler(StateChange changeInfo);

        public enum ChangeType { None, Startup, Update, Error, Shutdown };
        public ChangeType Type { get; set; }
        public object Subject { get; set; }
        public object Context { get; set; }

        public override string ToString()
        {
            var subject = Subject as string;
            if (subject == null && Subject != null)
                subject = Subject.GetType().Name;

            if (string.IsNullOrWhiteSpace(subject))
                subject = "none";

            var context = Context?.ToString() ?? "";
            return $"Type={Type} Subject={subject} Context={context}";
        }
    }
}
