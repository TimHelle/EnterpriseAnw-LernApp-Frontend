using System;
using System.Collections.Generic;
using System.Text;

namespace App
{
    public class Antworten
    {
        private uint? id;
        private String text;
        private bool status;

        public Antworten() { }
        public Antworten(String text, bool status)
        {
            setText(text);
            setStatus(status);
        }

        public uint? getId()
        {
            return this.id;
        }
        public String getText()
        {
            return this.text;
        }
        public void setText(String text)
        {
            this.text = text;
        }
        public bool getStatus()
        {
            return this.status;
        }
        public void setStatus(Boolean status)
        {
            this.status = status;
        }
    }
}
