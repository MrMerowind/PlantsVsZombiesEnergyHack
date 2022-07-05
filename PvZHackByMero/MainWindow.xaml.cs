using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PvZHackByMero
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static VAMemory vam = null;
        public static IntPtr mainProces;
        public static int[] oSunflower = { 0x729670, 0x868, 0x5578 };

        public static int CalculateValue(IntPtr ptr, int[] offsets)
        {
            int result = 0;
            for (int i = 0; i < offsets.Length + 1; i++)
            {
                if (i == 0)
                {
                    ptr = IntPtr.Add(ptr, offsets[i]);
                }
                else if (i == offsets.Length)
                {
                    result = (int)vam.ReadInt32(ptr);
                }
                else
                {
                    ptr = IntPtr.Add((IntPtr)vam.ReadInt32(ptr), offsets[i]);
                }
            }
            return result;
        }
        public static void SetValueInt(IntPtr ptr, int[] offsets, int value)
        {
            for (int i = 0; i < offsets.Length + 1; i++)
            {
                if (i == 0)
                {
                    ptr = IntPtr.Add(ptr, offsets[i]);
                }
                else if (i == offsets.Length)
                {
                    vam.WriteInt32(ptr, value);
                }
                else
                {
                    ptr = IntPtr.Add((IntPtr)vam.ReadInt32(ptr), offsets[i]);
                }
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            Process[] process = Process.GetProcessesByName("PlantsVsZombies");
            if(process.Length < 1)
            {
                MessageBox.Show("Najpierw włącz gre!");
                Application.Current.Shutdown();
            }
            else
            {
                mainProces = process[0].MainModule.BaseAddress;
                vam = new VAMemory("PlantsVsZombies");
            }


            
        }

        private void SetEnergy_Click(object sender, RoutedEventArgs e)
        {
            int val = -1;
            try
            {
                val = Convert.ToInt32(energyValue.Text);
            }
            catch(Exception exception)
            {
                MessageBox.Show("Błędna wartość.");
                return;
            }
            if(val >= 0)
            {
                SetValueInt(new IntPtr(0x0), oSunflower, val);
            }
            
        }
    }
}
