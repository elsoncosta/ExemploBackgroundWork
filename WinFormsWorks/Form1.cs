using System.Reflection;

namespace WinFormsWorks
{
    public partial class Form1 : Form
    {
        int timerInterval = 0;

        public Form1()
        {
            InitializeComponent();

            timerInterval = (2 * 1000);

            timerSincronizacao.Interval = timerInterval;
            timerSincronizacao.Start();
            timerSincronizacao_Tick(null, null);
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!backgroundWorker1.IsBusy)
                {
                    button1.Enabled = false;
                    backgroundWorker1.RunWorkerAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            for (int i = 0; i < 10000; i++)
            {
                AcionarMetodoEntreThreads(listBox1, "Items", "Add", new object[] { $"Posiçao : {i} - {DateTime.Now}" });
                Thread.Sleep(1000);
                if (backgroundWorker1.CancellationPending)
                {
                    break;
                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {

        }

        private void timerSincronizacao_Tick(object sender, EventArgs e)
        {
            try
            {

                if (!backgroundWorker1.IsBusy)
                {
                    button1.Enabled = false;
                    backgroundWorker1.RunWorkerAsync();
                }
             }
            catch (Exception)
            {

                throw;
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
           AcionarMetodoEntreThreads(listBox1, "Items", "Add", new object[] { $"Processo finalizado ou cancelado - {DateTime.Now}" });
            button1.Enabled = true;

            //inicia timer novamente
            timerSincronizacao.Interval = timerInterval = (10 * 1000); ;
            if (!timerSincronizacao.Enabled)
                timerSincronizacao.Start();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
            }
        }

        delegate void SetControlValueCall(Control oControl, string Propriedade, string Metodo, object[] propValue);
        public static void AcionarMetodoEntreThreads(Control oControl, string Propriedade, string Metodo, object[] propValue)
        {
            if (oControl.InvokeRequired)
            {
                oControl.Invoke(new SetControlValueCall(AcionarMetodoEntreThreads), new object[] { oControl, Propriedade, Metodo, propValue });
            }
            else
            {
                Type Tipo = oControl.GetType();

                var Items = Tipo.GetProperty(Propriedade).GetValue(oControl, null);

                var Insert = Items.GetType().GetMethod(Metodo);

                Insert.Invoke(Items, propValue);
            }
        }
    }
}