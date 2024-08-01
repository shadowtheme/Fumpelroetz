namespace EscapeRoomControlPanel
{
    public abstract class Behavior
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
    }

    public class PinStatusBehavior : Behavior
    {
        public override string Name => "PinStatus";
        public override string Description => "Beschreibung für PinStatus abfragen";
    }

    public class SzeneGestartetBehavior : Behavior
    {
        public override string Name => "Szene gestarted";
        public override string Description => "Beschreibung abfragen";
    }

    public class AudioStartetBehavior : Behavior
    {
        public override string Name => "Audio startet";
        public override string Description => "Beschreibung abfragen";
    }

    public class AudioStopptBehavior : Behavior
    {
        public override string Name => "Audio stoppt";
        public override string Description => "Beschreibung abfragen";
    }

    public class AudioStopptBehavior : Behavior
    {
        public override string Name => "Aktionbutton";
        public override string Description => "Beschreibung abfragen";
    }

}
