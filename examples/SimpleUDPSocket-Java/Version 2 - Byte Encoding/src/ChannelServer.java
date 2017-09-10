import java.net.InetSocketAddress;
import java.net.SocketAddress;
import java.nio.ByteBuffer;
import java.nio.channels.DatagramChannel;

public class ChannelServer extends Thread {

    public void run() {
        // use this option when you have a public IP
        // String ip = Utils.getCurrentIp();
        // use this option for local IP
        String ip = Utils.getLocalIp();

        DatagramChannel server = null;
        try {
            // start a new server
            server = DatagramChannel.open();
            InetSocketAddress sAddr = new InetSocketAddress(ip, Utils.PORT);
            server.bind(sAddr);
            System.out.println("Server is starting at " + ip);

            ByteBuffer buffer = ByteBuffer.allocate(1024);
            while (true) {
                // stop here before a new message to come from clients
                System.out.println("Waiting for message...");
                SocketAddress remoteAddr = server.receive(buffer);

                // storing data into buffer and print it out
                buffer.flip();
                int limits = buffer.limit();
                byte bytes[] = new byte[limits];
                buffer.get(bytes, 0, limits);
                Message msg = Message.decode(bytes);
                System.out.println("Client at " + remoteAddr + " sent a message: Id=" + msg.Id + ", Time=" + msg.Timestamp + ", Msg=" + msg.Text);
                buffer.rewind();

                // send back to client
                ByteBuffer returnBuffer = ByteBuffer.wrap(msg.Text.getBytes());
                server.send(returnBuffer, remoteAddr);
                buffer.clear();
            }

        } catch (Exception e) {
            System.err.println("[Server Error]: " + e.getMessage());
        } finally {
            try {
                server.close();
            } catch (Exception e) { }
        }
    }

    public static void main(String args[]) {
        new ChannelServer().start();
    }
}
