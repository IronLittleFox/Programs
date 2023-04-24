using Android.Content;
using Android.Hardware.Camera2;
using CameraApp.Droid;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidCameraService))]
namespace CameraApp.Droid
{
    
    public class AndroidCameraService : ICameraService
    {
        public async Task<bool> GetTorchStatusAsync()
        {
            CameraManager cameraManager = (CameraManager)Android.App.Application.Context.GetSystemService(Context.CameraService);
            string cameraId = cameraManager.GetCameraIdList()[0];
            CameraCharacteristics characteristics = cameraManager.GetCameraCharacteristics(cameraId);
            return (bool)characteristics.Get(CameraCharacteristics.FlashInfoAvailable) == true;
        }

        public async Task SetTorchAsync(bool on)
        {
            CameraManager cameraManager = (CameraManager)Android.App.Application.Context.GetSystemService(Context.CameraService);
            string cameraId = cameraManager.GetCameraIdList()[0];
            cameraManager.SetTorchMode(cameraId, on);
        }
    }
}