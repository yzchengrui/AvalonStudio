﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Models.Tools.Debuggers
{
    public class Frame
    {
        public static Frame FromDataString (string data)
        {
            var result = new Frame ();

            if(data[0] != '{')
            {
                throw new Exception ("Object expected to begin with '{'");
            }

            var pairs = data.RemoveBraces().ToNameValuePairs ();

            foreach (var pair in pairs)
            {
                switch (pair.Name)
                {
                    case "addr":
                        result.Address = Convert.ToUInt64 (pair.Value.Replace ("0x", ""), 16);
                        break;

                    case "func":
                        result.Function = pair.Value;
                        break;

                    case "args":
                        result.Arguments = new List<Variable>();
                        
                        var arguments = pair.Value.ToArray ();

                        foreach (var argument in arguments)
                        {
                            var newArgument = Variable.FromDataString (argument.RemoveBraces ());
                            newArgument.IsArgument = true;
                            result.Arguments.Add (newArgument);
                        }
                        break;

                    case "file":
                        result.File = pair.Value;
                        break;

                    case "fullname":
                        result.FullFileName = pair.Value;
                        break;

                    case "line":
                        if (pair.Value != string.Empty)
                        {
                            result.Line = Convert.ToInt32 (pair.Value);
                        }
                        break;

                    case "level":
                        if (pair.Value != string.Empty)
                        {
                            result.Level = Convert.ToUInt32(pair.Value);
                        }
                        break;
                }
            }

            return result;
        }

        public UInt32 Level { get; set; }
        public UInt64 Address { get; set; }
        public string Function { get; set; }
        public List<Variable> Arguments { get; set; }
        public string File { get; set; }
        public string FullFileName { get; set; }
        public int Line { get; set; }
    }
}
