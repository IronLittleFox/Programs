using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CameraApp
{
    public interface ICameraService
    {
        Task<bool> GetTorchStatusAsync();
        Task SetTorchAsync(bool on);
    }
}
