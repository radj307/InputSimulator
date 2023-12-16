using System;

namespace Example.WPF
{
    static class Program
    {

        [STAThread]
        static int Main(string[] args)
        {
            return new App().Run(new MainWindow());
        }
    }
}
