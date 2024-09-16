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
        string GenerateQRCodeAsync(Guid eventId);
        BaseResult<Guid> DecodeQRCodeAsync(string tokenString);
    }
}
