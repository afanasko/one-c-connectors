
package ocs2;

import java.net.MalformedURLException;
import java.net.URL;
import java.util.logging.Logger;
import javax.xml.namespace.QName;
import javax.xml.ws.Service;
import javax.xml.ws.WebEndpoint;
import javax.xml.ws.WebServiceClient;
import javax.xml.ws.WebServiceFeature;


/**
 * This class was generated by the JAX-WS RI.
 * JAX-WS RI 2.1.6 in JDK 6
 * Generated source version: 2.1
 * 
 */
@WebServiceClient(name = "OneCService2", targetNamespace = "http://OneCService2", wsdlLocation = "http://127.0.0.1:9000/OneCService2?wsdl")
public class OneCService2_Service
    extends Service
{

    private final static URL ONECSERVICE2_WSDL_LOCATION;
    private final static Logger logger = Logger.getLogger(ocs2.OneCService2_Service.class.getName());

    static {
        URL url = null;
        try {
            URL baseUrl;
            baseUrl = ocs2.OneCService2_Service.class.getResource(".");
            url = new URL(baseUrl, "http://127.0.0.1:9000/OneCService2?wsdl");
        } catch (MalformedURLException e) {
            logger.warning("Failed to create URL for the wsdl Location: 'http://127.0.0.1:9000/OneCService2?wsdl', retrying as a local file");
            logger.warning(e.getMessage());
        }
        ONECSERVICE2_WSDL_LOCATION = url;
    }

    public OneCService2_Service(URL wsdlLocation, QName serviceName) {
        super(wsdlLocation, serviceName);
    }

    public OneCService2_Service() {
        super(ONECSERVICE2_WSDL_LOCATION, new QName("http://OneCService2", "OneCService2"));
    }

    /**
     * 
     * @return
     *     returns OneCService2
     */
    @WebEndpoint(name = "BasicHttpBinding_OneCService2")
    public OneCService2 getBasicHttpBindingOneCService2() {
        return super.getPort(new QName("http://OneCService2", "BasicHttpBinding_OneCService2"), OneCService2.class);
    }

    /**
     * 
     * @param features
     *     A list of {@link javax.xml.ws.WebServiceFeature} to configure on the proxy.  Supported features not in the <code>features</code> parameter will have their default values.
     * @return
     *     returns OneCService2
     */
    @WebEndpoint(name = "BasicHttpBinding_OneCService2")
    public OneCService2 getBasicHttpBindingOneCService2(WebServiceFeature... features) {
        return super.getPort(new QName("http://OneCService2", "BasicHttpBinding_OneCService2"), OneCService2.class, features);
    }

}
