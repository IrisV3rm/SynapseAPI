using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSocketSharp;

namespace SynapseAPI
{
    public class API
    {
        private bool Exec = false;

        public void Execute(string Script)
        {
            using (var ws = new WebSocket("ws://localhost:24892/execute"))
            {
                ws.OnMessage += (sender, e) =>
                {
                    if (e.Data == "OK")
                    {
                        Exec = true;
                    }
                };
                if (Exec)
                {
                    ws.Send(Script);
                    Exec = false;
                }
                else
                {
                    MessageBox.Show("Script execution failed!", "Synapse API", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
        }
    
        public void Attach()
        {
            using (var ws = new WebSocket("ws://localhost:24892/attach"))
            {
                ws.OnMessage += (sender, e) =>
                {
                    switch (e.Data)
                    {
                        case "READY":
                            MessageBox.Show("Synapse has attached!", "Synapse API", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            break;
                        case "ALREADY_ATTACHED":
                            MessageBox.Show("Synapse is already attached!", "Synapse API", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            break;
                        case "INTERRUPT":
                            MessageBox.Show("Synapse has failed to attach!", "Synapse API", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            break;
                    }
                };
                ws.Connect();
                ws.Send("ATTACH");
            }
        }
    }
}
