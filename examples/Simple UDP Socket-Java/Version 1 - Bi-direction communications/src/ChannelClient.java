import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.InetSocketAddress;
import java.nio.ByteBuffer;
import java.nio.channels.DatagramChannel;

public class ChannelClient extends Thread {
    public void run() {
        // use this option when you have a public IP
        // String ip = Utils.getCurrentIp();
        // use this option for local IP
        String ip = Utils.getLocalIp();

        DatagramChannel client = null;
        try {
            // retrieve server address
            InetSocketAddress serverAddress = new InetSocketAddress(ip, Utils.PORT);

            // open a new client
            client = DatagramChannel.open();
            client.bind(null);
            System.out.println("Start a new client");

            // prepare the system input to get the customer's message
            BufferedReader br = new BufferedReader(new InputStreamReader(System.in));
            String msg = enterMessage(br);

            while(!msg.equals("Q")) {
                String text = enterText(br);

                // send a message to server and clean the buffer
                ByteBuffer buffer = ByteBuffer.wrap(text.getBytes());
                client.send(buffer, serverAddress);
                buffer.clear();

                // then listen to the response from server
                client.receive(buffer);
                buffer.flip();
                int limits = buffer.limit();
                byte bytes[] = new byte[limits];
                buffer.get(bytes, 0, limits);
                String response = new String(bytes);
                System.out.println("Server returns message: " + response);

                // ask for indicator again
                msg = enterMessage(br);
            }
            client.close();
        } catch (Exception e) {
            System.err.println("[Client Error]: " + e.getMessage());
        } finally {
            try {
                client.close();
            } catch (Exception e) { }
        }
    }

    private String enterMessage(BufferedReader br) throws IOException {
        String msg = "";
        while(!msg.equals("M") && !msg.equals("Q")) {
            System.out.print("Enter an indicator (M-message, Q-quit): ");
            msg = br.readLine();
        }
        return msg;
    }

    private String enterText(BufferedReader br) throws IOException {
        System.out.print("Enter a message: ");
        return br.readLine();
    }

    public static void main(String args[]) {
        new ChannelClient().start();
    }
}
