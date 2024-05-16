using SecTech.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecTech.Domain.Interfaces.Services
{
    public interface IQRCodeService
    {
        string GenerateQRCode(Guid eventId);
        BaseResult<Guid> DecodeQRCode(string tokenString);
    }
}
