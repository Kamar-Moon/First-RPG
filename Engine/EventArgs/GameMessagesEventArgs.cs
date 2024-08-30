using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//The ViewModel (GameSession) is going to communicate with the View (Player UI) by “raising events”.
//This will let the View know that something happened.
//But, the View also needs to know what text to display on the screen.
//To send additional information with an event, you use an “event argument”.
//We’re going to create a custom event argument that will hold the text to display in the View.
//when we raise the message event, we’ll instantiate a new GameMessageEventArgs object,
//with the message text, and pass that object by the event.

namespace Engine.EventArgs
{
    public class GameMessagesEventArgs : System.EventArgs
    {
        public string Message { get; private set; }

        public GameMessagesEventArgs(string message)
        {
            Message = message;
        }
    }
}
