using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;

namespace whoshomemobile
{
    public class IoTClientManager
    {
        private ServiceClient _serviceClient;
        
        public IoTClientManager()
        {
            _serviceClient = ServiceClient.CreateFromConnectionString(Authentification.ServiceConnectionString);
        }

        public async void ScanMethod(string deviceId)
        {
            CloudToDeviceMethod method = new CloudToDeviceMethod("scan");
            Task<CloudToDeviceMethodResult> resultTask = _serviceClient.InvokeDeviceMethodAsync(deviceId, method);
            CloudToDeviceMethodResult result = await resultTask;

        }
    }


}