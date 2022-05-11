using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaScriptInterpreter
{
  static public class LiamDebugger
  {
    static public int debugLevel = 3; // 1 = very important log info, 5 = heavy stream/update log info

    static public void Name(string className, string methodName, int Level)
    {
#if DEBUG
      if (Level <= debugLevel)
      {
        Console.WriteLine($"{className} : {methodName}" );
      }
#endif
    }

    static public void Message(string message, int level)
    {
#if DEBUG
      if (level <= debugLevel)
      {
        Console.WriteLine(message);
      }
#endif
    }
  }
}
