using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


//makecert -n "CN=DCSender" -r -sky exchange -sv "C:\ByronWork\DevCode\MiscHelper\MiscHelperTest\Certificates\DCSender.pvk" "C:\ByronWork\DevCode\MiscHelper\MiscHelperTest\Certificates\DCSender.cer"
//"C:\Program Files (x86)\Windows Kits\8.0\bin\x86\pvk2pfx.exe" -pvk "C:\ByronWork\DevCode\MiscHelper\MiscHelperTest\Certificates\DCSender.pvk" -spc "C:\ByronWork\DevCode\MiscHelper\MiscHelperTest\Certificates\DCSender.cer" -pfx "C:\ByronWork\DevCode\MiscHelper\MiscHelperTest\Certificates\DCSender.pfx"

//makecert -n "CN=DCReciever" -r -sky exchange -sv "C:\ByronWork\DevCode\MiscHelper\MiscHelperTest\Certificates\DCReciever.pvk" "C:\ByronWork\DevCode\MiscHelper\MiscHelperTest\Certificates\DCReciever.cer"
//"C:\Program Files (x86)\Windows Kits\8.0\bin\x86\pvk2pfx.exe" -pvk "C:\ByronWork\DevCode\MiscHelper\MiscHelperTest\Certificates\DCReciever.pvk" -spc "C:\ByronWork\DevCode\MiscHelper\MiscHelperTest\Certificates\DCReciever.cer" -pfx "C:\ByronWork\DevCode\MiscHelper\MiscHelperTest\Certificates\DCReciever.pfx"

namespace MiscHelper
{

    public class DigitalSignature
    {
        public string Cipher { get; set; }
        public string Signature { get; set; }


        public static DigitalSignature BuildSignedMessage(X509Certificate2 senderPrivate, X509Certificate2 recieverPublic, string message)
        {
            X509Encryption.Sign(senderPrivate, recieverPublic, message, out byte[] cipherBytes, out byte[] signatureHash);

            return new DigitalSignature()
            {
                Cipher = Convert.ToBase64String(cipherBytes),
                Signature = Convert.ToBase64String(signatureHash)
            };
        }


    }
}
