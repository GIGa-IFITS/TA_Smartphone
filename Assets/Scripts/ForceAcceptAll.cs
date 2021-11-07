using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class ForceAcceptAll : CertificateHandler
{
   protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true;
    }
}
