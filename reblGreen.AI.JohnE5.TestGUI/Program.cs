using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using reblGreen.AI.JohnE5;
using reblGreen.AI.JohnE5.Classes;

namespace TestJohnE5
{
    static class Program
    {
        public static string Directory = AppDomain.CurrentDomain.BaseDirectory;
        public static string TrainingModel = "en-training.bin";
        public static Classifier Classifier = new Classifier(BinaryModel.FromFile(AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + TrainingModel));

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmTest());
        }
    }
}
