using LibreHardwareMonitor.Hardware;
using System.Runtime.Versioning;
using System.Text;

namespace Lib.HardwareMonitor
{
    /// <summary>
    /// need administrator
    /// LibreHardwareMonitorLib (0.9.3)
    /// </summary>
    [SupportedOSPlatform(BuiltInRoleProcess.WINDOWS)]
    internal class HardwareMonitorProcess
    {
        private Computer _com;
        private BuiltInRoleProcess _builtInRole; //get administrator

        internal HardwareMonitorProcess()
        {
            _com = new Computer
            {
                //get info hardware
                IsCpuEnabled = true,
                IsGpuEnabled = true,
                IsMotherboardEnabled = true,
                IsStorageEnabled = true,
                IsBatteryEnabled = true,
                IsControllerEnabled = true,
                IsMemoryEnabled = true,
                IsNetworkEnabled = true,
                IsPsuEnabled = true,
            };

            var v = _com.Hardware;

            _builtInRole = new BuiltInRoleProcess();
        }

        public async void F()
        {
            await Open();
            var str = await GetInfoAfterUpdate();
            Console.WriteLine(str);
        }

        public async Task Open()
        {
            await _builtInRole.GetAdministrator();
            await Task.Run(() =>
            {
                _com.Open();
            });
        }

        public async Task<string> GetInfoAfterUpdate()
        {
            const string FAIL_VALUE = "-1"; //if no administrator, value is "-1"

            var sb = new StringBuilder();

            await Task.Run(() =>
            {
                foreach (var hard in _com.Hardware)
                {
                    hard.Update();
                    foreach (var sub in hard.SubHardware)
                    {
                        sub.Update();
                    }

                    foreach (var sensor in hard.Sensors)
                    {
                        string value = sensor.Value.HasValue ? sensor.Value.Value.ToString() : FAIL_VALUE;
                        string text = $"{sensor.Name} {sensor.SensorType} = {value}";
                        sb.AppendLine(text);
                    }
                }
            });
            return sb.ToString();
        }
    }
}
