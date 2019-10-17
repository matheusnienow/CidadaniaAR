using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Observer
{
    public class Message
    {
        public string Text { get; set; }

        public Message(string text)
        {
            Text = text;
        }
    }

}
