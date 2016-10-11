using System.Diagnostics;
using System.Windows.Forms;
using TuringMachine.Core;

namespace TuringMachine.Controls
{
    public partial class UPercentWave : UserControl
    {
        Stopwatch _Watch;
        /// <summary>
        /// Constructor
        /// </summary>
        public UPercentWave()
        {
            InitializeComponent();
            _Watch = new Stopwatch();
        }
        /// <summary>
        /// Pause
        /// </summary>
        public void Pause() { _Watch.Stop(); }
        /// <summary>
        /// Stop
        /// </summary>
        public void Stop()
        {
            _Watch.Stop();
            _Watch.Reset();
        }
        /// <summary>
        /// Start
        /// </summary>
        public void Start()
        {
            Stop();
            _Watch.Start();
        }
        /// <summary>
        /// Get percent factor
        /// </summary>
        /// <param name="secuencePercentage">Secuence percentage</param>
        public double GetPercentFactor(double secuencePercentage)
        {
            // Sin curve

            return 100;
        }
    }
}