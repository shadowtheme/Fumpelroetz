using System.Collections.Generic;

namespace EscapeRoomControlPanel
{
    public class SceneData
    {
        public string Name { get; set; }
        public int LocationX { get; set; }
        public int LocationY { get; set; }
        public List<string> Behaviors { get; set; }// Liste der Verhaltensweisen

        public SceneData()
        {
            Behaviors = new List<string>();
        }

    }
}

