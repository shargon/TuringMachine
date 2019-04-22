using TuringMachine.Core.Arguments;
using TuringMachine.Core.Sockets;

namespace TuringMachine.Core.Delegates
{
    public delegate bool delItsAlive(TuringSocket socket, TuringAgentArgs e);
}