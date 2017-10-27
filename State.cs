using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FurryDownloader
{
    public class State
    {
        public StateCode code;
        public string message;

        public State(StateCode code)
        {
            this.code = code;
        }

        public State(StateCode code, string message)
        {
            this.code = code;
            this.message = message;
        }
    }

    public enum StateCode {
        ok,
        error,
        finish
    }
}
