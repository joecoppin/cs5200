import java.net.Inet4Address;
import java.net.InetAddress;
import java.net.NetworkInterface;
import java.util.Enumeration;

public class Utils {
    public static int PORT = 8888;

    public static String getLocalIp() {
        return "127.0.0.1";
    }

    public static String getCurrentIp() {
        try {
            Enumeration<NetworkInterface> nets = NetworkInterface.getNetworkInterfaces();
            while (nets.hasMoreElements()) {
                NetworkInterface intf = nets.nextElement();
                // ignore the non-virtual, non-loopback and zero-interface networks
                if (!intf.isVirtual() && !intf.isLoopback() && intf.getInterfaceAddresses().size() > 0 &&
                        !intf.getDisplayName().toLowerCase().contains("virtual")) {
                    Enumeration<InetAddress> addrs = intf.getInetAddresses();
                    while (addrs.hasMoreElements()) {
                        InetAddress net = addrs.nextElement();
                        if (net instanceof Inet4Address) {
                            // return the first network IP that found
                            return net.getHostAddress();
                        }
                    }
                }
            }
            return "";
        } catch(Exception e) {
            e.printStackTrace();
            return "";
        }
    }

}
