using System.Threading;
using System.Windows.Forms;

using Yonay.Properties;

namespace Yonay
{
    internal class Program
    {
        private const string MutexName = "YoNayMutex";

        private static void Main(string[] args)
        {
            using (var mutex = new Mutex(true, Program.MutexName, out var created))
            {
                if (!created)
                {
                    return;
                }

                var notifyThread = new Thread(
                    () =>
                    {
                        var notifyIcon = new NotifyIcon
                                         {
                                             Icon = Resources.NOYone_1,
                                             Text = "YoNAY",
                                             BalloonTipText = "YoNAY is now protecting you from yourself",
                                             Visible = true
                                         };

                        notifyIcon.ShowBalloonTip(2);
                        Application.Run();
                    }
                );

                notifyThread.SetApartmentState(ApartmentState.STA);
                notifyThread.Start();

                using (new YoNay())
                {
                    notifyThread.Join();
                }
            }
        }
    }
}
