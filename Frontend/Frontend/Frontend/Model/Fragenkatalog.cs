using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace App
{
    public class Fragenkatalog
    {
        public static Collection<Fragen> katalog { get; set; } = new Collection<Fragen>();
        public static int fragenAnzahl { get; set; }
    }
}
