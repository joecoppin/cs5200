﻿namespace CommSub
{
    public class StateChange
    {
        public delegate void Handler(StateChange changeInfo);

        public enum ChangeType { None, Addition, Update, Deletion, Error, Shutdown };
        public ChangeType Type { get; set; }
        public object Subject { get; set; }

        public override string ToString()
        {
            string subject = Subject as string;
            if (subject==null && Subject!=null)
                subject = Subject.GetType().Name;

            if (string.IsNullOrWhiteSpace(subject))
                subject = "none";
            return $"Type={Type} Subject={subject}";
        }
    }
}
