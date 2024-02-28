using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace filesigner2
{
    public class CertificateItem
    {
        public X509Certificate2 certificate;
        public override string ToString()
        {
            if (certificate == null) return "empty certificate!";
            return certificate.GetNameInfo(X509NameType.SimpleName, false) + " - до " + certificate.NotAfter.ToShortDateString();// +" выдан " +certificate.GetNameInfo(X509NameType.SimpleName,true);
        }
        public CertificateItem(X509Certificate2 cert)
        {
            certificate = cert;
        }
    }
}
