using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecTech.Domain.Interfaces.Services
{
    public interface IQRCodeService
    {
        string GenerateQRCode(int eventId);
        int DecodeQRCode(string qrCodeData);
    }
}
