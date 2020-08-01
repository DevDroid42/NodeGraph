using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Makaretu.Dns;
using System.Linq;
using System;
using UnityEngine.Assertions.Must;

public class mdnsTest : MonoBehaviour
{
    MulticastService mdns = new MulticastService();
    ServiceDiscovery sd;
    // Start is called before the first frame update
    void Start()
    {
        sd = new ServiceDiscovery(mdns);
        mdns.NetworkInterfaceDiscovered += (s, e) =>
        {
            foreach (var nic in e.NetworkInterfaces)
            {
                Debug.Log($"NIC '{nic.Name}'");
            }

            // Ask for the name of all services.
            sd.QueryAllServices();
        };

        sd.ServiceDiscovered += (s, serviceName) =>
        {
            Debug.Log($"service '{serviceName}'");

            // Ask for the name of instances of the service.
            mdns.SendQuery(serviceName, type: DnsType.PTR);
        };

        sd.ServiceInstanceDiscovered += (s, e) =>
        {
            Debug.Log($"service instance '{e.ServiceInstanceName}'");
            

            // Ask for the service instance details.
            mdns.SendQuery(e.ServiceInstanceName, type: DnsType.SRV);
        };

        mdns.AnswerReceived += (s, e) =>
        {
            // Is this an answer to a service instance details?
            var servers = e.Message.Answers.OfType<SRVRecord>();
            foreach (var server in servers)
            {
                Debug.Log($"host '{server.Target}' for '{server.Name}'");

                // Ask for the host IP addresses.
                mdns.SendQuery(server.Target, type: DnsType.A);
                mdns.SendQuery(server.Target, type: DnsType.AAAA);
            }

            // Is this an answer to host addresses?
            var addresses = e.Message.Answers.OfType<AddressRecord>();
            foreach (var address in addresses)
            {                
                Debug.Log($"host '{address.Name}' at {address.Address}");
            }

        };

        try
        {
            mdns.Start();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

    }


    private void OnDisable()
    {
        sd.Dispose();
        mdns.Stop();
    }
}
