using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;

namespace whoshomemobile
{
    public class IoTClientManager
    {
        private static ServiceClient _serviceClient;

        internal static void InitIoTClientManager()
        {
            _serviceClient = ServiceClient.CreateFromConnectionString(Authentification.IoTServiceConnectionString);
        }

        public static async void ScanMethod(string deviceId)
        {
            CloudToDeviceMethod method = new CloudToDeviceMethod("scan");
            Task<CloudToDeviceMethodResult> resultTask = _serviceClient.InvokeDeviceMethodAsync(deviceId, method);
            CloudToDeviceMethodResult result = await resultTask;

        }
    }


}